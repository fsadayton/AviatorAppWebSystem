﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ILawEnforcementRepo.cs" company="UDRI">
//   Copyright © 2015 University of Dayton Research Institute. All rights reserved.
// </copyright>
// <summary>
//   The CompensationRepo interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DataEntry_Helpers.RepositoryInterfaces
{
    using System.Collections.Generic;

    /// <summary>
    /// The LawEnforcementRepo interface.
    /// </summary>
    public interface ILawEnforcementRepo
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
        List<ServiceProvider> GetServiceProviders(List<int> counties);
    }
}
