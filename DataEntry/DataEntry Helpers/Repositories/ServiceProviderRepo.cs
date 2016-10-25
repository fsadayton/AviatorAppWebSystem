// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ServiceProviderRepo.cs" company="UDRI">
//   Copyright © 2015 University of Dayton Research Institute. All rights reserved.
// </copyright>
// <summary>
//   The data access.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Data.Entity.Validation;

namespace DataEntry_Helpers.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;

    using Models;
    using Models.ServiceProvider;

    /// <summary>
    /// The data access.
    /// </summary>
    public class ServiceProviderRepo : Repository, IServiceProviderRepo
    {   

        /// <summary>
        /// The get all the active service providers.
        /// </summary>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<ServiceProvider> GetAllActiveServiceProviders()
        {
            try
            {
                var provider = this.db.ServiceProviders.Where(p => p.Active).OrderBy(p => p.ProviderName).ToList();

                return provider;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Get the count of all service providers by name.
        /// </summary>
        /// <param name="providerName">
        /// The provider Name.
        /// </param>
        /// <returns>
        /// </returns>
        public int GetAllServiceProvidersByNameCount(string providerName, int? countyId, int? categoryId)
        {
            try
            {
                var count = this.db.ServiceProviders
                    .Where(p =>
                            providerName == null ||
                            p.ProviderName.ToLower().Contains(providerName.ToLower()))
                    .Where(sp => countyId == null || sp.Locations.Any(l => l.ProviderCoverages.Any(pc => pc.AreaID == countyId)))
                    .Count(sp => categoryId == null || sp.ProviderServices.Any(ps => ps.ServiceID == categoryId));
                return count;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        /// <summary>
        /// The get all service providers by name.
        /// </summary>
        /// <param name="providerName">
        /// The provider name.
        /// </param>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<ServiceProvider> GetAllServiceProvidersByName(string providerName, int pageSize, int pageNumber, int? countyId, int? categoryId)
        {
            try
            {
                var provider =
                    this.db.ServiceProviders
                    .Where(p => providerName == null || p.ProviderName.ToLower().Contains(providerName.ToLower()))
                    .Where(sp => countyId == null || sp.Locations.Any(l => l.ProviderCoverages.Any(pc => pc.AreaID == countyId)))
                    .Where(sp => categoryId == null || sp.ProviderServices.Any(ps => ps.ServiceID == categoryId))
                    .OrderBy(p => p.ProviderName)
                    .Skip(pageSize * pageNumber)
                    .Take(pageSize)
                    .Include(p => p.Locations.Select(l => l.ProviderCoverages.Select(pc => pc.County)))
                    .Include(p => p.ProviderServices.Select(ps => ps.ProviderServiceCategory))
                    .ToList();
                return provider;
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        /// <summary>
        /// The get service provide.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="ServiceProvider"/>.
        /// </returns>
        public ServiceProvider GetServiceProvider(int id)
        {
            try
            {
                var provider = this.db.ServiceProviders.Single(p => p.ID == id);

                return provider;
            }
            catch (Exception)
            {
                return null;
            }
        }


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
        public List<ServiceProvider> GetServiceProviders(List<int> counties, List<int> categories)
        {
            var found = this.db.ServiceProviders
                .Where(sp => sp.Active && sp.ServiceTypes == 1)
                .Where(sp => sp.Locations.Any(l => l.ProviderCoverages.Any(pc => counties.Contains(pc.AreaID))))
                .Where(sp => sp.ProviderServices.Any(ps => categories.Contains(ps.ServiceID)))
                .OrderBy(sp => sp.DisplayRank).ThenBy(sp => sp.ProviderName)
                .ToList(); 
            
            return found;
        }

        /// <summary>
        /// The create service provider.
        /// </summary>
        /// <param name="serviceProvider">
        /// The service provider.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public int CreateServiceProvider(ServiceProvider serviceProvider)
        {
            try
            {
                var databaseProvider = this.db.ServiceProviders.Add(serviceProvider);
                this.db.SaveChanges();

                return databaseProvider.ID;
            }
            catch (Exception e)
            {
                return 0;
            }
        }

        /// <summary>
        /// The create contact.
        /// </summary>
        /// <param name="contact">
        /// The contact.
        /// </param>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        public int CreateContact(Contact contact)
        {
            try
            {
                var databaseContact = this.db.Contacts.Add(contact);
                this.db.SaveChanges();
                

                return databaseContact.ID;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        /// <summary>
        /// The create location.
        /// </summary>
        /// <param name="location">
        /// The location.
        /// </param>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        public int CreateLocation(Location location)
        {
            try
            {
                var databaseLocation = this.db.Locations.Add(location);
                this.db.SaveChanges();

                return databaseLocation.ID;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        /// <summary>
        /// The create contact person.
        /// </summary>
        /// <param name="person">
        /// The person.
        /// </param>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        public int CreateContactPerson(ContactPerson person)
        {
            try
            {
                var contactPerson = this.db.ContactPersons.Add(person);
                this.db.SaveChanges();

                return contactPerson.ID;
            }
            catch (DbEntityValidationException dbEx)
            {
                return 0;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        /// <summary>
        /// The create coverage.
        /// </summary>
        /// <param name="coverage">
        /// The coverage.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool CreateCoverage(ProviderCoverage coverage)
        {
            try
            {
                this.db.ProviderCoverages.Add(coverage);
                this.db.SaveChanges();

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        /// <summary>
        /// The create service area.
        /// </summary>
        /// <param name="service">
        /// The service.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool CreateServiceArea(ProviderService service)
        {
            try
            {
                this.db.ProviderServices.Add(service);
                this.db.SaveChanges();

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        /// <summary>
        /// The update service categories.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <param name="newList">
        /// The new list.
        /// </param>
        public bool UpdateServiceArea(int id, List<int> newList)
        {
            try
            {
                var currentCategories = this.db.ProviderServices.Where(c => c.ProviderID == id).ToList();
                var oldList = currentCategories.Select(currentCategory => currentCategory.ServiceID).ToList();

                var fistCheck = newList.Except(oldList);

                foreach (var i in fistCheck)
                {
                    var serviceId = i;
                    var providerService = new ProviderService { ProviderID = id, ServiceID = serviceId };
                    this.db.ProviderServices.Add(providerService);
                    this.db.SaveChanges();
                }

                var secondCheck = oldList.Except(newList);

                foreach (var i in secondCheck)
                {
                    var serviceId = i;
                    var service = this.db.ProviderServices.Single(x => x.ProviderID == id && x.ServiceID == serviceId);
                    this.db.ProviderServices.Remove(service);
                    this.db.SaveChanges();
                }
            }
            catch (Exception e)
            {
                return false;
            }

            return true;
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
        public bool UpdateProvider(WebsiteServiceProvider serviceProvider)
        {
            try
            {
                var databaseServiceProvider = this.db.ServiceProviders.Single(d => d.ID == serviceProvider.Id);

                databaseServiceProvider.Description     = serviceProvider.Description;
                databaseServiceProvider.ProviderName    = serviceProvider.Name;
                databaseServiceProvider.DisplayRank     = serviceProvider.DisplayRank;
                databaseServiceProvider.Active          = serviceProvider.IsActive;
                databaseServiceProvider.ServiceTypes    = serviceProvider.Type;

                this.db.Entry(databaseServiceProvider).State = EntityState.Modified;
                this.db.SaveChanges();

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
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
        public bool DeleteProvider(WebsiteServiceProvider serviceProvider)
        {
            try
            {
                var provider = this.db.ServiceProviders.Single(s => s.ID == serviceProvider.Id);

                foreach (var editRecord in provider.ServiceProviderEdits)
                {
                    editRecord.DeletedProviderName = provider.ProviderName;
                    editRecord.ProviderId = null;
                }
                this.db.SaveChanges();

                this.db.ServiceProviders.Remove(provider);
                this.db.SaveChanges();

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        /// <summary>
        /// The delete provider service.
        /// </summary>
        /// <param name="providerServiceId">
        /// The provider service id.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool DeleteProviderService(int providerServiceId)
        {
            try
            {
                var providerService = this.db.ProviderServices.Single(p => p.ID == providerServiceId);
                this.db.ProviderServices.Remove(providerService);
                this.db.SaveChanges();

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }


        /// <summary>
        /// The update location.
        /// </summary>
        /// <param name="serviceLocation">
        /// The service location.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool UpdateLocation(ServiceProviderLocation serviceLocation)
        {
            try
            {
                var location = this.db.Locations.Single(l => l.ID == serviceLocation.Id);

                location.Street = serviceLocation.Street;
                location.City = serviceLocation.City;
                location.StateID = serviceLocation.StateId;
                location.Zip = serviceLocation.Zip;
                location.CountryID = serviceLocation.CountryId;
                location.Display = serviceLocation.Display;
                location.Name = serviceLocation.Name;

                if (serviceLocation.ContactPerson != null)
                {
                    location.ContactPersonID = serviceLocation.ContactPerson.Id;
                }
                else
                {
                    location.ContactPersonID = null;
                }


                this.db.Entry(location).State = EntityState.Modified;
                this.db.SaveChanges();

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        /// <summary>
        /// The delete location.
        /// </summary>
        /// <param name="locationId">
        /// The location id.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool DeleteLocation(int locationId)
        {
            try
            {
                var location = this.db.Locations.Single(s => s.ID == locationId);
                this.db.Locations.Remove(location);
                this.db.SaveChanges();

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }


        /// <summary>
        /// The delete coverages.
        /// </summary>
        /// <param name="locationId">
        /// The location id.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool DeleteAllCoveragesForLocation(int locationId)
        {
            try
            {
                var providerCoverages = this.db.ProviderCoverages.Where(p => p.LocationID == locationId).ToList();
                foreach (var providerCoverage in providerCoverages)
                {
                    this.db.ProviderCoverages.Remove(providerCoverage);
                    this.db.SaveChanges();
                }
            }
            catch (Exception e)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// The delete coverage.
        /// </summary>
        /// <param name="coverageId">
        /// The coverage id.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool DeleteCoverage(int coverageId)
        {
            try
            {
                var providerCoverages = this.db.ProviderCoverages.Where(p => p.ID == coverageId).ToList();
                foreach (var providerCoverage in providerCoverages)
                {
                    this.db.ProviderCoverages.Remove(providerCoverage);
                    this.db.SaveChanges();
                }
            }
            catch (Exception e)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// The update coverage.
        /// </summary>
        /// <param name="id">
        ///     The id.
        /// </param>
        /// <param name="newList">
        ///     The new list.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool UpdateCoverage(int id, Coverage providerCoverage)
        {
            try
            {
                var currentCoverage =
                    this.db.ProviderCoverages.Single(c => c.LocationID == id & c.AreaID == providerCoverage.CountyId);

                currentCoverage.AreaID = providerCoverage.CountyId;

                this.db.Entry(currentCoverage).State = EntityState.Modified;
                this.db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        /// <summary>
        /// The update contact person.
        /// </summary>
        /// <param name="serviceProviderContactPerson">
        /// The service provider contact person.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool UpdateContactPerson(ServiceProviderContactPerson serviceProviderContactPerson)
        {
            try
            {
                var updatedContactPerson = this.db.ContactPersons.Single(u => u.ID == serviceProviderContactPerson.Id);

                updatedContactPerson.FirstName = serviceProviderContactPerson.FistName;
                updatedContactPerson.LastName = serviceProviderContactPerson.LastName;
                updatedContactPerson.JobTitle = serviceProviderContactPerson.JobTitle;

                this.db.Entry(updatedContactPerson).State = EntityState.Modified;
                this.db.SaveChanges();

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        /// <summary>
        /// The delete contact person.
        /// </summary>
        /// <param name="contactPerson">
        /// The contact person.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool DeleteContactPerson(ServiceProviderContactPerson contactPerson)
        {
            try
            {
                var person = this.db.ContactPersons.Single(s => s.ID == contactPerson.Id);

                // Remove the foreign key.
                foreach (var location in person.Locations)
                {
                    location.ContactPersonID = null;
                }
                
                db.SaveChanges();

                this.db.ContactPersons.Remove(person);
                this.db.SaveChanges();

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }


        /// <summary>
        /// Adds an entry into the log table for service provider changes.
        /// </summary>
        /// <param name="entryToSave"> The entry to save. </param>
        /// <returns> The <see cref="bool"/>. </returns>
        public bool LogServiceProviderEdit(ServiceProviderEdit entryToSave)
        {
            try
            {
                this.db.ServiceProviderEdits.Add(entryToSave);
                this.db.SaveChanges();

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        /// <summary>
        /// Gets the entire service provider edit log
        /// </summary>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<ServiceProviderEdit> GetServiceProviderEditLog()
        {
            try
            {
                return this.db.ServiceProviderEdits.ToList();
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// The update contact.
        /// </summary>
        /// <param name="contact">
        /// The contact.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool UpdateContact(Contact contact)
        {
            try
            {
                var updatedContact = this.db.Contacts.Single(u => u.ID == contact.ID);

                updatedContact.Email = contact.Email;
                updatedContact.Phone = contact.Phone;
                updatedContact.HelpLine = contact.HelpLine;
                updatedContact.Website = contact.Website;
                this.db.Entry(updatedContact).State = EntityState.Modified;
                this.db.SaveChanges();

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        /// <summary>
        /// The delete contact.
        /// </summary>
        /// <param name="contact">
        /// The contact.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool DeleteContact(Contact contact)
        {
            try
            {
                var contacts = this.db.Contacts.Single(s => s.ID == contact.ID);

                foreach (var contactPerson in contacts.ContactPersons)
                {
                    contactPerson.ContactID = null;
                }

                this.db.Contacts.Remove(contacts);
                this.db.SaveChanges();

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        
    }
}
