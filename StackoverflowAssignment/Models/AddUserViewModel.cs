using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace StackoverflowAssignment.Models
{
    public class AddUserViewModel
    {
    
        [Required(ErrorMessage ="Please Enter Valid Name!")]

        public string Name { get; set; }

        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter a password.")]
        [StringLength(10, ErrorMessage = "Please enter Valid Password.")]
        [Compare("ConfirmPassword")] 
        public string Password { get; set; }

        [Required(ErrorMessage = "Please confirm your password.")]
        [Display(Name = "Confirm Password")] 
        public string ConfirmPassword { get; set; }



        [Required(ErrorMessage = "Please enter a Contact.")]
        [StringLength(10, ErrorMessage = "Please enter Valid Contact.")]
        public string Contact { get; set; }



        public bool RememberMe { get; set; }
    }
}
