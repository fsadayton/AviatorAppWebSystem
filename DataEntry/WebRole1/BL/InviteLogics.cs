// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InviteLogics.cs" company="UDRI">
//   Copyright © 2015 University of Dayton Research Institute. All rights reserved.
// </copyright>
// <summary>
//   The invite logics.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Website.BL
{
    using System;
    using System.Collections.Generic;
    using DataEntry_Helpers;
    using DataEntry_Helpers.Repositories;
    using Email;
    using ModelConversions;
    using global::Models.AccountManagement;

    /// <summary>
    /// The invite logics.
    /// </summary>
    public class InviteLogics
    {
        /// <summary>
        /// The invite repository.
        /// </summary>
        private readonly InvitesRepo repo;

        /// <summary>
        /// The class to do conversions between invites and invite view models.
        /// </summary>
        private readonly InviteConversions conversions;

        /// <summary>
        /// Initializes a new instance of the <see cref="InviteLogics"/> class.
        /// </summary>
        public InviteLogics()
        {
            this.repo = new InvitesRepo();
            this.conversions = new InviteConversions();
        }

        /// <summary>
        /// The get invites by user.
        /// </summary>
        /// <param name="userId"> The user id.  </param>
        /// <returns> The <see cref="List"/>. </returns>
        public List<InviteViewModel> GetInvitesByUser(int userId)
        {
            return this.conversions.ConvertDatabaseModelListToViewModelList(this.repo.GetInvitesCreatedBy(userId));
        }


        /// <summary>
        /// Create an invite and send it to the user.
        /// </summary>
        /// <param name="invite"> The invite.  </param>
        /// <param name="userId"> The user id of the person doing the inviting.  </param>
        /// <param name="inviteIndexUri"> The invite Index Uri. </param>
        /// <returns> The <see cref="InviteViewModel"/>.  </returns>
        public InviteViewModel CreateInvite(InviteViewModel invite, int userId, Uri inviteIndexUri)
        {
            // Check if the invite already exists
            var foundInvite = this.repo.GetInvite(invite.InviteeEmailAddress);
            if (foundInvite != null)
            {
                return this.ResendInvite(foundInvite, inviteIndexUri)
                    ? this.conversions.ConvertDatabaseModelToViewModel(foundInvite)
                    : null;
            }

            var inviteToCreate = this.conversions.ConvertViewModelToDatabaseModel(invite);
            inviteToCreate.CreatorID = userId;
            inviteToCreate.CreatedAt = DateTime.Now;
            inviteToCreate.Token = Guid.NewGuid().ToString();

            var createdInvite = this.repo.CreateInvite(inviteToCreate);

            if (createdInvite == null)
            {
                return null;
            }

            var email = new InviteEmail(createdInvite);
            email.Send(inviteIndexUri);

            return this.conversions.ConvertDatabaseModelToViewModel(createdInvite);
        }


        /// <summary>
        /// The resend invite.
        /// </summary>
        /// <param name="inviteId"> The invite id. </param>
        /// <param name="inviteIndexUri"> The invite index uri. </param>
        /// <returns> The <see cref="bool"/>. </returns>
        public bool ResendInvite(int inviteId, Uri inviteIndexUri)
        {
            return this.ResendInvite(this.repo.GetInvite(inviteId), inviteIndexUri);
        }


        /// <summary>
        /// The resend invite.
        /// </summary>
        /// <param name="invite"> The invite. </param>
        /// <param name="inviteIndexUri"> The invite index uri.  </param>
        /// <returns> The <see cref="bool"/>.  </returns>
        public bool ResendInvite(Invite invite, Uri inviteIndexUri)
        {
            invite.CreatedAt = DateTime.Now;
            this.repo.UpdateInvite(invite);

            var email = new InviteEmail(invite);
            return email.Send(inviteIndexUri) != null;
        }


        /// <summary>
        /// The cancel an invite.
        /// </summary>
        /// <param name="inviteId"> The invite id. </param>
        /// <returns> The <see cref="bool"/>. </returns>
        public bool CancelInvite(int inviteId)
        {
            return this.repo.RemoveInvite(inviteId);
        }


        /// <summary>
        /// Validates that a invite exists and the token is valid.
        /// </summary>
        /// <param name="inviteId"> The invite id.  </param>
        /// <param name="token"> The token.  </param>
        /// <returns> The <see cref="InviteViewModel"/>. </returns>
        public InviteViewModel ValidateInvite(int inviteId, string token)
        {
            var foundInvite = this.repo.GetInvite(inviteId);
            return (foundInvite == null || foundInvite.Token != token) 
                ? null 
                : this.conversions.ConvertDatabaseModelToViewModel(foundInvite);
        }
    }
}