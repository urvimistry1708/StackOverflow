using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;

namespace StackoverflowAssignment.Models
{
    public class QuestionModel
    {

        [Key]
        [DisplayName("Question ID")]
        public int QuestionId { get; set; }

        [DisplayName("Question")]
        public string QuestionName { get; set; }

        [DisplayName("Last Updated")]
        public DateTime QuestionDateAndTime { get; set; }

        [DisplayName("Category ID")]
        public int CategoryId { get; set; }

        public int Views { get; set; }

        [DisplayName("Posted By")]
        public String Name { get; set; }


        public int AnsCount { get; set; }

        public int ViewCount { get; set; }


        [ForeignKey("CategoryId")]
        public CategoryModel Category { get; set; }


        public ICollection<AnswerModel> Answers { get; set; }

        public ICollection<QuestionVoteModel> QuestionVotes { get; set; }

    }
}
