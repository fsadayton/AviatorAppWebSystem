// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ToolsRepo.cs" company="UDRI">
//   Copyright © 2016 University of Dayton Research Institute. All rights reserved.
// </copyright>
// <summary>
//   The tools repo.
// </summary>
// --------------------------------------------------------------------------------------------------------------------


namespace DataEntry_Helpers.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;

    /// <summary>
    /// Tools repo
    /// </summary>
    public class ToolsRepo : Repository
    {

        /// <summary>
        /// Get all tools in the database.  
        /// </summary>
        /// <returns></returns>
        public List<Tool> GetAllTools()
        {
            List<Tool> tools;
            try
            {
                tools = db.Tools.OrderBy(t => t.Name).ToList();
            }
            catch (Exception ex)
            {
                tools = new List<Tool>();
            }

            return tools;
        }

        /// <summary>
        /// Gets only the active tools
        /// </summary>
        /// <returns>List of tools</returns>
        public List<Tool> GetActiveTools()
        {
            List<Tool> tools;
            try
            {
                tools = db.Tools.Where(t => t.Active).OrderBy(t => t.Name).ToList();
            }
            catch (Exception ex)
            {
                tools = new List<Tool>();
            }

            return tools;
        }

        /// <summary>
        /// Gets a single tool by ID
        /// </summary>
        /// <param name="id">ID of the tool</param>
        /// <returns>DB tool</returns>
        public Tool GetTool(int id)
        {
            try
            {
                return db.Tools.Single(t => t.ID == id);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Create a tool
        /// </summary>
        /// <param name="newTool"></param>
        /// <returns>DB Tool</returns>
        public Tool CreateTool(Tool newTool)
        {
            try
            {
                var createdTool = this.db.Tools.Add(newTool);
                this.db.SaveChanges();
                return createdTool;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Update a tool
        /// </summary>
        /// <param name="updatedTool">Updated tool</param>
        /// <returns>Bool</returns>
        public bool UpdateTool(Tool updatedTool)
        {
            try
            {
                var toolToUpdate = db.Tools.Single(t => t.ID == updatedTool.ID);
                toolToUpdate.Name = updatedTool.Name;
                toolToUpdate.Active = updatedTool.Active;
                toolToUpdate.AppleStoreWebsite = updatedTool.AppleStoreWebsite ?? String.Empty;
                toolToUpdate.Description = updatedTool.Description ?? String.Empty;
                toolToUpdate.GooglePlayWebsite = updatedTool.GooglePlayWebsite ?? String.Empty;
                toolToUpdate.Website = updatedTool.Website ?? String.Empty;

                this.db.Entry(toolToUpdate).State = EntityState.Modified;
                this.db.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Remove a tool
        /// </summary>
        /// <param name="toolId">Id of a tool</param>
        /// <returns>Bool</returns>
        public bool DeleteTool(int toolId)
        {
            try
            {
                var tool = this.db.Tools.Single(t => t.ID == toolId);
                if (tool == null) { return true; }

                this.db.Tools.Remove(tool);
                this.db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
    
}
