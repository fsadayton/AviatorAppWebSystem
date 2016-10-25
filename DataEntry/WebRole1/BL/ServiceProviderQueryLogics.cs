// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ServiceProviderQueryLogics.cs" company="UDRI">
//   Copyright © 2015 University of Dayton Research Institute. All rights reserved.
// </copyright>
// <summary>
//   The service provider query logics.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Website.BL
{
    using System.Collections.Generic;
    using DataEntry_Helpers;
    using DataEntry_Helpers.Repositories;

    using Website.Models;

    /// <summary>
    /// The service provider query logics.
    /// </summary>
    public class ServiceProviderQueryLogics
    {
        /// <summary>
        /// The service provider repo.
        /// </summary>
        private readonly IServiceProviderRepo serviceProviderRepo;

        private readonly DisplayProviderCreator displayProviderCreator;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceProviderQueryLogics"/> class.
        /// </summary>
        public ServiceProviderQueryLogics()
        {
            this.serviceProviderRepo = new ServiceProviderRepo();
            displayProviderCreator = new DisplayProviderCreator();
        }

        /// <summary>
        /// The service provider query.
        /// </summary>
        /// <param name="counties">
        /// The counties.
        /// </param>
        /// <param name="categories">
        /// The categories.
        /// </param>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<DisplayServiceProvider> ServiceProviderQuery(List<int> counties, List<int> categories)
        {
            var result = this.serviceProviderRepo.GetServiceProviders(counties, categories);
            return displayProviderCreator.CreateDisplayProvider(result, counties);
        }
    }
}