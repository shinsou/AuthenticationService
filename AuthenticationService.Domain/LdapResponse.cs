using System;
using System.Collections.Generic;
using System.Text;

namespace AuthenticationService.Domain
{
    public class CustomLdapResponse
    {
        public bool IsSuccess { get; }
        public string Message { get; } = String.Empty;
        public Exception Exception { get; }
        public ErrorTypes Type { get; }

        public CustomLdapResponse(bool isSuccess = true, string message = "", Exception ex = null, ErrorTypes type = ErrorTypes.Unknown)
        {
            this.IsSuccess = isSuccess;
            this.Message = message;
            this.Exception = ex;
            this.Type = type;
        }

        public enum ErrorTypes
        {
            Unknown = 1,
            InvalidCredentials = 49,    // LDAP__INVALID_CREDENTIALS
            HostUnreachable = 91,       // LDAP__CONNECT_ERROR
        }
    }
}
