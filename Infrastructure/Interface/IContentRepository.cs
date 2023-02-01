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
        Task<List<Content>> GetAllAsync(Messenger messenger, ushort levelLoading);
        Task<Content> GetAsync(Guid contentId);
        Task AddAsync(Content content);
        Task RemoveAsync(Content content);
        Task SaveAsync(Content content);
    }
}