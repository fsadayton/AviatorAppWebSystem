// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AppDataController.cs" company="UDRI">
//   Copyright © 2015 University of Dayton Research Institute. All rights reserved.
// </copyright>
// <summary>
//   Defines the AppDataController type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Data.Entity.ModelConfiguration.Configuration;
using Models.ServiceProvider;

namespace Website.Controllers
{
    using System.Collections.Generic;
    using System.Web.Http;
    using System.Web.Http.Cors;

    using global::Models;

    using Website.BL;
    using Website.Models;

    [EnableCors("*", "*", "*")]
    public class AppDataController : ApiController
    {
        /// <summary>
        /// The data logic.
        /// </summary>
        private readonly DataLogics dataLogic;

        private readonly FamiliesLogic familiesLogic;

        private readonly ServiceProviderQueryLogics serviceProviderLogic;

        /// <summary>
        /// Initializes a new instance of the <see cref="AppDataController"/> class.
        /// </summary>
        public AppDataController()
        {
            this.dataLogic = new DataLogics();
            this.familiesLogic = new FamiliesLogic();
            this.serviceProviderLogic = new ServiceProviderQueryLogics();
        }

        /// <summary>
        /// The get counties.
        /// </summary>
        /// <returns>
        /// The <see cref="Dictionary"/>.
        /// </returns>
        [HttpGet]
        public Dictionary<int, string> GetCounties()
        {
            var webCounties = this.dataLogic.GetCounties();
            return this.dataLogic.ConvertWebCounties(webCounties);
        }

        /// <summary>
        /// The get families.
        /// </summary>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        [HttpGet]
        public List<FamilyEditor> GetFamilies()
        {
           return this.familiesLogic.GetFamilies();
        }
        
        /// <summary>
        /// Get the special populations.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public List<FamilyEditor> GetSpecialPopulationFamilies()
        {
            return this.familiesLogic.GetSpecialPopulationFamilies();
        }

        /// <summary>
        /// The get service providers.
        /// </summary>
        /// <param name="counties">
        /// The counties.
        /// </param>
        /// <param name="categories">
        /// The categories.
        /// </param>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        [HttpGet]
        public List<DisplayServiceProvider> GetServiceProviders([FromUri] List<int> counties, [FromUri] List<int> categories)
        {
            return this.serviceProviderLogic.ServiceProviderQuery(counties, categories);
        }

        /// <summary>
        /// The get crime types.
        /// </summary>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        [HttpGet]
        public List<Category> GetCrimeTypes()
        {
            return this.dataLogic.GetCrimeCategories();
        }

        /// <summary>
        /// The get category lookup index.
        /// </summary>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        [HttpGet]
        public List<Category> GetCategoryLookupIndex()
        {
            return this.dataLogic.GetCategories();
        }



        /// <summary>
        /// Gets the categories associated with Law Enforcement.
        /// </summary>
        /// <returns>List of categories.</returns>
        [HttpGet]
        public List<WebsiteCategory> GetCategoriesByFamily(int familyId)
        {
           return this.dataLogic.GetWebsiteCategories(familyId);
        }
    }
}