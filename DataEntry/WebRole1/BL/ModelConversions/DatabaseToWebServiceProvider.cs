// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DatabaseToWebServiceProvider.cs" company="UDRI">
//   Copyright © 2015 University of Dayton Research Institute. All rights reserved.
// </copyright>
// <summary>
//   The database to web service provider.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Web.Services.Description;
using Website.Models;

namespace Website.BL.ModelConversions
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.WebPages;
    using DataEntry_Helpers;
    using DataEntry_Helpers.Repositories;
    using global::Models;
    using global::Models.ServiceProvider;

    using Microsoft.Ajax.Utilities;

    /// <summary>
    /// The database to web service provider.
    /// </summary>
    public class DatabaseToWebServiceProvider
    {
        /// <summary>
        /// The data access.
        /// </summary>
        private readonly IDataAccess dataAccess;

        /// <summary>
        /// The service provider repo.
        /// </summary>
        private readonly IServiceProviderRepo serviceProviderRepo;

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseToWebServiceProvider"/> class.
        /// </summary>
        public DatabaseToWebServiceProvider()
        {
            this.dataAccess = new DataAccess();
            this.serviceProviderRepo = new ServiceProviderRepo();
        }

        /// <summary>
        /// The get all service providers.
        /// </summary>
        /// <param name="providerName">
        /// The provider Name.
        /// </param>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<ServiceProviderSearchResult> GetAllServiceProviders(string providerName, int pageSize, int page, int? countyId, int? categoryId)
        {
            if (providerName == "")
            {
                providerName = null;
            }

            List<ServiceProvider> databaseProvider = this.serviceProviderRepo.GetAllServiceProvidersByName(providerName, pageSize, page, countyId, categoryId);
            return databaseProvider.Select(this.createServiceProviderSearchResult).ToList();
        }

        /// <summary>
        /// Gets all the service providers but only fills in the id, name and description for speed.
        /// </summary>
        /// <returns>List of Website Service Providers</returns>
        public List<WebsiteServiceProvider> GetAllServiceProvidersNameDesctiption()
        {
            List<ServiceProvider> databaseProviders = this.serviceProviderRepo.GetAllActiveServiceProviders();

            return databaseProviders.Select(createWebsiteServiceProviderNameDescription).ToList();
        }


        /// <summary>
        /// The get service provider.
        /// </summary>  
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="WebsiteServiceProvider"/>.
        /// </returns>
        public WebsiteServiceProvider GetServiceProvider(int id)
        {
            var databaseProvider = this.serviceProviderRepo.GetServiceProvider(id);
            var websiteProvider = this.CreateWebsiteServiceProvider(databaseProvider);

            return websiteProvider;
        }

        /// <summary>
        /// Creates a service provider search result view model from the corresponding database model
        /// </summary>
        /// <param name="databaseProvider"></param>
        /// <returns>A search result with only the needed data for the view</returns>
        private ServiceProviderSearchResult createServiceProviderSearchResult(ServiceProvider databaseProvider)
        {
            var result = new ServiceProviderSearchResult
            {
                Counties    = new List<string>(),
                Categories  = new List<string>(),
                Name        = databaseProvider.ProviderName,
                IsActive    = databaseProvider.Active,
                Id          = databaseProvider.ID
            };

            foreach (var coverage in databaseProvider.Locations
                .SelectMany(location => location.ProviderCoverages.Where(coverage => !result.Counties.Contains(coverage.County.Name))))
            {
                result.Counties.Add(coverage.County.Name);
            }

            foreach (var category in databaseProvider.ProviderServices)
            {
                result.Categories.Add(category.ProviderServiceCategory.Name);
            }

            return result;
        }

        /// <summary>
        /// The create website service provider.
        /// </summary>
        /// <param name="databaseProvider">
        /// The database provider.
        /// </param>
        /// <returns>
        /// The <see cref="WebsiteServiceProvider"/>.
        /// </returns>
        private WebsiteServiceProvider CreateWebsiteServiceProvider(ServiceProvider databaseProvider)
        {
            var locations = new List<ServiceProviderLocation>();
            foreach (var databaseLocation in databaseProvider.Locations)
            {
                var location = this.CreateLocation(databaseLocation);

                locations.Add(location);
            }

            var services = this.CreateProviderServices(databaseProvider);

            var serviceProvider = new WebsiteServiceProvider
            {
                Description     = databaseProvider.Description,
                DisplayRank     = databaseProvider.DisplayRank,
                Id              = databaseProvider.ID,
                Locations       = locations,
                Name            = databaseProvider.ProviderName,
                State           = ObjectStatus.ObjectState.Read,
                Services        = services,
                Type            = databaseProvider.ServiceTypes,
                TypeName        = databaseProvider.ServiceType.Name,
                IsActive        = databaseProvider.Active
            };

            return serviceProvider;
        }


        /// <summary>
        /// The create website service provider.
        /// </summary>
        /// <param name="databaseProvider">
        /// The database provider.
        /// </param>
        /// <returns>
        /// The <see cref="WebsiteServiceProvider"/>.
        /// </returns>
        private WebsiteServiceProvider createWebsiteServiceProviderNameDescription(ServiceProvider databaseProvider)
        {
            return  new WebsiteServiceProvider
            {
                Description = databaseProvider.Description,
                Name =  databaseProvider.ProviderName,
                DisplayRank = databaseProvider.DisplayRank,
                Id = databaseProvider.ID,
                Locations = new List<ServiceProviderLocation>(),
                Services = new WebServiceAreas(),
            };
        }


        /// <summary>
        /// The create provider services.
        /// </summary>
        /// <param name="databaseProvider">
        /// The database provider.
        /// </param>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        private WebServiceAreas CreateProviderServices(ServiceProvider databaseProvider)
        {
            var databaseProviderServices = this.dataAccess.GetProviderServices(databaseProvider.ID);
            var services =
                databaseProviderServices.Select(databaseProviderService => databaseProviderService.ServiceID).ToList();
            var webServiceAreas = new WebServiceAreas { ServiceAreas = services, State = ObjectStatus.ObjectState.Read };

            return webServiceAreas;
        }

        /// <summary>
        /// The create location.
        /// </summary>
        /// <param name="location">
        /// The location.
        /// </param>
        /// <returns>
        /// The <see cref="ServiceProviderLocation"/>.
        /// </returns>
        private ServiceProviderLocation CreateLocation(Location location)
        {
            var contact = this.CreateLocationContact(location.Contact);
            var contactPerson = this.CreateServiceProviderContactPerson(location.ContactPerson);
            var coverage = this.CreateCoverage(location);

            var serviceLocation = new ServiceProviderLocation
            {
                Name = location.Name,
                City = location.City,
                Contact = contact,
                ContactPerson = contactPerson,
                CountryId = location.CountryID,
                Coverage = coverage,
                Display = location.Display,
                Id = location.ID,
                State = ObjectStatus.ObjectState.Read,
                StateId = location.StateID,
                StateIdString = location.State.Abbreviation,
                Street = location.Street,
                Zip = location.Zip
            };

            return serviceLocation;
        }

        /// <summary>
        /// The create service provider contact person.
        /// </summary>
        /// <param name="contactPerson">
        /// The contact person.
        /// </param>
        /// <returns>
        /// The <see cref="ServiceProviderContactPerson"/>.
        /// </returns>
        public ServiceProviderContactPerson CreateServiceProviderContactPerson(ContactPerson contactPerson)
        {
            if (contactPerson == null)
            {
                return null;
            }

            var person = new ServiceProviderContactPerson
            {
                Id = contactPerson.ID,
                FistName = contactPerson.FirstName,
                LastName = contactPerson.LastName,
                JobTitle = contactPerson.JobTitle,
                Contact = this.CreateContact(contactPerson.Contact),
                State = ObjectStatus.ObjectState.Read
            };
            return person;
        }

        /// <summary>
        /// Get the service provider edit log.
        /// </summary>
        /// <returns> The <see cref="List"/> of the log items. </returns>f
        public List<EditLogItemViewModel> GetServiceProviderEditLog()
        {
            var found = this.serviceProviderRepo.GetServiceProviderEditLog();

            return found.Select(item => new EditLogItemViewModel
            {
                EditedProviderId = item.ProviderId,
                EditedProviderName = (item.ProviderId != null)? item.ServiceProvider.ProviderName : item.DeletedProviderName,
                UserId = item.UserId,
                UserFirstName = item.User.FirstName,
                UserLastName = item.User.LastName,
                EditedDateTime = item.DateTime,
                Action = item.Action
            }).ToList();
        }




        /// <summary>
        /// Creates a view model contact with a required phone number from a database contact.
        /// </summary>
        /// <param name="contact"> The contact. </param>
        /// <returns> The <see cref="ServiceProviderContact"/>. </returns>
        private ServiceProviderContactRequired CreateLocationContact(Contact contact)
        {
            var webContact = new ServiceProviderContactRequired
            {
                Email = contact.Email,
                HelpLine = contact.HelpLine,
                PhoneNumber = contact.Phone,
                Id = contact.ID,
                Website = contact.Website,
                State = ObjectStatus.ObjectState.Read
            };
            return webContact;
        }


        /// <summary>
        /// Creates a view model contact object from the database contact.
        /// </summary>
        /// <param name="contact"> The contact. </param>
        /// <returns> The <see cref="ServiceProviderContact"/>. </returns>
        private ServiceProviderContact CreateContact(Contact contact)
        {
            var webContact = new ServiceProviderContact
            {
                Email = contact.Email,
                HelpLine = contact.HelpLine,
                PhoneNumber = contact.Phone,
                Id = contact.ID,
                Website = contact.Website,
                State = ObjectStatus.ObjectState.Read
            };
            return webContact;
        }

        /// <summary>
        /// The create coverage.
        /// </summary>
        /// <param name="location">
        /// The location.
        /// </param>
        /// <returns>
        /// The <see cref="Coverage"/>.
        /// </returns>
        private List<Coverage> CreateCoverage(Location location)
        {
            var databaseCoverages = this.dataAccess.GetCoverages(location.ID);
            var webCoverage = new List<Coverage>();
            foreach (var databaseCoverage in databaseCoverages)
            {
                var coverage = new Coverage
                                   {
                                       Id = databaseCoverage.ID,
                                       CountryId = location.CountryID,
                                       CountryName = location.Country.FullName,
                                       StateId = location.StateID,
                                       StateName = location.State.FullName,
                                       CountyId = databaseCoverage.AreaID,
                                       CountyName = databaseCoverage.County.Name,
                                       State = ObjectStatus.ObjectState.Read
                                   };
                webCoverage.Add(coverage);
            }

            return webCoverage;
        }
    }
}