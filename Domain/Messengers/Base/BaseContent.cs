#region Library
using ChatWithSignal.Domain.Enum;
using System;
#endregion

namespace ChatWithSignal.Domain.Messengers.Base
{
    public abstract class BaseContent
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; protected set; }

        /// <summary>
        /// Messenger Id
        /// </summary>
        public Guid MessengerId { get; protected set; }

        /// <summary>
        /// Type
        /// </summary>
        public MessengerTypeEnum MessengerType { get; protected set; }

        /// <summary>
        /// Date Create // Дата створення
        /// </summary>
        public string DateCreated { get; protected set; }

        /// <summary>
        /// Time Create // Час створення 
        /// </summary>
        public string TimeCreated { get; protected set; }

        /// <summary>
        /// Sender Id / Id відправника
        /// </summary>
        public string SenderId { get; protected set; }
    }
}