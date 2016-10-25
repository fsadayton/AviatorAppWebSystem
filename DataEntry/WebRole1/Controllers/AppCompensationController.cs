// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AppCompensationController.cs" company="UDRI">
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
    /// The app compensation controller.
    /// </summary>
    [EnableCors("*", "*", "*")]
    public class AppCompensationController : ApiController
    {
        /// <summary>
        /// General data look up logic.
        /// </summary>
        private readonly DataLogics dataLogic;

        /// <summary>
        /// The veteran provider logic.
        /// </summary>
        private readonly CompensationProviderQueryLogics compensationProviderLogic;

        /// <summary>
        /// Initializes a new instance of the <see cref="AppCompensationController"/> class. 
        /// </summary>
        public AppCompensationController()
        {
            this.dataLogic = new DataLogics();
            this.compensationProviderLogic = new CompensationProviderQueryLogics();
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
        public List<DisplayServiceProvider> GetCompensationProviders([FromUri] List<int> counties)
        {
            var providers = this.compensationProviderLogic.ServiceProviderQuery(counties);

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
            return categories.Where(cat => cat.ServiceTypes.Any(st => st.Name == "Victim Compensation")).ToList();
        }
    }
}