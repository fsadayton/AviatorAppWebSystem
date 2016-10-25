// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AccountController.cs" company="UDRI">
//   Copyright © 2015 University of Dayton Research Institute. All rights reserved.
// </copyright>
// <summary>
//   Defines the AccountController type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------


namespace Website.Controllers
{
    using System;
    using System.Globalization;
    using System.Web;
    using System.Web.Http.Cors;
    using System.Web.Mvc;
    using System.Web.Security;
    using BL;
    using global::Models.AccountManagement;
    using Website.Models;

    /// <summary>
    /// The account controller.
    /// </summary>
    [EnableCors("*", "*", "*")]    
    [Authorize]
    public class AccountController : Controller
    {
        /// <summary>
        /// The account logics.
        /// </summary>
        private readonly AccountLogics accountLogics;

        /// <summary>
        /// Link to the home page.
        /// </summary>
        private Uri homeLink;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountController"/> class.
        /// </summary>
        public AccountController()
        {
            this.accountLogics = new AccountLogics();
            this.homeLink = null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountController"/> class.  Used for unit testing.
        /// </summary>
        /// <param name="testHomeLink">
        /// The test home link.
        /// </param>
        public AccountController(Uri testHomeLink)
        {
            this.accountLogics = new AccountLogics();
            this.homeLink = testHomeLink;
        }

        /// <summary>
        /// GET: /Account/Login
        /// The login form.  Can go directly here or use the model in the layout.
        /// </summary>
        /// <param name="returnUrl">
        /// The return url if coming from a different page.  
        /// </param>
        /// <returns>
        /// The <see cref="ActionResult"/>. The login form. 
        /// </returns>
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return this.View();
        }

        /// <summary>
        /// Login the user.  Verify the account information.
        /// POST: /Account/Login
        /// </summary>
        /// <param name="model"> The login view model.  </param>
        /// <param name="returnUrl"> The return url if coming from a different page.  </param>
        /// <returns>
        /// The <see cref="ActionResult"/>.  Login form if there are errors, redirects home if login is successful. 
        /// </returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return this.View(model);
            }
            
            model = this.accountLogics.Login(model);

            if (model.LoginResult == LoginResult.Success)
            {
               
                SetAuthenticationTicket(model.UserId, model.Roles);

                // Send the user to the original request if given.
                if (!string.IsNullOrEmpty(returnUrl))
                {
                    return this.Redirect(returnUrl);
                }

                // Otherwise, send the user home.
                return this.RedirectToAction("Index", "Web");
            }

