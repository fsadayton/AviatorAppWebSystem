// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CompensationProviderQueryLogics.cs" company="UDRI">
//   Copyright © 2015 University of Dayton Research Institute. All rights reserved.
// </copyright>
// <summary>
//   The service provider query logics.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Website.BL
{
    using System.Collections.Generic;
    using DataEntry_Helpers.Repositories;
    using DataEntry_Helpers.RepositoryInterfaces;

    using Website.Models;

    /// <summary>
    /// The Compensation provider query logics.
    /// </summary>
    public class CompensationProviderQueryLogics
    {
        /// <summary>
        /// The compensation repo.
        /// </summary>
        private readonly ICompensationRepo compensationRepo;


        /// <summary>
        /// Tool to create a provider.
        /// </summary>
        private readonly DisplayProviderCreator displayProviderCreator;

        /// <summary>
        /// Initializes a new instance of the <see cref="CompensationProviderQueryLogics"/> class.
        /// </summary>
        public CompensationProviderQueryLogics()
        {
            this.compensationRepo = new CompensationRepo();
            this.displayProviderCreator = new DisplayProviderCreator();
        }

        /// <summary>
        /// The service provider query.
        /// </summary>
        /// <param name="counties">
        /// The counties.
        /// </param>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<DisplayServiceProvider> ServiceProviderQuery(List<int> counties)
        {
            var result = this.compensationRepo.GetServiceProviders(counties);
            return this.displayProviderCreator.CreateDisplayProvider(result, counties);
        }
    }
}