#region Library
using System;
using System.ComponentModel.DataAnnotations.Schema;
using ChatWithSignal.Domain.Messengers;
#endregion

namespace ChatWithSignal.Domain.Search
{
    [NotMapped]
    /// <summary>
    /// Group for search / Група для пошуку
    /// </summary>
    public class SearchGroup
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; private set; }

        /// <summary>
        /// Name / Назва
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Members Count / Кількість учасників
        /// </summary>
        public int MembersCount { get; private set; }

        #region Constructors
        /// <summary>
        /// Create group for search by group / Створення групи для пошуку за групою
        /// </summary>
        /// <param name="group"></param>
        public SearchGroup(Group group)
        {
            Id = group.Id;
            Name = group.Name;
            MembersCount = group.Members.Count;
        }
        #endregion
    }
}