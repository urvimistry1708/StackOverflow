using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace StackoverflowAssignment.Models
{
    public class CategoryModel
    {
        [Key]
        [DisplayName("Category ID")]
        public int CategoryId { get; set; }

        [DisplayName("Category")]
        public string CategoryName { get; set; }

        public ICollection<QuestionModel> Questions { get; set; }
    }
}
