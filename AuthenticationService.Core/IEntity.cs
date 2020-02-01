using System;
using System.Collections.Generic;
using System.Text;

namespace AuthenticationService.Core
{
    public interface IEntity<out T>
    {
        T Id { get; }
    }
}
