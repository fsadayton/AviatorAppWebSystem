// --------------------------------------------------------------------------------------------------------------------
// <copyright file="VeteranProviderQueryLogics.cs" company="UDRI">
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
    /// The Veteran provider query logics.
    /// </summary>
    public class VeteranProviderQueryLogics
    {

        /// <summary>
        /// The Veteran provider repo.
        /// </summary>
        private readonly IVeteransRepo veteransRepo;


        /// <summary>
        /// Tool to create a provider.
        /// </summary>
        private readonly DisplayProviderCreator displayProviderCreator;

        /// <summary>
        /// Initializes a new instance of the <see cref="VeteranProviderQueryLogics"/> class.
        /// </summary>
        public VeteranProviderQueryLogics()
        {
            this.veteransRepo = new VeteransRepo();
            displayProviderCreator = new DisplayProviderCreator();
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
            var result = this.veteransRepo.GetServiceProviders(counties);
            return displayProviderCreator.CreateDisplayProvider(result, counties);
        }
      
    }
}