using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Models;
using Website.BL;
using Website.BL.ModelConversions;
using Website.Models;

namespace Website.Controllers
{
    public class PersonalCompensationController : Controller
    {
        /// <summary>
        /// Gets county list for compensation providers 
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
                return this.View("CompensationProviderList");
        }

        /// <summary>
        /// Shows a compensation provider details
        /// </summary>
        /// <param name="id"></param> id of provider to be shown 
        /// <param name="locationId"></param> Location id of provider
        /// <returns></returns>
        public ActionResult Details(int id, int? locationId)
        {
            DatabaseToWebServiceProvider dbConverter = new DatabaseToWebServiceProvider();
            var dataLogics = new DataLogics();
            WebsiteServiceProvider provider = dbConverter.GetServiceProvider(id);
            var categories = dataLogics.GetWebsiteCategories();
            categories.Sort((category1, category2) => category1.Name.CompareTo(category2.Name));
            ViewBag.AllCategories = categories;

            return this.View(provider);
        }

    }
}
