using Microsoft.EntityFrameworkCore;
using SurveyApi.Data;
using SurveyApi.DTOs;
using SurveyApi.Models;

namespace SurveyApi.Services
{
    public interface ISurveyService
    {
        Task<int> SubmitSurveyAsync(SurveySubmissionDto submission);
        Task<List<SurveyResponseDto>> GetAllSurveysAsync();
        Task<SurveyResponseDto?> GetSurveyByIdAsync(int surveyId);
        Task<SurveyStatisticsDto> GetSurveyStatisticsAsync();
    }

    public class SurveyService : ISurveyService
    {
        private readonly SurveyDbContext _context;

        public SurveyService(SurveyDbContext context)
        {
            _context = context;
        }

        public async Task<int> SubmitSurveyAsync(SurveySubmissionDto submission)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Create personal details record
                var personalDetails = new PersonalDetails
                {
                    FullName = submission.FullName,
                    Email = submission.Email,
                    DateOfBirth = submission.DateOfBirth,
                    ContactNumbers = submission.ContactNumbers,
                    SubmissionDate = DateTime.Now
                };

                _context.PersonalDetails.Add(personalDetails);
                await _context.SaveChangesAsync();

                var surveyId = personalDetails.SurveyID;

                // Create options records
                var options = new List<Options>();

                // Add food preferences (checkboxes - can have multiple)
                foreach (var food in submission.FavoriteFoods)
                {
                    options.Add(new Options
                    {
                        SurveyID = surveyId,
                        OptionValue = $"FavoriteFood:{food}",
                        Rating = null
                    });
                }

                // Add statement ratings (radio buttons - one rating per statement)
                if (submission.MovieRating.HasValue)
                {
                    options.Add(new Options
                    {
                        SurveyID = surveyId,
                        OptionValue = "MovieRating",
                        Rating = submission.MovieRating.Value
                    });
                }

                if (submission.RadioRating.HasValue)
                {
                    options.Add(new Options
                    {
                        SurveyID = surveyId,
                        OptionValue = "RadioRating",
                        Rating = submission.RadioRating.Value
                    });
                }

                if (submission.EatOutRating.HasValue)
                {
                    options.Add(new Options
                    {
                        SurveyID = surveyId,
                        OptionValue = "EatOutRating",
                        Rating = submission.EatOutRating.Value
                    });
                }

                if (submission.TVRating.HasValue)
                {
                    options.Add(new Options
                    {
                        SurveyID = surveyId,
                        OptionValue = "TVRating",
                        Rating = submission.TVRating.Value
                    });
                }

                if (options.Any())
                {
                    _context.Options.AddRange(options);
                    await _context.SaveChangesAsync();
                }

                await transaction.CommitAsync();
                return surveyId;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<List<SurveyResponseDto>> GetAllSurveysAsync()
        {
            var surveys = await _context.PersonalDetails
                .Include(p => p.Options)
                .ToListAsync();

            return surveys.Select(MapToSurveyResponseDto).ToList();
        }

