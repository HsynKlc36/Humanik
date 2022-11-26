using System.ComponentModel.DataAnnotations;

namespace HumanIK.UI.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "E-posta alanı zorunlu")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Şifre alanı zorunlu")]
        public string Password { get; set; }
    }
}
