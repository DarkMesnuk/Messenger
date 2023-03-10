#region Library
using ChatWithSignal.Domain.Messengers;
using ChatWithSignal.Infrastructure.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
#endregion

namespace ChatWithSignal.Infrastructure.EntityFramework
{
    public class EFGroupRepository : IGroupRepository
    {
        #region Default
        private readonly DatabaseContext _context;

        public EFGroupRepository(DatabaseContext context)
        {
            _context = context;
        }
        #endregion

        public async Task<ICollection<Group>> GetAsync()
        {
            return await _context.Groups.Where(x => x.IsPublic).ToListAsync();
        }

        public async Task<Group> GetAsync(Guid groupId)
        {
            return await _context.Groups.FirstOrDefaultAsync(x => x.Id == groupId);
        }

        public async Task AddAsync(Group group)
        {
            _context.Groups.Add(group);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveAsync(Group group)
        {
            _context.Groups.Remove(group);
            await _context.SaveChangesAsync();
        }

        public async Task SaveAsync(Group group)
        {
            _context.Groups.Update(group);
            await _context.SaveChangesAsync();
        }
    }
}