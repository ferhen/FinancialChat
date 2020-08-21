using FinancialChat.Models;
using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinancialChat.Data
{
    public class ApplicationDbContext : ApiAuthorizationDbContext<ApplicationUser>
    {
        public ApplicationDbContext(
            DbContextOptions options,
            IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options, operationalStoreOptions)
        {
        }

        public DbSet<Chatroom> Chatrooms { get; set; }
        public DbSet<Message> Messages { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Primary Keys
            builder.Entity<Chatroom>()
                .HasKey(p => p.Id);
            builder.Entity<Message>()
                .HasKey(p => p.Id);

            // Foreign Keys
            builder.Entity<Message>()
                .HasOne(p => p.Chatroom)
                .WithMany(p => p.Messages)
                .HasForeignKey(p => p.ChatroomId)
                .OnDelete(DeleteBehavior.Cascade);

            // Constraints
            builder.Entity<Chatroom>()
                .HasIndex(p => p.Name)
                .IsUnique();
                
            base.OnModelCreating(builder);
        }
    }
}
