// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AuthorizeProviderRedirect.cs" company="UDRI">
//   Copyright © 2015 University of Dayton Research Institute. All rights reserved.
// </copyright>
// <summary>
//   The authorize provider redirect.
// </summary>
// --------------------------------------------------------------------------------------------------------------------


namespace Website.Helpers
{
    using System;
    using System.Web;
    using BL;

    /// <summary>
    /// Redirects providers who try to access other provider data.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeProviderRedirect : AuthorizeRedirect
    {
        /// <summary>
        /// Sets whether the user is authorized to access the resource. 
        /// </summary>
        /// <param name="httpContext"> The http context. </param>
        /// <returns> The <see cref="bool"/>. </returns>
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            // Not logged in?  Send to error page
            var authorized = base.AuthorizeCore(httpContext);
            if (!authorized)
            {
                return false;
            }

            // Admins can do anything.
            if (httpContext.User.IsInRole("1"))
            {
                return true;
            }

            // Otherwise we are looking a provider.

            // Get the ID of the resource requested.
            var routeData = httpContext.Request.RequestContext.RouteData;
            var id = int.Parse(routeData.Values["id"].ToString());

            // Get the user's id and look them up
            var userId = httpContext.User.Identity.Name;
            var logics = new AccountLogics();
            var userInfo = logics.GetUser(int.Parse(userId));

            // Verify the user is tied to the same provider Id as the resource requested.
            return id == userInfo.ProviderId;
        }
    }
}