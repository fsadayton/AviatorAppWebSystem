// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AuthorizeRedirect.cs" company="UDRI">
//   Copyright © 2015 University of Dayton Research Institute. All rights reserved.
// </copyright>
// <summary>
//   The authorize redirect.
// </summary>
// --------------------------------------------------------------------------------------------------------------------


namespace Website.Helpers
{
    using System;
    using System.Web.Mvc;

    /// <summary>
    /// The authorize redirect.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeRedirect : AuthorizeAttribute
    {
        /// <summary>
        /// The redirect url.
        /// </summary>
        public string RedirectUrl = "~/Web/UnauthorizedError";

        /// <summary>
        /// The handle unauthorized request.
        /// </summary>
        /// <param name="filterContext">
        /// The filter context.
        /// </param>
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            base.HandleUnauthorizedRequest(filterContext);

            if (filterContext.RequestContext.HttpContext.User.Identity.IsAuthenticated)
            {
                filterContext.Result = new RedirectResult(this.RedirectUrl);
            }
        }
    }
}