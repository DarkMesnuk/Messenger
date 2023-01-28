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
        /// Members in Json / Учасники у Json
        /// </summary>
        public string MembersJson { get; protected set; }

        /// <summary>
        /// Members / Учасники
        /// </summary>
        [NotMapped]
        public Dictionary<string, MemberRoleEnum> Members { get; protected set; }

        #region Constructors
        /// <summary>
        /// Default / За замовчуванням
        /// </summary>
        protected BaseMessenger() { }

        /// <summary>
        /// Create Bmessenger with all parameters / Створення Бмесенджера з усіма параметрами
        /// </summary>
        /// <param name="id"></param>
        /// <param name="contents"></param>
        /// <param name="members"></param>
        protected BaseMessenger(Guid id, Dictionary<string, MemberRoleEnum> members)
        {
            Id = id;
            Members = members;
        }
        #endregion

        #region Processing

        /// <summary>
        /// Set information about messages and members from their json / Встановлення інформації про повідомлення та учасників із їхнього json
        /// </summary>
        /// <returns></returns>
        public Task GetFromJsonAsync()
        {
            Members = JsonSerializer.Deserialize<Dictionary<string, MemberRoleEnum>>(MembersJson);

            return Task.CompletedTask;
        }
        #endregion
    }
}