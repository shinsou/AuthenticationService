using AuthenticationService.Application.Repositories;
using AuthenticationService.Domain;
using AuthenticationService.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationService.Persistence.Repositories
{
    public class AuditRepository : IAuditRepository<UserAudit>
    {
        private DbContextOptions<ApplicationDbContext> Options { get; }

        public AuditRepository(DbContextOptions<ApplicationDbContext> options)
            => this.Options = options;

        public async Task StoreUserAudit(UserAudit audit)
        {
            //using (var context = new ApplicationDbContext(this.Options, this.AppOptions))
            using (var context = new ApplicationDbContext(this.Options))
            {
                await context.UserAuditEvents.AddAsync(audit);
                await context.SaveChangesAsync();
            }
        }
    }
}
