// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UserConversionsTest.cs" company="UDRI">
//   Copyright © 2015 University of Dayton Research Institute. All rights reserved.
// </copyright>
// <summary>
//   The user conversions test.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Linq;

namespace DataEntry.Tests.BL.ModelConversions
{
    using System;
    using System.Collections.Generic;
    using DataEntry_Helpers;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Models.AccountManagement;
    using Website.BL.ModelConversions;
    using Website.Models;

    /// <summary>
    /// The user conversions test.
    /// </summary>
    [TestClass]
    public class UserConversionsTest
    {
        /// <summary>
        /// The create account view model.
        /// </summary>
        private CreateAccountViewModel createAccountViewModel;

        /// <summary>
        /// The account admin view model.
        /// </summary>
        private AccountAdminViewModel accountAdminViewModel;

        /// <summary>
        /// The user.
        /// </summary>
        private User user;

        /// <summary>
        /// The conversions to test.
        /// </summary>
        private UserConversions conversions;


        /// <summary>
        /// The test initializer.
        /// </summary>
        [TestInitialize]
        public void TestInitialize()
        {
            this.conversions = new UserConversions();

            this.createAccountViewModel = new CreateAccountViewModel
            {
                Id = 1, 
                Password = "password", 
                ConfirmPassword = "password", 
                FirstName = "First", 
                LastName = "Last", 
                UserName = "test@example.com",
                UserType = UserRoleType.Admin
            };

            this.user = new User
            {
                ID = 1, 
                Contact = new Contact(), 
                FirstName = "First", 
                LastName = "Last", 
                UserCredential = new UserCredential { ID = 123, PasswordHash = "a298rasdfjkhn", UserName = "test@example.com", ResetToken = Guid.NewGuid().ToString() },
                UserRoles = new List<UserRole> { new UserRole { RoleTypeID = 1, UserID = 1 } },
                ServiceProviderID = null,
                IsActive = true
            };

            this.accountAdminViewModel = new AccountAdminViewModel
            {
                UserId = 1,
                Email = "test@example.com",
                FirstName = "First",
                LastName = "Last",
                IsActive = true,
                ProviderId = null,
                ProviderName = string.Empty,
                Role = UserRoleType.Admin
            };
        }

        /// <summary>
        /// The convert create account view model to user test.
        /// </summary>
        [TestMethod]
        public void ConvertCreateAccountViewModelToUserTest()
        {
            var result = this.conversions.ConvertCreateAccountViewModelToUser(this.createAccountViewModel);
            Assert.AreEqual(this.user.ID, result.ID);
            Assert.AreEqual(this.user.UserCredential.UserName, result.UserCredential.UserName);
            Assert.AreEqual(this.user.FirstName, result.FirstName);
            Assert.AreEqual(this.user.LastName, result.LastName);
        }

        /// <summary>
        /// The convert user to create account view model test.
        /// </summary>
        [TestMethod]
        public void ConvertUserToCreateAccountViewModelTest()
        {
            var result = this.conversions.ConvertUserToCreateAccountViewModel(this.user);
            Assert.AreEqual(this.createAccountViewModel.Id, result.Id);
            Assert.AreEqual(this.createAccountViewModel.UserName, result.UserName);
            Assert.AreEqual(this.createAccountViewModel.FirstName, result.FirstName);
            Assert.AreEqual(this.createAccountViewModel.LastName, result.LastName);
        }

        /// <summary>
        /// Test to convert a database user to a login view model
        /// </summary>
        [TestMethod]
        public void ConvertUserToLoginViewModelTest()
        {
            var result = this.conversions.ConvertUserToLoginViewModel(this.user);
            Assert.AreEqual(this.user.ID, result.UserId);
            Assert.AreEqual(this.user.ServiceProviderID, result.ProviderId);
            Assert.AreEqual(this.user.UserCredential.UserName, result.Email);
            Assert.AreEqual("1", result.Roles);
        }



