using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SurveyApi.Models
{
    [Table("options")]
    public class Options
    {
        [Key, Column(Order = 0)]
        public int SurveyID { get; set; }

        [Key, Column(Order = 1)]
        [StringLength(100)]
        public string OptionValue { get; set; } = string.Empty;

        public int? Rating { get; set; }

        // Navigation property
        [ForeignKey("SurveyID")]
        public virtual PersonalDetails PersonalDetails { get; set; } = null!;
    }
}
