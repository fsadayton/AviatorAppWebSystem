// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ServiceProviderLocation.cs" company="UDRI">
//   Copyright © 2014 University of Dayton Research Institute. All rights reserved.
// </copyright>
// <summary>
//   The service provider Locations.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Models
{
    using System.Collections.Generic;

    using Models.ServiceProvider;

    /// <summary>
    /// The service provider Locations.
    /// </summary>
    public class ServiceProviderLocation
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the Name of the location.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the street.
        /// </summary>
        [Required (ErrorMessage="Street Address is required")]
        public string Street { get; set; }

        /// <summary>
        /// Gets or sets the city.
        /// </summary>
        [Required (ErrorMessage="City is required")]
        public string City { get; set; }

        /// <summary>
        /// Gets or sets the country.
        /// </summary>
        [Required (ErrorMessage="Country is required")]
        public int CountryId { get; set; }

        /// <summary>
        /// Gets or sets the state.
        /// </summary>
       [Required (ErrorMessage="Please select a State")]
        public int StateId { get; set; }

        /// <summary>
        /// Gets or sets the zip.
        /// </summary>
       [RegularExpression(@"^\d{5}(?:[-\s]\d{4})?$", ErrorMessage = "Zip must be a number.")]
       [Required (ErrorMessage = "Please enter a zip code (Format #####)")]
        public string Zip { get; set; }

        /// <summary>
        /// Full address string for display purposes.
        /// </summary>
        public string AddressDisplay => $"{Street} {City}, {StateIdString} {Zip}";

        /// <summary>
        /// Gets or sets the contact.
        /// </summary>
        public ServiceProviderContactRequired Contact { get; set; }

        /// <summary>
        /// Gets or sets the contact person.
        /// </summary>
        public ServiceProviderContactPerson ContactPerson { get; set; }

        /// <summary>
        /// Gets or sets the coverage areas.
        /// </summary>
        [EnsureOneElement(ErrorMessage = "At least one coverage is required.")]
        public List<Coverage> Coverage { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to display address info to user.
        /// </summary>
        public bool Display { get; set; }

        /// <summary>
        /// Gets or sets the states for initial load.
        /// </summary>
        public SelectList States { get; set; }

        /// <summary>
        /// Gets or sets the State Id as a string for dropdown selection
        /// </summary>
        public string StateIdString { get; set; }


        /// <summary>
        /// Gets or sets the state of the object.
        /// </summary>
        public ObjectStatus.ObjectState State { get; set; }
    }
}
