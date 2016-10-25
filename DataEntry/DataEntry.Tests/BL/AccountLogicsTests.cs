// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AccountLogicsTests.cs" company="UDRI">
//   Copyright © 2015 University of Dayton Research Institute. All rights reserved.
// </copyright>
// <summary>
//   The account logics tests.
// </summary>
// --------------------------------------------------------------------------------------------------------------------


using Website.Models;

namespace DataEntry.Tests.BL
{
    using System;
    using System.Collections.Generic;
    using DataEntry_Helpers;
    using DataEntry_Helpers.Repositories.Fakes;
    using Microsoft.QualityTools.Testing.Fakes;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Models.AccountManagement;
    using Website.BL;
    using Website.BL.Email.Fakes;
    using Website.BL.ModelConversions.Fakes;

    /// <summary>
    /// The test for Account Logics
    /// </summary>
    [TestClass]
    public class AccountLogicsTests
    {
        /// <summary>
        /// The logics to test
        /// </summary>
        private AccountLogics logics;

        /// <summary>
        /// The sample database user.
        /// </summary>
        private User sampleDatabaseUser;

        /// <summary>
        /// The login view model.
        /// </summary>
        private LoginViewModel loginViewModel;


        /// <summary>
        /// The create account view model.
        /// </summary>
        private CreateAccountViewModel createAccountViewModel;


        /// <summary>
        /// The account admin view model.
        /// </summary>
        private AccountAdminViewModel accountAdminViewModel;

        /// <summary>
        /// The Reset Password View Model.
        /// </summary>
        private ResetPasswordViewModel sampleResetPasswordModel;

        private string guid;

        /// <summary>
        /// The test initialize.
        /// </summary>
        [TestInitialize]
        public void TestInitialize()
        {
            this.logics = new AccountLogics();
            this.guid = Guid.NewGuid().ToString();

            this.sampleDatabaseUser = new User
            {
                ID = 123,
                Contact = new Contact { Phone = "555-555-5555" },
                UserCredential = new UserCredential { ID = 4, PasswordHash = "asdfasdf asfasf", UserName = "asdf@test.com" },
                FirstName = "First",
                LastName = "Last",
                UserRoles = new List<UserRole> { new UserRole { RoleTypeID = 1, UserID = 123 } },
                ServiceProviderID = 2
            };

            this.loginViewModel = new LoginViewModel
            {
                UserId = 123,
                Email = "asdf@test.com",
                Password = "asfasfgasdf"
            };

            this.createAccountViewModel = new CreateAccountViewModel
            {
                Id = 0,
                Password = "afdasdf",
                ConfirmPassword = "afdasdf",
                FirstName = "First",
                LastName = "Last",
                UserName = "asdf@test.com",
                UserType = UserRoleType.Admin
            };

            this.accountAdminViewModel = new AccountAdminViewModel
            {
                Email = "asdf@test.com",
                FirstName = "First",
                LastName = "Last",
                ProviderId = 2,
                ProviderName = "Test Provider",
                Role = UserRoleType.Provider,
                UserId = 123
            };


            this.sampleResetPasswordModel = new ResetPasswordViewModel
            {
                UserId = this.sampleDatabaseUser.ID,
                Password = "test",
                Email = this.sampleDatabaseUser.UserCredential.UserName,
                Token = this.guid
            };
        }

        /// <summary>
        /// Login successful Test
        /// </summary>
        [TestMethod]
        public void LoginSuccessTest()
        {
            using (ShimsContext.Create())
            {
                ShimUserRepo.AllInstances.GetUserStringString = (repo, s, arg3) => this.sampleDatabaseUser;
                var result = this.logics.Login(this.loginViewModel);

                Assert.AreEqual(LoginResult.Success, result.LoginResult);
            }
        }

        /// <summary>
        /// Unsuccessful login test.
        /// </summary>
        [TestMethod]
        public void LoginUnsuccessfulTest()
        {
            using (ShimsContext.Create())
            {
                ShimUserRepo.AllInstances.GetUserStringString = (repo, s, arg3) => null;
                var result = this.logics.Login(this.loginViewModel);

                Assert.AreEqual(LoginResult.InvalidUsernamePassword, result.LoginResult);
            }
        }


        /// <summary>
        /// The create account test.
        /// </summary>
        [TestMethod]
        public void CreateAccountTest()
        {
            using (ShimsContext.Create())
            {
                ShimUserRepo.AllInstances.CreateUserUser = (repo, user) => this.sampleDatabaseUser;
                ShimAccountCreatedEmail.AllInstances.SendUri = (email, uri) => "body of the email";
                var result = this.logics.CreateAccount(this.createAccountViewModel, new Uri("http://localhost/famservices/Web/Index"));
                Assert.AreEqual(this.createAccountViewModel.UserName, result.UserName);
            }
        }

        /// <summary>
        /// The does user exist test.
        /// </summary>
        [TestMethod]
        public void DoesUserExistTest()
        {
            using (ShimsContext.Create())
            {
                ShimUserRepo.AllInstances.GetUserString = (repo, s) => this.sampleDatabaseUser;
                Assert.IsTrue(this.logics.DoesUserExist("asdfs@asdf.com"));

                ShimUserRepo.AllInstances.GetUserString = (repo, s) => null;
                Assert.IsFalse(this.logics.DoesUserExist("asdfs@asdf.com"));
            }
        }

        /// <summary>
        /// The get user test.
        /// </summary>
        [TestMethod]
        public void GetUserTest()
        {
            using (ShimsContext.Create())
            {
                ShimUserRepo.AllInstances.GetUserInt32Boolean = (repo, i, arg3) => this.sampleDatabaseUser; 
                var user = this.logics.GetUser(123);
                Assert.AreEqual(123, user.UserId);
                Assert.AreEqual(2, user.ProviderId);
            }
        }

