#region Library
using ChatWithSignal.Domain.Enum;
using ChatWithSignal.Domain.Identity;
using ChatWithSignal.Domain.Messengers;
using ChatWithSignal.Domain.Messengers.Components;
using ChatWithSignal.Domain.Search;
using ChatWithSignal.Infrastructure.Interface;
using ChatWithSignal.Models.Messenger;
using ChatWithSignal.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
#endregion

namespace ChatWithSignal.Service
{
    public class MessengerService : IMessengerService
    {
        #region Default
        private readonly IChatsRepository _chatsRepository;
        private readonly IGroupRepository _groupRepository;

        private readonly IProfileService _profileService;
        //private readonly IHubContext<MainHub> _hubContext;

        public MessengerService(IChatsRepository chatsRepository, IGroupRepository groupRepository, IProfileService profileService)
        {
            _chatsRepository = chatsRepository;
            _groupRepository = groupRepository;
            _profileService = profileService;
        }
        #endregion

        #region General
        public async Task<List<Messenger>> GetMessengersAsync(string profileEmail)
        {
            var messengers = new List<Messenger>();
            var profile = await _profileService.GetByEmailAsync(profileEmail);

            messengers.AddRange((await getChatsAsync(profile)).Select(x => new Messenger(x, profile)));
            messengers.AddRange((await getGroupsByProfileAsync(profile)).Select(x => new Messenger(x)));

            return messengers;
        }

        public async Task<Messenger> GetMessengerAsync(string profileEmail, string messengerId, MessengerTypeEnum messengerType)
        {
            switch (messengerType)
            {
                case MessengerTypeEnum.Chat:
                    var profile = await _profileService.GetByEmailAsync(profileEmail);
                    var chat = await getChatAsync(profile, new Guid(messengerId));
                    return new Messenger(chat, profile);

                case MessengerTypeEnum.Group:
                    var group = await GetOrJoinGroupAsync(profileEmail, new Guid(messengerId));
                    return new Messenger(group);

                default:
                    return null;
            }
        }

        public async Task SendContentAsync(Profile profile, Messenger messenger, Content content)
        {
            if (isMember(messenger.Id, profile, messenger.Type))
            {
                await _profileService.SetLastActiveTimeAsync(profile);

                switch (messenger.Type)
                {
                    case MessengerTypeEnum.Chat:
                        await sendChatContentAsync(messenger.Id, content);
                        break;
                    case MessengerTypeEnum.Group:
                        await sendGroupContentAsync(messenger.Id, content);
                        break;
                }
            }
        }

        private bool isMember(Guid messengerId, Profile profile, MessengerTypeEnum messengerType)
            => profile.Messengers.TryGetValue(messengerType, out var chatsId) && chatsId.Contains(messengerId);
        #endregion

        #region Chat
        public async Task<Chat> GetOrCreateChatAsync(string senderEmail, string recipientiId)
        {
            var recipient = await _profileService.GetByIdAsync(recipientiId);
            var sender = await _profileService.GetByEmailAsync(senderEmail);
            var chats = await getChatsAsync(recipient);

            var chat = chats.FirstOrDefault(x => x.Members.Where(x => x.ProfileId == sender.Id).Any());

            if (chat == null || chat == default)
            {;
                chat = new Chat(sender, recipient);

                await _profileService.JoinMessengerAsync(sender, new Messenger(chat));
                await _profileService.JoinMessengerAsync(recipient, new Messenger(chat));

                await _chatsRepository.CreateAsync(chat);
            }

            return chat;
        }

        private async Task sendChatContentAsync(Guid chatId, Content content)
        {
            var chat = await _chatsRepository.GetAsync(chatId);

            await chat.AddContent(content);

            await _chatsRepository.SaveAsync(chat);
        }

        private async Task<ICollection<Chat>> getChatsAsync(Profile profile)
        {
            var profileChats = new List<Chat>();

            if (!profile.Messengers.TryGetValue(MessengerTypeEnum.Chat, out var chatsId))
                return profileChats;

            foreach (var chatId in chatsId.ToList())
            {
                try
                {
                    profileChats.Add(await _chatsRepository.GetAsync(chatId));
                }
                catch
                {
                    await _profileService.LeaveMessengerAsync(profile, new Messenger(chatId, MessengerTypeEnum.Chat));
                }
            }

            return profileChats;
        }

        private async Task<Chat> getChatAsync(Profile profile, Guid messengerId)
        {
            var isMemberGroup = isMember(messengerId, profile, MessengerTypeEnum.Chat);

            if (isMemberGroup)
                return await _chatsRepository.GetAsync(messengerId);

            return null;
        }
        #endregion

        #region Group
        public async Task<ICollection<SGroup>> GetSGroupsAsync()
        {
            var groups = await _groupRepository.GetAsync();
            var sGroups = new List<SGroup>();

            foreach (var group in groups)
                sGroups.Add(new SGroup(group));

            return sGroups;
        }

        public async Task<Group> GetOrJoinGroupAsync(string profileEmail, Guid groupId)
        {
            var profile = await _profileService.GetByEmailAsync(profileEmail);
            var group = await _groupRepository.GetAsync(groupId);
            var isMemberGroup = isMember(groupId, profile, MessengerTypeEnum.Group);

            if (group != null && (group.IsPublic || isMemberGroup))
            {
                if (!isMemberGroup)
                {
                    await _profileService.JoinMessengerAsync(profile, new Messenger(group));
                    await group.AddMember(new Member(profile.Id));
                    await SaveGroupAsync(group);
                }

                return group;
            }

            return null;
        }

        public async Task CreateGroupAsync(string profileEmail, string groupName, bool isPublic)
        {
            var owner = await _profileService.GetByEmailAsync(profileEmail);
            var group = new Group(groupName, isPublic, owner);

            var messenger = new Messenger(group);

            await _groupRepository.CreateAsync(group);

            await _profileService.JoinMessengerAsync(owner, messenger);
        }

        public async Task ChangeGroupSettingsAsync(string profileEmail, GroupSettingsViewModel model)
        {
            var group = await GetOrJoinGroupAsync(profileEmail, model.GroupId);

            var groupName = string.IsNullOrEmpty(model.Name) ? group.Name : model.Name;
            var groupText = model.Text == null ? group.Text : model.Text;
            var groupIsPublic = model.IsPublic;

            await group.ChangeSetting(groupName, groupText, groupIsPublic);

            await SaveGroupAsync(group);
        }

        public async Task SaveGroupAsync(Group group)
        {
            await _groupRepository.SaveAsync(group);
        }

        private async Task sendGroupContentAsync(Guid groupId, Content content)
        {
            var group = await _groupRepository.GetAsync(groupId);

            await group.AddContent(content);

            await _groupRepository.SaveAsync(group);
        }

        private async Task<ICollection<Group>> getGroupsByProfileAsync(Profile profile)
        {
            var profileGroups = new List<Group>();

            if (!profile.Messengers.TryGetValue(MessengerTypeEnum.Group, out var groupsId))
                return profileGroups;

            foreach (var groupid in groupsId)
            {
                var group = await _groupRepository.GetAsync(groupid);

                if (group == null)
                    await _profileService.LeaveMessengerAsync(profile, new Messenger(groupid, MessengerTypeEnum.Group));
                else
                    profileGroups.Add(group);
            }

            return profileGroups;
        }
        #endregion
    }
}