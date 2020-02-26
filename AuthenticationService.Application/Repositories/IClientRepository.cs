//using IdentityServer4.EntityFramework.Entities;
using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationService.Application.Repositories
{
    public interface IClientRepository
    {
        Task<IEnumerable<Client>> GetClients(bool requireEnabled = false);
        Task<IEnumerable<Client>> GetClientsByName(string name, bool requireEnabled = false);
        Task<Client> GetClientById(int id, bool requireEnabled = false);
        Task<Client> GetClientByClientId(string clientId, bool requireEnabled = false);
    }
}
