#region Library
using ChatWithSignal.Domain.Messengers.Base;
using System;
#endregion

namespace ChatWithSignal.Domain.Messengers.Components
{
    public class Content : BContent
    {
        /// <summary>
        /// Message / Повідомлення
        /// </summary>
        public string Message { get; set; }

        #region Constructors
        /// <summary>
        /// Create content with all parametrs / Створення змісту з усіма параметрами
        /// </summary>
        /// <param name="id"></param>
        /// <param name="day"></param>
        /// <param name="time"></param>
        /// <param name="senderId"></param>
        /// <param name="file"></param>
        /// <param name="type"></param>
        public Content(string senderId, string message)
        {
            SenderId = senderId;
            Message = message;
            DateTimeCreated = DateTime.UtcNow.ToString();
        }
        #endregion
    }
}