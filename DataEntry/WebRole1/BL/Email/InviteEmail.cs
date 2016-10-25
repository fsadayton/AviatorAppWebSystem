// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InviteEmail.cs" company="UDRI">
//   Copyright © 2015 University of Dayton Research Institute. All rights reserved.
// </copyright>
// <summary>
//   The invite email.
// </summary>
// --------------------------------------------------------------------------------------------------------------------


namespace Website.BL.Email
{
    using System;
    using System.Configuration;
    using System.Net;
    using System.Net.Mail;
    using System.Web;
    using DataEntry_Helpers;
    using global::Models.AccountManagement;

    /// <summary>
    /// The invite email.
    /// </summary>
    public class InviteEmail
    {
        /// <summary>
        /// The invite.
        /// </summary>
        private readonly Invite invite;

        /// <summary>
        /// Initializes a new instance of the <see cref="InviteEmail"/> class.
        /// </summary>
        /// <param name="inviteToSend"> The invite to send.  </param>
        public InviteEmail(Invite inviteToSend)
        {
            this.invite = inviteToSend;
        }

        /// <summary>
        /// Send the email
        /// </summary>
        /// <param name="inviteIndexUri"> The callback url to create an account.  </param>
        /// <returns> The <see cref="string"/>.  </returns>
        public string Send(Uri inviteIndexUri)
        {
            var loginInfo = new MailMessage();
            loginInfo.To.Add(this.invite.Email);
            loginInfo.From = new MailAddress(ConfigurationManager.AppSettings["EmailFromAddress"]);
            loginInfo.Subject = "AVIATOR Account Creation";

            var callback = new Uri(inviteIndexUri + string.Format("?inviteId={0}&token={1}", this.invite.ID, HttpUtility.UrlEncode(this.invite.Token)));

            string roleTypeText = this.invite.RoleTypeID == (int)UserRoleType.Admin ? "Family Services Administrator" : "Service Provider Administrator";

            string paragraph = "You have been invited to be a {0} for the Family Services AVIATOR application.  Please click to create an account. <a href='{1}'>{1}</a>";
            string bodyText = string.Format(paragraph, roleTypeText, callback);

            loginInfo.Body = bodyText;

            loginInfo.IsBodyHtml = true;
            var smtp = new SmtpClient
            {
                Host = ConfigurationManager.AppSettings["SmtpServer"], 
                Port = int.Parse(ConfigurationManager.AppSettings["SmtpPort"]), 
                EnableSsl = true, 
                Credentials = new NetworkCredential(ConfigurationManager.AppSettings["EmailFromUsername"], ConfigurationManager.AppSettings["EmailFromPassword"])
            };

            try
            {
                if (ConfigurationManager.AppSettings["SendEmails"].ToLower() == "true")
                {
                    smtp.Send(loginInfo);
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