using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Website.BL;

namespace Website.Controllers
{
    public class CrisisContactsController : Controller
    {
        //Returns view for crisis contacts 
        // GET: CrisisContacts
        public ActionResult Index()
        {
            return View();
        }
        //Return HotLineproviders. Includes all providers that include a helpline.  
        [HttpGet]
        public ActionResult GetHotlineProviders()
        {
            var hotLogic = new HotlineLogics().GetHotlines();
            return this.Json(hotLogic, JsonRequestBehavior.AllowGet);

        }
        
    }
}