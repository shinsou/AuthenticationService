using AuthenticationService.Domain;
using AuthenticationService.Persistence.Contexts;
using IdentityServer4.EntityFramework.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace AuthenticationService.Persistence.Repositories
{
    public class UserRepository : UserStore<User, Role, ApplicationDbContext, Guid>
    {
        public UserRepository(ApplicationDbContext context, IdentityErrorDescriber describer = null) : base(context, describer)
        {
        }
        //#TODO
    }
}
