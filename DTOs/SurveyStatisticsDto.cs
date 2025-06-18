namespace SurveyApi.DTOs
{
    public class SurveyStatisticsDto
    {
        public int TotalSurveys { get; set; }
        public double AverageAge { get; set; }
        public int OldestAge { get; set; }
        public int YoungestAge { get; set; }
        public double PizzaPercentage { get; set; }
        public double PastaPercentage { get; set; }
        public double PapAndWorsPercentage { get; set; }
        public double MovieAverageRating { get; set; }
        public double RadioAverageRating { get; set; }
        public double EatOutAverageRating { get; set; }
        public double TVAverageRating { get; set; }
    }
}
