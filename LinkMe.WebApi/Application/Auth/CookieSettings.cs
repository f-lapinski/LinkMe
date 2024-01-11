﻿namespace LinkMe.WebApi.Application.Auth
{
    public class CookieSettings
    {
        public const string CookieName = "auth.token";

        public bool Secure { get; set; } = true;

        public SameSiteMode SameSite { get; set; } = SameSiteMode.Lax;
    }
}
