// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DatabaseToWebServiceProviderTests.cs" company="UDRI">
//   Copyright © 2015 University of Dayton Research Institute. All rights reserved.
// </copyright>
// <summary>
//   The conversion tests for service providers.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Linq;
using Website.Models;

namespace DataEntry.Tests.BL.ModelConversions
{
    using System.Collections.Generic;
    using DataEntry_Helpers;
    using DataEntry_Helpers.Repositories.Fakes;
    using Microsoft.QualityTools.Testing.Fakes;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Website.BL.ModelConversions;

    /// <summary>
    /// The date logic tests.
    /// </summary>
    [TestClass]
    public class DatabaseToWebServiceProviderTests
    {
        /// <summary>
        /// The target.
        /// </summary>
        private DatabaseToWebServiceProvider target;

        /// <summary>
        /// The contact.
        /// </summary>
        private Contact contact;

        /// <summary>
        /// The state.
        /// </summary>
        private State state;

        /// <summary>
        /// The location 1.
        /// </summary>
        private Location location1;

        /// <summary>
        /// The location 2.
        /// </summary>
        private Location location2;

        /// <summary>
        /// The locations.
        /// </summary>
        private List<Location> locations;

        /// <summary>
        /// The provider services 1.
        /// </summary>
        private List<ProviderService> providerServices1;

        /// <summary>
        /// The provider services 2.
        /// </summary>
        private List<ProviderService> providerServices2;

        /// <summary>
        /// The service provider 1.
        /// </summary>
        private ServiceProvider serviceProvider1;

        /// <summary>
        /// The service provider 2.
        /// </summary>
        private ServiceProvider serviceProvider2;

        /// <summary>
        /// The service provider list.
        /// </summary>
        private List<ServiceProvider> serviceProviderList;

        /// <summary>
        /// The provider coverage 1.
        /// </summary>
        private ProviderCoverage providerCoverage1;

        /// <summary>
        /// The provider coverage 2.
        /// </summary>
        private ProviderCoverage providerCoverage2;

        /// <summary>
        /// The country.
        /// </summary>
        private Country country;

        /// <summary>
        /// The county.
        /// </summary>
        private County county;

        /// <summary>
        /// The provider coverage list.
        /// </summary>
        private List<ProviderCoverage> providerCoverageList;

        /// <summary>
        /// The contact person.
        /// </summary>
        private ContactPerson contactPerson;

        #region Additional test attributes

        // You can use the following additional attributes as you write your tests:
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize]
        // public static void MyClassInitialize(TestContext testContext)
        // {
        // }
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup()
        // {
        // }
        // Use TestInitialize to run code before running each test

