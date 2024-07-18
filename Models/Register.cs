using Microsoft.AspNetCore.Authentication;
using System.ComponentModel.DataAnnotations;

namespace SchiftPlanner.Models
{
    public class Register
    {
        public IList<AuthenticationScheme> ExternalLogins { get; set; }



        [Required(ErrorMessage = "Pole nazwa jest wymagane!")]
        public string Login { get; set; }



        [Required(ErrorMessage = "Pole e-mail jest wymagane!")]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }


        [Required(ErrorMessage = "Pole hasło jest wymagane!")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }




        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "Hasła nie pasują do siebie.")]
        public string ConfirmPassword { get; set; }



    }

}

