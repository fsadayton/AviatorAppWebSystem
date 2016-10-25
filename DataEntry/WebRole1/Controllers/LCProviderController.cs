using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Models;
using Website.BL;
using Website.BL.ModelConversions;

namespace Website.Controllers
{
    public class LCProviderController : Controller
    {
        public ActionResult Index()
        {
            return this.View("LCProviderList");

        }

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