#region Library
using ChatWithSignal.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Text.Json;
#endregion

namespace ChatWithSignal.Domain.Messengers.Base
{
    public abstract class BaseRepositoryMessenger : BaseMessenger
    {
        /// <summary>
        /// Members in Json / Учасники у Json
        /// </summary>
        public string MembersJson { get; protected set; }

        #region Constructors
        /// <summary>
        /// Default / За замовчуванням
        /// </summary>
        protected BaseRepositoryMessenger() { }

        /// <summary>
        /// Create base messenger with all parameters / Створення базового месенджера з усіма параметрами
        /// </summary>
        /// <param name="id"></param>
        /// <param name="contents"></param>
        /// <param name="members"></param>
        protected BaseRepositoryMessenger(Dictionary<string, MemberRoleEnum> members)
        {
            Id = Guid.NewGuid();
            MembersJson = JsonSerializer.Serialize(members);
            var DateTimeCreate = DateTime.UtcNow;
            LastDateTimeActive = DateTimeCreate.ToShortDateString() + " " + DateTimeCreate.ToShortTimeString();
        }
        #endregion
    }
}
