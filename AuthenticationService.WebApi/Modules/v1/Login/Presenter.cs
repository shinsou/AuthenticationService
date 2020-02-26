using AuthenticationService.Core;
using AuthenticationService.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationService.WebApi.Modules.v1.Login
{
    internal class Presenter : IPresenter<ViewModel, LoginResponse>
    {
        public ViewModel Process(LoginResponse input)
            => input == null
                ? null
                : new ViewModel(input);
    }
}
