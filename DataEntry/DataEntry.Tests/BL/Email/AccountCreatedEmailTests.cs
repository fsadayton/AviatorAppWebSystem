// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AccountCreatedEmailTests.cs" company="">
//   Copyright © 2015 University of Dayton Research Institute. All rights reserved.
// </copyright>
// <summary>
//   Summary description for AccountEmailTests
// </summary>
// --------------------------------------------------------------------------------------------------------------------


using System;
using Microsoft.QualityTools.Testing.Fakes;
using Models.AccountManagement;
using Website.BL.Email;

namespace DataEntry.Tests.BL.Email
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Tests for sending a email sent for new account
    /// </summary>
    [TestClass]
    public class AccountCreatedEmailTests
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AccountCreatedEmailTests"/> class.
        /// </summary>
        public AccountCreatedEmailTests()
        {
            // TODO: Add constructor logic here
        }

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        #region Additional test attributes

// You can use the following additional attributes as you write your tests:
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        #endregion

        /// <summary>
        /// Test for a new admin
        /// </summary>
        [TestMethod]
        public void AdminSendTest()
        {
            using (ShimsContext.Create())
            {
                var email = new AccountCreatedEmail("Test@test.com", UserRoleType.Admin);
                var result = email.Send(new Uri("http://localhost/Web/Index"));

                Assert.IsTrue(result.Contains("an Administrator"));
            }
        }

        /// <summary>
        /// Test for a new provider
        /// </summary>
        [TestMethod]
        public void ProviderSendTest()
        {
            using (ShimsContext.Create())
            {
                var email = new AccountCreatedEmail("Test@test.com", UserRoleType.Provider);
                var result = email.Send(new Uri("http://localhost/Web/Index"));

                Assert.IsTrue(result.Contains("a Service Provider"));
            }
        }
    }
}
