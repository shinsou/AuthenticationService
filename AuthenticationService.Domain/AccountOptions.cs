﻿using System;
using System.Collections.Generic;
using System.Text;

namespace AuthenticationService.Domain
{
    public class AccountOptions
    {
        public static bool AllowLocalLogin => true;
        public static bool RememberLogin => true;
        public static TimeSpan RememberMeLoginDuration => TimeSpan.FromDays(30);

        public static bool ShowLogoutPrompt => false;
        public static bool AutomaticRedirectAfterSignOut => true;

        // specify the Windows authentication scheme being used
        public static string WindowsAuthenticationSchemeName => "Windows"; //exception => "IISIntegrations : Field not found" Microsoft.AspNetCore.Server.IISIntegration.IISDefaults.AuthenticationScheme;
        // if user uses windows auth, should we load the groups from windows
        public static bool IncludeWindowsGroups => false;

        public static string InvalidCredentialsErrorMessage => "Invalid username or password";
    }
}
