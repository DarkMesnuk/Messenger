#region Library
using ChatWithSignal.Domain.Messengers;
using ChatWithSignal.Infrastructure.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
#endregion

namespace ChatWithSignal.Infrastructure.EntityFramework
{
    public class EFChatsRepository : IChatsRepository
    {
        #region Default
        private readonly DatabaseContext _context;

        public EFChatsRepository(DatabaseContext context)
        {
            _context = context;
        }
        #endregion

        public async Task<Chat> GetAsync(Guid chatId)
        {
            var chat = await _context.Chats.FirstOrDefaultAsync(x => x.Id == chatId);
            await chat.GetFromJsonAsync();

            return chat;
        }

        public async Task AddAsync(Chat chat)
        {
            _context.Chats.Add(chat);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveAsync(Chat chat)
        {
            _context.Chats.Remove(chat);
            await _context.SaveChangesAsync();
        }

        public async Task SaveAsync(Chat chat)
        {
            _context.Chats.Update(chat);
            await _context.SaveChangesAsync();
        }
    }
}