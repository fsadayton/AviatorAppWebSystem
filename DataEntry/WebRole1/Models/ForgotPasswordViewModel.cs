// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ForgotPasswordViewModel.cs" company="UDRI">
//   Forgot Password View Model
// </copyright>
// <summary>
//   The forgot password view model.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Website.Models
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// The forgot password view model.
    /// </summary>
    public class ForgotPasswordViewModel
    {
        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}