#region Library
using ChatWithSignal.Domain.Enum;
using System;
using System.ComponentModel.DataAnnotations.Schema;
#endregion

namespace ChatWithSignal.Domain.Messengers.Components
{
    [NotMapped]
    public class Member
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Profile Id
        /// </summary>
        public string ProfileId { get; set; }

        /// <summary>
        /// Profile Role
        /// </summary>
        public MemberRoleEnum ProfileRole { get; set; }

        #region Constructors
        /// <summary>
        /// Creating a participant by profile id and the profile role in the messenger / Створення учасника за Id профілю та роллю профілю у месенджері
        /// </summary>
        /// <param name="profileId"></param>
        /// <param name="profileRole"></param>
        public Member(string profileId, MemberRoleEnum profileRole = MemberRoleEnum.User)
        {
            ProfileId = profileId;
            ProfileRole = profileRole;
        }
        #endregion
    }
}