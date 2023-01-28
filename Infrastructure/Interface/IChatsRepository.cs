#region Library
using ChatWithSignal.Domain.Messengers;
using System;
using System.Threading.Tasks;
#endregion

namespace ChatWithSignal.Infrastructure.Interface
{
    public interface IChatsRepository
    {
        Task<Chat> GetAsync(Guid chatId);

        Task AddAsync(Chat chat);
        
        Task RemoveAsync(Chat chat);

        Task SaveAsync(Chat chat);
    }
}