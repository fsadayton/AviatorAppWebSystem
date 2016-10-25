// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CrisisContactLogic.cs" company="UDRI">
//   Copyright © 2016 University of Dayton Research Institute. All rights reserved.
// </copyright>
// <summary>
//   Logic for working with Crisis Contacts
// </summary>
// --------------------------------------------------------------------------------------------------------------------


namespace Website.BL
{
    using System.Collections.Generic;
    using System.Linq;
    using DataEntry_Helpers;
    using DataEntry_Helpers.Repositories;
    using Website.Models;

    /// <summary>
    /// Logic for Crisis Contact Administration
    /// </summary>
    public class CrisisContactLogic
    {
        /// <summary>
        /// Logic layer for crisis contacts
        /// </summary>
        private readonly CrisisContactRepo repo;

        /// <summary>
        /// Constructor
        /// </summary>
        public CrisisContactLogic()
        {
            repo = new CrisisContactRepo();
        }

        /// <summary>
        /// Get the db representation of the crisis contact by ID.
        /// </summary>
        /// <param name="id">ID of the contact</param>
        /// <returns>DB version of the contact</returns>
        public CrisisContact GetDbCrisisContact(int id)
        {
            return repo.GetCrisisContact(id);
        }

        /// <summary>
        /// Gets a list of all the crisis contacts to be displayed
        /// </summary>
        /// <returns>Crisis Contact Display  List</returns>
        public List<CrisisContactDisplay> GetCrisisContactDisplays()
        {
            var dbList = repo.GetCrisisContacts();
            if (dbList == null)
            {
                return null;
            }
            return dbList.Select(curItem => new CrisisContactDisplay(curItem)).ToList();
        }

        /// <summary>
        /// Gets the list of crisis contacts to be displayed by name search
        /// </summary>
        /// <param name="nameSearchText">The search text</param>
        /// <returns>List of contacts to be displayed</returns>
        public List<CrisisContactDisplay> GetCrisisContactDisplays(string nameSearchText)
        {
            nameSearchText = nameSearchText.Trim();
            if (nameSearchText == "")
            {
                nameSearchText = null;
            }
            var dbList = repo.GetCrisisContactsByName(nameSearchText);
            if (dbList == null)
            {
                return null;
            }
            return dbList.Select(curItem => new CrisisContactDisplay(curItem)).ToList();
        }

        /// <summary>
        /// Gets a single contact to display by ID
        /// </summary>
        /// <param name="id">Id of the contact</param>
        /// <returns>display of a single contact if found.  Returns null if the contact is not found</returns>
        public CrisisContactDisplay GetCrisisContactDisplay(int id)
        {
            var dbContact = repo.GetCrisisContact(id);
            if (dbContact == null)
            {
                return null;
            }
            return new CrisisContactDisplay(dbContact);
        }

        /// <summary>
        /// Creates a crisis contact in the db.
        /// </summary>
        /// <param name="crisisContact">Display version of the contact to be created</param>
        /// <returns>The id of the newly created contact</returns>
        public int? CreateCrisisContact(CrisisContactDisplay crisisContact)
        {
            return repo.CreateCrisisContact(crisisContact.ToDbCrisisContact());
        }

        /// <summary>
        /// Updates the given crisis contact in the database
        /// </summary>
        /// <param name="crisisContact">Display version of the conact to be updated</param>
        /// <returns>Boolean representing if the update was successful</returns>
        public bool UpdateCrisisContact(CrisisContactDisplay crisisContact)
        {
            return repo.UpdateCrisisContact(crisisContact.ToDbCrisisContact());
        }

        /// <summary>
        /// Deletes the crisis contact with the given id.
        /// </summary>
        /// <param name="id">ID of the contact to be removed.</param>
        /// <returns>Boolean representing if the delete was successful.  Returns false if the contact was not found.</returns>
        public bool DeleteCrisisContact(int id)
        {
            var crisisContact = this.GetDbCrisisContact(id);
            return crisisContact != null && repo.DeleteCrisisContact(id);
        }
    }
}