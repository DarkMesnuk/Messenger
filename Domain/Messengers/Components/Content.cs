#region Library
using ChatWithSignal.Domain.Identity;
using ChatWithSignal.Domain.Messengers.Base;
using System;
#endregion

namespace ChatWithSignal.Domain.Messengers.Components
{
    public class Content : BaseContent
    {
        /// <summary>
        /// Message / Повідомлення
        /// </summary>
        public string Message { get; set; }

        #region Constructors
        /// <summary>
        /// Default / За замовчуванням
        /// </summary>
        public Content()
        { }

        /// <summary>
        /// Create content with all parametrs / Створення змісту з усіма параметрами
        /// </summary>
        /// <param name="senderId"></param>
        /// <param name="message"></param>
        public Content(Profile profile, BaseMessenger messenger, string message)
        {
            SenderId = profile.Id;
            Message = message;
            MessengerId = messenger.Id;
            DateTimeCreated = DateTime.UtcNow.ToString();
        }
        #endregion
    }
}