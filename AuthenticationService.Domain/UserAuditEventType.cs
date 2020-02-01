using System;
using System.Collections.Generic;
using System.Text;

namespace AuthenticationService.Domain
{
    public enum UserAuditEventType
    {
        Login = 1,
        FailedLogin = 2,
        LogOut = 3
    }
}
