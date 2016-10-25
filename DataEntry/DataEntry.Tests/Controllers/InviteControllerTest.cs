// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InviteControllerTest.cs" company="UDRI">
//   Copyright © 2015 University of Dayton Research Institute. All rights reserved.
// </copyright>
// <summary>
//   Defines the InviteControllerTest type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DataEntry.Tests.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Microsoft.QualityTools.Testing.Fakes;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Models.AccountManagement;
    using Website.BL.Fakes;
    using Website.Controllers;

    /// <summary>
    /// The invite controller test.
    /// </summary>
    [TestClass]
    public class InviteControllerTest
    {
        /// <summary>
        /// The controller to be tested.
        /// </summary>
        private InviteController controller;

        /// <summary>
        /// The sample invite from the database.
        /// </summary>
        private InviteViewModel sampleInvite;

        /// <summary>
        /// A second sample invite from the database.
        /// </summary>
        private InviteViewModel sampleInvite2;

        /// <summary>
        /// The test initialize.
        /// </summary>
        [TestInitialize]
        public void TestInitialize()
        {
            this.controller = new InviteController(new Uri("http://localhost/famservices/Invite/Index"));

            this.sampleInvite = new InviteViewModel
            {
                Id = 1, 
                DateSent = DateTime.Now, 
                InviteeEmailAddress = "test@example.com", 
                UserRoleType = UserRoleType.Admin
            };

            this.sampleInvite2 = new InviteViewModel
            {
                Id = 2, 
                DateSent = DateTime.Now, 
                InviteeEmailAddress = "tes2t@example.com", 
                UserRoleType = UserRoleType.Provider
            };
        }

        /// <summary>
        /// The get index test.
        /// </summary>
        [TestMethod]
        public void GetIndexSingleTest()
        {
            using (ShimsContext.Create())
            {
                var invitesFound = new List<InviteViewModel> { this.sampleInvite };
                ShimInviteLogics.AllInstances.GetInvitesByUserInt32 = (logics, i) => invitesFound;

                var result = this.controller.Index();
                Assert.IsNotNull(result);
                Assert.IsInstanceOfType(result, typeof(ViewResult));
                var model = (result as ViewResult).Model as List<InviteViewModel>;
                Assert.IsNotNull(model);
                Assert.AreEqual(1, model.Count);
                Assert.AreEqual(1, model[0].Id);
            }
        }

        /// <summary>
        /// The get index with multiple invites.
        /// </summary>
        [TestMethod]
        public void GetIndexMultipleTest()
        {
            using (ShimsContext.Create())
            {
                var invitesFound = new List<InviteViewModel> { this.sampleInvite, this.sampleInvite2 };
                ShimInviteLogics.AllInstances.GetInvitesByUserInt32 = (logics, i) => invitesFound;

                var result = this.controller.Index();
                Assert.IsNotNull(result);
                Assert.IsInstanceOfType(result, typeof(ViewResult));
                var model = (result as ViewResult).Model as List<InviteViewModel>;
                Assert.IsNotNull(model);
                Assert.AreEqual(2, model.Count);
            }
        }

        /// <summary>
        /// Test for the get create.
        /// </summary>
        [TestMethod]
        public void GetCreateTest()
        {
            var result = this.controller.Create((int)UserRoleType.Admin);
            var model = (result as ViewResult).Model as InviteViewModel;
            Assert.AreEqual(UserRoleType.Admin, model.UserRoleType);
        }

        /// <summary>
        /// Test for posting to create invite.
        /// </summary>
        [TestMethod]
        public void PostCreateTest()
        {
            using (ShimsContext.Create())
            {
                ShimAccountLogics.AllInstances.DoesUserExistString = (logics, s) => false;
                ShimInviteLogics.AllInstances.CreateInviteInviteViewModelInt32Uri =
                    (logics, model, arg3, arg4) => this.sampleInvite;

                var result = this.controller.Create(this.sampleInvite);

                Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
                var routeResult = result as RedirectToRouteResult;
                Assert.IsNotNull(routeResult);
                Assert.AreEqual(routeResult.RouteValues["action"], "Index");
            }
        }

        /// <summary>
        /// Test for posting to create invite for a user that already has an account.
        /// </summary>
        [TestMethod]
        public void PostCreateUserAlreadyExistsTest()
        {
            using (ShimsContext.Create())
            {
                ShimAccountLogics.AllInstances.DoesUserExistString = (logics, s) => true;
                ShimInviteLogics.AllInstances.CreateInviteInviteViewModelInt32Uri =
                    (logics, model, arg3, arg4) => this.sampleInvite;

                var result = this.controller.Create(this.sampleInvite);

                Assert.IsNotNull(result);
                Assert.IsInstanceOfType(result, typeof(ViewResult));

                Assert.IsTrue(this.controller.TempData.ContainsKey("Error"));
                Assert.IsTrue(this.controller.TempData["Error"].ToString().Contains("already has an account"));
            }
        }

        /// <summary>
        /// The post create with database failure.
        /// </summary>
        [TestMethod]
        public void PostCreateDbFailure()
        {
            using (ShimsContext.Create())
            {
                ShimAccountLogics.AllInstances.DoesUserExistString = (logics, s) => false;
                ShimInviteLogics.AllInstances.CreateInviteInviteViewModelInt32Uri =
                    (logics, model, arg3, arg4) => null;
                var result = this.controller.Create(this.sampleInvite);
                Assert.IsNotNull(result);
                Assert.IsInstanceOfType(result, typeof(ViewResult));
            }
        }

        /// <summary>
        /// The test for resending an invite.
        /// </summary>
        [TestMethod]
        public void ResendTest()
        {
            using (ShimsContext.Create())
            {
                ShimInviteLogics.AllInstances.ResendInviteInt32Uri = (logics, i, arg3) => true;
                var result = this.controller.Resend(1);
                Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
                var routeResult = result as RedirectToRouteResult;
                Assert.IsNotNull(routeResult);
                Assert.AreEqual(routeResult.RouteValues["action"], "Index");
            }
        }

        /// <summary>
        /// Test for removing an invite.
        /// </summary>
        [TestMethod]
        public void DeleteTest()
        {
            using (ShimsContext.Create())
            {
                ShimInviteLogics.AllInstances.CancelInviteInt32 = (logics, i) => true;
                var result = this.controller.Delete(1);
                Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
                var routeResult = result as RedirectToRouteResult;
                Assert.IsNotNull(routeResult);
                Assert.AreEqual(routeResult.RouteValues["action"], "Index");
            }
        }



        /// <summary>
        /// Test for removing an invite.
        /// </summary>
        [TestMethod]
        public void DeleteDbFailureTest()
        {
            using (ShimsContext.Create())
            {
                ShimInviteLogics.AllInstances.CancelInviteInt32 = (logics, i) => false;
                var result = this.controller.Delete(1);
                Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
                var routeResult = result as RedirectToRouteResult;
                Assert.IsNotNull(routeResult);
                Assert.AreEqual(routeResult.RouteValues["action"], "Index");
                Assert.IsTrue(this.controller.TempData.ContainsKey("Error"));
            }
        }


        /// <summary>
        /// Test for removing an invite.
        /// </summary>
        [TestMethod]
        public void DeleteDbExceptionTest()
        {
            using (ShimsContext.Create())
            {
                ShimInviteLogics.AllInstances.CancelInviteInt32 = (logics, i) =>
                {
                    throw new Exception("error occured");
                };
                var result = this.controller.Delete(1);
                Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
                var routeResult = result as RedirectToRouteResult;
                Assert.IsNotNull(routeResult);
                Assert.AreEqual(routeResult.RouteValues["action"], "Index");
                Assert.IsTrue(this.controller.TempData.ContainsKey("Error"));
            }
        }
    }
}
