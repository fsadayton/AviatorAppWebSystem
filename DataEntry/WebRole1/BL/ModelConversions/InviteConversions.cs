// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InviteConversions.cs" company="UDRI">
//   Copyright © 2015 University of Dayton Research Institute. All rights reserved.
// </copyright>
// <summary>
//   The invite conversions between database and view models.
// </summary>
// --------------------------------------------------------------------------------------------------------------------


namespace Website.BL.ModelConversions
{
    using System.Collections.Generic;
    using System.Linq;
    using DataEntry_Helpers;
    using global::Models.AccountManagement;

    /// <summary>
    /// The invite conversions between database and view models.
    /// </summary>
    public class InviteConversions
    {
        /// <summary>
        /// The convert database invite model to invite view model.
        /// </summary>
        /// <param name="databaseModel">
        /// The database model. 
        /// </param>
        /// <returns>
        /// The <see cref="InviteViewModel"/>. 
        /// </returns>
        public InviteViewModel ConvertDatabaseModelToViewModel(Invite databaseModel)
        {
            return new InviteViewModel
            {
                Id = databaseModel.ID, 
                UserRoleType = (UserRoleType)databaseModel.RoleTypeID, 
                InviteeEmailAddress = databaseModel.Email,
                DateSent = databaseModel.CreatedAt,
                ServiceProviderId = databaseModel.ProviderId
            };
        }

        /// <summary>
        /// The convert database model list to view model list.
        /// </summary>
        /// <param name="databaseModels"> The database models list. </param>
        /// <returns> The <see cref="List"/>. </returns>
        public List<InviteViewModel> ConvertDatabaseModelListToViewModelList(List<Invite> databaseModels)
        {
            return (databaseModels == null) ? null : databaseModels.Select(this.ConvertDatabaseModelToViewModel).ToList();
        }


        /// <summary>
        /// The convert view model to database model.
        /// </summary>
        /// <param name="viewModel"> The view model. </param>
        /// <returns> The <see cref="Invite"/>. </returns>
        public Invite ConvertViewModelToDatabaseModel(InviteViewModel viewModel)
        {
            return new Invite
            {
                ID = viewModel.Id,
                Email = viewModel.InviteeEmailAddress,
                RoleTypeID = (int)viewModel.UserRoleType,
                CreatedAt = viewModel.DateSent,
                ProviderId = viewModel.ServiceProviderId
            };
        }
    }
}