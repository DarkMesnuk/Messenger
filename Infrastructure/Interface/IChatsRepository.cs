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

        Task CreateAsync(Chat chat);
        
        Task DeleteAsync(Chat chat);

        Task SaveAsync(Chat chat);
    }
}