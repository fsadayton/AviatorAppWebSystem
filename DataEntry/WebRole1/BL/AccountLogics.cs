// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AccountLogics.cs" company="UDRI">
//   Copyright © 2015 University of Dayton Research Institute. All rights reserved.
// </copyright>
// <summary>
//   The account logics.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using Website.Models;

namespace Website.BL
{
    using System;
    using System.Collections.Generic;
    using DataEntry_Helpers;
    using DataEntry_Helpers.Repositories;
    using Email;
    using global::Models.AccountManagement;
    using ModelConversions;
    using global::Models;

    /// <summary>
    /// The account logics.
    /// </summary>
    public class AccountLogics
    {
        /// <summary>
        /// The user conversions class.
        /// </summary>
        private readonly UserConversions userConversions;

        /// <summary>
        /// The user repository.
        /// </summary>
        private readonly UserRepo userRepo;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountLogics"/> class.
        /// </summary>
        public AccountLogics()
        {
            this.userConversions = new UserConversions();
            this.userRepo = new UserRepo();
        }

        /// <summary>
        /// Gets a user by ID
        /// </summary>
        /// <param name="userId"> The user id. </param>
        /// <returns> The <see cref="LoginViewModel"/>. </returns>
        public LoginViewModel GetUser(int userId)
        {
            var foundUser = this.userRepo.GetUser(userId, true);
            return this.userConversions.ConvertUserToLoginViewModel(foundUser);
        }


        /// <summary>
        /// Gets a user for administration pages.  Ignores the active flag to find the user.
        /// </summary>
        /// <param name="userId"> The user id. </param>
        /// <returns> The <see cref="AccountAdminViewModel"/>. </returns>
        public AccountAdminViewModel GetUserForAdmin(int userId)
        {
            var foundUser = this.userRepo.GetUser(userId, false);
            return this.userConversions.ConvertUserToAccountViewModel(foundUser);
        }

        /// <summary> 
        /// Gets all the users for user administration. Ignores the active flag in getting the list of users.
        /// </summary>
        /// <returns> The <see cref="List"/>. </returns>
        public List<AccountAdminViewModel> GetUsersForAdmin()
        {
            var foundUsers = this.userRepo.GetUsers();
            return this.userConversions.ConvertUsersToAccountAdminViewModel(foundUsers);
        }
       

        /// <summary>
        /// Verify the login credentials.
        /// </summary>
        /// <param name="loginFields"> The login fields.   </param>
        /// <returns> The <see cref="LoginViewModel"/>.   </returns>
        public LoginViewModel Login(LoginViewModel loginFields)
        {
            // Hash the password
            var hashedPassword = PasswordHash.HashPassword(
                PasswordHash.CreateSalt(loginFields.Email.ToLower()), 
                loginFields.Password);

            // Check the password and username
            var foundUser = this.userRepo.GetUser(loginFields.Email, hashedPassword);

            if (foundUser != null)
            {
                loginFields.LoginResult = LoginResult.Success;
                loginFields.UserId = foundUser.ID;
                foreach (var curRole in foundUser.UserRoles)
                {
                    if (!string.IsNullOrEmpty(loginFields.Roles))
                    {
                        loginFields.Roles = loginFields.Roles + ";";
                    }
                    loginFields.Roles = loginFields.Roles + curRole.RoleTypeID;
                }
            }
            else
            {
                loginFields.LoginResult = LoginResult.InvalidUsernamePassword;
            }

            return loginFields;
        }

        /// <summary>
        /// Create an account from the view.
        /// </summary>
        /// <param name="createAccountViewModel"> The create account view model. </param>
        /// <param name="linkToHome"> The link To Home.  </param>
        /// <returns> The <see cref="CreateAccountViewModel"/>.   </returns>
        public CreateAccountViewModel CreateAccount(CreateAccountViewModel createAccountViewModel, Uri linkToHome)
        {
            // Convert the given View Model to the coresponding data model
            var userToCreate = this.userConversions.ConvertCreateAccountViewModelToUser(createAccountViewModel);

            // Set up the credentials with a hashed password.
            userToCreate.UserCredential = new UserCredential
            {
                UserName = createAccountViewModel.UserName, 
                PasswordHash = PasswordHash.HashPassword(PasswordHash.CreateSalt(createAccountViewModel.UserName.ToLower()), createAccountViewModel.Password)
            };

            userToCreate.IsActive = true;
            userToCreate.Contact = new Contact();

            // Create the user in the database.
            var createdUser = this.userRepo.CreateUser(userToCreate);
            var userView = this.userConversions.ConvertUserToCreateAccountViewModel(createdUser);

            if (userView != null)
            {
                var email = new AccountCreatedEmail(userView.UserName, userView.UserType);
                email.Send(linkToHome);
            }

            // Return the corresponding view model
            return userView;
        }


