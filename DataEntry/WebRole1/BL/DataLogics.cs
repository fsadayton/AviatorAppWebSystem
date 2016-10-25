// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataLogics.cs" company="UDRI">
//   Copyright © 2015 University of Dayton Research Institute. All rights reserved.
// </copyright>
// <summary>
//   The data logics.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using Models.ServiceProvider;
using Website.Models;

namespace Website.BL
{
    using System.Collections.Generic;
    using System.Linq;
    using DataEntry_Helpers;
    using DataEntry_Helpers.Repositories;
    using ModelConversions;
    using global::Models;

    /// <summary>
    /// The data logics.
    /// </summary>
    public class DataLogics
    {
        /// <summary>
        /// The data access.
        /// </summary>
        private readonly IDataAccess dataAccess;

        /// <summary>
        /// Converter from database models to Website models
        /// </summary>
        private readonly DatabaseToWebServiceProvider converter;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataLogics"/> class.
        /// </summary>
        public DataLogics()
        {
            this.dataAccess = new DataAccess();
            this.converter = new DatabaseToWebServiceProvider();
        }

        /// <summary>
        /// Gets the count of service providers for the given search fields
        /// </summary>
        /// <param name="searchText">The search text by provider name</param>
        /// <param name="countyId">County to search within </param>
        /// <param name="categoryId">Category id to search for</param>
        /// <returns></returns>
        public int GetServiceProvidersCount(string searchText, int? countyId, int? categoryId)
        {
            if (searchText == "")
            {
                searchText = null;
            }
            var serviceProviderRepo = new ServiceProviderRepo();
            return serviceProviderRepo.GetAllServiceProvidersByNameCount(searchText, countyId, categoryId);
        }

        /// <summary>
        /// Gets all the service providers in the database
        /// </summary>
        /// <returns>List of service providers</returns>
        public List<ServiceProviderSearchResult> GetServiceProviders(string searchText, int pageSize, int page, int? countyId,  int? categoryId)
        {
            return this.converter.GetAllServiceProviders(searchText, pageSize, page, countyId, categoryId);
        }

        /// <summary>
        /// Gets the Service provider with the given ID.
        /// </summary>
        /// <param name="id">ID of the service provider</param>
        /// <returns>Service provider that is found.</returns>
        public WebsiteServiceProvider GetServiceProviderById(int id)
        {
            return this.converter.GetServiceProvider(id);
        }


        /// <summary>
        /// Gets service provider names and descriptions.
        /// </summary>
        /// <returns> The <see cref="List"/>. </returns>
        public List<WebsiteServiceProvider> GetServiceProviderNamesAndDescription()
        {
            return this.converter.GetAllServiceProvidersNameDesctiption();
        }

        /// <summary>
        /// The get countries.
        /// </summary>
        /// <returns>
        /// The <see cref="WebsiteCountries"/>.
        /// </returns>
        public List<WebsiteCountry> GetCountries()
        {
            var databaseCountries = this.dataAccess.GetCountriesList();
            return this.ConvertDatabaseCountries(databaseCountries);
        }

        /// <summary>
        /// The get states.
        /// </summary>
        /// <param name="countryId">
        /// The country id.
        /// </param>
        /// <returns>
        /// The <see cref="WebsiteStates"/>.
        /// </returns>
        public List<WebsiteState> GetStates(int countryId)
        {
            var databaseStates = this.dataAccess.GetStateList(countryId);
            return this.ConvertDatabaseStates(databaseStates);
        }

        /// <summary>
        /// The get counties.
        /// </summary>
        /// <param name="stateId">
        /// The state id.
        /// </param>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<WebsiteCounty> GetCounties(int stateId = 34)
        {
            var databaseCounties = this.dataAccess.GetCountiesList(stateId);
            return this.ConvertDatabaseCounties(databaseCounties);
        }

        /// <summary>
        /// The get categories.
        /// </summary>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<Category> GetCategories()
        {
            var databaseCategories = this.dataAccess.GetCategories();
            return this.ConvertDatabaseCategories(databaseCategories);
        }

        /// <summary>
        /// The get crime categories.
        /// </summary>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<Category> GetCrimeCategories()
        {
            var databaseCategories = this.dataAccess.GetCrimeCategories();
            return this.ConvertDatabaseCategories(databaseCategories);
        }

