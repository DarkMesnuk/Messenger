#region Library
using System.ComponentModel.DataAnnotations;
#endregion

namespace ChatWithSignal.Models.Identity
{
    public class RegisterViewModel
    {
        [Required]
        [StringLength(32, ErrorMessage = "Довжина ніку не повинна перевищувати 32 символи")]
        [Display(Name = "Нік")]
        public string NickName { get; set; }

        [Required]
        [StringLength(64, ErrorMessage = "Довжина пошти не повинна перевищувати 64 символи")]
        [Display(Name = "Пошта")]
        public string Email { get; set; }

        [Required]
        [StringLength(18, ErrorMessage = "Пароль повинен мати від 6-18 символів", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "Паролі не однакові")]
        [DataType(DataType.Password)]
        [Display(Name = "Повторіть Пароль")]
        public string RepeatPassword { get; set; }
    }
}