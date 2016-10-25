// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WebToDatabaseServiceProviderTests.cs" company="UDRI">
//   Copyright © 2015 University of Dayton Research Institute. All rights reserved.
// </copyright>
// <summary>
//   The conversion tests for service providers.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DataEntry.Tests.BL.ModelConversions
{
    using System.Collections.Generic;
    using DataEntry_Helpers;
    using DataEntry_Helpers.Repositories.Fakes;
    using Microsoft.QualityTools.Testing.Fakes;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Models;
    using Models.ServiceProvider;
    using Website.BL.ModelConversions;

    /// <summary>
    /// The date logic tests.
    /// </summary>
    [TestClass]
    public class WebToDatabaseServiceProviderTests
    {
        /// <summary>
        /// The target.
        /// </summary>
        private WebToDatabaseServiceProvider target;

        /// <summary>
        /// The website service provider.
        /// </summary>
        private WebsiteServiceProvider websiteServiceProvider;

        /// <summary>
        /// The web service areas.
        /// </summary>
        private WebServiceAreas webServiceAreas;

        /// <summary>
        /// The location 1.
        /// </summary>
        private ServiceProviderLocation location1;

        /// <summary>
        /// The location 2.
        /// </summary>
        private ServiceProviderLocation location2;

        /// <summary>
        /// The state.
        /// </summary>
        private State state;

        /// <summary>
        /// The country.
        /// </summary>
        private Country country;

        /// <summary>
        /// The county.
        /// </summary>
        private County county;

        /// <summary>
        /// The locations.
        /// </summary>
        private List<ServiceProviderLocation> locations;

        /// <summary>
        /// The contact.
        /// </summary>
        private ServiceProviderContact contact;
        
        /// <summary>
        /// The location contact.
        /// </summary>
        private ServiceProviderContactRequired locationContact;

        /// <summary>
        /// The coverage 1.
        /// </summary>
        private Coverage coverage1;

        /// <summary>
        /// The coverage list.
        /// </summary>
        private List<Coverage> coverageList;

        /// <summary>
        /// The coverage 2.
        /// </summary>
        private Coverage coverage2;

        /// <summary>
        /// The contact person.
        /// </summary>
        private ServiceProviderContactPerson contactPerson;

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
            this.target = new WebToDatabaseServiceProvider();

            this.state = new State { Abbreviation = "OH", CountryID = 1, FullName = "Ohio", ID = 1 };
            this.country = new Country { Abbreviation = "USA", ID = 1, FullName = "United States of America" };
            this.county = new County { ID = 1, Name = "test county", StateId = 1, State = this.state };

            this.contact = new ServiceProviderContact
                               {
                                   Email = "Test@test.org", 
                                   Id = 1, 
                                   Website = "www.test.org", 
                                   PhoneNumber = "9373608284", 
                                   HelpLine = "9373608888", 
                                   State = ObjectStatus.ObjectState.Update
                               };

            this.locationContact = new ServiceProviderContactRequired
            {
                Email = "Test@test.org",
                Id = 1,
                Website = "www.test.org",
                PhoneNumber = "9373608284",
                HelpLine = "9373608888",
                State = ObjectStatus.ObjectState.Update
            };

            this.contactPerson = new ServiceProviderContactPerson
                                     {
                                         Id = 1, 
                                         Contact = this.contact, 
                                         FistName = "test", 
                                         LastName = "user", 
                                         JobTitle = "Tester", 
                                         State = ObjectStatus.ObjectState.Update
                                     };
            this.coverage1 = new Coverage
                                 {
                                     CountryId = 1, 
                                     CountryName = "USA", 
                                     CountyId = 1, 
                                     CountyName = "Clark", 
                                     Id = 1, 
                                     State = ObjectStatus.ObjectState.Update, 
                                     StateId = 1, 
                                     StateName = "Ohio"
                                 };
            this.coverage2 = new Coverage
                                 {
                                     CountryId = 1, 
                                     CountryName = "USA", 
                                     CountyId = 2, 
                                     CountyName = "Clark 2", 
                                     Id = 1, 
                                     State = ObjectStatus.ObjectState.Update, 
                                     StateId = 1, 
                                     StateName = "Ohio"
                                 };
            this.coverageList = new List<Coverage> { this.coverage1, this.coverage2 };

            this.location1 = new ServiceProviderLocation
                                 {
                                     Name = "Location1",
                                     Coverage = this.coverageList, 
                                     CountryId = 1, 
                                     Contact = this.locationContact, 
                                     ContactPerson = this.contactPerson, 
                                     City = "testville 1", 
                                     Id = 1, 
                                     Display = true, 
                                     Zip = "45344", 
                                     Street = "Test way 1", 
                                     StateId = this.state.ID, 
                                     State = ObjectStatus.ObjectState.Update
                                 };
            this.location2 = new ServiceProviderLocation
                                 {
                                     Name = "Location2",
                                     Coverage = this.coverageList, 
                                     CountryId = 1, 
                                     Contact = this.locationContact, 
                                     City = "testville 2", 
                                     Id = 2, 
                                     Display = true, 
                                     Zip = "45344", 
                                     Street = "Test way 2", 
                                     StateId = this.state.ID, 
                                     State = ObjectStatus.ObjectState.Update
                                 };

            this.locations = new List<ServiceProviderLocation> { this.location1, this.location2 };

            this.webServiceAreas = new WebServiceAreas
                                       {
                                           ServiceAreas = new List<int> { 1, 2, 3 }, 
                                           State = ObjectStatus.ObjectState.Update
                                       };

            this.websiteServiceProvider = new WebsiteServiceProvider
                                              {
                                                  Locations = this.locations, 
                                                  Services = this.webServiceAreas, 
                                                  Description = "Test Web Provider", 
                                                  DisplayRank = 1, 
                                                  Id = 1, 
                                                  Name = "test provider 1", 
                                                  Type = 1, 
                                                  IsActive = true,
                                                  State = ObjectStatus.ObjectState.Update
                                              };
        }

        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup()
        // {
        // }
        #endregion

        /// <summary>
        /// The update service provider fail test.
        /// </summary>
        [TestMethod]
        [TestCategory("WebToDatabaseServiceProviderTests")]
        public void UpdateServiceProviderFailTest()
        {
            using (ShimsContext.Create())
            {
                ShimServiceProviderRepo.AllInstances.UpdateServiceAreaInt32ListOfInt32 = (repo, i, arg3) => true;
                ShimServiceProviderRepo.AllInstances.UpdateContactPersonServiceProviderContactPerson =
                    (repo, person) => true;
                ShimServiceProviderRepo.AllInstances.UpdateContactContact = (repo, contact1) => true;
                ShimServiceProviderRepo.AllInstances.UpdateCoverageInt32Coverage = (repo, i, arg3) => true;
                ShimServiceProviderRepo.AllInstances.UpdateProviderWebsiteServiceProvider = (repo, provider) => false;
                ShimServiceProviderRepo.AllInstances.UpdateLocationServiceProviderLocation = (repo, location) => true;

                ShimServiceProviderRepo.AllInstances.LogServiceProviderEditServiceProviderEdit = (repo, edit) => true;

                var response = this.target.UpdateServiceProvider(this.websiteServiceProvider, 1);

                Assert.IsFalse(response);
            }
        }

        /// <summary>
        /// The update service provider update test.
        /// </summary>
        [TestMethod]
        [TestCategory("WebToDatabaseServiceProviderTests")]
        public void UpdateServiceProviderUpdateTest()
        {
            using (ShimsContext.Create())
            {
                ShimServiceProviderRepo.AllInstances.UpdateServiceAreaInt32ListOfInt32 = (repo, i, arg3) => true;
                ShimServiceProviderRepo.AllInstances.UpdateContactPersonServiceProviderContactPerson =
                    (repo, person) => true;
                ShimServiceProviderRepo.AllInstances.UpdateContactContact = (repo, contact1) => true;
                ShimServiceProviderRepo.AllInstances.UpdateCoverageInt32Coverage = (repo, i, arg3) => true;
                ShimServiceProviderRepo.AllInstances.UpdateProviderWebsiteServiceProvider = (repo, provider) => true;
                ShimServiceProviderRepo.AllInstances.UpdateLocationServiceProviderLocation = (repo, location) => true;


                ShimServiceProviderRepo.AllInstances.DeleteContactContact = (repo, contact1) => true;
                ShimServiceProviderRepo.AllInstances.DeleteContactPersonServiceProviderContactPerson =
                    (repo, person) => true;
                ShimServiceProviderRepo.AllInstances.UpdateContactContact =
                    (repo, providerContact) => true;
                ShimServiceProviderRepo.AllInstances.DeleteAllCoveragesForLocationInt32 = (repo, i) => true;
                ShimServiceProviderRepo.AllInstances.DeleteLocationInt32 = (repo, i) => true;
                ShimServiceProviderRepo.AllInstances.DeleteProviderServiceInt32 = (repo, i) => true;
                ShimServiceProviderRepo.AllInstances.DeleteProviderWebsiteServiceProvider = (repo, provider) => true;

                ShimServiceProviderRepo.AllInstances.CreateServiceProviderServiceProvider = (repo, provider) => 1;
                ShimServiceProviderRepo.AllInstances.CreateContactContact = (repo, contact1) => 1;
                ShimServiceProviderRepo.AllInstances.CreateContactPersonContactPerson = (repo, person) => 1;
                ShimServiceProviderRepo.AllInstances.CreateCoverageProviderCoverage = (repo, coverage) => true;
                ShimServiceProviderRepo.AllInstances.CreateLocationLocation = (repo, location) => 1;
                ShimServiceProviderRepo.AllInstances.CreateServiceAreaProviderService = (repo, service) => true;

                ShimServiceProviderRepo.AllInstances.LogServiceProviderEditServiceProviderEdit = (repo, edit) => true;

                this.websiteServiceProvider.State = ObjectStatus.ObjectState.Update;
                var response = this.target.UpdateServiceProvider(this.websiteServiceProvider, 1);

                Assert.IsTrue(response);

                this.websiteServiceProvider.State = ObjectStatus.ObjectState.Read;
                response = this.target.UpdateServiceProvider(this.websiteServiceProvider, 1);

                Assert.IsFalse(response);

                this.websiteServiceProvider.State = ObjectStatus.ObjectState.Delete;
                response = this.target.UpdateServiceProvider(this.websiteServiceProvider, 1);

                Assert.IsTrue(response);

                this.websiteServiceProvider.State = ObjectStatus.ObjectState.Create;
                response = this.target.UpdateServiceProvider(this.websiteServiceProvider, 1);

                Assert.IsTrue(response);
            }
        }

        /// <summary>
        /// The update service provider location test.
        /// </summary>
        [TestMethod]
        [TestCategory("WebToDatabaseServiceProviderTests")]
        public void UpdateServiceProviderLocationTest()
        {
            using (ShimsContext.Create())
            {
                ShimServiceProviderRepo.AllInstances.UpdateServiceAreaInt32ListOfInt32 = (repo, i, arg3) => true;
                ShimServiceProviderRepo.AllInstances.UpdateContactPersonServiceProviderContactPerson =
                    (repo, person) => true;
                ShimServiceProviderRepo.AllInstances.UpdateContactContact = (repo, contact1) => true;
                ShimServiceProviderRepo.AllInstances.UpdateCoverageInt32Coverage = (repo, i, arg3) => true;
                ShimServiceProviderRepo.AllInstances.UpdateProviderWebsiteServiceProvider = (repo, provider) => true;
                ShimServiceProviderRepo.AllInstances.UpdateLocationServiceProviderLocation = (repo, location) => true;


                ShimServiceProviderRepo.AllInstances.DeleteContactContact = (repo, contact1) => true;
                ShimServiceProviderRepo.AllInstances.DeleteContactPersonServiceProviderContactPerson =
                    (repo, person) => true;
                ShimServiceProviderRepo.AllInstances.UpdateContactContact =
                    (repo, providerContact) => true;
                ShimServiceProviderRepo.AllInstances.DeleteAllCoveragesForLocationInt32 = (repo, i) => true;
                ShimServiceProviderRepo.AllInstances.DeleteLocationInt32 = (repo, i) => true;
                ShimServiceProviderRepo.AllInstances.DeleteProviderServiceInt32 = (repo, i) => true;
                ShimServiceProviderRepo.AllInstances.DeleteProviderWebsiteServiceProvider = (repo, provider) => true;

                ShimServiceProviderRepo.AllInstances.CreateServiceProviderServiceProvider = (repo, provider) => 1;
                ShimServiceProviderRepo.AllInstances.CreateContactContact = (repo, contact1) => 1;
                ShimServiceProviderRepo.AllInstances.CreateContactPersonContactPerson = (repo, person) => 1;
                ShimServiceProviderRepo.AllInstances.CreateCoverageProviderCoverage = (repo, coverage) => true;
                ShimServiceProviderRepo.AllInstances.CreateLocationLocation = (repo, location) => 1;
                ShimServiceProviderRepo.AllInstances.CreateServiceAreaProviderService = (repo, service) => true;

                ShimServiceProviderRepo.AllInstances.LogServiceProviderEditServiceProviderEdit = (repo, edit) => true;

                this.websiteServiceProvider.State = ObjectStatus.ObjectState.Update;
                var response = this.target.UpdateServiceProvider(this.websiteServiceProvider, 1);

                Assert.IsTrue(response);

                this.location1.State = ObjectStatus.ObjectState.Read;
                response = this.target.UpdateServiceProvider(this.websiteServiceProvider, 1);

                Assert.IsTrue(response);

                this.location1.State = ObjectStatus.ObjectState.Delete;
                response = this.target.UpdateServiceProvider(this.websiteServiceProvider, 1);

                Assert.IsTrue(response);

                this.location1.State = ObjectStatus.ObjectState.Create;
                response = this.target.UpdateServiceProvider(this.websiteServiceProvider, 1);
                Assert.IsTrue(response);

                this.location1.State = ObjectStatus.ObjectState.Test;
                response = this.target.UpdateServiceProvider(this.websiteServiceProvider, 1);

                Assert.IsFalse(response);

                ShimServiceProviderRepo.AllInstances.DeleteLocationInt32 = (repo, i) => false;
                this.location1.State = ObjectStatus.ObjectState.Delete;
                response = this.target.UpdateServiceProvider(this.websiteServiceProvider, 1);

                Assert.IsFalse(response);
            }
        }

        /// <summary>
        /// The update service provider areas test.
        /// </summary>
        [TestMethod]
        [TestCategory("WebToDatabaseServiceProviderTests")]
        public void UpdateServiceProviderAreasTest()
        {
            using (ShimsContext.Create())
            {
                ShimServiceProviderRepo.AllInstances.UpdateServiceAreaInt32ListOfInt32 = (repo, i, arg3) => true;
                ShimServiceProviderRepo.AllInstances.UpdateContactPersonServiceProviderContactPerson =
                    (repo, person) => true;
                ShimServiceProviderRepo.AllInstances.UpdateContactContact = (repo, contact1) => true;
                ShimServiceProviderRepo.AllInstances.UpdateCoverageInt32Coverage = (repo, i, arg3) => true;
                ShimServiceProviderRepo.AllInstances.UpdateProviderWebsiteServiceProvider = (repo, provider) => true;
                ShimServiceProviderRepo.AllInstances.UpdateLocationServiceProviderLocation = (repo, location) => true;

                ShimServiceProviderRepo.AllInstances.DeleteContactPersonServiceProviderContactPerson =
                    (repo, person) => true;
                ShimServiceProviderRepo.AllInstances.UpdateContactContact =
                    (repo, providerContact) => true;
                ShimServiceProviderRepo.AllInstances.DeleteAllCoveragesForLocationInt32 = (repo, i) => true;
                ShimServiceProviderRepo.AllInstances.DeleteLocationInt32 = (repo, i) => true;
                ShimServiceProviderRepo.AllInstances.DeleteProviderServiceInt32 = (repo, i) => true;
                ShimServiceProviderRepo.AllInstances.DeleteProviderWebsiteServiceProvider = (repo, provider) => true;

                ShimServiceProviderRepo.AllInstances.CreateServiceProviderServiceProvider = (repo, provider) => 1;
                ShimServiceProviderRepo.AllInstances.CreateContactContact = (repo, contact1) => 1;
                ShimServiceProviderRepo.AllInstances.CreateContactPersonContactPerson = (repo, person) => 1;
                ShimServiceProviderRepo.AllInstances.CreateCoverageProviderCoverage = (repo, coverage) => true;
                ShimServiceProviderRepo.AllInstances.CreateLocationLocation = (repo, location) => 1;
                ShimServiceProviderRepo.AllInstances.CreateServiceAreaProviderService = (repo, service) => true;
                ShimServiceProviderRepo.AllInstances.LogServiceProviderEditServiceProviderEdit = (repo, edit) => true;

                this.websiteServiceProvider.State = ObjectStatus.ObjectState.Update;

                this.webServiceAreas.State = ObjectStatus.ObjectState.Update;
                var response = this.target.UpdateServiceProvider(this.websiteServiceProvider, 1);

                Assert.IsTrue(response);

                this.webServiceAreas.State = ObjectStatus.ObjectState.Read;
                response = this.target.UpdateServiceProvider(this.websiteServiceProvider, 1);

                Assert.IsTrue(response);

                this.webServiceAreas.State = ObjectStatus.ObjectState.Delete;
                response = this.target.UpdateServiceProvider(this.websiteServiceProvider, 1);

                Assert.IsTrue(response);

                this.webServiceAreas.State = ObjectStatus.ObjectState.Create;
                response = this.target.UpdateServiceProvider(this.websiteServiceProvider, 1);

                Assert.IsTrue(response);
            }
        }

        /// <summary>
        /// The update service provider contact person test.
        /// </summary>
        [TestMethod]
        [TestCategory("WebToDatabaseServiceProviderTests")]
        public void UpdateServiceProviderContactPersonTest()
        {
            using (ShimsContext.Create())
            {
                ShimServiceProviderRepo.AllInstances.UpdateServiceAreaInt32ListOfInt32 = (repo, i, arg3) => true;
                ShimServiceProviderRepo.AllInstances.UpdateContactPersonServiceProviderContactPerson =
                    (repo, person) => true;
                ShimServiceProviderRepo.AllInstances.UpdateContactContact = (repo, contact1) => true;
                ShimServiceProviderRepo.AllInstances.UpdateCoverageInt32Coverage = (repo, i, arg3) => true;
                ShimServiceProviderRepo.AllInstances.UpdateProviderWebsiteServiceProvider = (repo, provider) => true;
                ShimServiceProviderRepo.AllInstances.UpdateLocationServiceProviderLocation = (repo, location) => true;

                ShimServiceProviderRepo.AllInstances.DeleteContactContact = (repo, contact1) => true;
                ShimServiceProviderRepo.AllInstances.DeleteContactPersonServiceProviderContactPerson = (repo, person) => true;
                ShimServiceProviderRepo.AllInstances.UpdateContactContact = (repo, providerContact) => true;
                ShimServiceProviderRepo.AllInstances.DeleteAllCoveragesForLocationInt32 = (repo, i) => true;
                ShimServiceProviderRepo.AllInstances.DeleteLocationInt32 = (repo, i) => true;
                ShimServiceProviderRepo.AllInstances.DeleteProviderServiceInt32 = (repo, i) => true;
                ShimServiceProviderRepo.AllInstances.DeleteProviderWebsiteServiceProvider = (repo, provider) => true;

                ShimServiceProviderRepo.AllInstances.CreateServiceProviderServiceProvider = (repo, provider) => 1;
                ShimServiceProviderRepo.AllInstances.CreateContactContact = (repo, contact1) => 1;
                ShimServiceProviderRepo.AllInstances.CreateContactPersonContactPerson = (repo, person) => 1;
                ShimServiceProviderRepo.AllInstances.CreateCoverageProviderCoverage = (repo, coverage) => true;
                ShimServiceProviderRepo.AllInstances.CreateLocationLocation = (repo, location) => 1;
                ShimServiceProviderRepo.AllInstances.CreateServiceAreaProviderService = (repo, service) => true;

                ShimServiceProviderRepo.AllInstances.LogServiceProviderEditServiceProviderEdit = (repo, edit) => true;

                this.websiteServiceProvider.State = ObjectStatus.ObjectState.Update;

                this.contactPerson.State = ObjectStatus.ObjectState.Read;
                var response = this.target.UpdateServiceProvider(this.websiteServiceProvider, 1);

                Assert.IsTrue(response);

                this.contactPerson.State = ObjectStatus.ObjectState.Update;
                response = this.target.UpdateServiceProvider(this.websiteServiceProvider, 1);

                Assert.IsTrue(response);

                this.contactPerson.State = ObjectStatus.ObjectState.Delete;
                response = this.target.UpdateServiceProvider(this.websiteServiceProvider, 1);

                Assert.IsTrue(response);

                this.contactPerson.State = ObjectStatus.ObjectState.Create;
                response = this.target.UpdateServiceProvider(this.websiteServiceProvider, 1);

                Assert.IsTrue(response);
            }
        }

        /// <summary>
        /// The update service provider contact test.
        /// </summary>
        [TestMethod]
        [TestCategory("WebToDatabaseServiceProviderTests")]
        public void UpdateServiceProviderContactTest()
        {
            using (ShimsContext.Create())
            {
                ShimServiceProviderRepo.AllInstances.UpdateServiceAreaInt32ListOfInt32 = (repo, i, arg3) => true;
                ShimServiceProviderRepo.AllInstances.UpdateContactPersonServiceProviderContactPerson =
                    (repo, person) => true;
                ShimServiceProviderRepo.AllInstances.UpdateContactContact = (repo, contact1) => true;
                ShimServiceProviderRepo.AllInstances.UpdateCoverageInt32Coverage = (repo, i, arg3) => true;
                ShimServiceProviderRepo.AllInstances.UpdateProviderWebsiteServiceProvider = (repo, provider) => true;
                ShimServiceProviderRepo.AllInstances.UpdateLocationServiceProviderLocation = (repo, location) => true;

                ShimServiceProviderRepo.AllInstances.DeleteContactContact = (repo, contact1) => true;
                ShimServiceProviderRepo.AllInstances.DeleteContactPersonServiceProviderContactPerson =
                    (repo, person) => true;
                ShimServiceProviderRepo.AllInstances.UpdateContactContact =
                    (repo, providerContact) => true;
                ShimServiceProviderRepo.AllInstances.DeleteAllCoveragesForLocationInt32 = (repo, i) => true;
                ShimServiceProviderRepo.AllInstances.DeleteLocationInt32 = (repo, i) => true;
                ShimServiceProviderRepo.AllInstances.DeleteProviderServiceInt32 = (repo, i) => true;
                ShimServiceProviderRepo.AllInstances.DeleteProviderWebsiteServiceProvider = (repo, provider) => true;

                ShimServiceProviderRepo.AllInstances.CreateServiceProviderServiceProvider = (repo, provider) => 1;
                ShimServiceProviderRepo.AllInstances.CreateContactContact = (repo, contact1) => 1;
                ShimServiceProviderRepo.AllInstances.CreateContactPersonContactPerson = (repo, person) => 1;
                ShimServiceProviderRepo.AllInstances.CreateCoverageProviderCoverage = (repo, coverage) => true;
                ShimServiceProviderRepo.AllInstances.CreateLocationLocation = (repo, location) => 1;
                ShimServiceProviderRepo.AllInstances.CreateServiceAreaProviderService = (repo, service) => true;

                ShimServiceProviderRepo.AllInstances.LogServiceProviderEditServiceProviderEdit = (repo, edit) => true;

                this.websiteServiceProvider.State = ObjectStatus.ObjectState.Update;
                this.location1.ContactPerson = null;

                this.contact.State = ObjectStatus.ObjectState.Read;
                var response = this.target.UpdateServiceProvider(this.websiteServiceProvider, 1);

                Assert.IsTrue(response);

                this.contact.State = ObjectStatus.ObjectState.Update;
                response = this.target.UpdateServiceProvider(this.websiteServiceProvider, 1);

                Assert.IsTrue(response);

                this.contact.State = ObjectStatus.ObjectState.Delete;
                response = this.target.UpdateServiceProvider(this.websiteServiceProvider, 1);

                Assert.IsTrue(response);

                this.contact.State = ObjectStatus.ObjectState.Create;
                response = this.target.UpdateServiceProvider(this.websiteServiceProvider, 1);

                Assert.IsTrue(response);
            }
        }

        /// <summary>
        /// The update service provider coverage test.
        /// </summary>
        [TestMethod]
        [TestCategory("WebToDatabaseServiceProviderTests")]
        public void UpdateServiceProviderCoverageTest()
        {
            using (ShimsContext.Create())
            {
                ShimServiceProviderRepo.AllInstances.UpdateServiceAreaInt32ListOfInt32 = (repo, i, arg3) => true;
                ShimServiceProviderRepo.AllInstances.UpdateContactPersonServiceProviderContactPerson =
                    (repo, person) => true;
                ShimServiceProviderRepo.AllInstances.UpdateContactContact = (repo, contact1) => true;
                ShimServiceProviderRepo.AllInstances.UpdateCoverageInt32Coverage = (repo, i, arg3) => true;
                ShimServiceProviderRepo.AllInstances.UpdateProviderWebsiteServiceProvider = (repo, provider) => true;
                ShimServiceProviderRepo.AllInstances.UpdateLocationServiceProviderLocation = (repo, location) => true;

                ShimServiceProviderRepo.AllInstances.DeleteContactPersonServiceProviderContactPerson =
                    (repo, person) => true;
                ShimServiceProviderRepo.AllInstances.UpdateContactContact =
                    (repo, providerContact) => true;
                ShimServiceProviderRepo.AllInstances.DeleteAllCoveragesForLocationInt32 = (repo, i) => true;
                ShimServiceProviderRepo.AllInstances.DeleteLocationInt32 = (repo, i) => true;
                ShimServiceProviderRepo.AllInstances.DeleteProviderServiceInt32 = (repo, i) => true;
                ShimServiceProviderRepo.AllInstances.DeleteProviderWebsiteServiceProvider = (repo, provider) => true;

                ShimServiceProviderRepo.AllInstances.CreateServiceProviderServiceProvider = (repo, provider) => 1;
                ShimServiceProviderRepo.AllInstances.CreateContactContact = (repo, contact1) => 1;
                ShimServiceProviderRepo.AllInstances.CreateContactPersonContactPerson = (repo, person) => 1;
                ShimServiceProviderRepo.AllInstances.CreateCoverageProviderCoverage = (repo, coverage) => true;
                ShimServiceProviderRepo.AllInstances.CreateLocationLocation = (repo, location) => 1;
                ShimServiceProviderRepo.AllInstances.CreateServiceAreaProviderService = (repo, service) => true;

                ShimServiceProviderRepo.AllInstances.LogServiceProviderEditServiceProviderEdit = (repo, edit) => true;

                this.websiteServiceProvider.State = ObjectStatus.ObjectState.Update;
                this.location1.ContactPerson = null;

                this.coverage1.State = ObjectStatus.ObjectState.Update;
                this.coverage2.State = ObjectStatus.ObjectState.Update;
                var response = this.target.UpdateServiceProvider(this.websiteServiceProvider, 1);

                Assert.IsTrue(response);

                this.coverage1.State = ObjectStatus.ObjectState.Read;
                this.coverage2.State = ObjectStatus.ObjectState.Read;
                response = this.target.UpdateServiceProvider(this.websiteServiceProvider, 1);

                Assert.IsTrue(response);

                this.coverage1.State = ObjectStatus.ObjectState.Delete;
                this.coverage2.State = ObjectStatus.ObjectState.Delete;
                response = this.target.UpdateServiceProvider(this.websiteServiceProvider, 1);

                Assert.IsTrue(response);

                this.coverage1.State = ObjectStatus.ObjectState.Create;
                this.coverage2.State = ObjectStatus.ObjectState.Create;
                response = this.target.UpdateServiceProvider(this.websiteServiceProvider, 1);

                Assert.IsTrue(response);

                this.coverage1.State = ObjectStatus.ObjectState.Test;
                this.coverage2.State = ObjectStatus.ObjectState.Test;
                response = this.target.UpdateServiceProvider(this.websiteServiceProvider, 1);

                Assert.IsFalse(response);
            }
        }
    }
}
