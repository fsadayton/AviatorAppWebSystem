// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ServiceProviderControllerTest.cs" company="UDRI">
//   Copyright © 2015 University of Dayton Research Institute. All rights reserved.
// </copyright>
// <summary>
//   The Service Provider Controller tests.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using Website.Models;

namespace DataEntry.Tests.Controllers
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using System.Web.Script.Serialization;
    using Microsoft.QualityTools.Testing.Fakes;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Models;
    using Models.ServiceProvider;
    using Website.BL.Fakes;
    using Website.BL.ModelConversions.Fakes;
    using Website.Controllers;

    /// <summary>
    /// Service Provider Controller Test
    /// </summary>
    [TestClass]
    public class ServiceProviderControllerTest
    {
        /// <summary>
        /// The original location for testing
        /// </summary>
        private ServiceProviderLocation originalLocation;

        /// <summary>
        /// The original provider for testing
        /// </summary>
        private WebsiteServiceProvider originalProvider;

        /// <summary>
        /// The controller.
        /// </summary>
        private ServiceProviderController controller;

        /// <summary>
        /// Gets or sets the test context which provides
        /// information about and functionality for the current test run.
        /// </summary>
        public TestContext TestContext { get; set; }
        
        /// <summary>
        /// The initialize for testing
        /// </summary>
        [TestInitialize]
        public void MyTestInitialize()
        {
             this.controller = new ServiceProviderController();

             this.originalLocation = new ServiceProviderLocation
             {
                 Id = 1,
                 StateIdString = "34",
                 Contact = new ServiceProviderContactRequired
                 {
                     Id = 1223
                 },
                 Coverage = new List<Coverage>
                    {
                        new Coverage {CountyId = 1, CountryId = 1, Id = 2, StateId = 1 }
                    }
             };

             this.originalProvider = new WebsiteServiceProvider
             {
                 Id = 4,
                 Name = "Test",
                 Type = 2,
                 Locations = new List<ServiceProviderLocation> { this.originalLocation }
             };
         }

        /// <summary>
        /// The get for create.
        /// </summary>
        [TestMethod]
        public void GetCreate()
        {
            using (ShimsContext.Create())
            {
                var serviceTypes = new Dictionary<int, string> { { 1, "General" } };

                ShimDataLogics.AllInstances.GetCountries =
                  access => new List<WebsiteCountry> { new WebsiteCountry { Abbreviation = "US", FullName = "United States", Id = 1 } };

                ShimDataLogics.AllInstances.GetStatesInt32 =
                    (access, i) => new List<WebsiteState> { new WebsiteState { Id = 1, Name = "Ohio" }, new WebsiteState { Id = 2, Name = "Minnesota" } };

                ShimDataLogics.AllInstances.GetCategories =
                    access =>
                        new List<Category>
                        {
                            new Category
                            {
                                Name = "Test",
                                Id = 1,
                                Description = "this is a test",
                                Crime = false
                            }
                        };

                ShimServiceTypesLogics.AllInstances.GetServiceTypes = access => serviceTypes;

                var result = this.controller.Create() as ViewResult;
                Assert.IsNotNull(result);

                var provider = result.Model as WebsiteServiceProvider;
                Assert.IsNotNull(provider);
                
                Assert.AreEqual(1, provider.Locations.Count);
                Assert.AreEqual(0, provider.Locations[0].Coverage.Count);

                Assert.AreEqual(3, provider.DisplayRank);
            }
        }

        /// <summary>
        /// Test for successful creation of Service Provider
        /// </summary>
        [TestMethod]
        public void PostCreateToIndex()
        {

            using (ShimsContext.Create())
            {
                ShimWebToDatabaseServiceProvider.AllInstances.UpdateServiceProviderWebsiteServiceProviderInt32 =
                    (access, provider, id) => true;

                this.originalProvider.Locations[0].ContactPerson = new ServiceProviderContactPerson
                {
                    Id = 0
                };

                var result = this.controller.Create(this.originalProvider);

                Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
                RedirectToRouteResult routeResult = result as RedirectToRouteResult;
                Assert.IsNotNull(routeResult);
                Assert.AreEqual(routeResult.RouteValues["action"], "Index");
            }
        }

        /// <summary>
        /// Test for if the create method returns false.
        /// </summary>
        [TestMethod]
        public void PostCreateFailure()
        {
            using (ShimsContext.Create())
            {
                ShimWebToDatabaseServiceProvider.AllInstances.UpdateServiceProviderWebsiteServiceProviderInt32 =
                    (access, provider, userId) => false;

                this.originalProvider.Locations[0].ContactPerson = new ServiceProviderContactPerson
                {
                    Id = 0
                };

                var result = this.controller.Create(this.originalProvider);

                Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
            }
        }

        /// <summary>
        /// The get states by country id test.
        /// </summary>
        [TestMethod]
        public void GetStatesByCountryIdTest()
        {
            using (ShimsContext.Create())
            {
                ShimDataLogics.AllInstances.GetStatesInt32 =
                    (access, i) => new List<WebsiteState> { new WebsiteState { Id = 2, Name = "Minnesota" }, new WebsiteState { Id = 1, Name = "Ohio" } };
                
                var result = this.controller.GetStatesByCountryID(1) as JsonResult;
                Assert.IsNotNull(result);

                var serializer = new JavaScriptSerializer();
                var output = serializer.Serialize(result.Data);

                Assert.AreEqual("[{\"id\":2,\"name\":\"Minnesota\"},{\"id\":1,\"name\":\"Ohio\"}]", output);
            }
        }

        /// <summary>
        /// The get counties by state id test.
        /// </summary>
        [TestMethod]
        public void GetCountiesByStateIdTest()
        {
            using (ShimsContext.Create())
            {
                ShimDataLogics.AllInstances.GetCountiesInt32 =
                    (access, i) => new List<WebsiteCounty> { new WebsiteCounty { Id = 1, Name = "County1" } };

                var result = this.controller.GetCountiesByStateId(1) as JsonResult;
                Assert.IsNotNull(result);

                var serializer = new JavaScriptSerializer();
                var output = serializer.Serialize(result.Data);
                Assert.AreEqual("[{\"id\":1,\"name\":\"County1\"}]", output);
            }
        }

        /// <summary>
        /// The add blank location test.
        /// </summary>
        [TestMethod]
        public void AddBlankLocationTest()
        {
            using (ShimsContext.Create())
            {
                ShimDataLogics.AllInstances.GetCountries =
                    access => new List<WebsiteCountry> { new WebsiteCountry { Abbreviation = "US", FullName = "United States"} };

                ShimDataLogics.AllInstances.GetStatesInt32 =
                    (access, i) =>
                        new List<WebsiteState>
                        {
                            new WebsiteState { Id = 1, Name = "Ohio" },
                            new WebsiteState { Id = 2, Name = "Minnesota" }
                        };

                var result = this.controller.AddBlankLocation() as ViewResult;
                Assert.IsNotNull(result);

                var location = result.Model as ServiceProviderLocation;
                Assert.IsNotNull(location);

                Assert.AreEqual(0, location.Coverage.Count);
            }
        }


        /// <summary>
        /// The add coverage test.
        /// </summary>
        [TestMethod]
        public void AddCoverageTest()
        {
            using (ShimsContext.Create())
            {
                var countyName = "County Name";
                var stateName = "State Name";
                var countryName = "Country Name";
               
                var result = this.controller.AddCoverage(1, countyName, stateName, countryName);
                Assert.IsInstanceOfType(result.Model, typeof(Coverage));

                Coverage modelData = result.Model as Coverage;
                Assert.IsNotNull(modelData);
                
                Assert.AreEqual(1, modelData.CountyId);
                Assert.AreEqual(countyName, modelData.CountyName);
                Assert.AreEqual(stateName, modelData.StateName);
                Assert.AreEqual(countryName, modelData.CountryName);
            }
        }

        /// <summary>
        /// Test editing provider and only changing the location data.
        /// </summary>
        [TestMethod]
        public void EditChangeLocationOnly()
        {
            using (ShimsContext.Create())
            {
                var updatedLocation = new ServiceProviderLocation
                {
                    Id = 1,
                    StateIdString = "34",
                    City = "City name",
                    Contact = new ServiceProviderContactRequired
                    {
                        Id = 1223,
                        Email = "asdfs@adsfaf.com",
                    },
                    ContactPerson = new ServiceProviderContactPerson
                    {
                        Id = 0
                    },
                    Coverage = new List<Coverage>
                    {
                        new Coverage {CountyId = 1, CountryId = 1, Id = 2, StateId = 1}
                    }
                };

                var updatedProvider = new WebsiteServiceProvider
                {
                    Id = 4,
                    Name = "Test2",
                    Type = 2,
                    Locations = new List<ServiceProviderLocation> { updatedLocation }
                };

                ShimDataLogics.AllInstances.GetServiceProviderByIdInt32 = (access, id) => this.originalProvider;
                ShimWebToDatabaseServiceProvider.AllInstances.UpdateServiceProviderWebsiteServiceProviderInt32 =
                    (provider, serviceProvider, userId) => true;
                var result = this.controller.Edit(updatedProvider);

                Assert.AreEqual(ObjectStatus.ObjectState.Update, updatedProvider.State);
            }
        }


        /// <summary>
        /// Test changing provider by adding a new contact
        /// </summary>
        [TestMethod]
        public void EditChangeLocationAddContactPersonTest()
        {
               using (ShimsContext.Create())
            {

                var updatedLocation = new ServiceProviderLocation
                {
                    Id = 1,
                    StateIdString = "34",
                    Coverage = new List<Coverage>
                    {
                        new Coverage {CountyId = 1, CountryId = 1, Id = 2, StateId = 1 } 
                    },
                    Contact = new ServiceProviderContactRequired
                    {
                        Id = 1223
                    },
                    ContactPerson = new ServiceProviderContactPerson
                    {
                        FistName = "FirstName",
                        LastName = "LastName",
                        Contact = new ServiceProviderContact { Id = 0, Email = "asdf@asdf.com", }
                    }
                };

                var updatedProvider = new WebsiteServiceProvider
                {
                    Id = 4,
                    Name = "Test2",
                    Type = 2,
                    Locations = new List<ServiceProviderLocation> { updatedLocation }
                };

                ShimDataLogics.AllInstances.GetServiceProviderByIdInt32 = (access, id) => originalProvider;
                ShimWebToDatabaseServiceProvider.AllInstances.UpdateServiceProviderWebsiteServiceProviderInt32 =
                    (provider, serviceProvider, userId) => true;
                var result = this.controller.Edit(updatedProvider);

                Assert.AreEqual(ObjectStatus.ObjectState.Update, updatedProvider.State);
                Assert.AreEqual(ObjectStatus.ObjectState.Create, updatedProvider.Locations[0].ContactPerson.State);
                Assert.AreEqual(ObjectStatus.ObjectState.Create, updatedProvider.Locations[0].ContactPerson.Contact.State);
            }
        }

        /// <summary>
        /// Test to change the contact attached to a location on a provider.
        /// </summary>
        [TestMethod]
        public void EditProviderChangeLocationEditContactTest()
        {
            using (ShimsContext.Create())
            {
                this.originalProvider.Locations[0].Contact = new ServiceProviderContactRequired
                {

                    Id = 2123,
                    Email = "asdfasd@asdfasd.com"
                };

                this.originalProvider.Locations[0].ContactPerson = new ServiceProviderContactPerson
                    {
                        FistName = "asdfas",
                        LastName = "q234asdf",
                        Contact = new ServiceProviderContact { Id = 1, Email = "Test@asdf.com" }
                    };

                var updatedLocation = new ServiceProviderLocation
                {
                    Id = 1,
                    StateIdString = "34",
                    Coverage = new List<Coverage>
                    {
                        new Coverage {CountyId = 1, CountryId = 1, Id = 2, StateId = 1 } 
                    },
                    Contact = new ServiceProviderContactRequired
                    {
                        Id = 2123,
                        Email = "asdfasd@asdfasd.com"
                    },
                    ContactPerson = new ServiceProviderContactPerson
                    {
                        FistName = "asdfas",
                        LastName = "q234asdf",
                        Contact = new ServiceProviderContact { Id = 1, Email = "Testds@asdf.com" }
                    }
                };

                var updatedProvider = new WebsiteServiceProvider
                {
                    Id = 4,
                    Name = "Test2",
                    Type = 2,
                    Locations = new List<ServiceProviderLocation> { updatedLocation }
                };

                ShimDataLogics.AllInstances.GetServiceProviderByIdInt32 = (access, id) => this.originalProvider;
                ShimWebToDatabaseServiceProvider.AllInstances.UpdateServiceProviderWebsiteServiceProviderInt32 =
                    (provider, serviceProvider, userId) => true;
                var result = this.controller.Edit(updatedProvider);

                Assert.AreEqual(ObjectStatus.ObjectState.Update, updatedProvider.State);
                Assert.AreEqual(ObjectStatus.ObjectState.Update, updatedProvider.Locations[0].ContactPerson.State);
                Assert.AreEqual(ObjectStatus.ObjectState.Update, updatedProvider.Locations[0].ContactPerson.Contact.State);
            }
        }


        /// <summary>
        /// Test editing a provider and adding a location.
        /// </summary>
        [TestMethod]
        public void EditAddLocationTest()
        {
            using (ShimsContext.Create())
            {
                var updatedLocation1 = new ServiceProviderLocation
                {
                     Id = 1,
                     StateIdString = "34",
                     Contact = new ServiceProviderContactRequired
                     {
                         Id = 1223
                     },
                     ContactPerson = new ServiceProviderContactPerson
                     {
                         Id = 0
                     },
                     Coverage = new List<Coverage>
                        {
                            new Coverage {CountyId = 1, CountryId = 1, Id = 2, StateId = 1}
                        }
                };


                var updatedLocation2 = new ServiceProviderLocation
                {
                    Id = 0,
                    StateIdString = "34",
                    Contact = new ServiceProviderContactRequired
                    {
                        Id = 122323
                    },
                    ContactPerson = new ServiceProviderContactPerson
                    {
                        Id = 0
                     },
                    Coverage = new List<Coverage>
                    {
                        new Coverage {CountyId = 1, CountryId = 1, Id = 2, StateId = 1}
                    }
                };


                var updatedProvider = new WebsiteServiceProvider
                {
                    Id = 4,
                    Name = "Test2",
                    Type = 2,
                    Locations = new List<ServiceProviderLocation> { updatedLocation1, updatedLocation2 }
                };

                ShimDataLogics.AllInstances.GetServiceProviderByIdInt32 = (access, id) => this.originalProvider;
                ShimWebToDatabaseServiceProvider.AllInstances.UpdateServiceProviderWebsiteServiceProviderInt32 =
                    (provider, serviceProvider, userId) => true;
                var result = this.controller.Edit(updatedProvider);

                Assert.AreEqual(ObjectStatus.ObjectState.Update, updatedProvider.State);

                Assert.AreEqual(ObjectStatus.ObjectState.Update, updatedProvider.Locations[0].State);
                Assert.AreEqual(ObjectStatus.ObjectState.Create, updatedProvider.Locations[1].State);
            }
        }


        /// <summary>
        /// Test editing a provider and removing a location.
        /// </summary>
        [TestMethod]
        public void EditProviderRemoveLocationTest()
        {

            using (ShimsContext.Create())
            {
                var originalLocation2 = new ServiceProviderLocation
                {
                    Id = 0,
                    Contact = new ServiceProviderContactRequired
                    {
                        Id = 0
                    },
                    ContactPerson = new ServiceProviderContactPerson
                    {
                        Id = 0
                    },
                    Coverage = new List<Coverage>
                    {
                        new Coverage {CountyId = 4, CountryId = 1, Id = 2, StateId = 1}
                    }
                };

                this.originalProvider.Locations.Add(originalLocation2);

                var updatedLocation = new ServiceProviderLocation
                {
                    Id = 1,
                    StateIdString = "34",
                    Coverage = new List<Coverage>
                    {
                        new Coverage {CountyId = 1, CountryId = 1, Id = 2, StateId = 1 } 
                    },
                    Contact = new ServiceProviderContactRequired
                    {
                        Id = 2123,
                        Email = "asdfasd@asdfasd.com"
                    },
                    ContactPerson = new ServiceProviderContactPerson
                    {
                        FistName = "asdfas",
                        LastName = "q234asdf",
                        Contact = new ServiceProviderContact { Id = 1, Email = "Testds@asdf.com" }
                    }
                };

                var updatedProvider = new WebsiteServiceProvider
                {
                    Id = 4,
                    Name = "Test2",
                    Type = 2,
                    Locations = new List<ServiceProviderLocation> { updatedLocation }
                };

                ShimDataLogics.AllInstances.GetServiceProviderByIdInt32 = (access, id) => this.originalProvider;
                ShimWebToDatabaseServiceProvider.AllInstances.UpdateServiceProviderWebsiteServiceProviderInt32 =
                    (provider, serviceProvider, userId) => true;
                var result = this.controller.Edit(updatedProvider);

                Assert.AreEqual(ObjectStatus.ObjectState.Update, updatedProvider.State);

                Assert.AreEqual(ObjectStatus.ObjectState.Update, updatedProvider.Locations[0].State);
                Assert.AreEqual(ObjectStatus.ObjectState.Delete, updatedProvider.Locations[1].State);
            }
        }


        /// <summary>
        /// Tests editing a provider and adding a coverage item to a location.
        /// </summary>
        [TestMethod]
        public void EditProviderLocationAddCoverageTest()
        {
            using (ShimsContext.Create())
            {
                var updatedLocation1 = new ServiceProviderLocation
                {
                    Id = 1,
                    StateIdString = "34",
                    Contact = new ServiceProviderContactRequired
                    {
                        Id = 1223
                    },
                    ContactPerson = new ServiceProviderContactPerson
                    {
                        Id = 0
                    },
                    Coverage = new List<Coverage>
                    {
                        new Coverage {CountyId = 1, CountryId = 1, Id = 2, StateId = 1 },
                        new Coverage {CountyId = 5, CountryId = 1, Id = 0, StateId = 1 }
                    }
                };


                var updatedProvider = new WebsiteServiceProvider
                {
                    Id = 4,
                    Name = "Test2",
                    Type = 2,
                    Locations = new List<ServiceProviderLocation> { updatedLocation1 }
                };

                ShimDataLogics.AllInstances.GetServiceProviderByIdInt32 = (access, id) => this.originalProvider;
                ShimWebToDatabaseServiceProvider.AllInstances.UpdateServiceProviderWebsiteServiceProviderInt32 =
                    (provider, serviceProvider, userId) => true;
                var result = this.controller.Edit(updatedProvider);

                Assert.AreEqual(ObjectStatus.ObjectState.Update, updatedProvider.State);
                Assert.AreEqual(ObjectStatus.ObjectState.Update, updatedProvider.Locations[0].Coverage[0].State);
                Assert.AreEqual(ObjectStatus.ObjectState.Create, updatedProvider.Locations[0].Coverage[1].State);
            }
        }

        /// <summary>
        /// Tests editing a provider and removing a coverage item from a location.
        /// </summary>
        [TestMethod]
        public void EditProviderLocationRemoveCoverageTest()
        {
            using (ShimsContext.Create())
            {
                this.originalProvider.Locations[0].Coverage.Add(new Coverage { CountyId = 5, CountryId = 1, Id = 0, StateId = 1 });

                var updatedLocation1 = new ServiceProviderLocation
                {
                    Id = 1,
                    StateIdString = "34",
                    Contact = new ServiceProviderContactRequired
                    {
                        Id = 1223
                    },
                    ContactPerson = new ServiceProviderContactPerson
                    {
                        Id = 0
                    },
                    Coverage = new List<Coverage>
                    {
                        new Coverage {CountyId = 1, CountryId = 1, Id = 2, StateId = 1},
                    }
                };

                var updatedProvider = new WebsiteServiceProvider
                {
                    Id = 4,
                    Name = "Test2",
                    Type = 2,
                    Locations = new List<ServiceProviderLocation> { updatedLocation1 }
                };

                ShimDataLogics.AllInstances.GetServiceProviderByIdInt32 = (access, id) => this.originalProvider;
                var result = this.controller.Edit(updatedProvider);

                Assert.AreEqual(ObjectStatus.ObjectState.Update, updatedProvider.State);
                Assert.AreEqual(ObjectStatus.ObjectState.Update, updatedProvider.Locations[0].Coverage[0].State);
                Assert.AreEqual(ObjectStatus.ObjectState.Delete, updatedProvider.Locations[0].Coverage[1].State);
            }
        }

        /// <summary>
        /// The delete provider test.
        /// </summary>
        [TestMethod]
        public void DeleteProviderTest()
        {
            using (ShimsContext.Create())
            {
                ShimDataLogics.AllInstances.GetServiceProviderByIdInt32 = (logics, i) => this.originalProvider;
                ShimWebToDatabaseServiceProvider.AllInstances.UpdateServiceProviderWebsiteServiceProviderInt32 =
                    (provider, serviceProvider, userId) => true;

                var result = this.controller.Delete(1);
                Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
                var routeResult = result as RedirectToRouteResult;
                Assert.IsNotNull(routeResult);
                Assert.AreEqual(routeResult.RouteValues["action"], "Index");
            }
        }

        /// <summary>
        /// The search count test.
        /// </summary>
        [TestMethod]
        public void SearchCountTest()
        {
            using (ShimsContext.Create())
            {
                ShimDataLogics.AllInstances.GetServiceProvidersCountStringNullableOfInt32NullableOfInt32 =
                    (logics, s, arg3, arg4) => 5;

                var result = this.controller.SearchCount("asdfasd", null, null);
                Assert.IsNotNull(result);
                var jsonData = result as JsonResult;
                Assert.AreEqual(5, jsonData.Data);

            }
        }


        /// <summary>
        /// The search test.
        /// </summary>
        [TestMethod]
        public void SearchTest()
        {

            var searchResults = new List<ServiceProviderSearchResult>
            {
                new ServiceProviderSearchResult
                {
                    Counties = new List<string> {"county1"},
                    Categories = new List<string> {"category 1"},
                    Name = "Provider Name"
                },
                new ServiceProviderSearchResult
                {
                    Counties = new List<string> {"county1"},
                    Categories = new List<string> {"category 2"},
                    Name = "Provider Name 2"
                }
            };

            using (ShimsContext.Create())
            {
                ShimDataLogics.AllInstances.GetServiceProvidersStringInt32Int32NullableOfInt32NullableOfInt32 =
                    (logics, s, arg3, arg4, arg5, arg6) => searchResults;

                var result = this.controller.Search("asdfasd", 1, null, null);
                Assert.IsNotNull(result);
                Assert.IsNotNull(result.Model);
                Assert.AreEqual(2, (result.Model as List<ServiceProviderSearchResult>).Count);
            }
        }
    }
}
