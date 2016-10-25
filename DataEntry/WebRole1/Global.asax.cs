// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Global.asax.cs" company="UDRI">
//   Copyright © 2015 University of Dayton Research Institute. All rights reserved.
// </copyright>
// <summary>
//   Defines the MvcApplication type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Website
{
    using System;
    using System.Security.Principal;
    using System.Web;
    using System.Web.Http;
    using System.Web.Mvc;
    using System.Web.Optimization;
    using System.Web.Routing;
    using System.Web.Security;
    using App_Start;

    /// <summary>
    /// The MVC application.
    /// </summary>
    public class MvcApplication : HttpApplication
    {
        /// <summary>
        /// The application start.
        /// </summary>
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        /// <summary>
        /// The application_ post authenticate request event.
        /// </summary>
        /// <param name="sender"> The sender  </param>
        /// <param name="e"> The event args  </param>
        protected void Application_PostAuthenticateRequest(object sender, EventArgs e)
        {
            var authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];

            if (authCookie == null || authCookie.Value == string.Empty)
            {
                return;
            }

            var authTicket = FormsAuthentication.Decrypt(authCookie.Value);
            var identity = new FormsIdentity(authTicket);

            // retrieve roles from UserData
            string[] roles = authTicket.UserData.Split(';');

            // Later the roles will provider or global admin.
            var principal = new GenericPrincipal(identity, roles);
            HttpContext.Current.User =  new GenericPrincipal(identity, roles);
        }
    }
}
