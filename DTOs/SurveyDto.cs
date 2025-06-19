namespace SurveyApi.DTOs
{
    public class SurveySubmissionDto
    {
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public string ContactNumbers { get; set; } = string.Empty;
        
        // Food preferences (checkbox - can select multiple)
        public List<string> FavoriteFoods { get; set; } = new List<string>();
        
        // Statement ratings (radio buttons - one rating per statement)
        public int? MovieRating { get; set; }      // statement1: I like to watch movies
        public int? RadioRating { get; set; }      // statement2: I like to listen to radio
        public int? EatOutRating { get; set; }     // statement3: I like to eat out
        public int? TVRating { get; set; }         // statement4: I like to watch TV
    }
    
    public class SurveyResponseDto
    {
        public int SurveyID { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public string ContactNumbers { get; set; } = string.Empty;
        public DateTime SubmissionDate { get; set; }
        
        public List<string> FavoriteFoods { get; set; } = new List<string>();
        public int? MovieRating { get; set; }
        public int? RadioRating { get; set; }
        public int? EatOutRating { get; set; }
        public int? TVRating { get; set; }
    }
}
