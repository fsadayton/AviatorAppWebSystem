// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ResetPasswordEmail.cs" company="UDRI">
//   Reset Password Email
// </copyright>
// <summary>
//   The reset password email.
// </summary>
// --------------------------------------------------------------------------------------------------------------------



namespace Website.BL.Email
{
    using System;
    using System.Configuration;
    using System.Net;
    using System.Net.Mail;
    using System.Web;

    /// <summary>
    /// The reset password email.
    /// </summary>
    public class ResetPasswordEmail
    {
        /// <summary>
        /// Email address
        /// </summary>
        private readonly string email;

        /// <summary>
        /// The token for resetting the password
        /// </summary>
        private readonly string token;
        
        /// <summary>
        /// The user id.
        /// </summary>
        private readonly int userId;

        /// <summary>
        /// Initializes a new instance of the <see cref="ResetPasswordEmail"/> class.
        /// </summary>
        /// <param name="userId"> The user Id. </param>
        /// <param name="emailAddress"> The email address.  </param>
        /// <param name="resetToken"> The reset token.  </param>
        public ResetPasswordEmail(int userId, string emailAddress, string resetToken)
        {
            this.email = emailAddress;
            this.token = resetToken;
            this.userId = userId;
        }

        /// <summary>
        /// Sends the email.
        /// </summary>
        /// <param name="linkToNewPassword"> The link to creating a new password. </param>
        /// <returns> The <see cref="string"/> of the body of the email. </returns>
        public string Send(Uri linkToNewPassword)
        {
            var createdAccountEmail = new MailMessage();
            createdAccountEmail.To.Add(this.email);
            createdAccountEmail.From = new MailAddress(ConfigurationManager.AppSettings["EmailFromAddress"]);
            createdAccountEmail.Subject = "AVIATOR Password Reset";

            var callback = new Uri(linkToNewPassword + string.Format("?token={0}&id={1}", HttpUtility.UrlEncode(this.token), this.userId));

            var smtp = new SmtpClient
            {
                Host = ConfigurationManager.AppSettings["SmtpServer"], 
                Port = int.Parse(ConfigurationManager.AppSettings["SmtpPort"]), 
                EnableSsl = true, 
                Credentials = new NetworkCredential(ConfigurationManager.AppSettings["EmailFromUsername"], ConfigurationManager.AppSettings["EmailFromPassword"])
            };

            var bodyText = string.Format(
                "A request has been received to reset your AVIATOR account's password.  If you did not request your password to be reset, please ignore this email.<br><br>"
                + "To reset your password please <a href='{0}'>click here.</a>", 
                callback);

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