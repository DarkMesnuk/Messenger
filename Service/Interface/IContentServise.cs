#region Library
using ChatWithSignal.Domain.Messengers;
using ChatWithSignal.Domain.Messengers.Components;
using System.Collections.Generic;
using System.Threading.Tasks;
#endregion

namespace ChatWithSignal.Service.Interface
{
    public interface IContentServise
    {
        Task<Content> CreateAsync(Content content);
        Task<List<Content>> GetAllAsync(Messenger messenger, ushort levelLoading);
    }
}