using AuthenticationService.Application.Repositories;
//using IdentityServer4.EntityFramework.Entities;
using IdentityServer4.EntityFramework.Interfaces;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationService.Persistence.Repositories
{
    public class ClientRepository : IClientRepository
    {
        #region Private members

        private IConfigurationDbContext Context { get; }

        #endregion

        public ClientRepository(IConfigurationDbContext configurationDbContext)
        {
            this.Context = configurationDbContext;
        }

        #region Public methods

        /// <summary>
        /// Get client by its database Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="requireEnabled">Query condition, if client should have Enabled value true</param>
        /// <returns></returns>
        public async Task<Client> GetClientById(int id, bool requireEnabled = false)
        { 
            var client = await this.Context.Clients.FirstOrDefaultAsync(client => client.Id == id && (!requireEnabled || client.Enabled));
            return client.ToModel();
        }

        /// <summary>
        /// Get client by its unique clientId value
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="requireEnabled">Query condition, if client should have Enabled value true</param>
        /// <returns></returns>
        public async Task<Client> GetClientByClientId(string clientId, bool requireEnabled = false)
        {
            var client = await this.Context.Clients.FirstOrDefaultAsync(client => client.ClientId == clientId && (!requireEnabled || client.Enabled));

            return client.ToModel();
        }

        /// <summary>
        /// Get client(s) by client name (ClientName is not unique value and may result with multiple entities)
        /// </summary>
        /// <param name="name"></param>
        /// <param name="requireEnabled">Query condition, if client should have Enabled value true</param>
        /// <returns></returns>
        public async Task<IEnumerable<Client>> GetClientsByName(string name, bool requireEnabled = false)
        {
            var client = await this.Context.Clients
                .Where(client => client.ClientName == name && (!requireEnabled || client.Enabled))
                .ToListAsync();


            return client.Select(client => client.ToModel());
        }

        /// <summary>
        /// Get all clients in database
        /// </summary>
        /// <param name="requireEnabled">Query condition, if client should have Enabled value true</param>
        /// <returns></returns>
        public async Task<IEnumerable<Client>> GetClients(bool requireEnabled = false)
        {
            var clients = await this.Context.Clients.ToListAsync();
            if (requireEnabled)
                clients.RemoveAll(client => !client.Enabled);

            return clients.Select(client => client.ToModel());
        }

        #endregion
    }
}