        /// <summary>
        /// The convert web counties.
        /// </summary>
        /// <param name="webCounties">
        /// The web counties.
        /// </param>
        /// <returns>
        /// The <see cref="Dictionary"/>.
        /// </returns>
        public Dictionary<int, string> ConvertWebCounties(List<WebsiteCounty> webCounties)
        {
            return webCounties.ToDictionary(websiteCounty => websiteCounty.Id, websiteCounty => websiteCounty.Name);
        }

        /// <summary>
        /// The convert database countries.
        /// </summary>
        /// <param name="databaseCountries">
        /// The database countries.
        /// </param>
        /// <returns>
        /// The <see cref="WebsiteCountries"/>.
        /// </returns>
        public List<WebsiteCountry> ConvertDatabaseCountries(List<Country> databaseCountries)
        {
            return databaseCountries.Select(
                databaseCountry =>
                    new WebsiteCountry
                    {
                        Id = databaseCountry.ID,
                        Abbreviation = databaseCountry.Abbreviation,
                        FullName = databaseCountry.FullName
                    }).ToList();
        }

        /// <summary>
        /// The convert database counties.
        /// </summary>
        /// <param name="databaseCounties">
        /// The database counties.
        /// </param>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<WebsiteCounty> ConvertDatabaseCounties(List<County> databaseCounties)
        {
            return databaseCounties?.Select(
                    databaseCounty => new WebsiteCounty { Id = databaseCounty.ID, Name = databaseCounty.Name })
                    .ToList();
        }

        /// <summary>
        /// The convert database categories.
        /// </summary>
        /// <param name="databaseCategories">
        /// The database categories.
        /// </param>
        /// <returns>
        /// The <see cref="CreateCategories"/>.
        /// </returns>
        public List<Category> ConvertDatabaseCategories(List<ProviderServiceCategory> databaseCategories)
        {
            return databaseCategories.Select(
                    databaseCategory =>
                    new Category
                        {
                            Id = databaseCategory.ID,
                            Name = databaseCategory.Name,
                            Description = databaseCategory.Description,
                            Crime = databaseCategory.Crime
                        }).ToList();
        }

        /// <summary>
        /// The convert database states.
        /// </summary>
        /// <param name="databaseStates">
        /// The database states.
        /// </param>
        /// <returns>
        /// The <see cref="WebsiteStates"/>.
        /// </returns>
        public List<WebsiteState> ConvertDatabaseStates(List<State> databaseStates)
        {
           return databaseStates.Select(
                    databaseState => new WebsiteState { Id = databaseState.ID, Name = databaseState.Abbreviation })
                    .ToList();
        }

        /// <summary>
        /// The get website categories.
        /// </summary>
        /// <returns> The <see cref="List"/>. </returns>
        public List<WebsiteCategory> GetWebsiteCategories()
        {
            var databaseCategories = this.dataAccess.GetCategories();
            return this.ConvertDatabaseCategoriesToWebsiteCategories(databaseCategories);
        }

        /// <summary>
        /// Get categories for a family.
        /// </summary>
        /// <param name="familyId">The id of the family</param>
        /// <returns>List of website categories.</returns>
        public List<WebsiteCategory> GetWebsiteCategories(int familyId)
        {
            var databaseCategories = this.dataAccess.GetCategoriesByFamily(familyId);
            return this.ConvertDatabaseCategoriesToWebsiteCategories(databaseCategories);
        }

        /// <summary>
        /// The convert database categories to website categories.
        /// </summary>
        /// <param name="databaseCategories">
        /// The database categories.
        /// </param>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<WebsiteCategory> ConvertDatabaseCategoriesToWebsiteCategories(List<ProviderServiceCategory> databaseCategories)
        {
            if (databaseCategories == null)
            {
                return null;
            }

            var list = new List<WebsiteCategory>();

            foreach (var databaseCategory in databaseCategories)
            {
                var newCategory = new WebsiteCategory
                {
                    Id = databaseCategory.ID,
                    Name = databaseCategory.Name,
                    Description = databaseCategory.Description,
                    Crime = databaseCategory.Crime,
                    ServiceTypes = new List<WebsiteCategoryType>()
                };
                foreach (var databaseType in databaseCategory.CategoryTypes)
                {
                    newCategory.ServiceTypes.Add(
                        new WebsiteCategoryType
                        {
                            Id = databaseType.ServiceType.ID,
                            Name = databaseType.ServiceType.Name
                        });
                }
                list.Add(newCategory);
            }

            return list;
        }
    }
}