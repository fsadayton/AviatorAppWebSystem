// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ToolController.cs" company="UDRI">
//   Copyright © 2016 University of Dayton Research Institute. All rights reserved.
// </copyright>
// <summary>
//   The controller for the view of the tools
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Website.Controllers
{
    using System.Web.Mvc;
    using Website.BL;

    /// <summary>
    /// Tools Controller
    /// </summary>
    public class ToolsController : Controller
    {
        private BL.ToolLogic logic;

        public ToolsController()
        {
            logic = new ToolLogic();
        }
        // GET: Tools
        /// <summary>
        /// Show all the tools
        /// </summary>
        /// <returns>View with all the tools</returns>
        public ActionResult Index()
        {
            return View(logic.GetAllActiveTools());
        }
    }
}