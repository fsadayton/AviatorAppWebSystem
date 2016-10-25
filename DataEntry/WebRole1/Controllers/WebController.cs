// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HomeController.cs" company="UDRI">
//   Copyright © 2015 University of Dayton Research Institute. All rights reserved.
// </copyright>
// <summary>
//   Defines the HomeController type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Website.Controllers
{
    using System.Web.Mvc;
    using DataEntry_Helpers;
    using System.Web.Http.Cors;

    using DataEntry_Helpers.Repositories;
    using global::Models;

    [EnableCors("*", "*", "*")]
    public class WebController : BaseController
    {
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            return View();
        }

        public ActionResult UnauthorizedError()
        {
            ViewBag.Message = "You are not authorized to view this page.";
            return this.View("Error");
        }
    }
}