#region Library
using ChatWithSignal.Domain.Enum;
using ChatWithSignal.Domain.Identity;
using ChatWithSignal.Domain.Messengers.Base;
using ChatWithSignal.Domain.Messengers.Components;
using System;
using System.Collections.Generic;
using System.Text.Json;
#endregion

namespace ChatWithSignal.Domain.Messengers
{
    public class Chat : BMessenger
    {
        #region Constructors
        /// <summary>
        /// Default / За замовчуванням
        /// </summary>
        public Chat() { }

        /// <summary>
        /// Create chat from sender(active profile) to recipient / Створення чату від відправника (активного профілю) до отримувача
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="recipient"></param>
        public Chat(Profile sender, Profile recipient)
            : base(Guid.NewGuid(),
                  new List<Content> { new Content(sender.Id, "Ви створили чат!") },
                  new List<Member> { new Member(sender.Id, MemberRoleEnum.Owner), new Member(recipient.Id, MemberRoleEnum.Owner) })
        {
            Name = JsonSerializer.Serialize(new Dictionary<string, string> { 
                { sender.Id, recipient.NickName },
                { recipient.Id, sender.NickName} 
            });
            ContentsJson = JsonSerializer.Serialize(Contents);
            MembersJson = JsonSerializer.Serialize(Members);
        }
        #endregion
    }
}