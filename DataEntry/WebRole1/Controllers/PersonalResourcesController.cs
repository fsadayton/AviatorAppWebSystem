// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PersonalResourcesController.cs" company="UDRI">
//   Copyright © 2016 University of Dayton Research Institute. All rights reserved.
// </copyright>
// <summary>
//   The personal resources controller.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Website.Controllers
{
    using BL.ModelConversions;
    using Models;
    using System.Linq;
    using System.Web.Mvc;
    using BL;

    /// <summary>
    /// The personal resources controller.
    /// </summary>
    public class PersonalResourcesController : Controller
    {
        // GET: PersonalResources

        /// <summary>
        /// Get all the personal resources for display.
        /// </summary>
        /// <returns> The <see cref="ActionResult"/>. </returns>
        public ActionResult Index(string family, string category)
        {
            if (!string.IsNullOrEmpty(family) || !string.IsNullOrEmpty(category))
            {
                return this.View("ServiceProviderList");
            }

            var familiesLogic = new FamiliesLogic();
            var dataLogics = new DataLogics();

            var viewModel = new PersonalResourcesViewModel
            {
                General = familiesLogic.GetFamilies().OrderBy(fam => fam.Name).ToList(),
                Crime = dataLogics.GetCrimeCategories().OrderBy(crime => crime.Name).ToList()
            };
            return this.View(viewModel);
        }

        /// <summary>
        /// Shows a service provider
        /// </summary>
        /// <param name="id">id of the provider to be shown</param>
        /// <param name="locationId">Which location to show. (optional)</param>
        /// <returns></returns>
        public ActionResult Details(int id, int? locationId)
        {
            var dataLogics = new DataLogics();
            var categories = dataLogics.GetWebsiteCategories();
            categories.Sort((category1, category2) => category1.Name.CompareTo(category2.Name));
            ViewBag.AllCategories = categories;

            var dbConverter = new DatabaseToWebServiceProvider();
            return this.View(dbConverter.GetServiceProvider(id));
        }
    }
}