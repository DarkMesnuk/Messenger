#region Library
using ChatWithSignal.Domain.Identity;
using ChatWithSignal.Infrastructure.Interface;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
#endregion

namespace ChatWithSignal.Infrastructure.EntityFramework
{
    public class EFProfileRepository : IProfileRepository
    {
        #region Default
        private readonly DatabaseContext _context;

        public EFProfileRepository(DatabaseContext context) 
        {
            _context = context;
        }
        #endregion

        public async Task<ICollection<Profile>> GetAsync()
        {
            return await _context.Profiles.ToListAsync();
        }

        public async Task<Profile> GetByEmailAsync(string profileEmail)
        {
            var profile = await _context.Profiles.FirstOrDefaultAsync(x => x.Email == profileEmail);
            await profile.GetFromJsonAsync();

            return profile;
        }

        public async Task<Profile> GetByIdAsync(string profileId)
        {
            var profile = await _context.Profiles.FirstOrDefaultAsync(x => x.Id == profileId);
            await profile.GetFromJsonAsync();

            return profile;
        }

        public async Task SaveAsync(Profile profile)
        {
            _context.Profiles.Update(profile);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Profile profile)
        {
            _context.Profiles.Remove(profile);
            await _context.SaveChangesAsync();
        }
    }
}