        /// <summary>
        ///  Updates a user's first name, last name, and active flag.
        /// </summary>
        /// <param name="accountInfo"> The account info. </param>
        /// <returns> The <see cref="bool"/>. </returns>
        public bool UpdateUser(AccountAdminViewModel accountInfo)
        {
            var databaseUser = this.userConversions.ConvertAccountViewModelToUser(accountInfo);
            databaseUser.UserRoles = null;
            databaseUser.CredentialID = 0;
            return this.userRepo.UpdateUser(databaseUser);
        }

        /// <summary>
        /// Checks if a user exists.
        /// </summary>
        /// <param name="userName"> The user name.  </param>
        /// <returns> The <see cref="bool"/>.  </returns>
        public bool DoesUserExist(string userName)
        {
           return this.userRepo.GetUser(userName) != null;
        }

        /// <summary>
        /// Send the reset password email.
        /// </summary>
        /// <param name="userName">
        /// The user name. 
        /// </param>
        /// <param name="resetEmailUri">
        /// The reset email uri. 
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool SendResetPasswordEmail(string userName, Uri resetEmailUri)
        {
            // Save the token to the user's account.
            var foundUser = this.userRepo.GetUser(userName);
            if (foundUser == null)
            {
                return false;
            }

            foundUser.UserCredential.ResetToken = Guid.NewGuid().ToString();

            var success = this.userRepo.UpdateUserCredentialResetToken(foundUser.UserCredential);

            if (success)
            {
                // Create the reset password email.
                var resetEmail = new ResetPasswordEmail(foundUser.ID, foundUser.UserCredential.UserName, foundUser.UserCredential.ResetToken);
                resetEmail.Send(resetEmailUri);
            }

            return success;
        }


        /// <summary>
        /// The validate reset password token.
        /// </summary>
        /// <param name="token">
        /// The token.
        /// </param>
        /// <param name="resetModel">
        /// The reset model.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool ValidateResetPasswordToken(string token, ResetPasswordViewModel resetModel)
        {
            return token == resetModel.Token;
        }


        /// <summary>
        /// The get user for password reset.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="ResetPasswordViewModel"/>.
        /// </returns>
        public ResetPasswordViewModel GetUserForPasswordReset(int? id)
        {
            if (id == null)
            {
                return null;
            }

            var user = this.userRepo.GetUser(id.GetValueOrDefault(), true);
            return this.userConversions.ConvertUserToResetPasswordViewModel(user);
        }

        /// <summary>
        /// The update user credentials.
        /// </summary>
        /// <param name="model">The model. </param>
        /// <returns> The <see cref="bool"/>. </returns>
        public bool UpdateUserCredentials(ResetPasswordViewModel model)
        {
            var user = this.userRepo.GetUser(model.UserId, true);
            
            model.Token = null;
            var userCredential = this.userConversions.ConvertResetPasswordViewModelToUserCredential(model);
            userCredential.ID = user.CredentialID;
            userCredential.PasswordHash = PasswordHash.HashPassword(PasswordHash.CreateSalt(model.Email.ToLower()), model.Password);
            return this.userRepo.UpdateUserCredentialResetPassword(userCredential);
        }


        /// <summary>
        /// Search all users.  Searches first name, last name and service provider name if applicable.
        /// </summary>
        /// <param name="searchString"> The search string. </param>
        /// <returns> The <see cref="List"/>. </returns>
        public List<AccountAdminViewModel> SearchUsers(string searchString)
        {
            searchString = searchString ?? string.Empty;
            var databaseUsers = this.userRepo.GetUsers(searchString);
            return this.userConversions.ConvertUsersToAccountAdminViewModel(databaseUsers);
        }
    }
}