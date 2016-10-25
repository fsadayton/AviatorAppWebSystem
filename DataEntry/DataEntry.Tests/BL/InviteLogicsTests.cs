// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InviteLogicsTests.cs" company="UDRI">
//   Copyright © 2015 University of Dayton Research Institute. All rights reserved.
// </copyright>
// <summary>
//   The invite logics tests.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

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

    /// <summary>
    /// The invite logics tests.
    /// </summary>
    [TestClass]
    public class InviteLogicsTests
    {
        /// <summary>
        /// A sample invite.
        /// </summary>
        private Invite sampleInvite;

        /// <summary>
        /// The sample invite view model.
        /// </summary>
        private InviteViewModel sampleInviteViewModel;

        /// <summary>
        /// A GUID for testing that isn't going to change.
        /// </summary>
        private string guid;

        /// <summary>
        /// The logics to be tested.
        /// </summary>
        private InviteLogics logics;

        /// <summary>
        /// The test initialize.
        /// </summary>
        [TestInitialize]
        public void TestInitialize()
        {
            this.logics = new InviteLogics();

            this.guid = Guid.NewGuid().ToString();

            this.sampleInvite = new Invite
            {
                ID = 1,
                CreatedAt = DateTime.Now,
                CreatorID = 5,
                Email = "Test@test.com",
                RoleTypeID = 1,
                Token = this.guid
            };

            this.sampleInviteViewModel = new InviteViewModel
            {
                Id = 0,
                DateSent = DateTime.Now,
                InviteeEmailAddress = "Test@test.com",
                UserRoleType = UserRoleType.Admin
            };
        }

        /// <summary>
        /// The get invites by user test.
        /// </summary>
        [TestMethod]
        public void GetInvitesByUserTest()
        {
            using (ShimsContext.Create())
            {
                var foundInvites = new List<Invite> { this.sampleInvite };
                ShimInvitesRepo.AllInstances.GetInvitesCreatedByInt32 = (repo, i) => foundInvites;
                var inviteViewModels = this.logics.GetInvitesByUser(1);
                Assert.AreEqual(1, inviteViewModels.Count);
            }
        }

        /// <summary>
        /// The create invite test.
        /// </summary>
        [TestMethod]
        public void CreateInviteNewTest()
        {
            using (ShimsContext.Create())
            {
                ShimInvitesRepo.AllInstances.GetInviteString = (repo, s) => null;
                ShimInvitesRepo.AllInstances.CreateInviteInvite = (repo, invite) => this.sampleInvite;
                ShimInviteEmail.AllInstances.SendUri = (email, uri) => "Test email text";
                var result = this.logics.CreateInvite(
                    this.sampleInviteViewModel, 
                    32,
                    new Uri("http://localhost/Account/Create")) as InviteViewModel;

                Assert.AreEqual(1, result.Id);
            }
        }

        /// <summary>
        /// The create invite existing invite test.
        /// </summary>
        [TestMethod]
        public void CreateInviteExistingTest()
        {
            using (ShimsContext.Create())
            {
                ShimInvitesRepo.AllInstances.GetInviteString = (repo, s) => this.sampleInvite;
                ShimInviteEmail.AllInstances.SendUri = (email, uri) => "Test email text";
                ShimInvitesRepo.AllInstances.UpdateInviteInvite = (repo, invite) => true;

                var result = this.logics.CreateInvite(
                    this.sampleInviteViewModel,
                    32,
                    new Uri("http://localhost/Account/Create")) as InviteViewModel;

                Assert.AreEqual(1, result.Id);
            }
        }



        /// <summary>
        /// The create invite existing invite test.
        /// </summary>
        [TestMethod]
        public void CreateInviteDbFailureTest()
        {
            using (ShimsContext.Create())
            {
                ShimInvitesRepo.AllInstances.GetInviteString = (repo, s) => null;
                ShimInvitesRepo.AllInstances.CreateInviteInvite = (repo, invite) => null;
                ShimInviteEmail.AllInstances.SendUri = (email, uri) => "Test email text";
                var result = this.logics.CreateInvite(
                    this.sampleInviteViewModel,
                    32,
                    new Uri("http://localhost/Account/Create")) as InviteViewModel;

                Assert.IsNull(result);
            }
        }


        /// <summary>
        /// The create invite existing invite test.
        /// </summary>
        [TestMethod]
        public void CreateInviteExistingResendFailTest()
        {
            using (ShimsContext.Create())
            {
                ShimInvitesRepo.AllInstances.GetInviteString = (repo, s) => this.sampleInvite;
                ShimInviteEmail.AllInstances.SendUri = (email, uri) => null;
                ShimInvitesRepo.AllInstances.UpdateInviteInvite = (repo, invite) => true;

                var result = this.logics.CreateInvite(
                    this.sampleInviteViewModel,
                    32,
                    new Uri("http://localhost/Account/Create")) as InviteViewModel;

                Assert.IsNull(result);
            }
        }

        /// <summary>
        /// The resend invite test.
        /// </summary>
        [TestMethod]
        public void ResendInviteTest()
        {
            using (ShimsContext.Create())
            {
                ShimInviteEmail.AllInstances.SendUri = (email, uri) => "Test email text";
                ShimInvitesRepo.AllInstances.UpdateInviteInvite = (repo, invite) => true;

                var result = this.logics.ResendInvite(this.sampleInvite, new Uri("http://localhost/Account/Create"));
                Assert.IsTrue(result);
            }
        }


        /// <summary>
        /// The cancel invite test.
        /// </summary>
        [TestMethod]
        public void CancelInviteTest()
        {
            using (ShimsContext.Create())
            {
                ShimInvitesRepo.AllInstances.RemoveInviteInt32 = (repo, i) => true;
                Assert.IsTrue(this.logics.CancelInvite(12));
            }
        }

        /// <summary>
        /// The validate invite success test.
        /// </summary>
        [TestMethod]
        public void ValidateInviteTest()
        {
            using (ShimsContext.Create())
            {
                ShimInvitesRepo.AllInstances.GetInviteInt32 = (repo, i) => this.sampleInvite;
                Assert.IsNotNull(this.logics.ValidateInvite(1, this.guid));
            }
        }

        /// <summary>
        /// The validate invite fails test.
        /// </summary>
        [TestMethod]
        public void ValidateInviteInvalidTest()
        {
            using (ShimsContext.Create())
            {
                ShimInvitesRepo.AllInstances.GetInviteInt32 = (repo, i) => this.sampleInvite;
                Assert.IsNull(this.logics.ValidateInvite(1, Guid.NewGuid().ToString()));
            }
        }

        /// <summary>
        /// The rend invite by id.
        /// </summary>
        [TestMethod]
        public void RendInviteTest()
        {
            using (ShimsContext.Create())
            {
                ShimInviteEmail.AllInstances.SendUri = (email, uri) => "Test email text";
                ShimInvitesRepo.AllInstances.UpdateInviteInvite = (repo, invite) => true;
                ShimInvitesRepo.AllInstances.GetInviteInt32 = (repo, i) => this.sampleInvite;

                var result = this.logics.ResendInvite(1, new Uri("http://localhost/Account/Create"));
                Assert.IsTrue(result);
            }
        }
    }
}
