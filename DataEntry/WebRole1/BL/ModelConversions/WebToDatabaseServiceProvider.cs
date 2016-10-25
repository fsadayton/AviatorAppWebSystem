// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WebToDatabaseServiceProvider.cs" company="UDRI">
//   Copyright © 2015 University of Dayton Research Institute. All rights reserved.
// </copyright>
// <summary>
//   Defines the WebToDatabaseServiceProvider type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Website.BL.ModelConversions
{
    using System;
    using System.Collections.Generic;
    using DataEntry_Helpers;
    using DataEntry_Helpers.Repositories;
    using global::Models;
    using global::Models.ServiceProvider;

    /// <summary>
    /// The web to database service provider.
    /// </summary>
    public class WebToDatabaseServiceProvider
    {
        /// <summary>
        /// The service provider repo.
        /// </summary>
        private readonly IServiceProviderRepo serviceProviderRepo;

        /// <summary>
        /// The data access.
        /// </summary>
        private readonly IDataAccess dataAccess;

        /// <summary>
        /// Initializes a new instance of the <see cref="WebToDatabaseServiceProvider"/> class.
        /// </summary>
        public WebToDatabaseServiceProvider()
        {
            this.serviceProviderRepo = new ServiceProviderRepo();
            this.dataAccess = new DataAccess();
        }

        /// <summary>
        /// The update service provider.
        /// </summary>
        /// <param name="provider">
        /// The provider.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool UpdateServiceProvider(WebsiteServiceProvider provider, int userId)
        {
            return this.CheckServiceProvider(provider, userId);
        }

        /// <summary>
        /// The check service provider.
        /// </summary>
        /// <param name="serviceProvider">
        /// The service provider.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        private bool CheckServiceProvider(WebsiteServiceProvider serviceProvider, int userId)
        {
            bool response;
            switch (serviceProvider.State)
            {
                case ObjectStatus.ObjectState.Update:
                    response = this.UpdateProvider(serviceProvider, userId);
                    break;
                case ObjectStatus.ObjectState.Delete:
                    response = this.DeleteProvider(serviceProvider, userId);
                    break;
                case ObjectStatus.ObjectState.Create:
                    response = this.CreateServiceProvider(serviceProvider);
                    break;
                default:
                    response = false;
                    break;
            }

            return response;
        }

        /// <summary>
        /// The create service provider.
        /// </summary>
        /// <param name="provider">
        /// The provider.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        private bool CreateServiceProvider(WebsiteServiceProvider provider)
        {
            var databaseProviderId = this.CreateDatabaseProvider(provider);
            if (databaseProviderId == 0)
            {
                return false;
            }

            var createdServiceAreas = this.CreateServiceAreas(provider.Services.ServiceAreas, databaseProviderId);
            if (!createdServiceAreas)
            {
                return false;
            }

            var created = this.CreateLocations(provider.Locations, databaseProviderId);
            return created;
        }

        /// <summary>
        /// The delete provider.
        /// </summary>
        /// <param name="serviceProvider">
        /// The service provider.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        private bool DeleteProvider(WebsiteServiceProvider serviceProvider, int userId)
        {
            var name = serviceProvider.Name;
            if (!this.DeleteLocations(serviceProvider.Locations))
            {
                return false;
            }

            if (!this.DeleteProviderServices(serviceProvider))
            {
                return false;
            }

            if (!this.serviceProviderRepo.DeleteProvider(serviceProvider))
            {
                return false;
            }
            else
            {
                return this.serviceProviderRepo.LogServiceProviderEdit(new ServiceProviderEdit
                {
                    ProviderId = null,
                    UserId = userId,
                    DateTime = DateTime.Now,
                    Action = "Deleted",
                    DeletedProviderName = serviceProvider.Name
                });
            }
        }

        /// <summary>
        /// The delete provider services.
        /// </summary>
        /// <param name="serviceProvider">
        /// The service provider.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        private bool DeleteProviderServices(WebsiteServiceProvider serviceProvider)
        {
            var providerCoverages = this.dataAccess.GetProviderServices(serviceProvider.Id);
            if (providerCoverages == null || providerCoverages.Count <= 0)
            {
                return false;
            }

            return this.DeleteProviderServices(providerCoverages);
        }

        /// <summary>
        /// The delete locations.
        /// </summary>
        /// <param name="locations">
        /// The locations.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        private bool DeleteLocations(List<ServiceProviderLocation> locations)
        {
            foreach (var serviceProviderLocation in locations)
            {
                if (serviceProviderLocation.ContactPerson != null)
                {
                    if (!this.serviceProviderRepo.DeleteContact(this.ConvertServiceProviderContactToContact(serviceProviderLocation.ContactPerson.Contact)))
                    {
                        return false;
                    }

                    if (!this.serviceProviderRepo.DeleteContactPerson(serviceProviderLocation.ContactPerson))
                    {
                        return false;
                    }
                }

                if (!this.serviceProviderRepo.DeleteContact(this.ConvertServiceProviderContactToContact(serviceProviderLocation.Contact)))
                {
                    return false;
                }

                if (!this.serviceProviderRepo.DeleteAllCoveragesForLocation(serviceProviderLocation.Id))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Delete a location.
        /// </summary>
        /// <param name="location">
        /// The location.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        private bool DeleteLocation(ServiceProviderLocation location)
        {
            if (location.ContactPerson != null)
            {
                if (!this.serviceProviderRepo.DeleteContact(this.ConvertServiceProviderContactToContact(location.ContactPerson.Contact)))
                {
                    return false;
                }

                if (!this.serviceProviderRepo.DeleteContactPerson(location.ContactPerson))
                {
                    return false;
                }
            }

            if (!this.serviceProviderRepo.DeleteContact(this.ConvertServiceProviderContactToContact(location.Contact)))
            {
                return false;
            }

            if (!this.serviceProviderRepo.DeleteAllCoveragesForLocation(location.Id))
            {
                return false;
            }

            return this.serviceProviderRepo.DeleteLocation(location.Id);
        }

        // <summary>
        /// Delete provider services.
        /// </summary>
        /// <param name="providerCoverages">
        /// The provider coverages.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        private bool DeleteProviderServices(List<ProviderService> providerCoverages)
        {
            var deleted = true;
            foreach (var providerCoverage in providerCoverages)
            {
                if (!deleted)
                {
                    return false;
                }

                deleted = this.serviceProviderRepo.DeleteProviderService(providerCoverage.ID);
            }

            return deleted;
        }

        /// <summary>
        /// The update provider.
        /// </summary>
        /// <param name="serviceProvider">
        /// The service provider.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        private bool UpdateProvider(WebsiteServiceProvider serviceProvider, int userId)
        {
            if (!this.UpdateAreas(serviceProvider.Services, serviceProvider.Id))
            {
                return false;
            }

            if (!this.CheckLocations(serviceProvider.Locations, serviceProvider.Id))
            {
                return false;
            }

            var response = this.serviceProviderRepo.UpdateProvider(serviceProvider);
            this.serviceProviderRepo.LogServiceProviderEdit(new ServiceProviderEdit
            {
                ProviderId = serviceProvider.Id,
                UserId = userId,
                DateTime = DateTime.Now,
                Action = "Changed"
            });

            return response;
        }

        /// <summary>
        /// Checks locations.
        /// </summary>
        /// <param name="locations">
        ///     The locations.
        /// </param>
        /// <param name="id"></param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        private bool CheckLocations(List<ServiceProviderLocation> locations, int id)
        {
            bool response = true;
            foreach (var serviceProviderLocation in locations)
            {
                switch (serviceProviderLocation.State)
                {
                    case ObjectStatus.ObjectState.Update:
                        
                        if (serviceProviderLocation.ContactPerson != null)
                        {
                            if (!this.CheckContactPerson(serviceProviderLocation))
                            {
                                return false;
                            }
                        }

                        if (!this.CheckContact(serviceProviderLocation.Contact))
                        {
                            return false;
                        }

                        if (!this.CheckCoverage(serviceProviderLocation.Coverage, serviceProviderLocation.Id))
                        {
                            return false;
                        }


                        response = this.serviceProviderRepo.UpdateLocation(serviceProviderLocation);

                        break;
                    case ObjectStatus.ObjectState.Delete:
                        response = this.DeleteLocation(serviceProviderLocation);
                        if (!response)
                        {
                            return false;
                        }

                        continue;
                    case ObjectStatus.ObjectState.Create:
                        response = this.CreateLocation(serviceProviderLocation, id);
                        break;
                    case ObjectStatus.ObjectState.Read:
                        response = true;
                        break;
                    default:
                        return false;
                        break;
                }
            }

            return response;
        }

        /// <summary>
        /// The check coverage.
        /// </summary>
        /// <param name="coverage">
        /// The coverage.
        /// </param>
        /// <param name="locationId">
        /// The provider id.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        private bool CheckCoverage(List<Coverage> coverage, int locationId)
        {
            var response = true;
            foreach (var coverage1 in coverage)
            {
                if (!response)
                {
                    return false;
                }

                switch (coverage1.State)
                {
                    case ObjectStatus.ObjectState.Create:
                        var newCoverare = new ProviderCoverage { LocationID = locationId, AreaID = coverage1.CountyId };
                        response = this.serviceProviderRepo.CreateCoverage(newCoverare);
                        break;
                    case ObjectStatus.ObjectState.Update:
                        response = this.serviceProviderRepo.UpdateCoverage(locationId, coverage1);
                        break;
                    case ObjectStatus.ObjectState.Delete:
                        response = this.serviceProviderRepo.DeleteCoverage(coverage1.Id);
                        break;
                    case ObjectStatus.ObjectState.Read:
                        break;
                    default:
                        response = false;
                        break;
                }
            }

            return response;
        }

        /// <summary>
        /// Checks the contact person.
        /// </summary>
        /// <param name="contactPerson">
        /// The contact person.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        private bool CheckContactPerson(ServiceProviderLocation location)
        {
            bool response;
            var contactPerson = location.ContactPerson;
            switch (location.ContactPerson.State)
            {
                case ObjectStatus.ObjectState.Create:
                  
                    var contactPersonContactId = this.CreateContactId(this.ConvertServiceProviderContactToContact(contactPerson.Contact));
                    var contactPersonId = this.CreateContactPerson(contactPerson, contactPersonContactId);

                    location.ContactPerson.Id = contactPersonId;
                    return true;
                    break;
                case ObjectStatus.ObjectState.Update:
                    response = this.serviceProviderRepo.UpdateContactPerson(contactPerson);
                    break;
                case ObjectStatus.ObjectState.Delete:
                    var contact = contactPerson.Contact;
                    response = this.serviceProviderRepo.DeleteContactPerson(contactPerson)
                               && this.serviceProviderRepo.DeleteContact(this.ConvertServiceProviderContactToContact(contact));
                    return response;
                case ObjectStatus.ObjectState.Read:
                    response = true;
                    break;
                default:
                    response = false;
                    break;
            }

            if (!response)
            {
                return false;
            }

            response = this.CheckContact(contactPerson.Contact);

            return response;
        }

        /// <summary>
        /// The check contact.
        /// </summary>
        /// <param name="contact">
        /// The contact.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        private bool CheckContact(ServiceProviderContact contact)
        {
            bool response;
            switch (contact.State)
            {
                case ObjectStatus.ObjectState.Update:
                    response = this.serviceProviderRepo.UpdateContact(this.ConvertServiceProviderContactToContact(contact));
                    break;
                case ObjectStatus.ObjectState.Delete:
                    response = this.serviceProviderRepo.DeleteContact(this.ConvertServiceProviderContactToContact(contact));
                    break;
                case ObjectStatus.ObjectState.Read:
                    response = true;
                    break;
                default:
                    response = false;
                    break;
            }

            return response;
        }

        /// <summary>
        /// The check contact for a contact with a required phone number
        /// </summary>
        /// <param name="contact"> The contact. </param>
        /// <returns> The <see cref="bool"/>. </returns>
        private bool CheckContact(ServiceProviderContactRequired contact)
        {
            bool response;
            switch (contact.State)
            {
                case ObjectStatus.ObjectState.Update:
                    response = this.serviceProviderRepo.UpdateContact(this.ConvertServiceProviderContactToContact(contact));
                    break;
                case ObjectStatus.ObjectState.Delete:
                    response = this.serviceProviderRepo.DeleteContact(this.ConvertServiceProviderContactToContact(contact));
                    break;
                case ObjectStatus.ObjectState.Read:
                    response = true;
                    break;
                default:
                    response = false;
                    break;
            }

            return response;
        }

        /// <summary>
        /// The convert service provider contact to a view model contact.
        /// </summary>
        /// <param name="providerContact"> The provider contact. </param>
        /// <returns> The <see cref="Contact"/>. </returns>
        private Contact ConvertServiceProviderContactToContact(ServiceProviderContact providerContact)
        {
            return new Contact
            {
                ID = providerContact.Id,
                Email = providerContact.Email,
                Phone = providerContact.PhoneNumber,
                HelpLine = providerContact.HelpLine,
                Website = providerContact.Website
            };
        }


        /// <summary>
        /// The convert service provider contact to view model contact with required phone number.
        /// </summary>
        /// <param name="providerContact"> The provider contact. </param>
        /// <returns> The <see cref="Contact"/>. </returns>
        private Contact ConvertServiceProviderContactToContact(ServiceProviderContactRequired providerContact)
        {
            return new Contact
            {
                ID = providerContact.Id, 
                Email = providerContact.Email,
                Phone = providerContact.PhoneNumber,
                HelpLine = providerContact.HelpLine,
                Website = providerContact.Website
            };
        }


        /// <summary>
        /// The update areas.
        /// </summary>
        /// <param name="areas">
        /// The areas.
        /// </param>
        /// <param name="serviceProviderId">
        /// The service provider id.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        private bool UpdateAreas(WebServiceAreas areas, int serviceProviderId)
        {
            bool serviceAreaResponse;
            switch (areas.State)
            {
                case ObjectStatus.ObjectState.Update:
                case ObjectStatus.ObjectState.Delete:
                case ObjectStatus.ObjectState.Create:
                    serviceAreaResponse = this.serviceProviderRepo.UpdateServiceArea(
                        serviceProviderId,
                        areas.ServiceAreas);
                    break;
                case ObjectStatus.ObjectState.Read:
                    serviceAreaResponse = true;
                    break;
                default:
                    serviceAreaResponse = false;
                    break;
            }

            return serviceAreaResponse;
        }

        /// <summary>
        /// The create service areas.
        /// </summary>
        /// <param name="services">
        /// The services.
        /// </param>
        /// <param name="databaseProviderId">
        /// The database provider id.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        private bool CreateServiceAreas(List<int> services, int databaseProviderId)
        {
            foreach (var service in services)
            {
                var providerServiceArea = new ProviderService { ProviderID = databaseProviderId, ServiceID = service };
                var success = this.serviceProviderRepo.CreateServiceArea(providerServiceArea);
                if (!success)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// The create location.
        /// </summary>
        /// <param name="location">
        /// The location.
        /// </param>
        /// <param name="providerId">
        /// The provider id.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        private bool CreateLocations(List<ServiceProviderLocation> location, int providerId)
        {
            bool success = true;
            foreach (var serviceProviderLocation in location)
            {
                success = success && this.CreateLocation(serviceProviderLocation, providerId);
            }

            return success;
        }

        /// <summary>
        /// The create location.
        /// </summary>
        /// <param name="location">
        /// The location.
        /// </param>
        /// <param name="providerId">
        /// The provider id.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        private bool CreateLocation(ServiceProviderLocation location, int providerId)
        {
            // Create Contact for Location.
            var locationContactId = this.CreateContactId(this.ConvertServiceProviderContactToContact(location.Contact));
            if (locationContactId == 0)
            {
                return false;
            }

            // Convert to database Location
            var databaseLocation = this.ConvertLocation(location, providerId, locationContactId);

            // Create contact person and their contact information if not null
            if (location.ContactPerson != null)
            {
                var contactPersonContactId = this.CreateContactId(this.ConvertServiceProviderContactToContact(location.ContactPerson.Contact));
                var contactPersonId = this.CreateContactPerson(location.ContactPerson, contactPersonContactId);

                databaseLocation.ContactPersonID = contactPersonId;
            }

            var locationId = this.CreateDatabaseLocation(databaseLocation);

            foreach (var area in location.Coverage)
            {
                // Create a database coverage area
                var success = this.CreateServiceArea(area.CountyId, locationId);
                if (!success)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// The create database location.
        /// </summary>
        /// <param name="databaseLocation">
        /// The database location.
        /// </param>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        private int CreateDatabaseLocation(Location databaseLocation)
        {
            var id = this.serviceProviderRepo.CreateLocation(databaseLocation);

            return id;
        }

        /// <summary>
        /// The create database provider.
        /// </summary>
        /// <param name="provider">
        /// The provider.
        /// </param>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        private int CreateDatabaseProvider(WebsiteServiceProvider provider)
        {
            var databaseProvider = new ServiceProvider
            {
                Description     = provider.Description,
                DisplayRank     = provider.DisplayRank,
                ProviderName    = provider.Name,
                ServiceTypes    = provider.Type,
                Active          = provider.IsActive
            };

            var providerId = this.serviceProviderRepo.CreateServiceProvider(databaseProvider);

            return providerId;
        }

        /// <summary>
        /// The create contact id.
        /// </summary>
        /// <param name="contact">
        /// The contact.
        /// </param>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        private int CreateContactId(Contact serviceContact)
        {

            var id = this.serviceProviderRepo.CreateContact(serviceContact);

            return id;
        }

        /// <summary>
        /// The create contact person.
        /// </summary>
        /// <param name="contactPerson">
        /// The contact person.
        /// </param>
        /// <param name="contactPersonContactId">
        /// The contact person contact id.
        /// </param>
        /// <returns>
        /// The <see cref="ContactPerson"/>.
        /// </returns>
        private int CreateContactPerson(ServiceProviderContactPerson contactPerson, int contactPersonContactId)
        {
            var databaseContact = new ContactPerson
                                      {
                                          ContactID = contactPersonContactId,
                                          FirstName = contactPerson.FistName,
                                          JobTitle = contactPerson.JobTitle,
                                          LastName = contactPerson.LastName
                                      };

            var id = this.serviceProviderRepo.CreateContactPerson(databaseContact);

            return id;

        }

        /// <summary>
        /// The convert service area.
        /// </summary>
        /// <param name="areaId">
        /// The area id.
        /// </param>
        /// <param name="locationId">
        /// The location id.
        /// </param>
        /// <returns>
        /// The <see cref="ProviderCoverage"/>.
        /// </returns>
        private bool CreateServiceArea(int areaId, int locationId)
        {
            var coverage = new ProviderCoverage { LocationID = locationId, AreaID = areaId };

            var success = this.serviceProviderRepo.CreateCoverage(coverage);

            return success;
        }

        /// <summary>
        /// The convert location.
        /// </summary>
        /// <param name="location">
        /// The location.
        /// </param>
        /// <param name="providerId">
        /// The provider id.
        /// </param>
        /// <param name="locationContactId">
        /// The location contact id.
        /// </param>
        /// <returns>
        /// The <see cref="Location"/>.
        /// </returns>
        private Location ConvertLocation(ServiceProviderLocation location, int providerId, int locationContactId)
        {
            var newLocation = new Location
            {
                Name = location.Name,
                City = location.City,
                CountryID = location.CountryId,
                StateID = location.StateId,
                Street = location.Street,
                Zip = location.Zip,
                ProviderID = providerId,
                Display = location.Display,
                ContactID = locationContactId
            };

            return newLocation;
        }
    }
}