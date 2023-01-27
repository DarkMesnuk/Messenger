#region Library
using ChatWithSignal.Domain.Messengers.Components;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Threading.Tasks;
#endregion

namespace ChatWithSignal.Domain.Messengers.Base
{
    public abstract class BMessenger
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
        /// Contents in Json / Зміст у Json
        /// </summary>
        public string ContentsJson { get; protected set; }

        /// <summary>
        /// Contents / Зміст
        /// </summary>
        [NotMapped]
        public virtual ICollection<Content> Contents { get; private set; }

        /// <summary>
        /// Members / Учасники
        /// </summary>
        [NotMapped]
        public virtual ICollection<Member> Members { get; private set; }

        #region Constructors
        /// <summary>
        /// Default / За замовчуванням
        /// </summary>
        protected BMessenger() { }

        /// <summary>
        /// Create Bmessenger with all parameters / Створення Бмесенджера з усіма параметрами
        /// </summary>
        /// <param name="id"></param>
        /// <param name="contents"></param>
        /// <param name="members"></param>
        protected BMessenger(Guid id, ICollection<Content> contents, ICollection<Member> members)
        {
            Id = id;
            Contents = contents;
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
            Contents = JsonSerializer.Deserialize<List<Content>>(ContentsJson);
            Members = JsonSerializer.Deserialize<List<Member>>(MembersJson);

            return Task.CompletedTask;
        }

        /// <summary>
        /// Add content and save to json / Додавання змісту і зберігання у json
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public Task AddContent(Content content)
        {
            Contents.Add(content);
            ContentsJson = JsonSerializer.Serialize(Contents);

            return Task.CompletedTask;
        }
        #endregion
    }
}