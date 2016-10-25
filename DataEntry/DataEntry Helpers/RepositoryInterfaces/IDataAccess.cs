// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDataAccess.cs" company="UDRI">
//   Copyright © 2015 University of Dayton Research Institute. All rights reserved.
// </copyright>
// <summary>
//   Defines the IDataAccess type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DataEntry_Helpers
{
    using System.Collections.Generic;

    using Models;

    /// <summary>
    /// The DataAccess interface.
    /// </summary>
    public interface IDataAccess
    {
        /// <summary>
        /// The get countries list.
        /// </summary>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        List<Country> GetCountriesList();

        /// <summary>
        /// The get state list.
        /// </summary>
        /// <param name="countryId">
        /// The country id.
        /// </param>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        List<State> GetStateList(int countryId);

        /// <summary>
        /// The get counties list.
        /// </summary>
        /// <param name="stateId">
        /// The state id.
        /// </param>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        List<County> GetCountiesList(int stateId);

        /// <summary>
        /// The get categories.
        /// </summary>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        List<ProviderServiceCategory> GetCategories();

        /// <summary>
        /// The get all categories.
        /// </summary>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        List<ProviderServiceCategory> GetAllCategories();
        
        /// <summary>
        /// The get crime categories.
        /// </summary>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        List<ProviderServiceCategory> GetCrimeCategories();

        /// <summary>
        /// Get categories for the given family.
        /// </summary>
        /// <param name="familyId">The id of the family</param>
        /// <returns></returns>
        List<ProviderServiceCategory> GetCategoriesByFamily(int familyId);

        /// <summary>
        /// The get coverages.
        /// </summary>
        /// <param name="locationId">
        /// The location id.
        /// </param>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        List<ProviderCoverage> GetCoverages(int locationId);

        /// <summary>
        /// The get service providers.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="ServiceProvider"/>.
        /// </returns>
        ServiceProvider GetServiceProviders(int id);

        /// <summary>
        /// The get provider services.
        /// </summary>
        /// <param name="providerId">
        /// The provider id.
        /// </param>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        List<ProviderService> GetProviderServices(int providerId);
    }
}
