#region Library
using System;
#endregion

namespace ChatWithSignal.Domain.Messengers.Base
{
    public abstract class BaseContent
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Messenger Id
        /// </summary>
        public Guid MessengerId { get; set; }

        /// <summary>
        /// DateTime Create // Дата створення 
        /// </summary>
        public string DateTimeCreated { get; set; }

        /// <summary>
        /// Sender Id / Id відправника
        /// </summary>
        public string SenderId { get; set; }
    }
}