#region Library
using ChatWithSignal.Domain.Messengers.Components;
using System;
using System.Threading.Tasks;
#endregion

namespace ChatWithSignal.Domain.Messengers.Base
{
    public abstract class BaseMessenger
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; protected set; }

        /// <summary>
        /// Name / Назва
        /// </summary>
        public string Name { get; protected set; }

        /// <summary>
        /// Content Count / Кількість контенту
        /// </summary>
        public uint ContentCount { get; protected set; }

        /// <summary>
        /// Last dateTime sended content / Останній час надіслання контенту
        /// </summary>
        public string LastDateTimeActive { get;  set; }

        public Task AddContent(Content content)
        {
            ContentCount++;
            LastDateTimeActive = content.DateCreated + " " + content.TimeCreated;

            return Task.CompletedTask;
        }
    }
}