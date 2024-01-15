using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace StackoverflowAssignment.Models
{
    public class AskQuestionViewModel
    {
        public int QuestionId { get; set; }

        public string QuestionName { get; set; }

        
        public DateTime QuestionDateAndTime { get; set; }

        
        public int CategoryId { get; set; }

        public string CategoryName { get; set; }


        public int AnswerCount { get; set; }

        public int VoteCount { get; set; }

        public bool UserVote { get; set; }

        public int Views { get; set; }

        
        public String Name { get; set; }

        public int ViewCount { get; set; }


        [ForeignKey("CategoryId")]
        public CategoryModel Category { get; set; }


        public int AnswerId { get; set; }

        public string AnswerText { get; set; }

    
        public DateTime AnswerDateAndTime { get; set; }


    }
}
