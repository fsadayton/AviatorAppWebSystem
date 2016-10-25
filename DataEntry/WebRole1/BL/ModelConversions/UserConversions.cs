// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UserConversions.cs" company="UDRI">
//   Copyright © 2015 University of Dayton Research Institute. All rights reserved.
// </copyright>
// <summary>
//   The user conversions.
// </summary>
// --------------------------------------------------------------------------------------------------------------------


using System.Collections.Generic;
using Website.Models;


namespace Website.BL.ModelConversions
{
    using System.Linq;
    using DataEntry_Helpers;
    using global::Models.AccountManagement;

    /// <summary>
    /// The user conversions.
    /// </summary>
    public class UserConversions
    {
        /// <summary>
        /// Convert a create account view model to user database model.
        /// </summary>
        /// <param name="viewModel"> The view model. </param>
        /// <returns> The <see cref="User"/>. </returns>
        public User ConvertCreateAccountViewModelToUser(CreateAccountViewModel viewModel)
        {
            var convertedUser = new User
            {
                ID                  = viewModel.Id,
                FirstName           = viewModel.FirstName, 
                LastName            = viewModel.LastName,
                UserCredential      = new UserCredential { UserName = viewModel.UserName },
                ServiceProviderID   = viewModel.ProviderId
            };

            convertedUser.UserRoles.Add(new UserRole { RoleTypeID = (int)viewModel.UserType });
            return convertedUser;
        }

        /// <summary>
        /// The convert user to create account view model.
        /// </summary>
        /// <param name="databaseModel"> The database model. </param>
        /// <returns> The <see cref="CreateAccountViewModel"/>. </returns>
        public CreateAccountViewModel ConvertUserToCreateAccountViewModel(User databaseModel)
        {
            return new CreateAccountViewModel
            {
                Id          = databaseModel.ID,
                FirstName   = databaseModel.FirstName,
                LastName    = databaseModel.LastName,
                UserName    = databaseModel.UserCredential.UserName,
                UserType    = (UserRoleType)databaseModel.UserRoles.ToList().First().RoleTypeID,
                ProviderId  = databaseModel.ServiceProviderID
            };
        }

        /// <summary>
        /// The convert account view model to user.
        /// </summary>
        /// <param name="viewModel"> The view model. </param>
        /// <returns> The <see cref="User"/>. </returns>
        public User ConvertAccountViewModelToUser(AccountAdminViewModel viewModel)
        {
            var convertedUser = new User
            {
                ID = viewModel.UserId,
                FirstName = viewModel.FirstName,
                LastName = viewModel.LastName,
                ServiceProviderID = viewModel.ProviderId,
                UserRoles = new List<UserRole> { new UserRole { RoleTypeID = (int)viewModel.Role } },
                IsActive = viewModel.IsActive
            };

            return convertedUser;
        }

        /// <summary>
        /// Convert a user from the database to a login view model.
        /// </summary>
        /// <param name="databaseModel"> The database model. </param>
        /// <returns> The <see cref="LoginViewModel"/>. </returns>
        public LoginViewModel ConvertUserToLoginViewModel(User databaseModel)
        {
            if (databaseModel == null)
            {
                return null;
            }

            var userLoginInfo = new LoginViewModel
            {
                UserId = databaseModel.ID,
                Email = databaseModel.UserCredential.UserName,
                ProviderId = databaseModel.ServiceProviderID
            };

            // Build out a semicolon delimted list of roles to be used by authentication
            foreach (var curRole in databaseModel.UserRoles)
            {
                if (!string.IsNullOrEmpty(userLoginInfo.Roles))
                {
                    userLoginInfo.Roles = userLoginInfo.Roles + ";";
                }

                userLoginInfo.Roles = userLoginInfo.Roles + curRole.RoleTypeID;
            }

            return userLoginInfo;
        }

        /// <summary>
        /// The convert user to reset password view model.
        /// </summary>
        /// <param name="databaseModel"> The database model. </param>
        /// <returns> The <see cref="ResetPasswordViewModel"/>. </returns>
        public ResetPasswordViewModel ConvertUserToResetPasswordViewModel(User databaseModel)
        {
            if (databaseModel == null)
            {
                return null;
            }

            return new ResetPasswordViewModel
            {
                Email = databaseModel.UserCredential.UserName,
                Token = databaseModel.UserCredential.ResetToken,
                UserId = databaseModel.ID
            };
        }

        /// <summary>
        /// The convert reset password view model to user credential.
        /// </summary>
        /// <param name="viewModel">
        /// The view model.
        /// </param>
        /// <returns>
        /// The <see cref="UserCredential"/>.
        /// </returns>
        public UserCredential  ConvertResetPasswordViewModelToUserCredential(ResetPasswordViewModel viewModel)
        {
            if (viewModel == null)
            {
                return null;
            }

            return new UserCredential
            {
                UserName = viewModel.Email,
                ResetToken = viewModel.Token
            };
        }


        /// <summary>
        /// The convert users to account admin view model.
        /// </summary>
        /// <param name="usersList"> The users list. </param>
        /// <returns> The <see cref="List"/>. </returns>
        public List<AccountAdminViewModel> ConvertUsersToAccountAdminViewModel(List<User> usersList)
        {
            return usersList.Select(curUser => this.ConvertUserToAccountViewModel(curUser)).ToList();
        }

        /// <summary> 
        /// Convert user to account view model.
        /// </summary>
        /// <param name="userToConvert"> The user to convert. </param>
        /// <returns> The <see cref="AccountAdminViewModel"/>. </returns>
        public AccountAdminViewModel ConvertUserToAccountViewModel(User userToConvert)
        {
            var account = new AccountAdminViewModel
            {
                UserId = userToConvert.ID,
                Email = userToConvert.UserCredential.UserName,
                FirstName = userToConvert.FirstName,
                LastName = userToConvert.LastName,
                ProviderId = userToConvert.ServiceProviderID,
                Role = (UserRoleType)userToConvert.UserRoles.ToList().First().RoleTypeID,
                IsActive = userToConvert.IsActive
            };

            if (account.ProviderId != null)
            {
                account.ProviderName = userToConvert.ServiceProvider.ProviderName;
            }

            return account;
        }
    }
}