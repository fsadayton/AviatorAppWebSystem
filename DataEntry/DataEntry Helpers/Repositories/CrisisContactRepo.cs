// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CrisisContactRepo.cs" company="UDRI">
//   Copyright © 2016 University of Dayton Research Institute. All rights reserved.
// </copyright>
// <summary>
//   Repository access for crisis contacts.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DataEntry_Helpers.Repositories
{

    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Repo for Crisis Contacts
    /// </summary>
    public class CrisisContactRepo : Repository
    {

        /// <summary>
        /// Gets all the crisis contacts in the database.
        /// </summary>
        /// <returns>List of crisis contacts</returns>
        public List<CrisisContact> GetCrisisContacts()
        {
            try
            {
                return this.db.CrisisContacts.OrderBy(cs => cs.Name).ToList();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Searches the crisis contacts by name.
        /// </summary>
        /// <param name="name">Search text</param>
        /// <returns>List of crisis contacts matching the query</returns>
        public List<CrisisContact> GetCrisisContactsByName(string name)
        {
            try
            {
                return this.db.CrisisContacts
                    .Where(cc => name == null || cc.Name.ToLower().Contains(name.ToLower()))
                    .OrderBy(cc => cc.Name)
                    .ToList();
            }
            catch (Exception ex)
            {
                return null;
            }
        } 


        /// <summary>
        /// Gets a single crisis contact by id.
        /// </summary>
        /// <param name="id">id of the contact</param>
        /// <returns>The found crisis contact</returns>
        public CrisisContact GetCrisisContact(int id)
        {
            try
            {
                return this.db.CrisisContacts.Single(cc => cc.ID == id);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Creates a crisis contact
        /// </summary>
        /// <param name="crisisContactToCreate">Crisis contact to be created</param>
        /// <returns>ID of the new crisis contact</returns>
        public int? CreateCrisisContact(CrisisContact crisisContactToCreate)
        {
            try
            {
                var createdContact = this.db.CrisisContacts.Add(crisisContactToCreate);
                this.db.SaveChanges();
                return createdContact.ID;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Updates an existing crisis contact
        /// </summary>
        /// <param name="crisisContactToUpdate"></param>
        /// <returns></returns>
        public bool UpdateCrisisContact(CrisisContact crisisContactToUpdate)
        {
            try
            {
                var existingContact = this.db.CrisisContacts.Single(cc => cc.ID == crisisContactToUpdate.ID);
                existingContact.Name = crisisContactToUpdate.Name;
                existingContact.Contact.Phone = crisisContactToUpdate.Contact.Phone;
                db.SaveChanges();
                
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Deletes the given contact.
        /// </summary>
        /// <param name="id">ID of the contact to be deleted</param>
        /// <returns>boolean of success</returns>
        public bool DeleteCrisisContact(int id)
        {
            try
            {
                var crisisContact = this.db.CrisisContacts.Single(p => p.ID == id);
                this.db.CrisisContacts.Remove(crisisContact);
                this.db.SaveChanges();
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }
    }
}
