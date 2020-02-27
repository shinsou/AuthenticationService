using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationService.WebApi.Modules.v1.GetClientList
{
    internal class ViewModel : List<Client>
    {
        public ViewModel(IEnumerable<Client> input)
        {
            if (input?.Count() == 0)
            {
                // #TODO: remove when mockup data comes from database seed
                // add demo data
                input = new List<Client> {
                    new Client {
                        ClientId = "client.demo.1",
                        ClientName = "demo 1",
                        Description = "first demo client; this client doesn't actually exist, but is rather a mockup for UI",
                        Enabled = true
                    },
                    new Client {
                        ClientId = "client.demo.2",
                        ClientName = "demo 2",
                        Description = "second demo client; this client doesn't actually exist, but is rather a mockup for UI",
                        Enabled = true
                    },
                    new Client {
                        ClientId = "client.demo.3",
                        ClientName = "demo 3",
                        Description = "third demo client; this client doesn't actually exist, but is rather a mockup for UI",
                        Enabled = true
                    },
                };
            }

            this.AddRange(input);
        }
    }
}
