using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Website.BL.ModelConversions;
using Website.Helpers;

namespace Website.Controllers
{

    [AuthorizeRedirect(Roles = "1")]
    public class ProviderActivityController : Controller
    {
        private DatabaseToWebServiceProvider webServiceProviders = new DatabaseToWebServiceProvider();
        // GET: ProviderActivity
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// The get all providers JSON data.
        /// </summary>
        /// <returns> The <see cref="ActionResult"/>. </returns>
        public ActionResult GetAllActivity()
        {
            var changedProviders = webServiceProviders.GetServiceProviderEditLog();
            return this.Json(changedProviders, JsonRequestBehavior.AllowGet);
        }
    }
}