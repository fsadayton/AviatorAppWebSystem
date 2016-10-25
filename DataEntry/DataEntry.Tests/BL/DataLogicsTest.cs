// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataLogicsTest.cs" company="UDRI">
//   Copyright © 2015 University of Dayton Research Institute. All rights reserved.
// </copyright>
// <summary>
//   The date logic tests.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using Models.ServiceProvider;

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
    public class DateLogicTests
    {
        /// <summary>
        /// The target.
        /// </summary>
        private DataLogics target;

        /// <summary>
        /// The website countries list.
        /// </summary>
        private WebsiteCountry websiteCountry;

        /// <summary>
        /// The website state.
        /// </summary>
        private WebsiteState websiteState;

        private ProviderServiceCategory websiteCategory;

        private WebsiteCounty websiteCounty;

        #region Additional test attributes

        // You can use the following additional attributes as you write your tests:
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
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
        [TestCategory("DataLogics")]
        public void MyTestInitialize()
        {
            this.target = new DataLogics();

            this.websiteCountry = new WebsiteCountry { Abbreviation = "OH", FullName = "Ohio", Id = 1 };
            this.websiteState = new WebsiteState { Id = 1, Name = "OH" };
            this.websiteCounty = new WebsiteCounty { Id = 1, Name = "test" };
            this.websiteCategory = new ProviderServiceCategory
                                       {
                                           Active = true,
                                           Name = "Test",
                                           ID = 1,
                                           Description = "this is a test",
                                           Crime = false
                                       };

        }

        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup()
        // {
        // }
        #endregion

        /// <summary>
        /// The create child day test.
        /// </summary>
        [TestMethod]
        [TestCategory("DataLogics")]
        public void GetCountriesTest()
        {
            using (ShimsContext.Create())
            {
                ShimDataAccess.AllInstances.GetCountriesList =
                    access => new List<Country> { new Country { Abbreviation = "OH", FullName = "Ohio", ID = 1 } };

                var response = this.target.GetCountries();

                Assert.AreEqual(1, response.Count);
                Assert.AreEqual(this.websiteCountry.Abbreviation, response.First().Abbreviation);
                Assert.AreEqual(this.websiteCountry.FullName, response.First().FullName);
                Assert.AreEqual(this.websiteCountry.Id, response.First().Id);
            }
        }

        /// <summary>
        /// The get counties test.
        /// </summary>
        [TestMethod]
        [TestCategory("DataLogics")]
        public void GetCountiesTest()
        {
            using (ShimsContext.Create())
            {
                ShimDataAccess.AllInstances.GetCountiesListInt32 =
                    (access, i) => new List<County> { new County { ID = 1, Name = "test", StateId = 34 } };

                var response = this.target.GetCounties();

                Assert.AreEqual(1, response.Count);
                Assert.AreEqual(this.websiteCounty.Id, response.First().Id);
                Assert.AreEqual(this.websiteCounty.Name, response.First().Name);
            }
        }

        /// <summary>
        /// The get states test.
        /// </summary>
        [TestMethod]
        [TestCategory("DataLogics")]
        public void GetStatesTest()
        {
            using (ShimsContext.Create())
            {
                ShimDataAccess.AllInstances.GetStateListInt32 =
                    (access, i) => new List<State> { new State { Abbreviation = "OH", ID = 1, FullName = "Ohio" } };

                var response = this.target.GetStates(1);

                Assert.AreEqual(1, response.Count);
                Assert.AreEqual(this.websiteState.Id, response.First().Id);
                Assert.AreEqual(this.websiteState.Name, response.First().Name);
            }
        }

        /// <summary>
        /// The get categories test.
        /// </summary>
        [TestMethod]
        [TestCategory("DataLogics")]
        public void GetCategoriesTest()
        {
            using (ShimsContext.Create())
            {
                ShimDataAccess.AllInstances.GetCategories =
                    access =>
                    new List<ProviderServiceCategory>
                        {
                            new ProviderServiceCategory
                                {
                                    Name = "Test",
                                    ID = 1,
                                    Description = "this is a test",
                                    Crime = false
                                }
                        };

                var response = this.target.GetCategories();

                Assert.AreEqual(1, response.Count);
                Assert.AreEqual(this.websiteCategory.Name, response.First().Name);
                Assert.AreEqual(this.websiteCategory.Crime, response.First().Crime);
                Assert.AreEqual(this.websiteCategory.Description, response.First().Description);
                Assert.AreEqual(this.websiteCategory.ID, response.First().Id);
            }
        }

        /// <summary>
        /// The get categories test.
        /// </summary>
        [TestMethod]
        [TestCategory("DataLogics")]
        public void GetCrimeCategoriesTest()
        {
            using (ShimsContext.Create())
            {
                ShimDataAccess.AllInstances.GetCrimeCategories =
                    access =>
                    new List<ProviderServiceCategory>
                        {
                            new ProviderServiceCategory
                                {
                                    Name = "Test",
                                    ID = 1,
                                    Description = "this is a test",
                                    Crime = false
                                }
                        };

                var response = this.target.GetCrimeCategories();

                Assert.AreEqual(1, response.Count);
                Assert.AreEqual(this.websiteCategory.Name, response.First().Name);
                Assert.AreEqual(this.websiteCategory.Crime, response.First().Crime);
                Assert.AreEqual(this.websiteCategory.Description, response.First().Description);
                Assert.AreEqual(this.websiteCategory.ID, response.First().Id);
            }
        }
        
        /// <summary>
        /// The convert web counties test.
        /// </summary>
        [TestMethod]
        [TestCategory("DataLogics")]
        public void ConvertWebCountiesTest()
        {
            var webCounties = new List<WebsiteCounty>
                                  {
                                      new WebsiteCounty { Id = 1, Name = "test1" },
                                      new WebsiteCounty { Id = 2, Name = "test2" }
                                  };

            var response = this.target.ConvertWebCounties(webCounties);
            Assert.AreEqual(webCounties.Count, response.Count);
            Assert.IsTrue(response.ContainsKey(webCounties[0].Id));
            Assert.AreEqual(webCounties[0].Name, response[1]);
            Assert.IsTrue(response.ContainsKey(webCounties[1].Id));
            Assert.AreEqual(webCounties[1].Name, response[2]);
        }

        /// <summary>
        /// The convert database countries test.
        /// </summary>
        [TestMethod]
        [TestCategory("DataLogics")]
        public void ConvertDatabaseCountriesTest()
        {
            var databaseCountries = new List<Country>
                                        {
                                            new Country { Abbreviation = "T1", ID = 1, FullName = "Test 1" },
                                            new Country { Abbreviation = "T2", ID = 2, FullName = "Test 2" },
                                            new Country { Abbreviation = "T3", ID = 3, FullName = "Test 3" }
                                        };

            var response = this.target.ConvertDatabaseCountries(databaseCountries);

            Assert.IsNotNull(response);
            Assert.AreEqual(databaseCountries.Count, response.Count);

            Assert.AreEqual(databaseCountries[0].Abbreviation, response[0].Abbreviation);
            Assert.AreEqual(databaseCountries[0].FullName, response[0].FullName);
            Assert.AreEqual(databaseCountries[0].ID, response[0].Id);

            Assert.AreEqual(databaseCountries[1].Abbreviation, response[1].Abbreviation);
            Assert.AreEqual(databaseCountries[1].FullName, response[1].FullName);
            Assert.AreEqual(databaseCountries[1].ID, response[1].Id);

            Assert.AreEqual(databaseCountries[2].Abbreviation, response[2].Abbreviation);
            Assert.AreEqual(databaseCountries[2].FullName, response[2].FullName);
            Assert.AreEqual(databaseCountries[2].ID, response[2].Id);
        }

        /// <summary>
        /// The convert database counties test.
        /// </summary>
        [TestMethod]
        [TestCategory("DataLogics")]
        public void ConvertDatabaseCountiesTest()
        {
            var databaseCounties = new List<County>
                                       {
                                           new County { ID = 1, Name = "Test 1" },
                                           new County { ID = 2, Name = "Test 2" },
                                           new County { ID = 3, Name = "Test 3" }
                                       };

            var response = this.target.ConvertDatabaseCounties(databaseCounties);

            Assert.IsNotNull(response);
            Assert.AreEqual(databaseCounties.Count, response.Count);

            Assert.IsTrue(response.Exists(w => w.Id == databaseCounties[0].ID));
            Assert.AreEqual(databaseCounties[0].Name, response[0].Name);
            Assert.AreEqual(databaseCounties[0].ID, response[0].Id);

            Assert.IsTrue(response.Exists(w => w.Id == databaseCounties[1].ID));
            Assert.AreEqual(databaseCounties[1].Name, response[1].Name);
            Assert.AreEqual(databaseCounties[1].ID, response[1].Id);

            Assert.IsTrue(response.Exists(w => w.Id == databaseCounties[2].ID));
            Assert.AreEqual(databaseCounties[2].Name, response[2].Name);
            Assert.AreEqual(databaseCounties[2].ID, response[2].Id);
        }

        /// <summary>
        /// The convert database categories test.
        /// </summary>
        [TestMethod]
        [TestCategory("DataLogics")]
        public void ConvertDatabaseCategoriesTest()
        {
            var databaseCategories = new List<ProviderServiceCategory>
                                         {
                                             new ProviderServiceCategory
                                                 {
                                                     ID = 1,
                                                     Name = "test 1",
                                                     Description =
                                                         "Test description for test 1",
                                                     Crime = true
                                                 },
                                             new ProviderServiceCategory
                                                 {
                                                     ID = 2,
                                                     Name = "test 2",
                                                     Description =
                                                         "Test description for test 2",
                                                     Crime = false
                                                 },
                                             new ProviderServiceCategory
                                                 {
                                                     ID = 3,
                                                     Name = "test 3",
                                                     Description =
                                                         "Test description for test 3",
                                                     Crime = true
                                                 }
                                         };

            var response = this.target.ConvertDatabaseCategories(databaseCategories);

            Assert.IsNotNull(response);
            Assert.AreEqual(databaseCategories.Count, response.Count);

            Assert.IsTrue(response.Exists(w => w.Id == databaseCategories[0].ID));
            Assert.AreEqual(databaseCategories[0].Name, response[0].Name);
            Assert.AreEqual(databaseCategories[0].ID, response[0].Id);
            Assert.AreEqual(databaseCategories[0].Description, response[0].Description);
            Assert.AreEqual(databaseCategories[0].Crime, response[0].Crime);

            Assert.IsTrue(response.Exists(w => w.Id == databaseCategories[1].ID));
            Assert.AreEqual(databaseCategories[1].Name, response[1].Name);
            Assert.AreEqual(databaseCategories[1].ID, response[1].Id);
            Assert.AreEqual(databaseCategories[1].Description, response[1].Description);
            Assert.AreEqual(databaseCategories[1].Crime, response[1].Crime);

            Assert.IsTrue(response.Exists(w => w.Id == databaseCategories[2].ID));
            Assert.AreEqual(databaseCategories[2].Name, response[2].Name);
            Assert.AreEqual(databaseCategories[2].ID, response[2].Id);
            Assert.AreEqual(databaseCategories[2].Description, response[2].Description);
            Assert.AreEqual(databaseCategories[2].Crime, response[2].Crime);
        }

        /// <summary>
        /// The convert database states test.
        /// </summary>
        [TestMethod]
        [TestCategory("DataLogics")]
        public void ConvertDatabaseStatesTest()
        {
            var databaseStates = new List<State>
                                     {
                                         new State { ID = 1, Abbreviation = "T1" },
                                         new State { ID = 2, Abbreviation = "T2" },
                                         new State { ID = 3, Abbreviation = "T3" }
                                     };

            var response = this.target.ConvertDatabaseStates(databaseStates);

            Assert.IsNotNull(response);
            Assert.AreEqual(databaseStates.Count, response.Count);

            Assert.IsTrue(response.Exists(w => w.Id == databaseStates[0].ID));
            Assert.AreEqual(databaseStates[0].ID, response[0].Id);
            Assert.AreEqual(databaseStates[0].Abbreviation, response[0].Name);

            Assert.IsTrue(response.Exists(w => w.Id == databaseStates[1].ID));
            Assert.AreEqual(databaseStates[1].Abbreviation, response[1].Name);
            Assert.AreEqual(databaseStates[1].ID, response[1].Id);

            Assert.IsTrue(response.Exists(w => w.Id == databaseStates[2].ID));
            Assert.AreEqual(databaseStates[2].Abbreviation, response[2].Name);
            Assert.AreEqual(databaseStates[2].ID, response[2].Id);
        }


        /// <summary>
        /// The get website categories test.
        /// </summary>
        [TestMethod]
        [TestCategory("DataLogics")]
        public void GetWebsiteCategoriesTest()
        {
            var categoryList = new List<ProviderServiceCategory>
            {
                new ProviderServiceCategory()
                {
                    ID = 1,
                    Name = "test Category",
                    Description = "Description",
                    CategoryTypes = new List<CategoryType>
                    {
                        new CategoryType { CategoryId = 1, ServiceTypeId = 1, ServiceType = new ServiceType { ID = 1, Name = "General"}}
                    }
                }
            };

            using (ShimsContext.Create())
            {
                ShimDataAccess.AllInstances.GetCategories = access => categoryList;
                var response = this.target.GetWebsiteCategories();
                Assert.AreEqual(1, response.Count);
                Assert.AreEqual("General", response[0].ServiceTypes[0].Name);
            }
        }

        /// <summary>
        /// Test for getting Webiste Categories for a family.
        /// </summary>
        [TestMethod]
        public void GetWebsiteCategoriesByFamilyTest()
        {
            var databaseCategory = new List<ProviderServiceCategory>
            {
                new ProviderServiceCategory
                {
                    Active = true,
                    Description = "Description",
                    Name = "Name",
                    ID = 1,
                    CategoryTypes =
                        new List<CategoryType>
                        {
                            new CategoryType
                            {
                                ID = 1,
                                ServiceType =  new ServiceType { Name = "General", ID = 1 }
                            }
                        }
                },
                new ProviderServiceCategory
                {
                    Active = true,
                    Description = "Description2",
                    Name = "Name 2",
                    ID = 2,
                    CategoryTypes =
                        new List<CategoryType>
                        {
                            new CategoryType
                            {
                                ID = 1,
                                ServiceType =  new ServiceType { Name = "General", ID = 1 }
                            }
                        }
                }
            };

            var categories = new List<WebsiteCategory>
            {
                new WebsiteCategory
                {
                    Crime = false,
                    Description = "Description",
                    Name = "Name",
                    Id = 1,
                    ServiceTypes = new List<WebsiteCategoryType> {new WebsiteCategoryType {Id = 1, Name = "General"}}
                },
                new WebsiteCategory
                {
                    Crime = false,
                    Description = "Description2",
                    Name = "Name 2",
                    Id = 2,
                    ServiceTypes = new List<WebsiteCategoryType> {new WebsiteCategoryType {Id = 1, Name = "General"}}
                }
            };

            using (ShimsContext.Create())
            {
                ShimDataAccess.AllInstances.GetCategoriesByFamilyInt32 = (access, i) => databaseCategory;
                var results = target.GetWebsiteCategories(1);
                Assert.AreEqual(2, results.Count);
                Assert.AreEqual(categories[0].Name ,results[0].Name);
            }
        }
    }
}
