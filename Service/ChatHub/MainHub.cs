#region Library
using Microsoft.AspNetCore.SignalR;
using ChatWithSignal.Service.Interface;
using ChatWithSignal.Domain.Enum;
using System;
using System.Threading.Tasks;
using System.Linq;
using ChatWithSignal.Domain.Messengers;
using ChatWithSignal.Domain.Messengers.Components;
using ChatWithSignal.Domain.Identity;
using System.Collections.Generic;
using ChatWithSignal.Service.Server;
#endregion

namespace ChatWithSignal.Service.ChatHub
{
    public class MainHub : Hub
    {
        #region Default
        private readonly IMessengerService _messengerService;
        private readonly IProfileService _profileService;
        public MainHub(IMessengerService messengerService, IProfileService profileService)
        {
            _messengerService = messengerService;
            _profileService = profileService;
        }
        #endregion

        public async Task Load()
        {
            var profile = await getActiveProfileAsync();

            if(profile.CurrentMessengerId != default)
            {
                var messenger = await _messengerService.GetMessengerAsync(Context.User.Identity.Name, profile.CurrentMessengerId.ToString(), profile.CurrentMessengerType);
                await loadingMessengerAsync(messenger, "1");
            }
        }

        public async Task LoadingContent(string levelLoading)
        {
            var messenger = await _messengerService.GetCurrentMessengerAsync(Context.User.Identity.Name);

            await loadingMessengerAsync(messenger, levelLoading);
        }

        public async Task OpenMessenger(string messengerId, string messengerType)
        {
            var profile = await getActiveProfileAsync();
            var messenger = await _messengerService.GetMessengerAsync(Context.User.Identity.Name, messengerId, (MessengerTypeEnum)Convert.ToInt32(messengerType));

            if (messenger != null)
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, profile.CurrentMessengerId.ToString());
                await _profileService.SetCurrentMessagerAsync(profile, messenger);

                await Clients.Caller.SendAsync("Clear");

                await loadingMessengerAsync(messenger, "1");
            }
            else
                await sendCallerAsync(new Content(profile, messenger, "Error"), profile.NickName, profile.Email);
        }

        public async Task SendMessage(string message)
        {
            var profile = await getActiveProfileAsync();
            var messenger = await _messengerService.GetMessengerAsync(Context.User.Identity.Name, profile.CurrentMessengerId.ToString(), profile.CurrentMessengerType);

            var content = new Content(profile, messenger, message);

            string lastDateCreate = messenger.LastDateTimeActive.Split(" ")[0];
            await _messengerService.SendContentAsync(profile, messenger, content);

            if (lastDateCreate != content.DateCreated)
            {
                await sendCallerDateAsync(content.DateCreated.Remove(5));
                await sendOtherDateAsync(profile.CurrentMessengerId.ToString(), content.DateCreated.Remove(5));
            }

            await sendCallerAsync(content, profile.NickName, profile.Email);
            await sendOthersAsync(profile.CurrentMessengerId.ToString(), content, profile.NickName, "");
        }

        #region private
        private async Task<Profile> getActiveProfileAsync()
            => await _profileService.GetByEmailAsync(Context.User.Identity.Name);

        private async Task loadContentAsync(Content content, string nickName, string senderEmail, string levelLoading)
            => await Clients.Caller.SendAsync("Load", content.Message, content.TimeCreated, nickName, Context.User.Identity.Name, senderEmail, levelLoading);

        private async Task sendCallerAsync(Content content, string nickName, string senderEmail)
            => await Clients.Caller.SendAsync("Send", content.Message, content.TimeCreated, nickName, Context.User.Identity.Name, senderEmail);

        private async Task sendOthersAsync(string messengerId, Content content, string nickName, string senderEmail)
            => await Clients.OthersInGroup(messengerId).SendAsync("Send", content.Message, content.TimeCreated, nickName, Context.User.Identity.Name, senderEmail);

        private async Task loadDateAsync(string date)
            => await Clients.Caller.SendAsync("LoadDate", date);

        private async Task sendCallerDateAsync(string date)
            => await Clients.Caller.SendAsync("SendDate", date);

        private async Task sendOtherDateAsync(string messengerId, string date)
            => await Clients.OthersInGroup(messengerId).SendAsync("SendDate", date);

        private async Task loadingMessengerAsync(Messenger messenger, string levelLoading)
        {
            var profiles = await _profileService.GetByIdsAsync(messenger.Members.Keys);
            var profilesNickName = profiles.ToDictionary(x => x.Id, x => x.NickName);
            var profilesEmail = profiles.ToDictionary(x => x.Id, x => x.Email);

            await Groups.AddToGroupAsync(Context.ConnectionId, messenger.Id.ToString());

            var contents = await _messengerService.GetContentsAsync(messenger, Convert.ToUInt16(levelLoading));
            contents.Reverse();

            var datesCreate = contents.Select(x => x.DateCreated).Distinct().ToList();
            var isLastLoading = contents.Count != Config.CountLoadMessageByLevel + 1;

            if (!isLastLoading)
            {
                contents.RemoveAt(Config.CountLoadMessageByLevel);

                if (datesCreate.Count > 1)
                {
                    for (var dateCreateIndex = 0; dateCreateIndex < datesCreate.Count - 1; dateCreateIndex++)
                    {
                        var dateCreate = datesCreate[dateCreateIndex];
                        await loadContentWithManyDate(contents, dateCreate, profilesNickName, profilesEmail, levelLoading);
                    }
                }

                var lastDateCreate = datesCreate.Last();
                var messages = contents.Where(x => x.DateCreated == lastDateCreate).ToList();

                foreach (var message in messages)
                    await loadContentAsync(message, profilesNickName[message.SenderId], profilesEmail[message.SenderId], levelLoading);
            }
            else
            {
                foreach (var dateCreate in datesCreate)
                    await loadContentWithManyDate(contents, dateCreate, profilesNickName, profilesEmail, levelLoading);
            }
        }

        private async Task loadContentWithManyDate(List<Content> contents, string dateCreate, Dictionary<string, string> profilesNickName, Dictionary<string, string> profilesEmail, string levelLoading)
        {
            var messages = contents.Where(x => x.DateCreated == dateCreate).ToList();

            foreach (var message in messages)
                await loadContentAsync(message, profilesNickName[message.SenderId], profilesEmail[message.SenderId], levelLoading);

            if(DateTime.UtcNow.Year.ToString() == dateCreate.Split(".").Last())
                await loadDateAsync(dateCreate.Remove(5));
            else
                await loadDateAsync(dateCreate);
        }
        #endregion
    }
}