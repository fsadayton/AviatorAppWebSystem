// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AppLawEnforcementController.cs" company="UDRI">
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

    using Website.BL;
    using Website.Models;

    /// <summary>
    /// The app Law Enforcement controller.
    /// </summary>
    [EnableCors("*", "*", "*")]
    public class AppLawEnforcementController : ApiController
    {
        /// <summary>
        /// General data look up logic.
        /// </summary>
        private readonly DataLogics dataLogic;

        /// <summary>
        /// The veteran provider logic.
        /// </summary>
        private readonly LawEnforcementProviderQueryLogics lawEnforcementProviderLogic;

        /// <summary>
        /// Initializes a new instance of the <see cref="AppLawEnforcementController"/> class. 
        /// </summary>
        public AppLawEnforcementController()
        {
            this.dataLogic = new DataLogics();
            this.lawEnforcementProviderLogic = new LawEnforcementProviderQueryLogics();
        }

        /// <summary>
        /// Get compensation providers.
        /// </summary>
        /// <param name="counties">
        /// The counties.
        /// </param>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        [HttpGet]
        public List<DisplayServiceProvider> GetLawEnforcementProviders([FromUri] List<int> counties)
        {
            var providers = this.lawEnforcementProviderLogic.ServiceProviderQuery(counties);

            return providers;
        }
        

        /// <summary>
        /// Gets the categories associated with Law Enforcement.
        /// </summary>
        /// <returns>List of categories.</returns>
        [HttpGet]
        public List<WebsiteCategory> GetCategories()
        {
            var categories = this.dataLogic.GetWebsiteCategories();
            return categories.Where(cat => cat.ServiceTypes.Any(st => st.Name == "Law Enforcement")).ToList();
        }
    }
}