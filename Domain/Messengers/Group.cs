#region Library
using ChatWithSignal.Domain.Enum;
using ChatWithSignal.Domain.Identity;
using ChatWithSignal.Domain.Messengers.Base;
using ChatWithSignal.Domain.Messengers.Components;
using ChatWithSignal.Models.Messenger;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
#endregion

namespace ChatWithSignal.Domain.Messengers
{
    public class Group : BaseRepositoryMessenger
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
                new Dictionary<string, MemberRoleEnum> { {owner.Id, MemberRoleEnum.Owner }}
            )
        {
            Name = groupName;
            IsPublic = isPublic;
            OwnerId = owner.Id;
            Text = "";
        }
        #endregion

        #region Processing
        /// <summary>
        /// Change group settings / Змінна налаштувань групи
        /// </summary>
        /// <param name="name"></param>
        /// <param name="text"></param>
        /// <param name="isPublic"></param>
        /// <returns></returns>
        public Task ChangeSetting(GroupSettingsViewModel model)
        {
            Name = string.IsNullOrEmpty(model.Name) ? Name : model.Name;
            Text = model.Text == null ? Text : model.Text;
            IsPublic = model.IsPublic;

            return Task.CompletedTask;
        }

        public Task AddMember(Profile profile)
        {
            var members = JsonSerializer.Deserialize<Dictionary<string, MemberRoleEnum>>(MembersJson);
            members.Add(profile.Id, MemberRoleEnum.User);
            MembersJson = JsonSerializer.Serialize(members);

            return Task.CompletedTask;
        }
        #endregion
    }
}