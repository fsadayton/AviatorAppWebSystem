// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InvitesRepo.cs" company="UDRI">
//   Copyright © 2015 University of Dayton Research Institute. All rights reserved.
// </copyright>
// <summary>
//   The invites repo.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DataEntry_Helpers.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;

    /// <summary>
    /// The invites repo.
    /// </summary>
    public class InvitesRepo : Repository
    {
        /// <summary>
        /// Create an invitation to a location in the database.
        /// </summary>
        /// <param name="newInvite">The new invite. </param>
        /// <returns> The <see cref="Invite"/> as it was created. </returns>
        public Invite CreateInvite(Invite newInvite)
        {
            Invite toReturn;
            try
            {
                newInvite.CreatedAt = DateTime.Now;
                toReturn = db.Invites.Add(newInvite);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                toReturn = null;
            }

            return toReturn;
        }

        /// <summary>
        /// Gets an invite by ID.
        /// </summary>
        /// <param name="id"> The invite id. </param>
        /// <returns> The <see cref="Invite"/>. </returns>
        public Invite GetInvite(int id)
        {
            Invite foundInvite;
            try
            {
                foundInvite = db.Invites.Single(invite => invite.ID == id);
            }
            catch (Exception ex)
            {
                foundInvite = null;
            }

            return foundInvite;
        }


        /// <summary>
        /// Update an existing update.
        /// </summary>
        /// <param name="inviteData"> The invite data. </param>
        /// <returns> The <see cref="bool"/>. </returns>
        public bool UpdateInvite(Invite inviteData)
        {
            try
            {
                this.db.Entry(inviteData).State = EntityState.Modified;
                this.db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Get an invite by email.
        /// </summary>
        /// <param name="email"> The email address.  </param>
        /// <returns> The <see cref="Invite"/>.  </returns>
        public Invite GetInvite(string email)
        {
            Invite foundInvite;
            try
            {
                foundInvite = db.Invites.Single(invite => invite.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
            }
            catch (Exception ex)
            {
                foundInvite = null;
            }

            return foundInvite;
        }

        /// <summary>
        /// The get invites created by a user
        /// </summary>
        /// <param name="userId"> The user id. </param>
        /// <returns> The <see cref="List"/>. </returns>
        public List<Invite> GetInvitesCreatedBy(int userId)
        {
            List<Invite> inviteList;
            try
            {
                inviteList = db.Invites.Where(invite => invite.CreatorID == userId).ToList();
            }
            catch (Exception ex)
            {
                inviteList = new List<Invite>();
            }

            return inviteList;
        }

        /// <summary>
        /// The remove an invite from the database.
        /// </summary>
        /// <param name="id"> The invite id. </param>
        /// <returns> The <see cref="bool"/>. </returns>
        public bool RemoveInvite(int id)
        {
            try
            {
                var invite = db.Invites.Single(s => s.ID == id);
                db.Invites.Remove(invite);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }
    }
}
