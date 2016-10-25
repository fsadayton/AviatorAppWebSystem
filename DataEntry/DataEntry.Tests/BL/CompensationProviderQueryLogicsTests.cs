// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CompensationProviderQueryLogicsTests.cs" company="UDRI">
//   Copyright © 2015 University of Dayton Research Institute. All rights reserved.
// </copyright>
// <summary>
//   The date logic tests.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DataEntry.Tests.BL
{
    using System.Collections.Generic;
    using System.Linq;

    using DataEntry_Helpers;

    using DataEntry_Helpers.Repositories.Fakes;
    using Microsoft.QualityTools.Testing.Fakes;
    using Microsoft.VisualStudio.TestTools.UnitTesting;


    using Models;

    using Website.BL;

    /// <summary>
    /// The date logic tests.
    /// </summary>
    [TestClass]
    public class CompensationProviderQueryLogicsTests
    {
        /// <summary>
        /// The target.
        /// </summary>
        private CompensationProviderQueryLogics target;

        private Contact contact;

        private List<Location> locations;

        private Location location1;

        private Location location2;

        private ServiceProvider serviceProvider1;

        private ServiceProvider serviceProvider2;

        private List<ServiceProvider> serviceProviderList;

        private List<ProviderService> providerServices1;

        private List<ProviderService> providerServices2;

        private State state;

        private string websiteAddress1;

        private string websiteAddress2;

        #region Additional test attributes

        // You can use the following additional attributes as you write your tests:
        // Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //    }
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
        [TestCategory("DataLogics")]
        public void MyTestInitialize()
        {
            this.target = new CompensationProviderQueryLogics();

            this.contact = new Contact
                               {
                                   Email = "Test@test.org",
                                   ID = 1,
                                   Website = "www.test.org",
                                   Phone = "9373608284",
                                   HelpLine = "9373608888"
                               };
            this.state = new State { Abbreviation = "OH", CountryID = 1, FullName = "Ohio", ID = 1 };

            this.location1 = new Location
                                 {
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
                                     ProviderCoverages = new List<ProviderCoverage> { new ProviderCoverage { AreaID = 1, LocationID = 1 } }
            };
            this.location2 = new Location
                                 {
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
                                     ProviderCoverages = new List<ProviderCoverage> { new ProviderCoverage { AreaID = 2, LocationID = 2 } }
            };

            this.locations = new List<Location> { this.location1, this.location2 };

            this.websiteAddress1 = string.Format(
                "{0}, {1}, {2}, {3}",
                this.location1.Street,
                this.location1.City,
                this.location1.State.Abbreviation,
                this.location1.Zip);

            this.websiteAddress2 = string.Format(
                "{0}, {1}, {2}, {3}",
                this.location2.Street,
                this.location2.City,
                this.location2.State.Abbreviation,
                this.location2.Zip);

            var providerService11 = new ProviderService { ID = 1, ProviderID = 1, ServiceID = 1 };
            var providerService12 = new ProviderService { ID = 2, ProviderID = 1, ServiceID = 2 };
            var providerService13 = new ProviderService { ID = 3, ProviderID = 1, ServiceID = 3 };

            var providerService21 = new ProviderService { ID = 1, ProviderID = 2, ServiceID = 1 };
            var providerService22 = new ProviderService { ID = 2, ProviderID = 2, ServiceID = 2 };
            var providerService23 = new ProviderService { ID = 3, ProviderID = 2, ServiceID = 3 };

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
                                            ServiceTypes = 4,
                                            DisplayRank = 1,
                                            ProviderName = "Test 1",
                                            Locations = this.locations,
                                            ProviderServices = this.providerServices1
                                        };
            this.serviceProvider2 = new ServiceProvider
                                        {
                                            Description = "Test description for test 2",
                                            ID = 2,
                                            ServiceTypes = 4,
                                            DisplayRank = 2,
                                            ProviderName = "Test 2",
                                            Locations = this.locations,
                                            ProviderServices = this.providerServices2
                                        };

            this.serviceProviderList = new List<ServiceProvider> { this.serviceProvider1, this.serviceProvider2 };
        }

        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup()
        // {
        // }
        #endregion

        /// <summary>
        /// The Compensation provider query test.
        /// </summary>
        [TestMethod]
        [TestCategory("CompensationProviderQueryLogicsTests")]
        public void CompensationProviderQueryTest()
        {
            using (ShimsContext.Create())
            {
                ShimCompensationRepo.AllInstances.GetServiceProvidersListOfInt32 = (repo, ints) => this.serviceProviderList;

                var response = this.target.ServiceProviderQuery(new List<int> { 1, 2 });

                Assert.IsNotNull(response);
                Assert.AreEqual(4, response.Count);


                Assert.AreEqual(this.websiteAddress1, response[0].Address);
                Assert.AreEqual(this.serviceProviderList[0].ProviderServices.Count, response[0].Categories.Count);

                Assert.AreEqual(
                    this.serviceProviderList[0].Locations.First().Contact.HelpLine,
                    response[0].CrisisNumber);
                Assert.AreEqual(
                    this.serviceProviderList[0].Locations.First().Contact.Email,
                    response[0].Email);
                Assert.AreEqual(
                    this.serviceProviderList[0].Locations.First().Contact.Phone,
                    response[0].PhoneNumber);
                Assert.AreEqual(
                    this.serviceProviderList[0].Locations.First().Contact.Website,
                    response[0].Website);

                Assert.AreEqual(this.serviceProviderList[0].Description, response[0].Description);
                Assert.AreEqual(this.serviceProviderList[0].DisplayRank, response[0].DisplayRank);
                Assert.AreEqual(this.serviceProviderList[0].Description, response[0].Description);
                Assert.AreEqual(this.serviceProviderList[0].ProviderName, response[0].Name);

                Assert.AreEqual(this.websiteAddress2, response[1].Address);
                Assert.AreEqual(this.serviceProviderList[0].ProviderServices.Count, response[1].Categories.Count);

                Assert.AreEqual(
                    this.serviceProviderList[0].Locations.Last().Contact.HelpLine,
                    response[1].CrisisNumber);
                Assert.AreEqual(
                    this.serviceProviderList[0].Locations.Last().Contact.Email,
                    response[1].Email);
                Assert.AreEqual(
                    this.serviceProviderList[0].Locations.Last().Contact.Phone,
                    response[1].PhoneNumber);
                Assert.AreEqual(
                    this.serviceProviderList[0].Locations.Last().Contact.Website,
                    response[1].Website);

                Assert.AreEqual(this.serviceProviderList[0].Description, response[1].Description);
                Assert.AreEqual(this.serviceProviderList[0].DisplayRank, response[1].DisplayRank);
                Assert.AreEqual(this.serviceProviderList[0].Description, response[1].Description);
                Assert.AreEqual(this.serviceProviderList[0].ProviderName, response[1].Name);

                Assert.AreEqual(this.websiteAddress1, response[2].Address);
                Assert.AreEqual(this.serviceProviderList[1].ProviderServices.Count, response[2].Categories.Count);

                Assert.AreEqual(
                    this.serviceProviderList[1].Locations.First().Contact.HelpLine,
                   response[2].CrisisNumber);
                Assert.AreEqual(
                    this.serviceProviderList[1].Locations.First().Contact.Email,
                   response[2].Email);
                Assert.AreEqual(
                    this.serviceProviderList[1].Locations.First().Contact.Phone,
                   response[2].PhoneNumber);
                Assert.AreEqual(
                    this.serviceProviderList[1].Locations.First().Contact.Website,
                   response[2].Website);

                Assert.AreEqual(this.serviceProviderList[1].Description, response[2].Description);
                Assert.AreEqual(this.serviceProviderList[1].DisplayRank, response[2].DisplayRank);
                Assert.AreEqual(this.serviceProviderList[1].Description, response[2].Description);
                Assert.AreEqual(this.serviceProviderList[1].ProviderName, response[2].Name);

                Assert.AreEqual(this.websiteAddress2, response[3].Address);
                Assert.AreEqual(this.serviceProviderList[1].ProviderServices.Count, response[3].Categories.Count);

                Assert.AreEqual(
                    this.serviceProviderList[1].Locations.Last().Contact.HelpLine,
                   response[3].CrisisNumber);
                Assert.AreEqual(
                    this.serviceProviderList[1].Locations.Last().Contact.Email,
                   response[3].Email);
                Assert.AreEqual(
                    this.serviceProviderList[1].Locations.Last().Contact.Phone,
                   response[3].PhoneNumber);
                Assert.AreEqual(
                    this.serviceProviderList[1].Locations.Last().Contact.Website,
                   response[3].Website);

                Assert.AreEqual(this.serviceProviderList[1].Description, response[3].Description);
                Assert.AreEqual(this.serviceProviderList[1].DisplayRank, response[3].DisplayRank);
                Assert.AreEqual(this.serviceProviderList[1].Description, response[3].Description);
                Assert.AreEqual(this.serviceProviderList[1].ProviderName, response[3].Name);
            }
        }
    }
}
