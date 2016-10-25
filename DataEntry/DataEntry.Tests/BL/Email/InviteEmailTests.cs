// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InviteEmailTests.cs" company="UDRI">
//   Copyright © 2015 University of Dayton Research Institute. All rights reserved.
// </copyright>
// <summary>
//   Defines the InviteEmailTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DataEntry.Tests.BL.Email
{
    using System;
    using DataEntry_Helpers;
    using Microsoft.QualityTools.Testing.Fakes;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Models.AccountManagement;
    using Website.BL.Email;

    /// <summary>
    /// The invite email tests.
    /// </summary>
    [TestClass]
    public class InviteEmailTests
    {
        /// <summary>
        /// The invite to send.
        /// </summary>
        private Invite inviteToSend;


        /// <summary>
        /// The test initialize.
        /// </summary>
        [TestInitialize]
        public void TestInitialize()
        {
            this.inviteToSend = new Invite
            {
                CreatedAt   = DateTime.Now,
                CreatorID   = 1,
                Email       = "test@test.com",
                ID          = 4,
                RoleTypeID  = (int)UserRoleType.Admin,
                Token       = Guid.NewGuid().ToString()
            };
        }

        /// <summary>
        /// The send to an admin test.  Make sure SendEmails is set to false in the app.config file
        /// </summary>
        [TestMethod]
        public void SendAdminTest()
        {
            using (ShimsContext.Create())
            {
                var url = "http://localhost/DataEntry/Account/Create";
                var builtUrl = "http://localhost/DataEntry/Account/Create?inviteId=4&token=" + this.inviteToSend.Token; 

                var email  = new InviteEmail(this.inviteToSend);
                var result = email.Send(new Uri(url));

                Assert.IsTrue(result.Contains(builtUrl));
                Assert.IsTrue(result.Contains("Family Services Admin"));
            }
        }


        /// <summary>
        /// The send to a provider test.  Make sure SendEmails is set to false in the app.config file
        /// </summary>
        [TestMethod]
        public void SendProviderTest()
        {
            using (ShimsContext.Create())
            {
                this.inviteToSend.RoleTypeID = (int) UserRoleType.Provider;
                var url = "http://localhost/DataEntry/Account/Create";
                var email = new InviteEmail(this.inviteToSend);
                var result = email.Send(new Uri(url));

                Assert.IsTrue(result.Contains("Provider"));
            }
        }
    }
}
