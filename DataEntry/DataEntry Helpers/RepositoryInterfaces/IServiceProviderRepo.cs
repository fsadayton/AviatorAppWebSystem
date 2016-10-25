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
    using Models.ServiceProvider;

    /// <summary>
    /// The DataAccess interface.
    /// </summary>
    public interface IServiceProviderRepo
    {
        /// <summary>
        /// The get all the active service providers.
        /// </summary>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        List<ServiceProvider> GetAllActiveServiceProviders();

        /// <summary>
        /// The get all service providers by name.
        /// </summary>
        /// <param name="providerName">
        /// The provider Name.
        /// </param>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        List<ServiceProvider> GetAllServiceProvidersByName(string providerName, int pageSize, int pageNumber, int? countyId, int? categoryId);


        /// <summary>
        /// Get the count of all service providers by name.
        /// </summary>
        /// <param name="providerName">
        /// The provider Name.
        /// </param>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        int GetAllServiceProvidersByNameCount(string providerName, int? countyId, int? categoryId);
        

        /// <summary>
        /// The get service provide.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="ServiceProvider"/>.
        /// </returns>
        ServiceProvider GetServiceProvider(int id);
        

        /// <summary>
        /// The get service providers.
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
        List<ServiceProvider> GetServiceProviders(List<int> counties, List<int> categories);

        /// <summary>
        /// The update service provider.
        /// </summary>
        /// <param name="serviceProvider">
        /// The service provider.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
      //  bool UpdateServiceProvider(WebsiteServiceProvider serviceProvider);

        bool UpdateServiceArea(int id, List<int> newList);

        bool UpdateProvider(WebsiteServiceProvider serviceProvider);

        bool UpdateContactPerson(ServiceProviderContactPerson serviceProviderContactPerson);

        bool DeleteContactPerson(ServiceProviderContactPerson contactPerson);
        
        bool UpdateCoverage(int id, Coverage newList);
        bool UpdateContact(Contact contact);

        bool UpdateLocation(ServiceProviderLocation serviceLocation);

        /// <summary>
        /// The create service provider.
        /// </summary>
        /// <param name="serviceProvider">
        /// The service provider.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        int CreateServiceProvider(ServiceProvider serviceProvider);

        /// <summary>
        /// The create contact.
        /// </summary>
        /// <param name="contact">
        /// The contact.
        /// </param>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        int CreateContact(Contact contact);

        /// <summary>
        /// The create location.
        /// </summary>
        /// <param name="location">
        /// The location.
        /// </param>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        int CreateLocation(Location location);

        /// <summary>
        /// The create contact person.
        /// </summary>
        /// <param name="person">
        /// The person.
        /// </param>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        int CreateContactPerson(ContactPerson person);

        /// <summary>
        /// The create coverage.
        /// </summary>
        /// <param name="coverage">
        /// The coverage.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        bool CreateCoverage(ProviderCoverage coverage);

        /// <summary>
        /// The create service area.
        /// </summary>
        /// <param name="service">
        /// The service.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        bool CreateServiceArea(ProviderService service);

        /// <summary>
        /// The delete provider service.
        /// </summary>
        /// <param name="providerServiceId">
        /// The provider service id.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        bool DeleteProviderService(int providerServiceId);

        /// <summary>
        /// The delete location.
        /// </summary>
        /// <param name="locationId">
        /// The location id.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        bool DeleteLocation(int locationId);

        /// <summary>
        /// The delete contact.
        /// </summary>
        /// <param name="contact">
        /// The contact.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        bool DeleteContact(Contact contact);

        /// <summary>
        /// The delete coverages.
        /// </summary>
        /// <param name="locationId">
        /// The location id.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        bool DeleteAllCoveragesForLocation(int locationId);

        /// <summary>
        /// Delete a coverage.
        /// </summary>
        /// <param name="coverageId">
        /// The coverage id.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        bool DeleteCoverage(int coverageId);

        /// <summary>
        /// The delete provider.
        /// </summary>
        /// <param name="serviceProvider">
        /// The service provider.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        bool DeleteProvider(WebsiteServiceProvider serviceProvider);


        /// <summary>
        /// The get service provider edit log.
        /// </summary>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        List<ServiceProviderEdit> GetServiceProviderEditLog();

        /// <summary>
        /// The log service provider edit.
        /// </summary>
        /// <param name="entryToSave">
        /// The entry to save.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        bool LogServiceProviderEdit(ServiceProviderEdit entryToSave);
    }
}
