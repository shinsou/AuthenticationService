using AuthenticationService.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationService.Application.Repositories
{
    public interface IAuditRepository<T> where T : IEntity<Guid>
    {
        Task StoreUserAudit(T userAudit);
    }
}
