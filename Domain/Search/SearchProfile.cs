#region Library
using System.ComponentModel.DataAnnotations.Schema;
using ChatWithSignal.Domain.Identity;
#endregion

namespace ChatWithSignal.Domain.Search
{
    [NotMapped]
    /// <summary>
    /// Profile for search / Профіль для пошуку
    /// </summary>
    public class SearchProfile
    {
        /// <summary>
        /// Id
        /// </summary>
        public string Id { get; private set; }

        /// <summary>
        /// Nickname / Нік
        /// </summary>
        public string NickName { get; private set; }

        #region Constructors
        /// <summary>
        /// Create profile for search by profile / Створення профілю для пошуку за профілем
        /// </summary>
        /// <param name="profile"></param>
        public SearchProfile(Profile profile)
        {
            Id = profile.Id;
            NickName = profile.NickName;
        }
        #endregion
    }
}