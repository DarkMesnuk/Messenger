#region Library
using ChatWithSignal.Domain.Enum;
using ChatWithSignal.Domain.Messengers;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Threading.Tasks;
#endregion

namespace ChatWithSignal.Domain.Identity
{
    public class Profile : IdentityUser
    {
        #region Public 
        /// <summary>
        /// Nickname / Нік
        /// </summary>
        [Display(Name = "Нік")]
        public string NickName { get; set; }

        /// <summary>
        /// Last time when profile was active / Останній час коли профіль був активний
        /// </summary>
        public DateTime LastActiveTime { get; private set; }
        #endregion

        #region Messenger
        /// <summary>
        /// Messenger Id now active by profile / Id месенджера що тепер відкритий користувачем
        /// </summary>
        public Guid CurrentMessengerId { get; private set; }

        /// <summary>
        /// Messenger type now active by profile / Тип месенджера що тепер відкритий користувачем
        /// </summary>
        public MessengerTypeEnum CurrentMessengerType { get; private set; }

        /// <summary>
        /// Messengers in which the user is present Json / Месенджери у яких користувач присутній Json
        /// </summary>
        public string MessengersJson { get; private set; }

        /// <summary>
        /// Messengers in which the user is present / Месенджери у яких користувач присутній
        /// </summary>
        [NotMapped]
        public Dictionary<MessengerTypeEnum, ICollection<Guid>> Messengers { get; private set; }
        #endregion

        #region Create Profile
        /// <summary>
        /// Default / За замовчуванням
        /// </summary>
        public Profile() { }

        public Profile(string nickName, string eMail)
        {
            NickName = nickName;
            Email = eMail;
            UserName = eMail;
            MessengersJson = JsonSerializer.Serialize(new Dictionary<MessengerTypeEnum, ICollection<Guid>>());
        }
        #endregion

        #region Processing

        #region Profile additional information
        /// <summary>
        /// Set information from json / Встановлення інформації із json
        /// </summary>
        /// <returns></returns>
        public Task GetFromJsonAsync()
        {
            Messengers = JsonSerializer.Deserialize<Dictionary<MessengerTypeEnum, ICollection<Guid>>>(MessengersJson);

            return Task.CompletedTask;
        }

        /// <summary>
        /// Set last time when profile was active / Встановлення останнього часу коли профіль був активний
        /// </summary>
        /// <returns></returns>
        public Task SetLastActiveTime()
        {
            LastActiveTime = DateTime.UtcNow;

            return Task.CompletedTask;
        }
        #endregion

        #region Messenger
        /// <summary>
        /// Join to messenger and save to json / Вступ до месенджера і зберігання у json
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public Task JoinMessenger(Messenger messenger)
        {
            Messengers[messenger.Type].Add(messenger.Id);
            MessengersJson = JsonSerializer.Serialize(Messengers);

            return Task.CompletedTask;
        }

        /// <summary>
        /// Leave from messenger and save to json / Вихід з месенджера і зберігання у json
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public Task LeaveMessenger(Messenger messenger)
        {
            Messengers[messenger.Type].Remove(messenger.Id);
            MessengersJson = JsonSerializer.Serialize(Messengers);

            return Task.CompletedTask;
        }

        /// <summary>
        /// Set messenger now active on profile / Встановлення месенджера що тепер відкритий користувачем
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public Task SetCurrentMessager(Messenger messager)
        {
            CurrentMessengerType = messager.Type;
            CurrentMessengerId = messager.Id;

            return Task.CompletedTask;
        }
        #endregion

        #endregion
    }
}