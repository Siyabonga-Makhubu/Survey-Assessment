using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SurveyApi.Models
{
    [Table("personaldetails")]
    public class PersonalDetails
    {
        [Key]
        public int SurveyID { get; set; }

        [Required]
        [StringLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [StringLength(20)]
        public string ContactNumbers { get; set; } = string.Empty;

        [Required]
        public DateTime SubmissionDate { get; set; } = DateTime.Now;

        // Navigation property
        public virtual ICollection<Options> Options { get; set; } = new List<Options>();
    }
}
