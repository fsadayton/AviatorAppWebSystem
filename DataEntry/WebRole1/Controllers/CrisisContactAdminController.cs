// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CrisisContactAdminController.cs" company="UDRI">
//   Copyright © 2016 University of Dayton Research Institute. All rights reserved.
// </copyright>
// <summary>
//   The crisis contact controller for administration.
// </summary>
// --------------------------------------------------------------------------------------------------------------------


namespace Website.Controllers
{

    using System.Web.Http.Cors;
    using System.Web.Mvc;
    using Website.BL;
    using Website.Helpers;
    using Website.Models;

    /// <summary>
    /// Controller for Crisis Contact Administration
    /// </summary>
    [EnableCors("*", "*", "*")]
    public class CrisisContactAdminController : BaseController
    {
        /// <summary>
        /// Logics for crisis contacts
        /// </summary>
        private CrisisContactLogic logics;

       /// <summary>
       /// Crisis contacts controller constructor
       /// </summary>
        public CrisisContactAdminController()
        {
            logics = new CrisisContactLogic();
        }

        // GET: CrisisContact
        /// <summary>
        /// Shows all the crisis contacts
        /// </summary>
        /// <returns>View of the contacts</returns>
        [AuthorizeRedirect(Roles = "1")]
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// GET: CrisisContact/Create 
        /// Create action of crisis contacts.   Creates a new blank crisis contact and hands it over to the view. 
        /// </summary>
        /// <returns>The action result</returns>
        [AuthorizeRedirect(Roles = "1")]
        public ActionResult Create()
        {
            var crisisContact = new CrisisContactDisplay();
            return this.View(crisisContact);
        }

        /// <summary>
        /// Accepts the post for creating a new crisis contact. 
        /// POST: CrisisContact/Create
        /// </summary>
        /// <param name="crisisContact">Crisis Contact to create</param>
        /// <returns>The redirect to index</returns>
        [HttpPost]
        [AuthorizeRedirect(Roles = "1")]
        public ActionResult Create(CrisisContactDisplay crisisContact)
        {
            if(logics.CreateCrisisContact(crisisContact) != null)
            {
                this.TempData["Info"] = $"The crisis contact {crisisContact.Name} was created succesfully.";
                return this.RedirectToAction("Index");
            }
            else
            {
                this.TempData["Error"] = $"The crisis contact {crisisContact.Name} was not created.";
                return this.View(crisisContact);
            }
        }


        /// <summary>
        /// GET: CrisisContact/Edit/5
        /// </summary>
        /// <param name="id">ID to edit</param>
        /// <returns>Form for editing</returns>
        [AuthorizeProviderRedirect(Roles = "1,2")]
        public ActionResult Edit(int id)
        {
            var crisisContact =  logics.GetCrisisContactDisplay(id);   
            return this.View(crisisContact);
        }

        /// <summary>
        /// POST: CrisisContact/Edit/5
        /// </summary>
        /// <param name="provider"> The provider.</param>
        /// <returns> Index if saved correctly </returns>
        [HttpPost]
        [AuthorizeProviderRedirect(Roles = "1,2")]
        public ActionResult Edit(CrisisContactDisplay crisisContact)
        {
            var isSuccessful = logics.UpdateCrisisContact(crisisContact);
            if (isSuccessful)
            {
                this.TempData["Info"] = $"The crisis contact {crisisContact.Name} was updated succesfully.";
                return this.RedirectToAction("Index");
            }
            else
            {
                this.TempData["Error"] = $"The crisis contact {crisisContact.Name} was not updated.";
                return this.View(crisisContact);
            }
        }


        /// <summary>
        /// Searches the crisis contacts by name and shows the result.
        /// </summary>
        /// <param name="searchText">Name search</param>
        /// <returns>View Result</returns>
        [HttpGet]
        public ViewResult Search(string searchText)
        {
            return this.View("CrisisContactList", this.logics.GetCrisisContactDisplays(searchText));
        }
        

        /// <summary>
        /// GET: CrisisContact/Delete/5
        /// </summary>
        /// <param name="id">ID to delete</param>
        /// <returns>View of Delete</returns>
        [AuthorizeRedirect(Roles = "1")]
        public ActionResult Delete(int id)
        {
            var isSuccessful = this.logics.DeleteCrisisContact(id);
            if (isSuccessful)
            {
                this.TempData["Info"] = "The crisis contact was deleted successfully.";
            }
            else
            {
                this.TempData["Error"] = "An error occured while deleting the crisis contact.";
            }
            return this.RedirectToAction("Index");
        }

    }
}