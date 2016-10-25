// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ToolViewMOdel.cs" company="UDRI">
//   Copyright © 2016 University of Dayton Research Institute. All rights reserved.
// </copyright>
// <summary>
//   The View model for Tools.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Website.Models
{
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// View Model for a Tool
    /// </summary>
    public class ToolViewModel
    {
        /// <summary>
        /// ID of the Tool
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Name of the tool
        /// </summary>
        [Required(ErrorMessage = "Please enter a name.")]
        public string Name { get; set; }

        /// <summary>
        /// Description of the tool
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// URL of the website for the tool
        /// </summary>
        [DisplayName("Webiste URL")]
        public string WebsiteUrl { get; set; }

        /// <summary>
        /// URL of the app in the Apple App Store
        /// </summary>
        [DisplayName("Apple Store URL")]
        public string AppleStore { get; set; }

        /// <summary>
        /// URL of the app in the Google Play Store
        /// </summary>
        [DisplayName("Google Play URL")]
        public string GooglePlayStore { get; set; }

        /// <summary>
        /// Gets or sets if the Tool is active
        /// </summary>
        [DisplayName("Active")]
        public bool IsActive { get; set; }
    }
}