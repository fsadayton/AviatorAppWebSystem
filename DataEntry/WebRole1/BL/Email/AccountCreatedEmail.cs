// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AccountCreatedEmail.cs" company="UDRI">
//   Copyright © 2015 University of Dayton Research Institute. All rights reserved.
// </copyright>
// <summary>
//   The account created email.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Website.BL.Email
{
    using System;
    using System.Configuration;
    using System.Net;
    using System.Net.Mail;
    using global::Models.AccountManagement;

    /// <summary>
    /// The account created email.
    /// </summary>
    public class AccountCreatedEmail
    {
        /// <summary>
        /// The user email.
        /// </summary>
        private readonly string userEmail;

        /// <summary>
        /// The user role type.
        /// </summary>
        private readonly UserRoleType userRoleType;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountCreatedEmail"/> class.
        /// </summary>
        /// <param name="email"> The user's email. </param>
        /// <param name="userType"> The user type - Admin or Service provider </param>
        public AccountCreatedEmail(string email, UserRoleType userType)
        {
            this.userEmail = email;
            this.userRoleType = userType;
        }

        /// <summary>
        /// Function to send the email.  Settings are in the web.config.
        /// </summary>
        /// <param name="linkToHome"> The link to the home page of the site.  </param>
        /// <returns> The <see cref="string"/> of the body of the email. </returns>
        public string Send(Uri linkToHome)
        {
            var createdAccountEmail = new MailMessage();
            createdAccountEmail.To.Add(this.userEmail);
            createdAccountEmail.From = new MailAddress(ConfigurationManager.AppSettings["EmailFromAddress"]);
            createdAccountEmail.Subject = "AVIATOR Account Creation";

            var smtp = new SmtpClient
            {
                Host = ConfigurationManager.AppSettings["SmtpServer"],
                Port = int.Parse(ConfigurationManager.AppSettings["SmtpPort"]),
                EnableSsl = true,
                Credentials = new NetworkCredential(ConfigurationManager.AppSettings["EmailFromUsername"], ConfigurationManager.AppSettings["EmailFromPassword"])
            };

            // Set the body of the email based on the user type that is being created.
            var type = this.userRoleType == UserRoleType.Admin ? "an Administrator" : "a Service Provider";
            var abilities = this.userRoleType == UserRoleType.Admin
                ? "User accounts, Service Providers, Categories, etc."
                : "your organization's information.";

            var bodyText = string.Format(
                "Welcome to Family Services AVIATOR.  You have created an account as {0}.  <a href='{1}'>Click here</a> to visit the site.  <br><br>" +
                "Click Login to log into the site.  Once logged in, you will be able to administer {2}",
                type,
                linkToHome,
                abilities);

            createdAccountEmail.Body = bodyText;
            createdAccountEmail.IsBodyHtml = true;

            try
            {
                if (ConfigurationManager.AppSettings["SendEmails"].ToLower() == "true")
                {
                    smtp.Send(createdAccountEmail);
                }
            }
            catch (Exception ex)
            {
                bodyText = null;
            }

            return bodyText;
        }
    }
}