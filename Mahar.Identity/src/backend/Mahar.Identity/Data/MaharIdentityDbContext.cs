using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace Mahar.Identity.Data
{
    public class MaharIdentityDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        public MaharIdentityDbContext(DbContextOptions<MaharIdentityDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Additional model configuration can be done here
        }
    }
}