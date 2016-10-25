// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WebToDatabaseServiceProvider.cs" company="UDRI">
//   Copyright © 2015 University of Dayton Research Institute. All rights reserved.
// </copyright>
// <summary>
//   Defines the ServiceProviderSearchResult type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------


namespace Website.Models
{
    using System.Collections.Generic;

    /// <summary>
    /// Service providers to show for search
    /// </summary>
    public class ServiceProviderSearchResult
    {
        
        /// <summary>
        /// The ID of the service provider
        /// </summary>
        public int Id { get; set; }
        
        /// <summary>
        /// The name of the service provider
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// List of county names that the provider covers across all locations
        /// </summary>
        public List<string>  Counties { get; set; }
        
        /// <summary>
        /// List of category names the service provider includes
        /// </summary>
        public List<string> Categories { get; set; } 
       
        /// <summary>
        /// Active flag
        /// </summary>
        public bool IsActive { get; set; }
    }
}