// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InviteViewModel.cs" company="UDRI">
//   Copyright © 2015 University of Dayton Research Institute. All rights reserved.
// </copyright>
// <summary>
//   The user role type
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Models.AccountManagement
{
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// The user role type
    /// </summary>
    public enum UserRoleType
    {
        /// <summary>
        /// Administrator role
        /// </summary>
        Admin = 1, 

        /// <summary>
        /// Service provider role
        /// </summary>
        Provider = 2
    }

    /// <summary>
    /// The invite view model.
    /// </summary>
    public class InviteViewModel
    {
        /// <summary>
        /// Gets or sets the unique id for the invite.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the user role type.
        /// </summary>
        [Required]
        [Display(Name = "Invite Type")]
        public UserRoleType UserRoleType { get; set; }

        /// <summary>
        /// Gets or sets the date sent.
        /// </summary>
        [Display(Name = "Date Sent")]
       public DateTime DateSent { get; set; }

        /// <summary>
        /// Gets or sets the email address of the invitee.
        /// </summary>
        [Required (ErrorMessage="An email address is required")]
        [EmailAddress]
        [Display(Name = "Email Address")]
        public string InviteeEmailAddress { get; set; }


        /// <summary>
        /// Gets or sets the service provider id for users with Service Provider Type
        /// </summary>
        [Display(Name = "Service Provider")]
        public int? ServiceProviderId { get; set; }
    }
}