        /// <summary>
        /// Test to convert a database user with multiple roles to a login view model
        /// </summary>
        [TestMethod]
        public void ConvertUserMultipleRolesToLoginViewModelTest()
        {
            this.user.UserRoles.Add(new UserRole {ID = 12, RoleTypeID = 2, UserID = 1});
            var result = this.conversions.ConvertUserToLoginViewModel(this.user);
            Assert.AreEqual(this.user.ID, result.UserId);
            Assert.AreEqual(this.user.ServiceProviderID, result.ProviderId);
            Assert.AreEqual(this.user.UserCredential.UserName, result.Email);
            Assert.AreEqual("1;2", result.Roles);
        }


        /// <summary>
        /// The convert database users to account admin view model test.
        /// </summary>
        [TestMethod]
        public void ConvertUsersToAccountAdminViewModelTest()
        {
            var userList = new List<User> { this.user };

            var result = this.conversions.ConvertUsersToAccountAdminViewModel(userList)[0];
            Assert.AreEqual(this.user.ID, result.UserId);
            Assert.AreEqual(this.user.UserCredential.UserName, result.Email);
            Assert.AreEqual(this.user.FirstName, result.FirstName);
            Assert.AreEqual(this.user.LastName, result.LastName);
            Assert.AreEqual(this.user.IsActive, result.IsActive);
            Assert.AreEqual(this.user.ServiceProviderID, result.ProviderId);
            Assert.AreEqual(this.user.UserRoles.First().RoleTypeID, (int)result.Role);
        }

        /// <summary>
        /// The convert database user to account view model test.
        /// </summary>
        [TestMethod]
        public void ConvertUserToAccountViewModelTest()
        {
            var result = this.conversions.ConvertUserToAccountViewModel(this.user);
            Assert.AreEqual(this.user.ID, result.UserId);
            Assert.AreEqual(this.user.UserCredential.UserName, result.Email);
            Assert.AreEqual(this.user.FirstName, result.FirstName);
            Assert.AreEqual(this.user.LastName, result.LastName);
            Assert.AreEqual(this.user.IsActive, result.IsActive);
            Assert.AreEqual(this.user.ServiceProviderID, result.ProviderId);
            Assert.AreEqual(this.user.UserRoles.First().RoleTypeID, (int)result.Role);
        }

        /// <summary>
        /// The convert account view model to database user test.
        /// </summary>
        [TestMethod]
        public void ConvertAccountViewModelToUserTest()
        {
            var result = this.conversions.ConvertAccountViewModelToUser(this.accountAdminViewModel);
            Assert.AreEqual(this.accountAdminViewModel.UserId, result.ID);
            Assert.AreEqual(this.accountAdminViewModel.FirstName, result.FirstName);
            Assert.AreEqual(this.accountAdminViewModel.LastName, result.LastName);
            Assert.AreEqual(this.accountAdminViewModel.IsActive, result.IsActive);
            Assert.AreEqual(this.accountAdminViewModel.ProviderId, result.ServiceProviderID);
            Assert.AreEqual((int)this.accountAdminViewModel.Role, result.UserRoles.First().RoleTypeID);
        }

        /// <summary>
        /// Test converting a database user to a reset password view model.
        /// </summary>
        [TestMethod]
        public void ConvertUserToResetPasswordViewModelTest()
        {
            var result = this.conversions.ConvertUserToResetPasswordViewModel(this.user);
            Assert.AreEqual(this.user.UserCredential.UserName, result.Email);
            Assert.AreEqual(this.user.UserCredential.ResetToken, result.Token);
            Assert.AreEqual(this.user.ID, result.UserId);
        }

        /// <summary>
        /// The convert reset password view model to user test.
        /// </summary>
        [TestMethod]
        public void ConvertResetPasswordViewModelToUserTest()
        {
            var model = new ResetPasswordViewModel
            {
                Email = this.user.UserCredential.UserName,
                Token = this.user.UserCredential.ResetToken
            };
            var result = this.conversions.ConvertResetPasswordViewModelToUserCredential(model);
            Assert.AreEqual(this.user.UserCredential.UserName, result.UserName);
            Assert.AreEqual(this.user.UserCredential.ResetToken, result.ResetToken);
        }
    }
}