        /// <summary>
        /// The my test initialize.
        /// </summary>
        [TestInitialize]
        public void MyTestInitialize()
        {
            this.target = new DatabaseToWebServiceProvider();
            
            this.contact = new Contact
                               {
                                   Email = "Test@test.org", 
                                   ID = 1, 
                                   Website = "www.test.org", 
                                   Phone = "9373608284", 
                                   HelpLine = "9373608888"
                               };

            this.contactPerson = new ContactPerson
                                     {
                                         Contact = this.contact, 
                                         ContactID = 1, 
                                         ID = 1, 
                                         FirstName = "Test", 
                                         LastName = "Person", 
                                         JobTitle = "Tester"
                                     };

            this.state = new State { Abbreviation = "OH", CountryID = 1, FullName = "Ohio", ID = 1 };
            this.country = new Country { Abbreviation = "USA", ID = 1, FullName = "United States of America" };
            this.county = new County { ID = 1, Name = "test county", StateId = 1, State = this.state };

            this.location1 = new Location
                                 {
                                     Name = "Location1",
                                     CountryID = 1, 
                                     Country = this.country, 
                                     City = "testville 1", 
                                     ContactID = 1, 
                                     Contact = this.contact, 
                                     ContactPersonID = 1, 
                                     ID = 1, 
                                     Display = true, 
                                     Zip = "45344", 
                                     Street = "Test way 1", 
                                     State = this.state, 
                                     StateID = this.state.ID,
                                     ProviderCoverages = new List<ProviderCoverage> {  new ProviderCoverage { County = new County { Name = "County1"} } }
                                 };
            this.location2 = new Location
                                 {
                                     Name="Location2",
                                     CountryID = 1, 
                                     Country = this.country, 
                                     City = "testville 2", 
                                     ContactID = 2, 
                                     Contact = this.contact, 
                                     ContactPersonID = 2, 
                                     ID = 2, 
                                     Display = true, 
                                     Zip = "45344", 
                                     Street = "Test way 2", 
                                     State = this.state, 
                                     StateID = this.state.ID,
                ProviderCoverages = new List<ProviderCoverage> { new ProviderCoverage { County = new County { Name = "County2" } } }
            };

            this.locations = new List<Location> { this.location1, this.location2 };

            var providerService11 = new ProviderService { ID = 1, ProviderID = 1, ServiceID = 1, ProviderServiceCategory = new ProviderServiceCategory{ Name = "Category 1"} };
            var providerService12 = new ProviderService { ID = 2, ProviderID = 1, ServiceID = 2, ProviderServiceCategory = new ProviderServiceCategory { Name = "Category 2" } };
            var providerService13 = new ProviderService { ID = 3, ProviderID = 1, ServiceID = 3, ProviderServiceCategory = new ProviderServiceCategory { Name = "Category 3" } };

            var providerService21 = new ProviderService { ID = 1, ProviderID = 2, ServiceID = 1, ProviderServiceCategory = new ProviderServiceCategory { Name = "Category 1" } };
            var providerService22 = new ProviderService { ID = 2, ProviderID = 2, ServiceID = 2, ProviderServiceCategory = new ProviderServiceCategory { Name = "Category 2" } };
            var providerService23 = new ProviderService { ID = 3, ProviderID = 2, ServiceID = 3, ProviderServiceCategory = new ProviderServiceCategory { Name = "Category 3" } };

            this.providerServices1 = new List<ProviderService>
                                         {
                                             providerService11, 
                                             providerService12, 
                                             providerService13, 
                                         };

            this.providerServices2 = new List<ProviderService>
                                         {
                                             providerService21, 
                                             providerService22, 
                                             providerService23
                                         };

            this.serviceProvider1 = new ServiceProvider
                                        {
                                            Description = "Test description for test 1", 
                                            ID = 1, 
                                            DisplayRank = 1, 
                                            ProviderName = "Test 1", 
                                            Locations = this.locations, 
                                            ProviderServices = this.providerServices1, 
                                            ServiceTypes = 1,
                                            Active = true,
                                            ServiceType = new ServiceType { Description = "Description", ID = 1, Name = "Name" }
                                        };
            this.serviceProvider2 = new ServiceProvider
                                        {
                                            Description = "Test description for test 2", 
                                            ID = 2, 
                                            DisplayRank = 2, 
                                            ProviderName = "Test 2", 
                                            Locations = this.locations, 
                                            ProviderServices = this.providerServices2,
                                            ServiceTypes = 1,
                                            Active = false,
                                            ServiceType = new ServiceType { Description = "Description", ID = 1, Name = "Name" }
                                        };

            this.serviceProviderList = new List<ServiceProvider> { this.serviceProvider1, this.serviceProvider2 };

            this.providerCoverage1 = new ProviderCoverage { AreaID = 1, County = this.county, ID = 1, LocationID = 1 };
            this.providerCoverage2 = new ProviderCoverage { AreaID = 1, County = this.county, ID = 2, LocationID = 2 };

            this.providerCoverageList = new List<ProviderCoverage> { this.providerCoverage1, this.providerCoverage2 };
        }

        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup()
        // {
        // }
        #endregion
            
