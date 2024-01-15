using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace StackoverflowAssignment.Models
{
    public class AnswerModel
    {
        [Key]
        [DisplayName("Answer ID")]
        public int AnswerId { get; set; }

        [DisplayName("Question ID")]
        public int QuestionId { get; set; }

        [DisplayName("Answer")]
        public string AnswerText { get; set; }

        [DisplayName("Last Updated")]
        public DateTime AnswerDateAndTime { get; set; }

        [DisplayName("Posted By")]
        public String Name { get; set; }

        public string UserId { get; set; }


        [ForeignKey("UserId")]
        public IdentityUser User { get; set; }


        [ForeignKey("QuestionId")]
        public QuestionModel Question { get; set; }


    }
}
