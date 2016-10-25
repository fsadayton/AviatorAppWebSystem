// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AccountAdminViewModel.cs" company="UDRI">
//   Account Addmin View Model
// </copyright>
// <summary>
//   The account admin view model.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
using System.ComponentModel.DataAnnotations;
namespace Models.AccountManagement
{
    /// <summary>
    /// The account admin view model.
    /// </summary>
    public class AccountAdminViewModel
    {
        /// <summary>
        /// Gets or sets the user id.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        [Required (ErrorMessage="First Name is required")]
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        [Required (ErrorMessage="Last Name is required")]
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        [Required (ErrorMessage="Email is required")]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the roles for the user.
        /// </summary>
        public UserRoleType Role { get; set; }

        /// <summary>
        /// Gets or sets the provider id.
        /// </summary>
        public int? ProviderId { get; set; }

        /// <summary>
        /// Gets or sets the provider name.
        /// </summary>
        public string ProviderName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user's account is active.
        /// </summary>
        public bool IsActive { get; set; }
    }
}
