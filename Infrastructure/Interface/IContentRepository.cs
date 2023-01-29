#region Library
using ChatWithSignal.Domain.Messengers;
using ChatWithSignal.Domain.Messengers.Components;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
#endregion

namespace ChatWithSignal.Infrastructure.Interface
{
    public interface IContentRepository
    {
        Task AddAsync(Content content);
        Task RemoveAsync(Content content);
        Task<Content> GetAsync(Guid contentId);
        Task SaveAsync(Content content);
        Task<List<Content>> GetAllAsync(Messenger messenger, ushort levelLoading);
    }
}