using Microsoft.AspNetCore.Mvc;
using SurveyApi.DTOs;
using SurveyApi.Services;

namespace SurveyApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SurveyController : ControllerBase
    {
        private readonly ISurveyService _surveyService;

        public SurveyController(ISurveyService surveyService)
        {
            _surveyService = surveyService;
        }

        /// <summary>
        /// Submit a new survey response
        /// </summary>
        [HttpPost("submit")]
        public async Task<ActionResult<int>> SubmitSurvey([FromBody] SurveySubmissionDto submission)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var surveyId = await _surveyService.SubmitSurveyAsync(submission);
                return Ok(new { SurveyId = surveyId, Message = "Survey submitted successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while submitting the survey", Error = ex.Message });
            }
        }

        /// <summary>
        /// Get all survey responses
        /// </summary>
        [HttpGet("all")]
        public async Task<ActionResult<List<SurveyResponseDto>>> GetAllSurveys()
        {
            try
            {
                var surveys = await _surveyService.GetAllSurveysAsync();
                return Ok(surveys);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while retrieving surveys", Error = ex.Message });
            }
        }

        /// <summary>
        /// Get a specific survey by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<SurveyResponseDto>> GetSurveyById(int id)
        {
            try
            {
                var survey = await _surveyService.GetSurveyByIdAsync(id);
                if (survey == null)
                {
                    return NotFound(new { Message = $"Survey with ID {id} not found" });
                }
                return Ok(survey);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while retrieving the survey", Error = ex.Message });
            }
        }

        /// <summary>
        /// Get survey statistics and analytics
        /// </summary>
        [HttpGet("statistics")]
        public async Task<ActionResult<SurveyStatisticsDto>> GetSurveyStatistics()
        {
            try
            {
                var statistics = await _surveyService.GetSurveyStatisticsAsync();
                return Ok(statistics);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while retrieving statistics", Error = ex.Message });
            }
        }
    }
}