        public async Task<SurveyResponseDto?> GetSurveyByIdAsync(int surveyId)
        {
            var survey = await _context.PersonalDetails
                .Include(p => p.Options)
                .FirstOrDefaultAsync(p => p.SurveyID == surveyId);

            return survey != null ? MapToSurveyResponseDto(survey) : null;
        }        public async Task<SurveyStatisticsDto> GetSurveyStatisticsAsync()
        {
            var totalSurveys = await _context.PersonalDetails.CountAsync();
            
            if (totalSurveys == 0)
            {
                return new SurveyStatisticsDto
                {
                    TotalSurveys = 0,
                    AverageAge = 0,
                    OldestAge = 0,
                    YoungestAge = 0,
                    PizzaPercentage = 0,
                    PastaPercentage = 0,
                    PapAndWorsPercentage = 0,
                    MovieAverageRating = 0,
                    RadioAverageRating = 0,
                    EatOutAverageRating = 0,
                    TVAverageRating = 0
                };
            }

            // Calculate age statistics
            var allPersons = await _context.PersonalDetails.ToListAsync();
            var ages = allPersons.Select(p => DateTime.Now.Year - p.DateOfBirth.Year).ToList();
            
            var averageAge = ages.Average();
            var oldestAge = ages.Max();
            var youngestAge = ages.Min();

            // Calculate food preferences percentages
            var foodOptions = await _context.Options
                .Where(o => o.OptionValue.StartsWith("FavoriteFood:"))
                .ToListAsync();

            var pizzaCount = foodOptions.Count(o => o.OptionValue == "FavoriteFood:Pizza");
            var pastaCount = foodOptions.Count(o => o.OptionValue == "FavoriteFood:Pasta");
            var papWorsCount = foodOptions.Count(o => o.OptionValue == "FavoriteFood:Pap and Wors");

            var pizzaPercentage = (double)pizzaCount / totalSurveys * 100;
            var pastaPercentage = (double)pastaCount / totalSurveys * 100;
            var papAndWorsPercentage = (double)papWorsCount / totalSurveys * 100;

            // Calculate average ratings
            var movieRatings = await _context.Options
                .Where(o => o.OptionValue == "MovieRating" && o.Rating.HasValue)
                .Select(o => o.Rating!.Value)
                .ToListAsync();

            var radioRatings = await _context.Options
                .Where(o => o.OptionValue == "RadioRating" && o.Rating.HasValue)
                .Select(o => o.Rating!.Value)
                .ToListAsync();

            var eatOutRatings = await _context.Options
                .Where(o => o.OptionValue == "EatOutRating" && o.Rating.HasValue)
                .Select(o => o.Rating!.Value)
                .ToListAsync();

            var tvRatings = await _context.Options
                .Where(o => o.OptionValue == "TVRating" && o.Rating.HasValue)
                .Select(o => o.Rating!.Value)
                .ToListAsync();

            return new SurveyStatisticsDto
            {
                TotalSurveys = totalSurveys,
                AverageAge = Math.Round(averageAge, 1),
                OldestAge = oldestAge,
                YoungestAge = youngestAge,
                PizzaPercentage = Math.Round(pizzaPercentage, 1),
                PastaPercentage = Math.Round(pastaPercentage, 1),
                PapAndWorsPercentage = Math.Round(papAndWorsPercentage, 1),
                MovieAverageRating = movieRatings.Any() ? Math.Round(movieRatings.Average(), 1) : 0,
                RadioAverageRating = radioRatings.Any() ? Math.Round(radioRatings.Average(), 1) : 0,
                EatOutAverageRating = eatOutRatings.Any() ? Math.Round(eatOutRatings.Average(), 1) : 0,
                TVAverageRating = tvRatings.Any() ? Math.Round(tvRatings.Average(), 1) : 0
            };        }

        private static SurveyResponseDto MapToSurveyResponseDto(PersonalDetails survey)
        {
            var favoriteFoods = survey.Options
                .Where(o => o.OptionValue.StartsWith("FavoriteFood:"))
                .Select(o => o.OptionValue.Replace("FavoriteFood:", ""))
                .ToList();

            var movieRating = survey.Options
                .FirstOrDefault(o => o.OptionValue == "MovieRating")?.Rating;
            
            var radioRating = survey.Options
                .FirstOrDefault(o => o.OptionValue == "RadioRating")?.Rating;
            
            var eatOutRating = survey.Options
                .FirstOrDefault(o => o.OptionValue == "EatOutRating")?.Rating;
            
            var tvRating = survey.Options
                .FirstOrDefault(o => o.OptionValue == "TVRating")?.Rating;

            return new SurveyResponseDto
            {
                SurveyID = survey.SurveyID,
                FullName = survey.FullName,
                Email = survey.Email,
                DateOfBirth = survey.DateOfBirth,
                ContactNumbers = survey.ContactNumbers,
                SubmissionDate = survey.SubmissionDate,
                FavoriteFoods = favoriteFoods,
                MovieRating = movieRating,
                RadioRating = radioRating,
                EatOutRating = eatOutRating,
                TVRating = tvRating
            };
        }
    }
}
