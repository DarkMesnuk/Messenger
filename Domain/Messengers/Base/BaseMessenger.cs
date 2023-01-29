#region Library
using ChatWithSignal.Domain.Enum;
using ChatWithSignal.Domain.Messengers.Components;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
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
        /// Content Count/Кількість контенту
        /// </summary>
        public uint ContentCount { get; protected set; }

        public void AddContent() 
            => ContentCount++;
    }
}