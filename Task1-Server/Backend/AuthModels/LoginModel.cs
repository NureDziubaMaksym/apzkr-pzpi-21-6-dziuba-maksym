using System.ComponentModel.DataAnnotations;

namespace Backend.AuthModels
{
    public class LoginModel
    {
        [Required(ErrorMessage = "User Login is required")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }
}
