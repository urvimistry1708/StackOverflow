using Microsoft.AspNetCore.Identity;
using StackoverflowAssignment.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StackoverflowAssignment.Models
{
    public class QuestionVoteModel
    {

        [Key]
        public int Id {  get; set; }
       
        public int QuestionId {  get; set; }

        [ForeignKey("QuestionId")]
        public QuestionModel Question { get; set; }

        public string UserId {  get; set; }

        [ForeignKey("UserId")]
        public IdentityUser User { get; set; }
    }
}