        /// <summary>
        /// The get user for password reset test.
        /// </summary>
        [TestMethod]
        public void GetUserForPasswordResetTest()
        {
            using (ShimsContext.Create())
            {
                ShimUserRepo.AllInstances.GetUserInt32Boolean = (repo, i, checkActive) => this.sampleDatabaseUser;
                var user = this.logics.GetUserForPasswordReset(123);
                Assert.AreEqual(this.sampleDatabaseUser.ID, user.UserId);
            }
        }

        /// <summary>
        /// The update user credentials test.
        /// </summary>
        [TestMethod]
        public void UpdateUserCredentialsTest()
        {
            using (ShimsContext.Create())
            {
                var viewModel = new ResetPasswordViewModel
                {
                    UserId = this.sampleDatabaseUser.ID,
                    Password = "test",
                    Email = this.sampleDatabaseUser.UserCredential.UserName,
                    Token = Guid.NewGuid().ToString()
                };
                ShimUserRepo.AllInstances.GetUserInt32Boolean = (repo, i, checkActive) => this.sampleDatabaseUser;
                ShimUserRepo.AllInstances.UpdateUserCredentialResetPasswordUserCredential = (repo, credential) => true;
                var results = this.logics.UpdateUserCredentials(viewModel);
                Assert.IsTrue(results);
            }
        }

        /// <summary>
        /// The validate reset password token test.
        /// </summary>
        [TestMethod]
        public void ValidateResetPasswordTokenTest()
        {
            var result = this.logics.ValidateResetPasswordToken(this.guid, this.sampleResetPasswordModel);
            Assert.IsTrue(result);
        }

        /// <summary>
        /// The validate reset password token test.
        /// </summary>
        [TestMethod]
        public void ValidateResetPasswordTokenInvalidTest()
        {
            var result = this.logics.ValidateResetPasswordToken(Guid.NewGuid().ToString(), this.sampleResetPasswordModel);
            Assert.IsFalse(result);
        }

        /// <summary>
        /// The send reset password email test.
        /// </summary>
        [TestMethod]
        public void SendResetPasswordEmailTest()
        {
            using (ShimsContext.Create())
            {
                ShimResetPasswordEmail.AllInstances.SendUri = (email, uri) => "Body text";
                ShimUserRepo.AllInstances.UpdateUserCredentialResetTokenUserCredential = (repo, credential) => true;
                ShimUserRepo.AllInstances.GetUserString = (repo, username) => this.sampleDatabaseUser;
                var results = this.logics.SendResetPasswordEmail(this.sampleDatabaseUser.UserCredential.UserName, new Uri("http://localhost/"));
                Assert.IsTrue(results);
            }
        }


        /// <summary>
        /// The send reset password email with database failure test.
        /// </summary>
        [TestMethod]
        public void SendResetPasswordEmailDbFailureTest()
        {
            using (ShimsContext.Create())
            {
                ShimResetPasswordEmail.AllInstances.SendUri = (email, uri) => "Body text";
                ShimUserRepo.AllInstances.UpdateUserCredentialResetTokenUserCredential = (repo, credential) => false;
                ShimUserRepo.AllInstances.GetUserString = (repo, username) => this.sampleDatabaseUser;
                var results = this.logics.SendResetPasswordEmail(this.sampleDatabaseUser.UserCredential.UserName, new Uri("http://localhost/"));
                Assert.IsFalse(results);
            }
        }

        /// <summary>
        /// The get users for administration test.
        /// </summary>
        [TestMethod]
        public void GetUsersForAdminTest()
        {
            var sampleAccounts = new List<AccountAdminViewModel>();
            var userList = new List<User> { this.sampleDatabaseUser };
            sampleAccounts.Add(this.accountAdminViewModel);
            using (ShimsContext.Create())
            {
                ShimUserRepo.AllInstances.GetUsers = repo => userList;
                ShimUserConversions.AllInstances.ConvertUsersToAccountAdminViewModelListOfUser =
                    (conversions, list) => sampleAccounts;

                var results = this.logics.GetUsersForAdmin();
                Assert.AreEqual(results[0].ProviderName, sampleAccounts[0].ProviderName);
            }
        }

        /// <summary>
        /// The update a user test.
        /// </summary>
        [TestMethod]
        public void UpdateUserTest()
        {
            using (ShimsContext.Create())
            {
                ShimUserConversions.AllInstances.ConvertAccountViewModelToUserAccountAdminViewModel =
                    (conversions, model) => this.sampleDatabaseUser;
                ShimUserRepo.AllInstances.UpdateUserUser = (repo, user) => true;
                var result = this.logics.UpdateUser(this.accountAdminViewModel);
                Assert.IsTrue(result);
            }
        }


        /// <summary>
        /// The search users test.
        /// </summary>
        [TestMethod]
        public void SearchUsersTest()
        {
            var sampleAccounts = new List<AccountAdminViewModel>();
            var userList = new List<User> { this.sampleDatabaseUser };
            sampleAccounts.Add(this.accountAdminViewModel);
            using (ShimsContext.Create())
            {
                ShimUserRepo.AllInstances.GetUsersString = (repo, s) => userList;
                ShimUserConversions.AllInstances.ConvertUsersToAccountAdminViewModelListOfUser =
                    (conversions, list) => sampleAccounts;
                 var results = this.logics.SearchUsers("test");
                Assert.AreEqual(results[0].ProviderName, sampleAccounts[0].ProviderName);
            }
        }
    }
}
