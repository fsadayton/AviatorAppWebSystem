// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CreateAccountViewModel.cs" company="UDRI">
//   Copyright © 2015 University of Dayton Research Institute. All rights reserved.
// </copyright>
// <summary>
//   The create account view model.
// </summary>
// --------------------------------------------------------------------------------------------------------------------


namespace Models.AccountManagement
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// The create account view model.
    /// </summary>
    public class CreateAccountViewModel
    {
        /// <summary>
        /// Gets or sets the user id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        [Display(Name = "Email")]
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string UserName { get; set; }


        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        [Display(Name = "First Name")]
        [Required(ErrorMessage = "First name is required.")]
        public string FirstName { get; set; }


        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "Last name is required.")]
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        [Required(ErrorMessage = "Password is required.")]
        [MinLength(8,  ErrorMessage = "Password must be at least 8 characters long.")]
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the confirm password.
        /// </summary>
        [Display(Name = "Confirm Password")]
        [Required(ErrorMessage = "Confirm password is required.")]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; }

        /// <summary>
        /// Gets or sets the user type.
        /// </summary>
        public UserRoleType UserType { get; set; }


        /// <summary>
        /// Gets or sets the id of the corresponding invite.
        /// </summary>
        public int InviteId { get; set; }

        /// <summary>
        /// Gets or sets the provider id.
        /// </summary>
        public int? ProviderId { get; set; }
    }
}
