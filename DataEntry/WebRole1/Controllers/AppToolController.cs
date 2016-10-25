// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AppToolController.cs" company="UDRI">
//   Copyright © 2016 University of Dayton Research Institute. All rights reserved.
// </copyright>
// <summary>
//   The controller for tools in the API.
// </summary>
// --------------------------------------------------------------------------------------------------------------------


namespace Website.Controllers
{
    using System.Web.Http;
    using Website.BL;
    using Website.BL.ModelConversions;
    using System.Collections.Generic;
    using System.Web.Http.Cors;


    [EnableCors("*", "*", "*")]
    public class AppToolController : ApiController
    {
        private readonly BL.ToolLogic logic;

        public AppToolController()
        {
            logic = new ToolLogic();
        }

        /// <summary>
        /// Gets all the crisis contacts for the app
        /// </summary>
        /// <returns>List of Crisis Contacts</returns>
        [HttpGet]
        public List<Models.ToolApiModel> GetToolsForIOS()
        {
            return logic.GetActiveToolsForApi(PhoneType.Apple);
        }


        // GET: AppCrisisContact
        /// <summary>
        /// Gets all the crisis contacts for the app
        /// </summary>
        /// <returns>List of Crisis Contacts</returns>
        [HttpGet]
        public List<Models.ToolApiModel> GetToolsForAndroid()
        {
            return logic.GetActiveToolsForApi(PhoneType.Google);
        }
    }
}