            // Invalid account information - show the user an error.
            this.TempData["Error"] = "Invalid email or password.";
            return this.View(model);
        }

        /// <summary>
        /// The form for registering a new account on the site.
        /// GET: /Account/Register
        /// </summary>
        /// <param name="inviteId">
        /// The invite Id for creating an account. 
        /// </param>
        /// <param name="token">
        /// The invite token. 
        /// </param>
        /// <returns>
        /// The <see cref="ActionResult"/>.  Register form.  
        /// </returns>
        [AllowAnonymous]
        public ActionResult Register(int? inviteId, string token)
        {
            if (inviteId == null)
            {
                ViewBag.ValidInvite = false;
            }
            else
            {
                // Validate the invite.
                var inviteLogics = new InviteLogics();
                var validInvite = inviteLogics.ValidateInvite(inviteId.GetValueOrDefault(0), token);
                ViewBag.ValidInvite = validInvite != null;

                if (ViewBag.ValidInvite)
                {
                    // Preset the form fields from the invite information.
                    var viewModel = new CreateAccountViewModel
                    {
                        UserName = validInvite.InviteeEmailAddress, 
                        UserType = validInvite.UserRoleType,
                        InviteId = inviteId.GetValueOrDefault(0),
                        ProviderId = validInvite.ServiceProviderId
                    };

                    return this.View(viewModel);
                }
                else
                {
                    // Invalid invite - shown an error
                    this.TempData["Error"] = "You invite to create an account is invaild, please contact your adminstrator for a new invitation.";
                    return View();
                }
            }
            
            // No invite id was given.
            this.TempData["Error"] = "You must be invited to create an account.  Please contact your adminstrator for an invitation.";
            return View();
        }

        /// <summary>
        /// Register an account in the site.  Checks if the account already exists.
        /// Logs the user in after successful registration. 
        /// POST: /Account/Register
        /// </summary>
        /// <param name="model">
        /// The create account view model.
        /// </param>
        /// <returns>
        /// The <see cref="ActionResult"/>. Register form if there are errors, 
        /// successful creation takes the user home 
        /// </returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(CreateAccountViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.View(model);
            }

            // Check if the user exists.
            if (this.accountLogics.DoesUserExist(model.UserName))
            {
                this.TempData["Error"] = "Account already exists.";
                return this.View(model);
            }

            // Set the link to the home page
            if (this.homeLink == null)
            {
                var urlHelper = new UrlHelper(ControllerContext.RequestContext);
                this.homeLink = new Uri(urlHelper.Action("Index", "Web", null, "http"));
            }

            // Create the account
            var createdUser = this.accountLogics.CreateAccount(model, this.homeLink);

            // If account creation fails, show an error.
            if (createdUser == null)
            {
                this.TempData["Error"] = "Unable to create account. Please try again.";
                return this.View(model);
            }

            // Remove the invite
            InviteLogics inviteLogics = new InviteLogics();
            inviteLogics.CancelInvite(model.InviteId);

            this.SetAuthenticationTicket(createdUser.Id, ((int) model.UserType).ToString());

            return this.RedirectToAction("Index", "Web");
        }


        /// <summary>
        /// The forgot password page.
        /// </summary>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        /// <summary>
        /// POST for forgot password page.  Takes the user's info, sends them an email with the link to the 
        /// </summary>
        /// <param name="model">
        /// The model.
        /// </param>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ForgotPassword(ForgotPasswordViewModel model)
        {
            var isUserFound = this.accountLogics.DoesUserExist(model.Email);
            if (!isUserFound)
            {
                ViewBag.Error = "A user with this email address was not found.";
                return this.View(model);
            }


            if (this.homeLink == null)
            {
                var urlHelper = new UrlHelper(ControllerContext.RequestContext);
                this.homeLink = new Uri(urlHelper.Action("ResetPassword", "Account", null, "http"));
            }

            var result = this.accountLogics.SendResetPasswordEmail(model.Email, this.homeLink);
            if (result)
            {
                return this.View("ForgotPasswordConfirmation");
            }

            ViewBag.Error = "An error occurred when sending the email. Please try again.";
            return this.View(model);
        }

        /// <summary>
        /// The reset password page.  Checks for the user's ID and a token in the URL parameters.
        /// </summary>
        /// <param name="id"> The user's id. </param>
        /// <param name="token"> The token. </param>
        /// <returns> The <see cref="ActionResult"/>. </returns>
        [AllowAnonymous]
        public ActionResult ResetPassword(int? id, string token)
        {
            if (id == null)
            {
                ViewBag.Error = "No account found. Please request to reset your password again.";
                return this.View();
            }

            var model = this.accountLogics.GetUserForPasswordReset(id);

            if (model == null)
            {
                ViewBag.Error = "User not found.";
                return this.View();
            }

            // Check that the user has that token
            var isValidToken = this.accountLogics.ValidateResetPasswordToken(token, model);

            if (!isValidToken)
            {
                ViewBag.Error = "Token for resetting your password is invalid.  Please request to reset your password again.";
                return this.View();
            }

            return this.View(model);
        }

        /// <summary>
        /// Reset the user's password.
        /// </summary>
        /// <param name="model"> The model. </param>
        /// <returns> The <see cref="ActionResult"/>. </returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(ResetPasswordViewModel model)
        {
            var success  = this.accountLogics.UpdateUserCredentials(model);
            if (success)
            {
                return this.View("ResetPasswordConfirmation");
            }

            ViewBag.Error = "Unable to save password.  Please try again.";
            return this.View(model);
        }

        /// <summary>
        /// Log off action.  Signs the user out.
        /// </summary>
        /// <returns> The <see cref="ActionResult"/>.  Redirects home after logout. </returns>
        [HttpPost]
        public ActionResult LogOff()
        {
            // Remove the auth cookie.
           FormsAuthentication.SignOut();

           // Send the user home
           return this.RedirectToAction("Index", "Web");
        }

        /// <summary>
        /// Sets the authentication ticket for a user.
        /// </summary>
        /// <param name="userId"> The user id. </param>
        /// <param name="roles"> The roles for that user. </param>
        private void SetAuthenticationTicket(int userId, string roles)
        {
            var authTicket = new FormsAuthenticationTicket(
                1, // version
                userId.ToString(CultureInfo.InvariantCulture), // user id
                DateTime.Now, // created
                DateTime.Now.AddMinutes(60), // expires
                true, // persistent?
                roles); // can be used to store roles

            string encryptedTicket = FormsAuthentication.Encrypt(authTicket);

            var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
            HttpContext.Response.Cookies.Add(authCookie);
        }
    }
}