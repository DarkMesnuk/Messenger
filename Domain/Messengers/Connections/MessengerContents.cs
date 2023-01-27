#region Library
using ChatWithSignal.Domain.Messengers.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
#endregion

namespace ChatWithSignal.Domain.Messengers.Connections
{
    public class MessengerContents
    {
        /// <summary>
        /// Messenger Id
        /// </summary>
        public Guid MessengerId { get; set; }

        /// <summary>
        /// Contents Id
        /// </summary>
        public ICollection<Guid> ContentsId { get; set; }

        #region Constructors
        public MessengerContents()
        {}

        public MessengerContents(Chat chat)
        {
            MessengerId = chat.Id;
            ContentsId = chat.Contents.Select(x => x.Id).ToList();
        }
        public MessengerContents(Group group)
        {
            MessengerId = group.Id;
            ContentsId = group.Contents.Select(x => x.Id).ToList();
        }
        #endregion
    }
}