        /// <summary>
        /// The get service provider test.
        /// </summary>
        [TestMethod]
        [TestCategory("DatabaseToWebServiceProviderTests")]
        public void GetServiceProviderTest()
        {
            using (ShimsContext.Create())
            {
                ShimServiceProviderRepo.AllInstances.GetServiceProviderInt32 = (repo, i) => this.serviceProvider1;
                ShimDataAccess.AllInstances.GetCoveragesInt32 = (access, i) => this.providerCoverageList;
                var response = this.target.GetServiceProvider(1);

                Assert.IsNotNull(response);

                Assert.AreEqual(this.serviceProvider1.Description, response.Description);
                Assert.AreEqual(this.serviceProvider1.DisplayRank, response.DisplayRank);
                Assert.AreEqual(this.serviceProvider1.ID, response.Id);
                Assert.AreEqual(this.serviceProvider1.ProviderName, response.Name);

                Assert.AreEqual(this.locations.Count, response.Locations.Count);

                Assert.AreEqual(this.providerCoverageList.Count, response.Locations[0].Coverage.Count);
                Assert.AreEqual(this.providerCoverage1.ID, response.Locations[0].Coverage[0].Id);
                Assert.AreEqual(this.state.ID, response.Locations[0].Coverage[0].StateId);
                Assert.AreEqual(this.state.FullName, response.Locations[0].Coverage[0].StateName);
                Assert.AreEqual(this.country.ID, response.Locations[0].Coverage[0].CountryId);
                Assert.AreEqual(this.country.FullName, response.Locations[0].Coverage[0].CountryName);
                Assert.AreEqual(this.county.ID, response.Locations[0].Coverage[0].CountyId);
                Assert.AreEqual(this.county.Name, response.Locations[0].Coverage[0].CountyName);

                Assert.AreEqual(this.providerCoverage2.ID, response.Locations[0].Coverage[1].Id);
                Assert.AreEqual(this.state.ID, response.Locations[0].Coverage[1].StateId);
                Assert.AreEqual(this.state.FullName, response.Locations[0].Coverage[1].StateName);
                Assert.AreEqual(this.country.ID, response.Locations[0].Coverage[1].CountryId);
                Assert.AreEqual(this.country.FullName, response.Locations[0].Coverage[1].CountryName);
                Assert.AreEqual(this.county.ID, response.Locations[0].Coverage[1].CountyId);
                Assert.AreEqual(this.county.Name, response.Locations[0].Coverage[1].CountyName);

                Assert.AreEqual(this.providerCoverageList.Count, response.Locations[1].Coverage.Count);

                Assert.AreEqual(this.providerCoverage1.ID, response.Locations[1].Coverage[0].Id);
                Assert.AreEqual(this.state.ID, response.Locations[1].Coverage[0].StateId);
                Assert.AreEqual(this.state.FullName, response.Locations[1].Coverage[0].StateName);
                Assert.AreEqual(this.country.ID, response.Locations[1].Coverage[0].CountryId);
                Assert.AreEqual(this.country.FullName, response.Locations[1].Coverage[0].CountryName);
                Assert.AreEqual(this.county.ID, response.Locations[1].Coverage[0].CountyId);
                Assert.AreEqual(this.county.Name, response.Locations[1].Coverage[0].CountyName);

                Assert.AreEqual(this.providerCoverage2.ID, response.Locations[1].Coverage[1].Id);
                Assert.AreEqual(this.state.ID, response.Locations[1].Coverage[1].StateId);
                Assert.AreEqual(this.state.FullName, response.Locations[1].Coverage[1].StateName);
                Assert.AreEqual(this.country.ID, response.Locations[1].Coverage[1].CountryId);
                Assert.AreEqual(this.country.FullName, response.Locations[1].Coverage[1].CountryName);
                Assert.AreEqual(this.county.ID, response.Locations[1].Coverage[1].CountyId);
                Assert.AreEqual(this.county.Name, response.Locations[1].Coverage[1].CountyName);
            }
        }

        /// <summary>
        /// The create service provider contact person test.
        /// </summary>
        [TestMethod]
        [TestCategory("DatabaseToWebServiceProviderTests")]
        public void CreateServiceProviderContactPersonTest()
        {
            var contactTest =
                this.target.CreateServiceProviderContactPerson(this.contactPerson);

            Assert.IsNotNull(contactTest);
            
            Assert.AreEqual(this.contactPerson.JobTitle, contactTest.JobTitle);
            Assert.AreEqual(this.contactPerson.LastName, contactTest.LastName);
            Assert.AreEqual(this.contactPerson.FirstName, contactTest.FistName);
        }

