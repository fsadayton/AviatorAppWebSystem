// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Coverage.cs" company="UDRI">
//   Copyright © 2015 University of Dayton Research Institute. All rights reserved.
// </copyright>
// <summary>
//   The coverage.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Models.ServiceProvider
{
    using System.Collections.Generic;

    /// <summary>
    /// The coverage.
    /// </summary>
    public class Coverage
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the country.
        /// </summary>
        public int CountryId { get; set; }

        /// <summary>
        /// Gets or sets the country name.
        /// </summary>
        public string CountryName { get; set; }

        /// <summary>
        /// Gets or sets the state.
        /// </summary>
        public int StateId { get; set; }

        /// <summary>
        /// Gets or sets the state name.
        /// </summary>
        public string StateName { get; set; }

        /// <summary>
        /// Gets or sets the county id.
        /// </summary>
        public int CountyId { get; set; }

        /// <summary>
        /// Gets or sets the county name.
        /// </summary>
        public string CountyName { get; set; }

        /// <summary>
        /// Gets or sets the state of the object.
        /// </summary>
        public ObjectStatus.ObjectState State { get; set; }
    }
}
