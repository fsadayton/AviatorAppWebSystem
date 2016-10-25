// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BaseController.cs" company="UDRI">
//   Copyright © 2015 University of Dayton Research Institute. All rights reserved.
// </copyright>
// <summary>
//   The base controller.  Sets the UserId for any controller that
// </summary>
// --------------------------------------------------------------------------------------------------------------------


using Website.BL;

namespace Website.Controllers
{
    using System.Web.Mvc;
    using System.Web.Routing;

    /// <summary>
    /// The base controller.  Sets the UserId for any controller that 
    /// </summary>
    public class BaseController : Controller
    {
        /// <summary>
        /// Gets or sets the user id.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets the user's default service provider id.
        /// </summary>
        public int UserDefaultServiceProviderId { get; set; }

        /// <summary>
        /// Initialize for all controllers that need access to the user id.
        /// </summary>
        /// <param name="requestContext">
        /// The request context. 
        /// </param>
        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);

            if (requestContext.HttpContext.User.Identity.IsAuthenticated)
            {
                this.UserId = int.Parse(requestContext.HttpContext.User.Identity.Name);
                ViewBag.UserId = this.UserId;
                var logics = new AccountLogics();
                var userInfo = logics.GetUser(this.UserId);

                if (userInfo != null)
                {
                    // Verify the user is tied to the same provider Id as the resource requested.
                    ViewBag.ServiceProviderId = userInfo.ProviderId;
                }
            }
        }
    }
}