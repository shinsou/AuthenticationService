using AuthenticationService.Application.Repositories;
using AuthenticationService.Application.UseCases;
using Carter;
using Carter.Request;
using Carter.Response;
using IdentityServer4.EntityFramework.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationService.WebApi.Modules.v1.GetClientList
{
    public class GetClientListModule : VersionModule
    {
        public GetClientListModule(IClientRepository clientRepository) : base("/clients")
        {
            this.RequiresAuthorization();

            Get("/", async (req, res) => {
                var interactor = new GetAllClientsInteractor(clientRepository);
                var viewModel = await interactor.HandleAsync(
                    new Presenter());

                await res.Negotiate(viewModel);
            });
        }
    }
}
