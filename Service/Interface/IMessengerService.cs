#region Library
using ChatWithSignal.Domain.Enum;
using ChatWithSignal.Domain.Identity;
using ChatWithSignal.Domain.Messengers;
using ChatWithSignal.Domain.Messengers.Base;
using ChatWithSignal.Domain.Messengers.Components;
using ChatWithSignal.Models.Messenger;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
#endregion

namespace ChatWithSignal.Service.Interface
{
    public interface IMessengerService
    {
        Task SendContentAsync(Profile profile, Messenger messenger, Content content);

        Task CreateGroupAsync(string profileEmail, string groupName, bool isPublic);

        Task<Chat> GetOrCreateChatAsync(string senderEmail, string recipientiId);

        Task<Group> GetOrJoinGroupAsync(string profileEmail, Guid groupId);

        Task<List<Messenger>> GetMessengersAsync(string profileEmail);

        Task<Messenger> GetMessengerAsync(string profileEmail, string messengerId, MessengerTypeEnum messengerType);

        Task<Messenger> GetCurrentMessengerAsync(string profileEmail);

        Task<List<Content>> GetContentsAsync(Messenger messenger, ushort levelLoading);

        Task<ICollection<BaseMessenger>> GetSearchGroupsAsync();

        Task ChangeGroupSettingsAsync(string profileEmail, GroupSettingsViewModel model);
    }
}