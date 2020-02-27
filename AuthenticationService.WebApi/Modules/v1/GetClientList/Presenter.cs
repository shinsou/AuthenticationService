using AuthenticationService.Core;
using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationService.WebApi.Modules.v1.GetClientList
{
    internal class Presenter : IPresenter<ViewModel, IEnumerable<Client>>
    {
        public ViewModel Process(IEnumerable<Client> input)
            => input == null
                ? null
                : new ViewModel(input);
    }
}
