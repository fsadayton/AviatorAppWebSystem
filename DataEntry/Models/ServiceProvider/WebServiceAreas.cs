// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WebServiceAreas.cs" company="UDRI">
//   Copyright © 2015 University of Dayton Research Institute. All rights reserved.
// </copyright>
// <summary>
//   The web service areas.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Models.ServiceProvider
{
    using System.Collections.Generic;

    /// <summary>
    /// The web service areas.
    /// </summary>
    public class WebServiceAreas
    {
        /// <summary>
        /// Gets or sets the service areas ids.
        /// </summary>
        [EnsureOneElement(ErrorMessage = "At least one category is required.")]
        public List<int> ServiceAreas { get; set; }

        /// <summary>
        /// Gets or sets the state of the object.
        /// </summary>
        public ObjectStatus.ObjectState State { get; set; }
    }
}
