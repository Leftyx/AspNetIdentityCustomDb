using System;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using AspNetIdentityCustomDb.Models;
using System.Threading.Tasks;

namespace AspNetIdentityCustomDb
{
    public partial class Startup
    {
        public void ConfigureAuth(IAppBuilder app)
        {
            var folderStorage = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "_Storage");

            // Configure the db context, user manager and signin manager to use a single instance per request
            // app.CreatePerOwinContext(ApplicationDbContext.Create);
            app.CreatePerOwinContext<Custom.Identity.UserManager>(() => new Custom.Identity.UserManager(new Custom.Identity.UserStore(folderStorage)));
            app.CreatePerOwinContext<Custom.Identity.RoleManager>(() => new Custom.Identity.RoleManager(new Custom.Identity.RoleStore(folderStorage)));
            app.CreatePerOwinContext<Custom.Identity.SignInService>((options, context) => new Custom.Identity.SignInService(context.GetUserManager<Custom.Identity.UserManager>(), context.Authentication));

            // Enable the application to use a cookie to store information for the signed in user
            // and to use a cookie to temporarily store information about a user logging in with a third party login provider
            // Configure the sign in cookie
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                Provider = new CookieAuthenticationProvider
                {
                    // Enables the application to validate the security stamp when the user logs in.
                    // This is a security feature which is used when you change a password or add an external login to your account.  
                    OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<Custom.Identity.UserManager, Custom.Identity.User, int>(
                    validateInterval: TimeSpan.FromMinutes(30),
                    regenerateIdentityCallback: (manager, user) =>
                    {
                        var userIdentity = manager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
                        return (userIdentity);
                    },
                    getUserIdCallback: (id) => (Int32.Parse(id.GetUserId()))
                    )
                }
            });            
        }
    }
}