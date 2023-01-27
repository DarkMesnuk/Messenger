#region Library
using System;
using System.Collections.Generic;
using System.Linq;
#endregion

namespace ChatWithSignal.Domain.Messengers.Connections
{
    public class MessengerMembers
    {
        /// <summary>
        /// Messenger Id
        /// </summary>
        public Guid MessengerId { get; set; }

        /// <summary>
        /// Members Id
        /// </summary>
        public ICollection<Guid> MembersId { get; set; }

        #region Constructors
        public MessengerMembers()
        { }

        public MessengerMembers(Chat chat)
        {
            MessengerId = chat.Id;
            MembersId = chat.Members.Select(x => x.Id).ToList();
        }
        public MessengerMembers(Group group)
        {
            MessengerId = group.Id;
            MembersId = group.Members.Select(x => x.Id).ToList();
        }
        #endregion
    }
}