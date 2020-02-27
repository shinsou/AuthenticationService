using AuthenticationService.Application.Repositories;
using AuthenticationService.Core;
using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationService.Application.UseCases
{
    public class GetAllClientsInteractor : IInteractor<IEnumerable<Client>>
    {
        private IClientRepository ClientRepository { get; }

        public GetAllClientsInteractor(IClientRepository clientRepository)
        {
            this.ClientRepository = clientRepository;
        }

        public async Task<TViewModel> HandleAsync<TViewModel>(IPresenter<TViewModel, IEnumerable<Client>> presenter)
            => presenter.Process(await this.ClientRepository.GetClients());
    }
}
