using AuthenticationService.Domain;
using AuthenticationService.Persistence.Contexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace AuthenticationService.Persistence.Repositories
{
    public class RoleRepository : RoleStore<Role, ApplicationDbContext, Guid>
    {
        public RoleRepository(ApplicationDbContext context, IdentityErrorDescriber describer = null) : base(context, describer)
        {
        }

        //#TODO
    }
}
