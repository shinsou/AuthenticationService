using AuthenticationService.Application.Gateways;
using AuthenticationService.Application.Settings;
using AuthenticationService.Domain;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Novell.Directory.Ldap;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationService.Infrastructure.Gateways
{
    public class LdapGateway : ILdapGateway<User>
    {
        #region Members
        private LdapConnection LdapConnection { get; set; }
        private LdapSettings Settings { get; }
        private ILogger<LdapGateway> Logger { get; }
        #endregion

        public LdapGateway(ILogger<LdapGateway> logger, IOptions<LdapSettings> options)
        {
            this.Logger = logger;
            this.Settings = options.Value;
        }

        public async Task<CustomLdapResponse> AuthenticateAsync(string username, string password)
        {
            Logger.LogTrace("Entering into LDAP {method} method", "AuthenticateAsync");

            return await Task.Run<CustomLdapResponse>(() =>
            {
                Logger.LogDebug("Validating username and password for LDAP authentication");
                if (String.IsNullOrWhiteSpace(username) || String.IsNullOrWhiteSpace(password))
                {
                    Logger.LogDebug("Username and/or password missing");
                    return new CustomLdapResponse
                    (
                        false,
                        "Invalid credentials!",
                        type: CustomLdapResponse.ErrorTypes.InvalidCredentials
                    );
                }

                Logger.LogTrace("Verifying LdapConnection object being initialized");
                if (this.LdapConnection == null)
                {
                    Logger.LogDebug("LdapConnection object was null, defining new");
                    this.LdapConnection = new LdapConnection();
                }

                try
                {
                    Logger.LogDebug("Attempting to authenticate username and password against LDAP server");

                    // Opening connection with specific user, will validate credentials
                    // In case failure, error will be thrown
                    OpenConnection(username, password);

                    Logger.LogDebug("Successfully opened connection and authenticated against LDAP server");

                    return new CustomLdapResponse();
                }
                catch (LdapException le)
                {
                    Logger.LogError(le, $"Failed to connect to LDAP server, with reason: {le.Message}");

                    return new CustomLdapResponse
                    (
                        false,
                        le.Message,
                        le,
                        (CustomLdapResponse.ErrorTypes)le.ResultCode
                    );
                }
                finally
                {
                    Logger.LogDebug("Disconnecting from LDAP server");

                    this.LdapConnection.Disconnect();
                }
            });
        }

        public async Task<User> FindUserAsync(string username, string domain = null)
        {
            Logger.LogTrace("Entering into LDAP {method} method", "FindUserAsync");

            return await Task.Run<User>(() =>
            {
                Logger.LogDebug("Validating username and password for LDAP authentication");
                if (String.IsNullOrWhiteSpace(username))
                {
                    Logger.LogDebug("Username and/or password missing");
                    throw new ArgumentNullException("Invalid method arguments!");
                }

                try {
                    Logger.LogDebug("Attempting to authenticate username and password against LDAP server");
                    var result = null as User;
                    using (this.LdapConnection = this.LdapConnection ?? new LdapConnection()) { 

                        OpenConnection(this.Settings.Username, this.Settings.Password);
                    
                        //#TODO
                        // FindUser...

                        Logger.LogDebug("Successfully opened connection and authenticated against LDAP server");
                    }
                    return result;
                } catch (LdapException le) {
                    Logger.LogError(le, $"Failed to connect to LDAP server, with reason: {le.Message}");

                    throw new Exception("Error in LdapConnection", le);
                } finally {
                    Logger.LogDebug("Disconnecting from LDAP server");

                    if(this.LdapConnection != null) {
                        // this is abnormal procedure and works as fail-safe case
                        this.LdapConnection.Disconnect();
                        this.LdapConnection.Dispose();
                    }
                }
            });
        }

        #region Private methods

        private void OpenConnection(string username, string password)
        {
            Logger.LogDebug($"Opening connection to LDAP server, with [{Settings.Host}] host and [{Settings.Port}] port");

            this.LdapConnection.Connect(Settings.Host, Settings.Port);

            Logger.LogTrace("Connection established to LDAP server");

            Logger.LogDebug($"Binding username and password to LDAP connection");

            this.LdapConnection.Bind(username, password);

            Logger.LogTrace("Username and password bound successfully for LDAP connection");
        }

        private void SearchEntry(
            string searchBase = "CN=Users",
            string searchFilterString = "(samAccountName=*{alias}*",
            string entityAlias = "",
            string []attributes = null)
        {
            var aliasString = "{alias}";
            if (this.LdapConnection == null || !this.LdapConnection.Connected)
                return;

            searchFilterString = searchFilterString.Replace(aliasString, entityAlias);

            LdapSearchResults queryResults = this.LdapConnection.Search(
                searchBase,
                LdapConnection.SCOPE_SUB,
                searchFilterString,
                attributes,
                false
                );

            //while (queryResults.HasMore())
            //{
            //    var nextEntry = queryResults.Next();
            //    nextEntry.getAttributeSet();
            //}
        }

        #endregion
    }
}
