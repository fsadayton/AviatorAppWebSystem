// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InviteController.cs" company="UDRI">
//   Copyright © 2015 University of Dayton Research Institute. All rights reserved.
// </copyright>
// <summary>
//   The invite controller.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Website.Controllers
{
    using System;
    using System.Web.Http.Cors;
    using System.Web.Mvc;
    using BL;
    using global::Models.AccountManagement;
    using Helpers;

    /// <summary>
    /// The invite controller.
    /// </summary>
    [EnableCors("*", "*", "*")]  
    [AuthorizeRedirect(Roles = "1")]
    public class InviteController : BaseController
    {
        /// <summary>
        /// The invite logics.
        /// </summary>
        private readonly InviteLogics inviteLogics;


        /// <summary>
        /// Link to the register account page.
        /// </summary>
        private Uri registerLink;

        /// <summary>
        /// Initializes a new instance of the <see cref="InviteController"/> class.
        /// </summary>
        public InviteController()
        {
            this.inviteLogics = new InviteLogics();
            this.registerLink = null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InviteController"/> class for testing.
        /// </summary>
        /// <param name="registerForTest"> The register for test. </param>
        public InviteController(Uri registerForTest)
        {
            this.inviteLogics = new InviteLogics();
            this.registerLink = registerForTest;
        }

        /// <summary>
        /// GET: Invite
        /// The list of all the user's invites.
        /// </summary>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        public ActionResult Index()
        {
            var invites = this.inviteLogics.GetInvitesByUser(this.UserId);
            return this.View(invites);
        }

        /// <summary>
        /// GET: Invite/Create
        /// The create.
        /// </summary>
        /// <param name="userRoleType"> The user Role Type. </param>
        /// <returns> The <see cref="ActionResult"/>. </returns>
        public ActionResult Create(int userRoleType)
        {
            var model = new InviteViewModel
            {
                UserRoleType = (UserRoleType)userRoleType
            };

            var isAdmin = userRoleType == (int)UserRoleType.Admin;
            var modelName = isAdmin ? "CreateAdmin" : "CreateProviderAdmin";

            return this.View(modelName, model);
        }

        /// <summary>
        /// POST: Invite/Create
        /// Create an invite. Send the invite to the user's email address.
        /// </summary>
        /// <param name="invite"> The invite view model. </param>
        /// <returns> The <see cref="ActionResult"/>. </returns>
        [HttpPost]
        public ActionResult Create(InviteViewModel invite)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var modelName = invite.UserRoleType == UserRoleType.Admin ? "CreateAdmin" : "CreateProviderAdmin";
                    return this.View(modelName, invite);
                }

                // Check if the user already has an account.
                AccountLogics accountInfo = new AccountLogics();
                if (accountInfo.DoesUserExist(invite.InviteeEmailAddress))
                {
                    this.TempData["Error"] = "This user already has an account with the site.";
                    return this.View(invite);
                }

                this.SetRegisterLink();

                // Create the invite
                var createdInvite = this.inviteLogics.CreateInvite(invite, this.UserId, this.registerLink);
                if (createdInvite == null)
                {
                    // Show an error if invite did not get created.
                    this.TempData["Error"] = "There was an issue creating the invite, please try again.";
                    return this.View(invite);
                }

                return this.RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                this.TempData["Error"] = "There was an issue creating the invite, please try again.";
                return this.View();
            }
        }

        /// <summary>
        /// Invite/Delete/5
        /// Cancel an invite.  Remove it from the database.
        /// </summary>
        /// <param name="id"> The id of the invite </param>
        /// <returns> The <see cref="ActionResult"/>. </returns>
        public ActionResult Delete(int id)
        {
            try
            {
                // Delete the given invite from the database
                if (this.inviteLogics.CancelInvite(id))
                {
                    return this.RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                // TODO: Log
            }

            this.TempData["Error"] = "Unable to cancel the invite.";
            return this.RedirectToAction("Index");
        }

        /// <summary>
        /// The resend.
        /// </summary>
        /// <param name="id"> The invite id. </param>
        /// <returns> The <see cref="ActionResult"/>. </returns>
        public ActionResult Resend(int id)
        {
            try
            {
                this.SetRegisterLink();

                if (!this.inviteLogics.ResendInvite(id, this.registerLink))
                {
                    this.TempData["Error"] = "Unable to resend the invite.";
                }
            }
            catch
            {
                this.TempData["Error"] = "Unable to resend the invite.";
            }

            return this.RedirectToAction("Index");
        }

        /// <summary>
        /// Sets the register link value if it hasn't already been set.
        /// </summary>
        private void SetRegisterLink()
        {
            if (this.registerLink == null)
            {
                var url = new UrlHelper(ControllerContext.RequestContext);
                this.registerLink = new Uri(url.Action("Register", "Account", null, "http"));
            }
        }
    }
}
