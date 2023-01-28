#region Library
using ChatWithSignal.Domain.Identity;
using ChatWithSignal.Domain.Messengers;
using ChatWithSignal.Domain.Messengers.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
#endregion

namespace ChatWithSignal.Infrastructure
{
    public class DatabaseContext : IdentityDbContext<IdentityUser>
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options)
        { }

        #region DbSet
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<Chat> Chats { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Content> Contents { get; set; }
        #endregion

        protected override void OnModelCreating(ModelBuilder modelbuilder) => base.OnModelCreating(modelbuilder);
    }
}