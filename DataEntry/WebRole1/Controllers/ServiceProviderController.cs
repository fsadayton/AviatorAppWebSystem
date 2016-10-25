// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ServiceProviderController.cs" company="UDRI">
//   Copyright © 2015 University of Dayton Research Institute. All rights reserved.
// </copyright>
// <summary>
//   Website Service Provider Controller
// </summary>
// --------------------------------------------------------------------------------------------------------------------


using System.Collections;
using DataEntry_Helpers.Repositories;
using Microsoft.Ajax.Utilities;

namespace Website.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Web.Http.Cors;
    using System.Web.Mvc;
    using BL;
    using BL.ModelConversions;
    using DataEntry_Helpers;
    using Helpers;
    using global::Models;
    using global::Models.ServiceProvider;

    /// <summary>
    /// Controller for Service Providers
    /// </summary>
    [EnableCors("*", "*", "*")]
    public class ServiceProviderController : BaseController
    {
        /// <summary>
        /// Data logic object
        /// </summary>
        private readonly DataLogics dataLogics;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceProviderController"/> class.
        /// </summary>
        public ServiceProviderController()
        {
            this.dataLogics = new DataLogics();
        }
           
        /// <summary>
        /// GET: ServiceProvider
        /// </summary>
        /// <returns>Index view </returns>
        [AuthorizeRedirect(Roles = "1")]
        public ActionResult Index()
        {
            return this.View();
        }


        /// <summary>
        /// GET: ServiceProvider/Details/5
        /// </summary>
        /// <param name="id">ID of service provider to get</param>
        /// <returns>View of details</returns>
        public ActionResult Details(int id)
        {
            // Load non-model related data.
            this.SetupViewBag();
            var provider = this.dataLogics.GetServiceProviderById(id);
            return this.View(provider);
        }

        /// <summary>
        /// GET: ServiceProvider/Create 
        /// Create action of the service provider.   Creates a new blank provider with one location and hands it over to the view. 
        /// </summary>
        /// <returns>The action result</returns>
        [AuthorizeRedirect(Roles = "1")]
        public ActionResult Create()
        {
            // Load non-model related data.
            this.SetupViewBag();

            // Create a location list with one blank location.
            var locationList = new List<ServiceProviderLocation> { this.BuildBlankLocation() };

            // Create a blank provider with any defaults.
            var provider = new WebsiteServiceProvider
            {
                Locations = locationList,
                Services = new WebServiceAreas { ServiceAreas = new List<int>() },
                Type = 1,
                DisplayRank = 3,
            };

            return this.View(provider);
        }
        
        /// <summary>
        /// Accepts the post for creating a new service provider. 
        /// POST: ServiceProvider/Create
        /// </summary>
        /// <param name="provider">Service Provider to create</param>
        /// <returns>The redirect to index</returns>
        [HttpPost]
        [AuthorizeRedirect(Roles = "1")]
        public ActionResult Create(WebsiteServiceProvider provider)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    this.ResetForInvalidModel(provider);

                    this.SetErrorMessage();

                    return this.View(provider);
                }
                
                // In each location, don't create a location contact if no information is given.
                foreach (var curLocation in provider.Locations)
                {
                    curLocation.StateId = int.Parse(curLocation.StateIdString);
                    if (this.IsContactPersonEmpty(curLocation.ContactPerson))
                    {
                        curLocation.ContactPerson = null;
                    }
                }

                var webToDb = new WebToDatabaseServiceProvider();
                provider.State = ObjectStatus.ObjectState.Create;

                // Need an empty service areas object if no service areas were included.
                if (provider.Services == null)
                {
                    provider.Services = new WebServiceAreas();
                }

                webToDb.UpdateServiceProvider(provider, this.UserId);
                this.TempData["Info"] = string.Format("The provider {0} was created succesfully.", provider.Name);
                return this.RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                this.SetupViewBag();
                this.SetupLocationStateDropdown(provider);
                foreach (var curLocation in provider.Locations.Where(curLocation => curLocation.Coverage == null))
                {
                    curLocation.Coverage = new List<Coverage>();
                }

                this.TempData["Error"] = "An error occured while saving the service provider.";
                return this.View(provider);
            }
        }


        /// <summary>
        /// GET: ServiceProvider/Edit/5
        /// </summary>
        /// <param name="id">ID to edit</param>
        /// <returns>Form for editing</returns>
        [AuthorizeProviderRedirect(Roles = "1,2")]
        public ActionResult Edit(int id)
        {
            // Load non-model related data.
            this.SetupViewBag();
            var provider = this.dataLogics.GetServiceProviderById(id);
            this.SetupLocationStateDropdown(provider);
            return this.View(provider);
        }

        /// <summary>
        /// POST: ServiceProvider/Edit/5
        /// </summary>
        /// <param name="provider"> The provider.</param>
        /// <returns> Index if saved correctly </returns>
        [HttpPost]
        [AuthorizeProviderRedirect(Roles = "1,2")]
        public ActionResult Edit(WebsiteServiceProvider provider)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    this.ResetForInvalidModel(provider);
                    this.SetErrorMessage();
                    return this.View(provider);
                }

                var original = this.dataLogics.GetServiceProviderById(provider.Id);

                if (provider.Services == null)
                {
                    provider.Services = new WebServiceAreas
                    {
                        State = ObjectStatus.ObjectState.Update,
                        ServiceAreas = new List<int>()
                    };
                }

                // Check against original locations to see which are new, and what has been removed.
                this.SetStateFlagsLocations(provider, original);

                provider.State = ObjectStatus.ObjectState.Update;

                var webToDb = new WebToDatabaseServiceProvider();
                webToDb.UpdateServiceProvider(provider, this.UserId);

                this.TempData["Info"] = string.Format("The provider {0} was updated succesfully.", provider.Name);

                if (User.IsInRole("1"))
                {
                    return this.RedirectToAction("Index");
                }

                return this.RedirectToAction("Details", new { id = provider.Id});
            }
            catch (Exception ex)
            {
                return this.View();
            }
        }

        /// <summary>
        /// GET: ServiceProvider/Delete/5
        /// </summary>
        /// <param name="id">ID to delete</param>
        /// <returns>View of Delete</returns>
        [AuthorizeRedirect(Roles = "1")]
        public ActionResult Delete(int id)
        {
            var provider = this.dataLogics.GetServiceProviderById(id);
            provider.State = ObjectStatus.ObjectState.Delete;
            var webToDb = new WebToDatabaseServiceProvider();
            if (!webToDb.UpdateServiceProvider(provider, this.UserId))
            {
                this.TempData["Error"] = "An error occured while deleting the service provider.";
            }
            else
            {
                this.TempData["Info"] = "The provider was deleted succesfully.";
            }
            return this.RedirectToAction("Index");
        }

        /// <summary>
        /// Method to create a new blank location and show it in the view.
        /// </summary>
        /// <returns>A partial view of a blank location row</returns>
        public ViewResult AddBlankLocation()
        {
            this.BuildCountryList();
            return this.View("LocationRow", this.BuildBlankLocation());
        }

        /// <summary>
        /// Creates the coverage input fields for the given counties
        /// </summary>
        /// <param name="countyId">Comma delimited string of county ids</param>
        /// <param name="countyName">Name of county so lookup doesn't have to be done</param>
        /// <param name="stateName">Name of State</param>
        /// <param name="countryName">Name of Country</param>
        /// <returns>View of all the given counties in textboxes</returns>
        public ViewResult AddCoverage(int countyId, string countyName, string stateName, string countryName)
        {
            var coverage = new Coverage
            {
                CountyId = countyId,
                CountyName = countyName,
                StateName = stateName,
                CountryName = countryName
            };

            return this.View("CoverageRow", coverage);
        }
      
        /// <summary>
        /// Gets all the states for the given country
        /// </summary>
        /// <param name="countryId">ID of Country</param>
        /// <returns>JSON of the states retrieved</returns>
        public ActionResult GetStatesByCountryID(int countryId)
        {
            var stateList = this.dataLogics.GetStates(countryId);
            var result = (from s in stateList
                          select new
                          {
                              id = s.Id,
                              name = s.Name
                          }).ToList();
            return this.Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Gets the counties for a state
        /// </summary>
        /// <param name="stateId">State ID</param>
        /// <returns>JSON data of all the counties</returns>
        public ActionResult GetCountiesByStateId(int stateId)
        {
            var countyList = this.dataLogics.GetCounties(stateId);
            var result = (from s in countyList
                          select new
                          {
                              id = s.Id,
                              name = s.Name
                          }).ToList();
           
            return this.Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SearchCount(string searchText, int? countyId, int? categoryId)
        {
            var count = this.dataLogics.GetServiceProvidersCount(searchText, countyId, categoryId);
            return this.Json(count, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Searches the providers and shows the result.
        /// </summary>
        /// <param name="searchText">Name search</param>
        /// <returns>View Result</returns>
        [HttpGet]
        public ViewResult Search(string searchText, int page, int? countyId, int? categoryId)
        {
            var foundProviders = this.dataLogics.GetServiceProviders(searchText, 20, page, countyId, categoryId);
            return this.View("ServiceProviderList", foundProviders);
        }



        /// <summary>
        /// The get all providers JSON data.
        /// </summary>
        /// <returns> The <see cref="ActionResult"/>. </returns>
        public ActionResult GetAllProviders()
        {
            var foundProviders = this.dataLogics.GetServiceProviderNamesAndDescription();
            return this.Json(foundProviders, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// Gets the country list from the database and clears out the state.
        /// </summary>
        private void BuildCountryList()
        {
            var countryList = this.dataLogics.GetCountries();

            if (countryList == null)
            {
                return;
            }

            // Default to US.
            ViewBag.CountryId = new SelectList(countryList, "ID", "FullName", null);
            var usa = countryList.Find(country => country.FullName == "United States");
            ViewBag.StateId = this.dataLogics.GetStates(usa.Id).Select(s =>
                                  new SelectListItem()
                                  {
                                      Value = s.Id.ToString(CultureInfo.InvariantCulture),
                                      Text = s.Name
                                  }); 

            ViewBag.CountyId = new SelectList(new List<County>(), "ID", "Name", null);
        }

        /// <summary>
        /// The reset for invalid model.
        /// </summary>
        /// <param name="provider"> The provider. </param>
        private void ResetForInvalidModel(WebsiteServiceProvider provider)
        {
            // Get everything set back up before sending the user back.
            this.SetupViewBag();
            if (provider.Services == null)
            {
                provider.Services = new WebServiceAreas
                {
                    ServiceAreas = new List<int>()
                };
            }

            // Reset the state lists.
            var countryList = this.dataLogics.GetCountries();
            var usa = countryList.Find(country => country.FullName == "United States");
            var states = this.dataLogics.GetStates(usa.Id);
            foreach (var location in provider.Locations)
            {
                location.StateId = int.Parse(location.StateIdString);
                location.States = new SelectList(states, "Id", "Name");

                if (location.Coverage == null)
                {
                    location.Coverage = new List<Coverage>();
                }
            }
        }


        /// <summary>
        /// Creates a blank location with properties set
        /// </summary>
        /// <returns>Blank location</returns>
        private ServiceProviderLocation BuildBlankLocation()
        {
            var coverageArea = new List<Coverage>();

            // Default to US
            var countryList = this.dataLogics.GetCountries();
            var usa = countryList.Find(country => country.FullName == "United States");
            var states = this.dataLogics.GetStates(usa.Id);
            
            // Set up the blank location defaulted to OH
            return new ServiceProviderLocation
            {
                Contact = new ServiceProviderContactRequired(),
                ContactPerson = new ServiceProviderContactPerson { Contact = new ServiceProviderContact() },
                Coverage = coverageArea,
                CountryId = usa.Id,
                Display = true,
                StateIdString = "34",
                States = new SelectList(states, "Id", "Name")
            };
        }

        /// <summary>
        /// Gets all the data needed for the Create view and puts it in the ViewBag
        /// </summary>
        private void SetupViewBag()
        {
            var serviceInfo = new ServiceTypesLogics();
            ViewBag.types = serviceInfo.GetServiceTypes().ToList().Select(x => new SelectListItem() 
                                  {
                                      Value = x.Key.ToString(),
                                      Text = x.Value
                                  });
;
            var categories = this.dataLogics.GetWebsiteCategories();
            categories.Sort((category1, category2) => category1.Name.CompareTo(category2.Name));
            ViewBag.AllCategories = categories;

            this.BuildCountryList();
        }


        /// <summary>
        /// Sets the state flags and puts in deleted items for the locations in a provider.
        /// </summary>
        /// <param name="provider">The updated provider</param>
        /// <param name="original">The original provider to compare against</param>
        private void SetStateFlagsLocations(WebsiteServiceProvider provider, WebsiteServiceProvider original)
        {
            // Go through each location.  Add removed items back in with deleted flag.
            foreach (var location in provider.Locations)
            {
                location.StateId = int.Parse(location.StateIdString);
                location.State = location.Id == 0 ? ObjectStatus.ObjectState.Create : ObjectStatus.ObjectState.Update;

                // Get the original location and set the current location's new/update/delete flags
                var location1 = location;
                var originalLocationFound = original.Locations.Where(x => x.Id == location1.Id).ToList();
                if (originalLocationFound.Count > 0)
                {
                    // Set the flags for the contact person
                    var originalLocation = originalLocationFound.First();
                    if (originalLocation.ContactPerson != null)
                    {
                        location.ContactPerson.State = ObjectStatus.ObjectState.Update;
                        location.ContactPerson.Contact.State = ObjectStatus.ObjectState.Update;
                    }

                    // If the contact person is blank, check if the original was blank.  If it wasn't delete the contact.  
                    // Otherwise keep the contact person as null.
                    if (this.IsContactPersonEmpty(location.ContactPerson))
                    {
                        if (originalLocation.ContactPerson != null)
                        {
                            location.ContactPerson.State = ObjectStatus.ObjectState.Delete;
                        }
                        else
                        {
                            location.ContactPerson = null;
                        }
                    }
                    else
                    {
                        // If the original is null but now it has a contact person, create one.
                        if (originalLocation.ContactPerson == null)
                        {
                            location.ContactPerson.State = ObjectStatus.ObjectState.Create;
                            location.ContactPerson.Contact.State = ObjectStatus.ObjectState.Create;
                        }
                    }


                    location.Contact.State = ObjectStatus.ObjectState.Update;

                    this.SetStateFlagsCoverages(location, originalLocation);
                }
                else
                {
                    if (this.IsContactPersonEmpty(location.ContactPerson))
                    {
                        location.ContactPerson = null;
                    }
                }
            }

            // Get the deleted items and set them to be deleted
            var deletedItems = new List<ServiceProviderLocation>();
            var deletedList = from curLocation
                in original.Locations
                              let found = provider.Locations.Find(x => x.Id == curLocation.Id)
                              let hasBeenDeleted = found == null
                              where hasBeenDeleted
                              select curLocation;

            foreach (var curLocation in deletedList)
            {
                curLocation.State = ObjectStatus.ObjectState.Delete;
                deletedItems.Add(curLocation);
            }

            if (deletedItems.Count > 0)
            {
                provider.Locations.AddRange(deletedItems);
            }
        }


        /// <summary>
        /// The set state flags for coverage in a location.
        /// </summary>
        /// <param name="location"> The updated location. </param>
        /// <param name="original"> The original location. </param>
        private void SetStateFlagsCoverages(ServiceProviderLocation location, ServiceProviderLocation original)
        {
            // Set all the existing coverages to be updated.
            foreach (var curCoverage in location.Coverage) 
            {
                curCoverage.State = curCoverage.Id == 0 ? ObjectStatus.ObjectState.Create : ObjectStatus.ObjectState.Update;
            }

            // Get the deleted items and set them to be deleted
            var deletedItems = new List<Coverage>();
            var deletedList = from coverage
                               in original.Coverage
                              let found = location.Coverage.Find(x => x.Id == coverage.Id)
                              let hasBeenDeleted = found == null
                              where hasBeenDeleted
                              select coverage;

            foreach (var curLocation in deletedList)
            {
                curLocation.State = ObjectStatus.ObjectState.Delete;
                deletedItems.Add(curLocation);
            }

            if (deletedItems.Count > 0)
            {
                location.Coverage.AddRange(deletedItems);
            }
        }

        /// <summary>
        /// Checks if the contact person empty.
        /// </summary>
        /// <param name="contact"> The contact. </param>
        /// <returns> The <see cref="bool"/>. </returns>
        private bool IsContactPersonEmpty(ServiceProviderContactPerson contact)
        {
            return string.IsNullOrEmpty(contact.FistName)
                    && string.IsNullOrEmpty(contact.LastName);
        }


        /// <summary>
        /// Setup the selected value for each location's selected state.  Used when editing a provider form for an 
        /// existing provider. Also used for repopulating a new provider's values with an error message.
        /// </summary>
        /// <param name="provider"> The provider. </param>
        private void SetupLocationStateDropdown(WebsiteServiceProvider provider)
        {
            // Set up the dropdowns.
            var countryList = this.dataLogics.GetCountries();
            var usa = countryList.Find(country => country.FullName == "United States");
            var states = this.dataLogics.GetStates(usa.Id);

            // Set up defaults for locations.
            foreach (var location in provider.Locations)
            {
                if (location.ContactPerson == null)
                {
                    location.ContactPerson = new ServiceProviderContactPerson {Contact = new ServiceProviderContact()};
                }

                location.StateIdString = location.StateId.ToString(CultureInfo.InvariantCulture);
                location.States = new SelectList(states, "Id", "Name");
            }
        }


        /// <summary>
        /// The set the error message from the model state errors.
        /// </summary>
        private void SetErrorMessage()
        {
            foreach (var error in ModelState.Values.SelectMany(curState => curState.Errors))
            {
                this.TempData["Error"] = this.TempData["Error"] + error.ErrorMessage + "<br>";
            }
        }
    }
}
