// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PersonalResourcesViewModel.cs" company="UDRI">
//   The View model used by the Personal Resources Controller.
// </copyright>
// <summary>
//   The personal resources view model.
// </summary>
// --------------------------------------------------------------------------------------------------------------------


namespace Website.Models
{
    using System.Collections.Generic;
    using global::Models;

    /// <summary>
    /// The personal resources view model.
    /// </summary>
    public class PersonalResourcesViewModel
    {
        /// <summary>
        /// Gets or sets the general families.
        /// </summary>
        public List<FamilyEditor> General { get; set; }

        /// <summary>
        /// Gets or sets the crime categories.
        /// </summary>
        public List<Category> Crime { get; set; } 
    }
}