        /// <summary>
        /// The get service provider edit log with a deleted provider test.
        /// </summary>
        [TestMethod]
        [TestCategory("DatabaseToWebServiceProviderTests")]
        public void GetServiceProviderEditLogTest()
        {
            var datetime = DateTime.Now;
            var logEntires = new List<ServiceProviderEdit>
            {
                new ServiceProviderEdit
                {
                    ID = 1,
                    UserId = 1,
                    User = new User { ID = 1, FirstName = "First", LastName = "Last" },
                    DateTime = datetime,
                    ProviderId = 1,
                    Action = "Edited",
                    ServiceProvider = new ServiceProvider {ID = 1, ProviderName = "Test name"}
                }
            };

            using (ShimsContext.Create())
            {
                ShimServiceProviderRepo.AllInstances.GetServiceProviderEditLog = repo => logEntires;
                var retreivedLog = this.target.GetServiceProviderEditLog();
                Assert.AreEqual(1, retreivedLog.Count);
                Assert.AreEqual(1, retreivedLog[0].UserId);
                Assert.AreEqual(1, retreivedLog[0].EditedProviderId);
                Assert.AreEqual(datetime, retreivedLog[0].EditedDateTime);
                Assert.AreEqual("Test name", retreivedLog[0].EditedProviderName);
                Assert.AreEqual("First", retreivedLog[0].UserFirstName);
                Assert.AreEqual("Last", retreivedLog[0].UserLastName);
                Assert.AreEqual("Edited", retreivedLog[0].Action);
            }
        }



        /// <summary>
        /// The get service provider edit log test.
        /// </summary>
        [TestMethod]
        [TestCategory("DatabaseToWebServiceProviderTests")]
        public void GetServiceProviderEditLogDeletedProviderTest()
        {
            var datetime = DateTime.Now;
            var logEntires = new List<ServiceProviderEdit>
            {
                new ServiceProviderEdit
                {
                    ID = 1,
                    UserId = 1,
                    User = new User { ID = 1, FirstName = "First", LastName = "Last" },
                    DateTime = datetime,
                    ProviderId = null,
                    ServiceProvider = null,
                    Action = "Deleted",
                    DeletedProviderName = "Removed Provider Name"
                  
                }
            };

            using (ShimsContext.Create())
            {
                ShimServiceProviderRepo.AllInstances.GetServiceProviderEditLog = repo => logEntires;
                var retreivedLog = this.target.GetServiceProviderEditLog();
                Assert.AreEqual(1, retreivedLog.Count);
                Assert.AreEqual(1, retreivedLog[0].UserId);
                Assert.AreEqual(datetime, retreivedLog[0].EditedDateTime);
                Assert.AreEqual("Removed Provider Name", retreivedLog[0].EditedProviderName);
                Assert.AreEqual("First", retreivedLog[0].UserFirstName);
                Assert.AreEqual("Last", retreivedLog[0].UserLastName);
                Assert.AreEqual("Deleted", retreivedLog[0].Action);
            }
        }

        [TestMethod]
        public void GetAllServiceProvidersTest()
        {
            using (ShimsContext.Create())
            {
                ShimServiceProviderRepo.AllInstances
                    .GetAllServiceProvidersByNameStringInt32Int32NullableOfInt32NullableOfInt32
                    = (repo, s, arg3, arg4, arg5, arg6) => serviceProviderList;

                List<ServiceProviderSearchResult> result = this.target.GetAllServiceProviders("test", 10, 1, 2, 2);
                
                Assert.AreEqual(serviceProviderList.Count, result.Count);
                Assert.IsTrue(result[0].Categories.Contains("Category 1"));
                Assert.IsTrue(result[0].Counties.Contains("County1"));
            }
            
        }
    }
}
