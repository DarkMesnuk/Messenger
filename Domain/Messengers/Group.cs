#region Library
using ChatWithSignal.Domain.Enum;
using ChatWithSignal.Domain.Identity;
using ChatWithSignal.Domain.Messengers.Base;
using ChatWithSignal.Domain.Messengers.Components;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
#endregion

namespace ChatWithSignal.Domain.Messengers
{
    public class Group : BaseMessenger
    {
        /// <summary>
        /// Description / Опис
        /// </summary>
        public string Text { get; private set; }

        /// <summary>
        /// Is public? / Це публічна?
        /// </summary>
        public bool IsPublic { get; private set; }

        /// <summary>
        /// Owner Id / Id власника
        /// </summary>
        public string OwnerId { get; private set; }

        #region Constructors
        /// <summary>
        /// Default / За замовчуванням
        /// </summary>
        public Group() { }

        /// <summary>
        /// Create group by name group, isPublic and owner(active profile) / Створення групи за назвою групи, чи публічна та власником(активного профілю)
        /// <param name="groupName"></param>
        /// <param name="isPublic"></param>
        /// <param name="owner"></param>
        public Group(string groupName, bool isPublic, Profile owner)
           : base(
                Guid.NewGuid(),
                new Dictionary<string, MemberRoleEnum> { {owner.Id, MemberRoleEnum.Owner }}
            )
        {
            Name = groupName;
            IsPublic = isPublic;
            OwnerId = owner.Id;
            Text = "";
            MembersJson = JsonSerializer.Serialize(Members);
        }
        #endregion

        #region Processing
        /// <summary>
        /// Add member and save to json / Додавання учасника і зберігання у json
        /// </summary>
        /// <param name="member"></param>
        /// <returns></returns>
        public Task AddMember(Profile profile)
        {
            Members.Add(profile.Id, MemberRoleEnum.User);
            setMembersJson();

            return Task.CompletedTask;
        }

        /// <summary>
        /// Change group settings / Змінна налаштувань групи
        /// </summary>
        /// <param name="name"></param>
        /// <param name="text"></param>
        /// <param name="isPublic"></param>
        /// <returns></returns>
        public Task ChangeSetting(string name, string text, bool isPublic)
        {
            Name = name;
            Text = text;
            IsPublic = isPublic;

            return Task.CompletedTask;
        }

        private void setMembersJson() 
            => MembersJson = JsonSerializer.Serialize(Members);
        #endregion
    }
}