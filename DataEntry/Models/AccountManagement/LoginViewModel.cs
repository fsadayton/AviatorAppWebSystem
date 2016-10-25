// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LoginViewModel.cs" company="UDRI">
//   Copyright © 2015 University of Dayton Research Institute. All rights reserved.
// </copyright>
// <summary>
//   The login view model.
// </summary>
// --------------------------------------------------------------------------------------------------------------------


namespace Models.AccountManagement
{
    using System.ComponentModel.DataAnnotations;
    
    /// <summary>
    /// The login result.
    /// </summary>
    public enum LoginResult
    {
        /// <summary>
        /// Login was successful.
        /// </summary>
        Success, 

        /// <summary>
        /// The invalid username password.
        /// </summary>
        InvalidUsernamePassword
    }


    /// <summary>
    /// The login view model.
    /// </summary>
    public class LoginViewModel
    {
        /// <summary>
        /// Gets or sets the user id.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        [Required]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether remember me is set.
        /// </summary>
        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }


        /// <summary>
        /// Gets or sets the login result.
        /// </summary>
        public LoginResult LoginResult { get; set; }

        /// <summary>
        /// Gets or sets the roles for the user.
        /// </summary>
        public string Roles { get; set; }


        /// <summary>
        /// Gets or sets the provider id.
        /// </summary>
        public int? ProviderId { get; set; }
    }
}