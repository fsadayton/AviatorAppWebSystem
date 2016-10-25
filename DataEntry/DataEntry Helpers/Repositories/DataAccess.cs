// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataAccess.cs" company="UDRI">
//   Copyright © 2015 University of Dayton Research Institute. All rights reserved.
// </copyright>
// <summary>
//   The data access.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DataEntry_Helpers.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// The data access.
    /// </summary>
    public class DataAccess : Repository, IDataAccess
    {
        /// <summary>
        /// The get countries list.
        /// </summary>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<Country> GetCountriesList()
        {
            List<Country> response;
            try
            {
                response = this.db.Countries.ToList();
            }
            catch (Exception e)
            {
                response = null;
            }

            return response;
        }

        /// <summary>
        /// The get state list.
        /// </summary>
        /// <param name="countryId">
        /// The country id.
        /// </param>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<State> GetStateList(int countryId)
        {
            List<State> response;
            try
            {
                response = this.db.States.Where(r => r.CountryID == countryId).OrderBy(n => n.FullName).ToList();
            }
            catch (Exception e)
            {
                response = null;
            }
           
            return response;
        }

        /// <summary>
        /// The get counties list.
        /// </summary>
        /// <param name="stateId">
        /// The state id.
        /// </param>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<County> GetCountiesList(int stateId)
        {
            List<County> response;
            try
            {
                response = this.db.Counties.Where(r => r.StateId == stateId).ToList();
            }
            catch (Exception e)
            {
                response = null;
            }

            return response;
        }

        /// <summary>
        /// The get categories.
        /// </summary>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<ProviderServiceCategory> GetCategories()
        {
            List<ProviderServiceCategory> response;
            try
            {
                response = this.db.ProviderServiceCategories.Where(r => r.Active).ToList();
            }
            catch (Exception e)
            {
                response = null;
            }

            return response;
        }

        /// <summary>
        /// Get all categories.
        /// </summary>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<ProviderServiceCategory> GetAllCategories()
        {
            List<ProviderServiceCategory> response;
            try
            {
                response = this.db.ProviderServiceCategories.ToList();
            }
            catch (Exception e)
            {
                response = null;
            }

            return response;
        }

        public List<ProviderService> GetProviderServiceCategories(int providerId)
        {
            List<ProviderService> response;
            try
            {
                response = this.db.ProviderServices.Where(p => p.ProviderID == providerId).ToList();
            }
            catch (Exception e)
            {
                response = null;
            }

            return response;
        }

        public List<ProviderServiceCategory> GetCrimeCategories()
        {
            List<ProviderServiceCategory> response;
            try
            {
                response = this.db.ProviderServiceCategories.Where(r => r.Active & r.Crime).ToList();
            }
            catch (Exception e)
            {
                response = null;
            }

            return response;
        }

        /// <summary>
        /// Get list of categories for the given family 
        /// </summary>
        /// <param name="familyId">Id of the family</param>
        /// <returns>List of categories</returns>
        public List<ProviderServiceCategory> GetCategoriesByFamily(int familyId)
        {
            List<ProviderServiceCategory> response;
            try
            {
                response =
                    this.db.FamilyServices.Where(fs => fs.FamilyID == familyId && fs.ProviderServiceCategory.Active)
                        .Select(fs => fs.ProviderServiceCategory)
                        .ToList();
            }
            catch (Exception ex)
            {
                response = null;
            }

            return response;
        }

        /// <summary>
        /// The get provider services.
        /// </summary>
        /// <param name="providerId">
        /// The provider id.
        /// </param>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<ProviderService> GetProviderServices(int providerId)
        {
            var services = new List<ProviderService>();
            try
            {
                services.AddRange(this.db.ProviderServices.Where(p => p.ProviderID == providerId));
            }
            catch (Exception)
            {
                services = null;
            }
           
            return services;
        }

        /// <summary>
        /// The get service providers.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="ServiceProvider"/>.
        /// </returns>
        public ServiceProvider GetServiceProviders(int id)
        {
            var provider = this.db.ServiceProviders.Single(p => p.ID == id);

            return provider;
        }

        /// <summary>
        /// The get coverages.
        /// </summary>
        /// <param name="locationId">
        /// The location id.
        /// </param>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<ProviderCoverage> GetCoverages(int locationId)
        {
            try
            {
                var coverageAreas = this.db.ProviderCoverages.Where(c => c.LocationID == locationId).ToList();

                return coverageAreas;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
