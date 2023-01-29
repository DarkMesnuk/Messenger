#region Library
using ChatWithSignal.Domain.Messengers;
using ChatWithSignal.Domain.Messengers.Components;
using ChatWithSignal.Infrastructure.Interface;
using ChatWithSignal.Service.Server;
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

        public async Task<List<Content>> GetAllAsync(Messenger messenger, ushort levelLoading)
        {
            var countLoadMessageByLevel = Config.CountLoadMessageByLevel;

            if (levelLoading * countLoadMessageByLevel >= messenger.ContentCount)
            {
                if (levelLoading * countLoadMessageByLevel <= messenger.ContentCount + countLoadMessageByLevel)
                    return await _context.Contents.Where(x => x.MessengerType == messenger.Type).Where(x => x.MessengerId == messenger.Id).Take((int)(messenger.ContentCount - (countLoadMessageByLevel * (levelLoading - 1)))).ToListAsync();
                else    
                    return new List<Content>();
            }

            return await _context.Contents.Where(x => x.MessengerType == messenger.Type).Where(x => x.MessengerId == messenger.Id).Skip((int)(messenger.ContentCount - (countLoadMessageByLevel * levelLoading))).Take(countLoadMessageByLevel).ToListAsync();
        }

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

    }
}