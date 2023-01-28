#region Library
using ChatWithSignal.Domain.Messengers;
using ChatWithSignal.Domain.Messengers.Components;
using ChatWithSignal.Infrastructure.Interface;
using ChatWithSignal.Service.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;
#endregion

namespace ChatWithSignal.Service
{
    public class ContentServise : IContentServise
    {
        #region Default
        private readonly IContentRepository _contentRepository;

        public ContentServise(IContentRepository contentRepository)
        {
            _contentRepository = contentRepository;
        }
        #endregion

        public async Task<Content> CreateAsync(Content content)
        {
            await _contentRepository.AddAsync(content);
            return content;
        }

        public async Task<List<Content>> GetAll(Messenger messenger)
        {
            return await _contentRepository.GetAllAsync(messenger.Id);
        }
    }
}
