// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ServiceProviderContactPerson.cs" company="UDRI">
//   Copyright © 2014 University of Dayton Research Institute. All rights reserved.
// </copyright>
// <summary>
//   The service provider contact person.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;
namespace Models
{
    /// <summary>
    /// The service provider contact person.
    /// </summary>
    public class ServiceProviderContactPerson
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        public string FistName { get; set; }

        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the job title.
        /// </summary>
        public string JobTitle { get; set; }

        /// <summary>
        /// Gets or sets the contact.
        /// </summary>
        public ServiceProviderContact Contact { get; set; }

        /// <summary>
        /// Gets or sets the state of the object.
        /// </summary>
        public ObjectStatus.ObjectState State { get; set; }
    }
}
