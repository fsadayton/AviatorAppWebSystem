// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ToolAdminController.cs" company="UDRI">
//   Copyright © 2016 University of Dayton Research Institute. All rights reserved.
// </copyright>
// <summary>
//   The controller for the admin of tools.
// </summary>
// --------------------------------------------------------------------------------------------------------------------


namespace Website.Controllers
{
    using System.Web.Mvc;
    using Helpers;
    using Models;

    /// <summary>
    /// Tools Controller
    /// </summary>
    public class ToolAdminController : Controller
    {
        private readonly BL.ToolLogic logic;

        /// <summary>
        /// Contstructor for the controller
        /// </summary>
        public ToolAdminController()
        {
            this.logic = new BL.ToolLogic();
        }

        // GET: Tool
        /// <summary>
        /// The index action for Tools
        /// </summary>
        /// <returns>View for Index</returns>
        public ActionResult Index()
        {
            return View(logic.GetAllTools());
        }
        

        /// <summary>
        /// Create Action 
        /// </summary>
        /// <returns>View of Create Tool</returns>
        [AuthorizeRedirect(Roles = "1")]
        public ActionResult Create()
        {
            var tool = new ToolViewModel();
            return this.View(tool);
        }

        /// <summary>
        /// Create the Tool 
        /// </summary>
        /// <param name="tool">The tool to create</param>
        /// <returns>Index view if successful, Create view if unsuccessful</returns>
        [HttpPost]
        [AuthorizeRedirect(Roles = "1")]
        public ActionResult Create(ToolViewModel tool)
        {
            if(ModelState.IsValid && logic.CreateTool(tool) != null) { 
                this.TempData["Info"] = $"The tool {tool.Name} was created succesfully.";
                return this.RedirectToAction("Index");
            }
            else
            {
                this.TempData["Error"] = $"The tool {tool.Name} was not created.";
                return this.View(tool);
            }
        }


        /// <summary>
        /// GET: Tool/Edit/5
        /// </summary>
        /// <param name="id">ID to edit</param>
        /// <returns>Form for editing</returns>
        [AuthorizeProviderRedirect(Roles = "1,2")]
        public ActionResult Edit(int id)
        {
            return this.View(logic.GetTool(id));
        }

        /// <summary>
        /// POST: Tool/Edit/5
        /// </summary>
        /// <param name="tool"> The Tool to save.</param>
        /// <returns> Index if saved correctly </returns>
        [HttpPost]
        [AuthorizeProviderRedirect(Roles = "1,2")]
        public ActionResult Edit(ToolViewModel tool)
        {
            var isSuccessful = logic.UpdateTool(tool);
            if (isSuccessful)
            {
                this.TempData["Info"] = $"The tool {tool.Name} was updated succesfully.";
                return this.RedirectToAction("Index");
            }
            else
            {
                this.TempData["Error"] = $"The tool {tool.Name} was not updated.";
                return this.View(tool);
            }
        }


        /// <summary>
        /// GET: Tool/Delete/5
        /// </summary>
        /// <param name="id">ID to delete</param>
        /// <returns>View of Delete</returns>
        [AuthorizeRedirect(Roles = "1")]
        public ActionResult Delete(int id)
        {
            var isSuccessful = this.logic.DeleteTool(id);
            if (isSuccessful)
            {
                this.TempData["Info"] = "The tool was deleted successfully.";
            }
            else
            {
                this.TempData["Error"] = "An error occured while deleting the tool.";
            }
            return this.RedirectToAction("Index");
        }

    }
}