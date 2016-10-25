// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AccountControllerTests.cs" company="UDRI">
//   Copyright © 2015 University of Dayton Research Institute. All rights reserved.
// </copyright>
// <summary>
//   The account controller tests.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DataEntry.Tests.Controllers
{
    using System;
    using System.Web.Mvc;
    using System.Web.Security.Fakes;
    using Microsoft.QualityTools.Testing.Fakes;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Models.AccountManagement;
    using Website.BL.Fakes;
    using Website.Controllers;
    using Website.Models;

    /// <summary>
    /// The account controller tests.
    /// </summary>
    [TestClass]
    public class AccountControllerTests
    {
        /// <summary>
        /// The account controller.
        /// </summary>
        private AccountController accountController;

        /// <summary>
        /// The login view model.
        /// </summary>
        private LoginViewModel loginViewModel;

        /// <summary>
        /// The create account view model.
        /// </summary>
        private CreateAccountViewModel createAccountViewModel;

        /// <summary>
        /// The test initialize.
        /// </summary>
        [TestInitialize]
        public void TestInitialize()
        {
            this.accountController = new AccountController(new Uri("http://localhost/famservices/Web/Index"));
            this.loginViewModel = new LoginViewModel
            {
                UserId = 123,
                Email = "test@example.com",
                LoginResult = LoginResult.Success,
                Password = "asdfasf",
                Roles = "1"
            };
            this.createAccountViewModel = new CreateAccountViewModel
            {
                Id = 123,
                FirstName = "First",
                LastName = "last",
                Password = "asdfasdf",
                ConfirmPassword = "asdfasdf",
                UserName = "test@example.com",
                UserType = UserRoleType.Admin
            };
        }

        /// <summary>
        /// The get login test.
        /// </summary>
        [TestMethod]
        public void GetLoginTest()
        {
            var result = this.accountController.Login(null) as ViewResult;
            Assert.IsNotNull(result);
            Assert.IsNull(result.ViewBag.ReturnUrl);
        }

        /// <summary>
        /// The get login test.
        /// </summary>
        [TestMethod]
        public void GetLoginRedirectTest()
        {
            var urlString = "TestString";
            var result = this.accountController.Login(urlString) as ViewResult;
            Assert.IsNotNull(result);
            Assert.AreEqual(urlString, result.ViewBag.ReturnUrl);
        }

        /// <summary>
        /// The post login test. Not currently running due to not being able to mock adding the authentication cookie
        /// </summary>
        public void PostLoginTest()
        {
            using (ShimsContext.Create())
            {
                ShimFormsAuthentication.SetAuthCookieStringBooleanString = (s, b, arg3) => { };
                ShimAccountLogics.AllInstances.LoginLoginViewModel = (logics, model) => this.loginViewModel;
                

                var result = this.accountController.Login(this.loginViewModel, null);

                Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
                var routeResult = result as RedirectToRouteResult;
                Assert.IsNotNull(routeResult);
                Assert.AreEqual(routeResult.RouteValues["action"], "Index");
                Assert.AreEqual(routeResult.RouteValues["controller"], "Web");
            }
        }

        /// <summary>
        /// The post login with invalid password test.
        /// </summary>
        [TestMethod]
        public void PostLoginInvalidPasswordTest()
        {
            using (ShimsContext.Create())
            {
                ShimFormsAuthentication.SetAuthCookieStringBooleanString = (s, b, arg3) => { };
                this.loginViewModel.LoginResult = LoginResult.InvalidUsernamePassword;
                ShimAccountLogics.AllInstances.LoginLoginViewModel = (logics, model) => this.loginViewModel;

                var result = this.accountController.Login(this.loginViewModel, null);

                Assert.IsInstanceOfType(result, typeof(ViewResult));
                var routeResult = result as ViewResult;
                Assert.IsNotNull(routeResult);
                var loginModel = routeResult.Model as LoginViewModel;
                Assert.IsNotNull(this.loginViewModel);
                Assert.AreEqual(this.loginViewModel.Email, loginModel.Email);
            }
        }

        /// <summary>
        /// Get register test.
        /// </summary>
        [TestMethod]
        public void GetRegisterTest()
        {
            var result = this.accountController.Register(null, null) as ViewResult;
            Assert.IsNotNull(result);
        }

        /// <summary>
        /// The post register test. Not currently running due to not being able to mock adding the authentication cookie
        /// </summary>
        public void PostRegisterTest()
        {
            using (ShimsContext.Create())
            {
                ShimFormsAuthentication.SetAuthCookieStringBooleanString = (s, b, arg3) => { };
                ShimAccountLogics.AllInstances.DoesUserExistString = (logics, s) => false;
                ShimAccountLogics.AllInstances.CreateAccountCreateAccountViewModelUri = (logics, model, arg3) => this.createAccountViewModel;

                var result = this.accountController.Register(this.createAccountViewModel);

                Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
                var routeResult = result as RedirectToRouteResult;
                Assert.IsNotNull(routeResult);
                Assert.AreEqual(routeResult.RouteValues["action"], "Index");
                Assert.AreEqual(routeResult.RouteValues["controller"], "Web");
            }
        }

        /// <summary>
        /// The post register test.  Not currently running due to not being able to mock adding the authentication cookie
        /// </summary>
        [TestMethod]
        public void PostRegisterExistingAccountTest()
        {
            using (ShimsContext.Create())
            {
                ShimFormsAuthentication.SetAuthCookieStringBooleanString = (s, b, arg3) => { };
                ShimAccountLogics.AllInstances.DoesUserExistString = (logics, s) => true;
                ShimAccountLogics.AllInstances.CreateAccountCreateAccountViewModelUri = (logics, model, arg3) => this.createAccountViewModel;

                var result = this.accountController.Register(this.createAccountViewModel);

                Assert.IsInstanceOfType(result, typeof(ViewResult));
                var routeResult = result as ViewResult;
                Assert.IsNotNull(routeResult);
            }
        }

        /// <summary>
        /// The get forgot password test.
        /// </summary>
        [TestMethod]
        public void GetForgotPasswordTest()
        {
            var result = this.accountController.ForgotPassword();
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }


        /// <summary>
        /// The post forgot password test.
        /// </summary>
        [TestMethod]
        public void PostForgotPasswordTest()
        {
            var sampleForgotPassword = new ForgotPasswordViewModel
            {
                Email = "test@test.com"
            };

            using (ShimsContext.Create())
            {
                ShimAccountLogics.AllInstances.DoesUserExistString = (logics, s) => true;
                ShimAccountLogics.AllInstances.SendResetPasswordEmailStringUri = (logics, s, arg3) => true;
                var result = this.accountController.ForgotPassword(sampleForgotPassword);
                Assert.IsInstanceOfType(result, typeof(ViewResult));
            }
        }


        /// <summary>
        /// The post forgot password invalid email test.
        /// </summary>
        [TestMethod]
        public void PostForgotPasswordInvalidEmailTest()
        {
            var sampleForgotPassword = new ForgotPasswordViewModel
            {
                Email = "test@test.com"
            };

            using (ShimsContext.Create())
            {
                ShimAccountLogics.AllInstances.DoesUserExistString = (logics, s) => false;
                ShimAccountLogics.AllInstances.SendResetPasswordEmailStringUri = (logics, s, arg3) => true;
                var result = this.accountController.ForgotPassword(sampleForgotPassword);
                Assert.IsInstanceOfType(result, typeof(ViewResult));

                var view = result as ViewResult;
                Assert.IsTrue(view.ViewBag.Error.Contains("not found"));
            }
        }

        /// <summary>
        /// The post forgot password email fail test.
        /// </summary>
        [TestMethod]
        public void PostForgotPasswordEmailFailTest()
        {
            var sampleForgotPassword = new ForgotPasswordViewModel
            {
                Email = "test@test.com"
            };

            using (ShimsContext.Create())
            {
                ShimAccountLogics.AllInstances.DoesUserExistString = (logics, s) => true;
                ShimAccountLogics.AllInstances.SendResetPasswordEmailStringUri = (logics, s, arg3) => false;
                var result = this.accountController.ForgotPassword(sampleForgotPassword);
                Assert.IsInstanceOfType(result, typeof(ViewResult));

                var view = result as ViewResult;
                Assert.IsTrue(view.ViewBag.Error.Contains("try again"));
            }
        }


        /// <summary>
        /// The get reset password test.
        /// </summary>
        [TestMethod]
        public void GetResetPasswordTest()
        {
            var token = Guid.NewGuid().ToString();

            var sampleResetPassViewModel = new ResetPasswordViewModel
            {
                Email = "test@test.com",
                Password = "TestPass",
                Token = Guid.NewGuid().ToString(),
                UserId = 1
            };
            using (ShimsContext.Create())
            {
                ShimAccountLogics.AllInstances.GetUserForPasswordResetNullableOfInt32 = (logics, i) => sampleResetPassViewModel;
                ShimAccountLogics.AllInstances.ValidateResetPasswordTokenStringResetPasswordViewModel = (logics, s, arg3) => true;
                var result = this.accountController.ResetPassword(1, token);
                Assert.IsInstanceOfType(result, typeof(ViewResult));
                var view = result as ViewResult;
                Assert.IsTrue(view.ViewBag.Error == null);
            }
        }

        /// <summary>
        /// Tests the get reset password with no id.
        /// </summary>
        [TestMethod]
        public void GetResetPasswordWithNoIdTest()
        {
            var token = Guid.NewGuid().ToString();
            using (ShimsContext.Create())
            {
                var result = this.accountController.ResetPassword(null, token);
                Assert.IsInstanceOfType(result, typeof(ViewResult));
                var view = result as ViewResult;

                Assert.IsTrue(view.ViewBag.Error.Contains("No account found"));
            }
        }

        /// <summary>
        /// Test the get reset password with an invalid user.
        /// </summary>
        [TestMethod]
        public void GetResetPasswordWithInvalidTokenTest()
        {
            var token = Guid.NewGuid().ToString();

            using (ShimsContext.Create())
            {
                ResetPasswordViewModel foundUser = new ResetPasswordViewModel
                {
                    Email = "test@test.com",
                    Token = "asdfasghaipubehr2134",
                    UserId = 1,
                    Password = "adpshyasdfgjkasdf"
                };
                ShimAccountLogics.AllInstances.GetUserForPasswordResetNullableOfInt32 = (logics, i) => foundUser;
                ShimAccountLogics.AllInstances.ValidateResetPasswordTokenStringResetPasswordViewModel = (logics, s, arg3) => false;
                var result = this.accountController.ResetPassword(1, token);
                Assert.IsInstanceOfType(result, typeof(ViewResult));
                var view = result as ViewResult;
                Assert.IsTrue(view.ViewBag.Error.Contains("Please request to reset your password again"));
            }
        }

        /// <summary>
        /// The post reset password test.
        /// </summary>
        [TestMethod]
        public void PostResetPasswordTest()
        {
            var sampleResetPassViewModel = new ResetPasswordViewModel
            {
                Email = "test@test.com",
                Password = "TestPass",
                Token = Guid.NewGuid().ToString(),
                UserId = 1
            };
            using (ShimsContext.Create())
            {
                ShimAccountLogics.AllInstances.UpdateUserCredentialsResetPasswordViewModel = (logics, model) => true;
                var result = this.accountController.ResetPassword(sampleResetPassViewModel);
                Assert.IsInstanceOfType(result, typeof(ViewResult));
                var view = result as ViewResult;
                Assert.IsTrue(view.ViewBag.Error == null);
            }
        }


        /// <summary>
        /// The post reset password with a database fail.
        /// </summary>
        [TestMethod]
        public void PostResetPasswordDbFailTest()
        {
            var sampleResetPassViewModel = new ResetPasswordViewModel
            {
                Email = "test@test.com",
                Password = "TestPass",
                Token = Guid.NewGuid().ToString(),
                UserId = 1
            };
            using (ShimsContext.Create())
            {
                ShimAccountLogics.AllInstances.UpdateUserCredentialsResetPasswordViewModel = (logics, model) => false;
                var result = this.accountController.ResetPassword(sampleResetPassViewModel);
                Assert.IsInstanceOfType(result, typeof(ViewResult));
                var view = result as ViewResult;
                Assert.IsTrue(view.ViewBag.Error != null);
            }
        }
    }
}
