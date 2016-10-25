// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ToolLogic.cs" company="UDRI">
//   Copyright © 2016 University of Dayton Research Institute. All rights reserved.
// </copyright>
// <summary>
//   The logic for Tools.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Website.BL
{
    using System.Collections.Generic;
    using DataEntry_Helpers.Repositories;
    using ModelConversions;
    using Models;

    /// <summary>
    /// Tool CRUD Logic
    /// </summary>
    public class ToolLogic
    {
        private ToolsRepo repo;
        public ToolLogic()
        {
            repo = new ToolsRepo();
        }
        /// <summary>
        /// Get the tools fromt the DB. 
        /// </summary>
        /// <returns></returns>
        public List<Models.ToolViewModel> GetAllTools()
        {
            var dbTools = repo.GetAllTools();
            return (dbTools != null) ? ToolConversions.ConvertDbToViewModels(dbTools) : null;
        }

        /// <summary>
        /// Get the Active tools for the web view.
        /// </summary>
        /// <returns>List of active tools</returns>
        public List<Models.ToolViewModel> GetAllActiveTools()
        {
            var dbTools = repo.GetActiveTools();
            return (dbTools != null) ? ToolConversions.ConvertDbToViewModels(dbTools) : null;
        } 

        /// <summary>
        /// Get the list of all active tools for the API
        /// </summary>
        /// <param name="phoneType">Type of phone we're retrieving the data for.</param>
        /// <returns></returns>
        public List<ToolApiModel> GetActiveToolsForApi(PhoneType phoneType)
        {
            var dbTools = repo.GetActiveTools();
            return (dbTools != null) ? ToolConversions.ConvertDbModelToApiModel(dbTools, phoneType) : null;
        } 

        /// <summary>
        /// Get single tool from the DB
        /// </summary>
        /// <param name="id">ID of the tool to get.</param>
        /// <returns>View model of the tool.  Return null if there's a problem or it's not found.</returns>
        public ToolViewModel GetTool(int id)
        {
            var dbTool = repo.GetTool(id);
            return (dbTool != null) ? ToolConversions.ConvertDbToViewModel(dbTool) : null;
        }

        /// <summary>
        /// Add a new tool to the database.
        /// </summary>
        /// <param name="newTool">The new tool to create</param>
        /// <returns>The viewModel of the new tool.  Returns null if there is a problem.</returns>
        public ToolViewModel CreateTool(ToolViewModel newTool)
        {
            var dbTool = ToolConversions.ConvertViewModelToDb(newTool);
            var createdTool =  repo.CreateTool(dbTool);
            return (createdTool != null) ? ToolConversions.ConvertDbToViewModel(createdTool) : null;
        }

        /// <summary>
        /// Update an existing tool with new values
        /// </summary>
        /// <param name="updatedTool">The tool with the updated values</param>
        /// <returns>Returns true if the update is successfull.  Returns false if the update fails.</returns>
        public bool UpdateTool(ToolViewModel updatedTool)
        {
            var dbTool = ToolConversions.ConvertViewModelToDb(updatedTool);
            var success = repo.UpdateTool(dbTool);
            return success;
        }

        /// <summary>
        /// Delete a tool from the database
        /// </summary>
        /// <param name="id">ID of the tool to remove</param>
        /// <returns>Return true if the delete is successfull and false if it fails</returns>
        public bool DeleteTool(int id)
        {
            return repo.DeleteTool(id);
        }

    }
}