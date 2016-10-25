// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LawEnforcementRepo.cs" company="UDRI">
//   Copyright © 2015 University of Dayton Research Institute. All rights reserved.
// </copyright>
// <summary>
//   The compensation repo.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DataEntry_Helpers.Repositories
{
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;

    using DataEntry_Helpers.RepositoryInterfaces;

    /// <summary>
    /// The Law Enforcement repo.
    /// </summary>
    public class LawEnforcementRepo : Repository, ILawEnforcementRepo
    {
        /// <summary>
        /// The get service providers.
        /// </summary>
        /// <param name="counties">
        /// The counties.
        /// </param>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<ServiceProvider> GetServiceProviders(List<int> counties)
        {
            var found = this.db.ServiceProviders
             .Where(sp => sp.Active && sp.ServiceTypes == 2)
             .Where(sp => sp.Locations.Any(l => l.ProviderCoverages.Any(pc => counties.Contains(pc.AreaID))))
             .OrderBy(sp => sp.DisplayRank).ThenBy(sp => sp.ProviderName)
             .ToList();

            return found;

        }
    }
}
