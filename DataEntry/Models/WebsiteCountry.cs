// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WebsiteCountry.cs" company="UDRI">
//   Copyright © 2015 University of Dayton Research Institute. All rights reserved.
// </copyright>
// <summary>
//   Defines the WebsiteCountry type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Models
{
    /// <summary>
    /// The website country.
    /// </summary>
    public class WebsiteCountry
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the full name.
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Gets or sets the abbreviation.
        /// </summary>
        public string Abbreviation { get; set; }
    }
}
