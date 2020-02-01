using System;
using System.Collections.Generic;
using System.Text;

namespace AuthenticationService.Core
{
    public interface IPresenter<out TOut, in TIn>
    {
        TOut Process(TIn input);
    }
}
