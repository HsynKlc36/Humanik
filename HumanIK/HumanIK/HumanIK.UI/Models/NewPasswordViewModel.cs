using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HumanIK.UI.Models
{
    public class NewPasswordViewModel
    {
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,15}$", ErrorMessage = "Şifre kriterlere uymuyor. ( ℹ️ )")]
        [Required(ErrorMessage = "Bu alan zorunlu!")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Bu alan zorunlu!")]
        [Compare("Password", ErrorMessage = "Şifreler eşleşmiyor!")]// ilk şifreyle karşılaştırır!
        public string SecondPassword { get; set; }
        public string CurrentPassword { get; set; }
        public string Token { get; set; }
        public string Email { get; set; }
    }
}
