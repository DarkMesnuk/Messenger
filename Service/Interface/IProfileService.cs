#region Library
using ChatWithSignal.Domain.Identity;
using ChatWithSignal.Domain.Messengers;
using ChatWithSignal.Domain.Search;
using System.Collections.Generic;
using System.Threading.Tasks;
#endregion

namespace ChatWithSignal.Service.Interface
{
    public interface IProfileService
    {
        Task<Profile> GetByEmailAsync(string profileEmail);

        Task<Profile> GetByIdAsync(string profileId);

        Task<List<Profile>> GetByIdsAsync(IEnumerable<string> profilesId);

        Task<ICollection<SProfile>> GetSAsync();

        Task JoinMessengerAsync(Profile profile, Messenger messenger);

        Task LeaveMessengerAsync(Profile profile, Messenger messenger);

        Task SetLastActiveTimeAsync(Profile profile);

        Task SetCurrentMessagerAsync(Profile profile, Messenger messager);

        Task SaveAsync(Profile profile);
    }
}