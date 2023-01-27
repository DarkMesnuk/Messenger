#region Library
using System.ComponentModel.DataAnnotations;
#endregion

namespace ChatWithSignal.Models.Messenger
{
    public class CreateGroupViewModel
    {
        [Required]
        [Display(Name = "Назва")]
        public string Name { get; set; }

        [Display(Name = "Публічна група")]
        public bool IsPublic { get; set; }
    }
}
