// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ResetPasswordEmailTests.cs" company="UDRI">
//   Reset Password Email Tests
// </copyright>
// <summary>
//   The reset password email tests.
// </summary>
// --------------------------------------------------------------------------------------------------------------------


namespace DataEntry.Tests.BL.Email
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Website.BL.Email;

    /// <summary>
    /// The reset password email tests.
    /// </summary>
    [TestClass]
    public class ResetPasswordEmailTests
    {
        /// <summary>
        /// The send test.
        /// </summary>
        [TestMethod]
        public void SendTest()
        {
            var token = Guid.NewGuid().ToString();
            var email = new ResetPasswordEmail(1, "jennifer.smith@udri.udayton.edu", token);
            var emailBody = email.Send(new Uri("http://localhost:81"));
            Assert.IsTrue(emailBody.Contains("http://localhost:81"));
        }
    }
}
