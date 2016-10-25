// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DisplayProviderCreatorTests.cs" company="UDRI">
//   Copyright © 2015 University of Dayton Research Institute. All rights reserved.
// </copyright>
// <summary>
//   The DisplayProviderCreator Tests
// </summary>
// --------------------------------------------------------------------------------------------------------------------


namespace DataEntry.Tests.BL
{
    using System.Collections.Generic;
    using DataEntry_Helpers;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Website.BL;

    /// <summary>
    /// Tests for the Display Provider Creator Test
    /// </summary>
    [TestClass]
    public class DisplayProviderCreatorTests
    {
        /// <summary>
        /// Target of the tests
        /// </summary>
        private DisplayProviderCreator target;

        /// <summary>
        /// Provider list to use as shim data
        /// </summary>
        private List<ServiceProvider> providerList;

        /// <summary>
        /// The initializer for the tests
        /// </summary>
        [TestInitialize]
        public void TestInitialize()
        {
            target = new DisplayProviderCreator();

            this.providerList = new List<ServiceProvider>
            {
                new ServiceProvider {
                    Active =  true,
                    ProviderName = "Provider Name",
                    Locations = new List<Location>
                    {
                        new Location
                        {
                            Name = "Location 1",
                            City = "City",
                            State = new State { Abbreviation = "OH"},
                            Zip = "45645",
                            Contact = new Contact { Email = "asdfsa@test.com", ID = 1, Phone = "555555555"},
                            Display = true,
                            ProviderCoverages = new List<ProviderCoverage>
                            {
                                new ProviderCoverage { AreaID = 1 },
                                new ProviderCoverage { AreaID = 2}
                            }
                        },
                        new Location
                        {
                            Name = "Location 2",
                            City = "City2",
                            State = new State { Abbreviation = "OH"},
                            Zip = "45645",
                            Contact = new Contact { Email = "asdfsa@test.com", ID = 1, Phone = "555555555"},
                            Display = false,
                             ProviderCoverages = new List<ProviderCoverage>
                            {
                                new ProviderCoverage { AreaID = 1 },
                                new ProviderCoverage { AreaID = 2}
                            }
                        }
                    },
                    Description = "Service Provider 1",
                    ProviderServices = new List<ProviderService>
                    {
                        new ProviderService { ProviderServiceCategory = new ProviderServiceCategory {Description = "asdfasdf"} }
                    }
                }
            };
        }

        /// <summary>
        /// The test for an inactive location.  
        /// </summary>
        [TestMethod]
        public void CreateDisplayProviderLocationInactiveTest()
        {
            var countyIds = new List<int> { 1 };
            var result = target.CreateDisplayProvider(this.providerList, countyIds);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("Provider Name", result[0].Name);
            Assert.AreEqual("Location 1", result[0].LocationName);
        }
    }
}
