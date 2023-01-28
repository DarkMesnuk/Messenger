#region Library
using ChatWithSignal.Domain.Enum;
using ChatWithSignal.Domain.Identity;
using ChatWithSignal.Domain.Messengers.Base;
using ChatWithSignal.Domain.Messengers.Components;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Threading.Tasks;
#endregion

namespace ChatWithSignal.Domain.Messengers
{
    [NotMapped]
    public class Messenger : BaseMessenger
    {
        /// <summary>
        /// Type
        /// </summary>
        public MessengerTypeEnum Type { get; private set; }

        /// <summary>
        /// Members / Учасники
        /// </summary>
        [NotMapped]
        public Dictionary<string, MemberRoleEnum> Members { get; protected set; }

        #region Constructors
        /// <summary>
        /// Create messenger with Id and type messenger / Створення месенджера лише з Id та типу месенджера
        /// </summary>
        /// <param name="id"></param>
        /// <param name="type"></param>
        public Messenger(Guid id, MessengerTypeEnum type)
        {
            Id = id;
            Type = type;
        }

        /// <summary>
        /// Create messenger based on group / Створення месенджера на основі групи
        /// </summary>
        /// <param name="group"></param>
        public Messenger(Group group, bool needMembers = true) 
            : this(group.Id, MessengerTypeEnum.Group, group.MembersJson, needMembers) 
        {
            Name = group.Name;
        }

        /// <summary>
        /// Create messenger based on chat / Створення месенджера на основі чату
        /// </summary>
        /// <param name="chat"></param>
        public Messenger(Chat chat, bool needMembers = true)
            : this(chat.Id, MessengerTypeEnum.Chat, chat.MembersJson, needMembers)
            { }

        /// <summary>
        /// Create messenger based on chat and get current name / Створення месенджера на основі чату і отримання теперішньої назви
        /// </summary>
        /// <param name="chat"></param>
        public Messenger(Chat chat, Profile activeProfile)
            : this(chat)
        {
            var chatMemberNickName = JsonSerializer.Deserialize<Dictionary<string, string>>(chat.Name);
            Name = chatMemberNickName[activeProfile.Id];
        }

        private Messenger(Guid id, MessengerTypeEnum type, string membersJson, bool needMembers)
        { 
            Id = id;
            Type = type;

            if(needMembers)
                Members = JsonSerializer.Deserialize<Dictionary<string, MemberRoleEnum>>(membersJson);
        }
        #endregion
    }
}