// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AppCrisisContactController.cs" company="UDRI">
//   Copyright © 2016 University of Dayton Research Institute. All rights reserved.
// </copyright>
// <summary>
//  Controller to get all the crisis contacts for the phone app
// </summary>
// --------------------------------------------------------------------------------------------------------------------



namespace Website.Controllers
{
    using System.Collections.Generic;
    using System.Web.Http;
    using System.Web.Http.Cors;
    using Website.BL;
    using Website.Models;

    /// <summary>
    /// Controller for Crisis Contacts for the app
    /// </summary>
    [EnableCors("*", "*", "*")]
    public class AppCrisisContactController : ApiController
    {
        /// <summary>
        /// Logic for crisis contacts
        /// </summary>
        private readonly CrisisContactLogic logic;

        /// <summary>
        /// Cris Contact API constructor
        /// </summary>
        public AppCrisisContactController()
        {
            this.logic = new CrisisContactLogic();
        }

        // GET: AppCrisisContact
        /// <summary>
        /// Gets all the crisis contacts for the app
        /// </summary>
        /// <returns>List of Crisis Contacts</returns>
        public List<CrisisContactDisplay> GetCrisisContacts()
        {
            return this.logic.GetCrisisContactDisplays();
        }
    }
}