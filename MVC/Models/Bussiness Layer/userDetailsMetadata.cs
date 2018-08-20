using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVC.Models
{
    [MetadataType(typeof(userDetailsMetadata))]
    public partial class userDetails
    {
        public string ConfirmPassword { get; set; }
    }
    public class userDetailsMetadata
    {
        [Display(Name = "First Name"), RegularExpression((@"^[a-zA-Z]+$"),ErrorMessage ="Enter only Alphabets")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "First name required")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Last name required"), RegularExpression((@"^[a-zA-Z]+$"),ErrorMessage = "Enter only Alphabets")]
        public string LastName { get; set; }

        [Display(Name = "Email ID"),RegularExpression((@"^([\w\.\-_]+)?\w+@[\w-_]+(\.\w+){1,}"),
            ErrorMessage = "Please enter a valid email address as activation code will be sent to the email address used to register")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Email ID required")]
        public string emailId { get; set; }

        [Display(Name = "Date of birth")]
        [DataType(DataType.Date),Required(ErrorMessage ="Please enter a valid date of birth")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public System.DateTime dateOfBirth { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Password is required")]
        [DataType(DataType.Password),RegularExpression((@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[a-zA-Z]).{8,}$"),
            ErrorMessage = "Password must contain: Minimum 8 characters atleast 1 UpperCase Alphabet, 1 LowerCase Alphabet, 1 Number and 1 Special Character")]
        public string password { get; set; }

        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        [Compare("password", ErrorMessage = "Confirm password and password do not match")]
        public string ConfirmPassword { get; set; }
    }
}