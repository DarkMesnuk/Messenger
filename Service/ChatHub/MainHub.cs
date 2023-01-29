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
using ChatWithSignal.Domain.Search;
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
            var profile = await activeProfile();

            if(profile.CurrentMessengerId != default)
            {
                var messenger = await _messengerService.GetMessengerAsync(Context.User.Identity.Name, profile.CurrentMessengerId.ToString(), profile.CurrentMessengerType);
                await loadingMessenger(messenger, "1");
            }
        }

        public async Task LoadingContent(string levelLoading)
        {
            var profile = await activeProfile();
            var messenger = await _messengerService.GetMessengerAsync(Context.User.Identity.Name, profile.CurrentMessengerId.ToString(), profile.CurrentMessengerType);

            await loadingMessenger(messenger, levelLoading);
        }

        public async Task OpenMessenger(string messengerId, string messengerType)
        {
            var profile = await activeProfile();
            var messenger = await _messengerService.GetMessengerAsync(Context.User.Identity.Name, messengerId, (MessengerTypeEnum)Convert.ToInt32(messengerType));

            if (messenger != null)
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, profile.CurrentMessengerId.ToString());
                await _profileService.SetCurrentMessagerAsync(profile, messenger);

                await Clients.Caller.SendAsync("Clear");

                await loadingMessenger(messenger, "1");
            }
            else
                await sendCallerAsync("Send", "Error", DateTime.UtcNow.ToShortTimeString(), profile.NickName, profile.Email);
        }

        public async Task SendMessage(string message)
        {
            var profile = await activeProfile();
            var messenger = await _messengerService.GetMessengerAsync(Context.User.Identity.Name, profile.CurrentMessengerId.ToString(), profile.CurrentMessengerType);
            var content = new Content(profile, messenger, message);

            await _messengerService.SendContentAsync(profile, messenger, content);

            await sendCallerAsync("Send", content.Message, content.DateTimeCreated.Split(' ')[1], profile.NickName, profile.Email);
            await sendOthersInGroupAsync(profile.CurrentMessengerId.ToString(), "Send", content.Message, content.DateTimeCreated.Split(' ')[1], profile.NickName, "");
        }

        #region private
        private async Task<Profile> activeProfile()
            => await _profileService.GetByEmailAsync(Context.User.Identity.Name);

        private async Task loadContentAsync(string method, string message, string time, string nickName, string senderEmail, string levelLoading)
            => await Clients.Caller.SendAsync(method, message, time.Remove(5), nickName, Context.User.Identity.Name, senderEmail, levelLoading);

        private async Task sendCallerAsync(string method, string message, string time, string nickName, string senderEmail)
            => await Clients.Caller.SendAsync(method, message, time.Remove(5), nickName, Context.User.Identity.Name, senderEmail);

        private async Task sendOthersInGroupAsync(string messengerId, string method, string message, string time, string nickName, string senderEmail)
            => await Clients.OthersInGroup(messengerId).SendAsync(method, message, time.Remove(5), nickName, Context.User.Identity.Name, senderEmail);

        private async Task loadingMessenger(Messenger messenger, string levelLoading)
        {
            var profiles = await _profileService.GetByIdsAsync(messenger.Members.Keys);
            var profilesNickName = profiles.ToDictionary(x => x.Id, x => x.NickName);
            var profilesEmail = profiles.ToDictionary(x => x.Id, x => x.Email);

            await Groups.AddToGroupAsync(Context.ConnectionId, messenger.Id.ToString());

            var contents = await _messengerService.GetContentsAsync(messenger, Convert.ToUInt16(levelLoading));
            contents.Reverse();

            foreach (var message in contents)
                await loadContentAsync("Load", message.Message, message.DateTimeCreated.Split(' ')[1], profilesNickName[message.SenderId], profilesEmail[message.SenderId], levelLoading);
        }
        #endregion
    }
}