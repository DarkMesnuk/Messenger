#region Library
using System.Collections.Generic;
using System.Threading.Tasks;
using ChatWithSignal.Domain.Identity;
#endregion

namespace ChatWithSignal.Infrastructure.Interface
{
    public interface IProfileRepository
    {
        Task<ICollection<Profile>> GetAsync();

        Task<Profile> GetByEmailAsync(string profileEmail);

        Task<Profile> GetByIdAsync(string profileId);

        Task SaveAsync(Profile profile);

        Task DeleteAsync(Profile profile);
    }
}