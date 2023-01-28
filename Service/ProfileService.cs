#region Library
using ChatWithSignal.Domain.Identity;
using ChatWithSignal.Domain.Messengers;
using ChatWithSignal.Domain.Search;
using ChatWithSignal.Infrastructure.Interface;
using ChatWithSignal.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
#endregion

namespace ChatWithSignal.Service
{
    public class ProfileService : IProfileService
    {
        #region Default
        private readonly IProfileRepository _profileRepository;

        public ProfileService(IProfileRepository profileRepository)
        {
            _profileRepository = profileRepository;
        }
        #endregion

        public async Task<Profile> GetByEmailAsync(string profileEmail)
        {
            return await _profileRepository.GetByEmailAsync(profileEmail);
        }

        public async Task<Profile> GetByIdAsync(string profileId)
        {
            return await _profileRepository.GetByIdAsync(profileId);
        }

        public async Task<List<Profile>> GetByIdsAsync(IEnumerable<string> profilesId)
        {
            var profiles = new List<Profile>();

            foreach (var profileId in profilesId)
                profiles.Add(await GetByIdAsync(profileId));

            return profiles;
        }

        public async Task<ICollection<SearchProfile>> GetSAsync()
        {
            var profiles = await _profileRepository.GetAsync();
            var sprofiles = new List<SearchProfile>();

            foreach (var profile in profiles)
                sprofiles.Add(new SearchProfile(profile));

            return sprofiles;
        }

        public async Task JoinMessengerAsync(Profile profile, Messenger messenger)
        {
            if (!profile.Messengers.TryGetValue(messenger.Type, out var messengersId))
                profile.Messengers.Add(messenger.Type, new List<Guid>());

            if (profile.Messengers[messenger.Type].FirstOrDefault(x => x == messenger.Id) == default)
            {
                await SetLastActiveTimeAsync(profile);
                await profile.JoinMessenger(messenger);
                await _profileRepository.SaveAsync(profile);
            }
        }

        public async Task LeaveMessengerAsync(Profile profile, Messenger messenger)
        {
            if (profile.Messengers.TryGetValue(messenger.Type, out var messengersId) && messengersId.FirstOrDefault(x => x == messenger.Id) != default)
            {
                await profile.LeaveMessenger(messenger);
                await _profileRepository.SaveAsync(profile);
            }
        }

        public async Task SetLastActiveTimeAsync(Profile profile)
        {
            await profile.SetLastActiveTime();
            await _profileRepository.SaveAsync(profile);
        }

        public async Task SetCurrentMessagerAsync(Profile profile, Messenger messeger)
        {
            await profile.SetCurrentMessager(messeger);
            await SetLastActiveTimeAsync(profile);
            await _profileRepository.SaveAsync(profile);
        }

        public async Task SaveAsync(Profile profile)
        {
            await _profileRepository.SaveAsync(profile);
        }
    }
}
