using System.ComponentModel.DataAnnotations;

namespace SchiftPlanner.Models
{
    public class Login
    {
        [Required(ErrorMessage = "Pole nazwa jest wymagane!")]
        public string LoginAccount { get; set; }

        [Required(ErrorMessage = "Pole nazwa jest wymagane!")]
        public string PasswordAccount { get; set; }
    }
}
