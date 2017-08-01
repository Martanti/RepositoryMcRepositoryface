using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using System;

namespace ProjectyMcProjectface
{
    public class Startup
    {
        public readonly int expireTimeSpanInMinutes = 30;
        public void Configuration(IAppBuilder app)
        {
            app.UseCookieAuthentication(new CookieAuthenticationOptions {
                AuthenticationType = "ApplicationCookie",
                LoginPath = new PathString("/login"),
                ExpireTimeSpan = TimeSpan.FromMinutes(expireTimeSpanInMinutes),
                SlidingExpiration = true
            });
        }
    }
}