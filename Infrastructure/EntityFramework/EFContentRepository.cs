#region Library
using ChatWithSignal.Domain.Messengers.Components;
using ChatWithSignal.Infrastructure.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
#endregion

namespace ChatWithSignal.Infrastructure.EntityFramework
{
    public class EFContentRepository : IContentRepository
    {
        #region Default
        private readonly DatabaseContext _context;

        public EFContentRepository(DatabaseContext context)
        {
            _context = context;
        }
        #endregion

        public async Task<Content> GetAsync(Guid contentId)
        {
            return await _context.Contents.FirstOrDefaultAsync(x => x.Id == contentId);
        }

        public async Task AddAsync(Content content)
        {
            _context.Contents.Add(content);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveAsync(Content content)
        {
            _context.Contents.Remove(content);
            await _context.SaveChangesAsync();
        }

        public async Task SaveAsync(Content content)
        {
            _context.Contents.Update(content);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Content>> GetAllAsync(Guid messengerId)
        {
            return await _context.Contents.Where(x => x.MessengerId == messengerId).ToListAsync();
        }
    }
}