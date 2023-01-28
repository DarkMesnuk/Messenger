#region Library
using ChatWithSignal.Domain.Messengers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
#endregion

namespace ChatWithSignal.Infrastructure.Interface
{
    public interface IGroupRepository
    {
        Task<ICollection<Group>> GetAsync();

        Task<Group> GetAsync(Guid groupId);

        Task AddAsync(Group group);

        Task RemoveAsync(Group group);

        Task SaveAsync(Group group);
    }
}