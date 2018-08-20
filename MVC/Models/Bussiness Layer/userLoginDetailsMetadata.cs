using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Web.Mvc;

namespace MVC.Models
{
    [MetadataType(typeof(userLoginDetailsMetadata))]
    public partial class userLoginDetails
    {
        [Required(ErrorMessage = "This field is required"),
            DataType(DataType.Password),
            System.ComponentModel.DataAnnotations.Compare("password"),
            DisplayName("Confirm Password")]
        public string ConfirmPassword { get; set; }
        public string ErrorMessage { get; set; }
        public bool UserNameInUse { get; set; }
    }
    public class userLoginDetailsMetadata
    {
        [Required(ErrorMessage = "This field is required"),
            StringLength(20, MinimumLength = 4, ErrorMessage = "Must be at least 4 characters long."),
            DisplayName("User Name"),
            Remote("IsUserNameAvailable", "Register", HttpMethod = "POST", ErrorMessage = "User Name already exists")]
        public string userName { get; set; }
        [Required(ErrorMessage = "This field is required"),
            DataType(DataType.Password),
            DisplayName("Password")]
        public string password { get; set; }
        [Required, DataType(DataType.EmailAddress, ErrorMessage = "Invalid Email Id"),
            DisplayName("Email Id")]
        public string email { get; set; }
    }
}

// when i tried to have remote validation on multiple fields post action doesn't work.
//, Remote("IsEmaiIdAvailable", "Register",HttpMethod = "POST", ErrorMessage = "Email Id already exists", AdditionalFields = "userName")