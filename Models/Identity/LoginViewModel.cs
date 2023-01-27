#region Library
using System.ComponentModel.DataAnnotations;
#endregion

namespace ChatWithSignal.Models.Identity
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Пошта")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [Display(Name = "Запам'ятати мене")]
        public bool RememberMe { get; set; }
    }
}
