// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AppVeteransController.cs" company="UDRI">
//   Copyright © 2015 University of Dayton Research Institute. All rights reserved.
// </copyright>
// <summary>
//   Defines the AppDataController type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Linq;
using Models.ServiceProvider;

namespace Website.Controllers
{
    using System.Collections.Generic;
    using System.Web.Http;
    using System.Web.Http.Cors;

    using global::Models;

    using Website.BL;
    using Website.Models;

    /// <summary>
    /// The veterans controller.
    /// </summary>
    [EnableCors("*", "*", "*")]
    public class AppVeteransController : ApiController
    {
        /// <summary>
        /// General data look up logic.
        /// </summary>
        private readonly DataLogics dataLogic;

        /// <summary>
        /// The veteran provider logic.
        /// </summary>
        private readonly VeteranProviderQueryLogics veteranProviderLogic;

        /// <summary>
        /// Initializes a new instance of the <see cref="AppVeteransController"/> class.
        /// </summary>
        public AppVeteransController()
        {
            this.dataLogic =  new DataLogics();
            this.veteranProviderLogic = new VeteranProviderQueryLogics();
        }

        /// <summary>
        /// The get veteran providers.
        /// </summary>
        /// <param name="counties">
        /// The counties.
        /// </param>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        [HttpGet]
        public List<DisplayServiceProvider> GetVeteranProviders([FromUri] List<int> counties)
        {
            var providers = this.veteranProviderLogic.ServiceProviderQuery(counties);

            return providers;
        }


        /// <summary>
        /// Gets the categories associated with Veterans.
        /// </summary>
        /// <returns>List of categories.</returns>
        [HttpGet]
        public List<WebsiteCategory> GetCategories()
        {
            var categories = this.dataLogic.GetWebsiteCategories();
            return categories.Where(cat => cat.ServiceTypes.Any(st => st.Name == "Veteran")).ToList();
        }
    }
}