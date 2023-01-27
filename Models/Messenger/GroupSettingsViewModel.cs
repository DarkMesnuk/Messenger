#region Library
using System;
using System.ComponentModel.DataAnnotations;
#endregion

namespace ChatWithSignal.Models.Messenger
{
    public class GroupSettingsViewModel
    {
        public Guid GroupId { get; set; }
        [Required]
        [Display(Name = "Назва")]
        public string Name { get; set; }

        [Display(Name = "Опис")]
        public string Text { get; set; }

        [Display(Name = "Публічна група")]
        public bool IsPublic { get; set; }
    }
}
