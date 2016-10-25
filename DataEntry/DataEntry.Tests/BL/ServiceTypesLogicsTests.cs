// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ServiceTypesLogicsTests.cs" company="UDRI">
//   Copyright © 2015 University of Dayton Research Institute. All rights reserved.
// </copyright>
// <summary>
//   The date logic tests.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DataEntry.Tests.BL
{
    using System.Collections.Generic;
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
    public class ServiceTypesLogicsTests
    {
        /// <summary>
        /// The target.
        /// </summary>
        private ServiceTypesLogics target;

        private ProviderServiceCategory providerServiceCategory1;

        private ProviderServiceCategory providerServiceCategory2;

        private List<ProviderServiceCategory> providerServiceCategoryList;

        private Category categoryTest;

        private ServiceType serviceType1;

        private ServiceType serviceType2;

        private List<ServiceType> serviceTypeList;

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
        public void MyTestInitialize()
        {
            this.target = new ServiceTypesLogics();

            this.providerServiceCategory1 = new ProviderServiceCategory
                                               {
                                                   Active = true,
                                                   Crime = true,
                                                   Description = "Service Description",
                                                   ID = 1,
                                                   Name = "Test 1"
                                               };
            this.providerServiceCategory2 = new ProviderServiceCategory
                                                {
                                                    Active = true,
                                                    Crime = true,
                                                    Description = "Service Description",
                                                    ID = 2,
                                                    Name = "Test 2"
                                                };
            this.providerServiceCategoryList = new List<ProviderServiceCategory>
                                                   {
                                                       this.providerServiceCategory1,
                                                       this.providerServiceCategory2
                                                   };
            this.categoryTest = new Category { Description = "Service Description", Name = "Test 1", Id = 1 };
            this.serviceType1 = new ServiceType { Description = "Test Type 1", ID = 1, Name = "Type 1" };
            this.serviceType2 = new ServiceType { Description = "Test Type 2", ID = 2, Name = "Type 2" };

            this.serviceTypeList = new List<ServiceType> { this.serviceType1, this.serviceType2 };
        }

        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup()
        // {
        // }
        #endregion

        [TestMethod]
        [TestCategory("ServiceTypesLogicsTests")]
        public void CreateNewCategoryTest()
        {
            using (ShimsContext.Create())
            {
                ShimServiceTypesRepo.AllInstances.CreateNewCategoryCategory = (repo, category) => this.providerServiceCategory1;
                var response = this.target.CreateNewCategory(this.categoryTest);

                Assert.IsNotNull(response);
                Assert.AreEqual(this.providerServiceCategory1.ID, response.Id);
                Assert.AreEqual(this.providerServiceCategory1.Name, response.Name);
                Assert.AreEqual(this.providerServiceCategory1.Description, response.Description);
            }
        }

        /// <summary>
        /// The convert database category test.
        /// </summary>
        [TestMethod]
        [TestCategory("ServiceTypesLogicsTests")]
        public void ConvertDatabaseCategoryTest()
        {
            var response = this.target.ConvertDatabaseCategory(this.providerServiceCategory1);

            Assert.IsNotNull(response);
            Assert.AreEqual(this.providerServiceCategory1.ID, response.Id);
            Assert.AreEqual(this.providerServiceCategory1.Name, response.Name);
            Assert.AreEqual(this.providerServiceCategory1.Description, response.Description);
        }

        [TestMethod]
        [TestCategory("ServiceTypesLogicsTests")]
        public void GetCategoriesTest()
        {
            using (ShimsContext.Create())
            {
                ShimDataAccess.AllInstances.GetCategories = access => this.providerServiceCategoryList;

                var response = this.target.GetCategories();

                Assert.IsNotNull(response);
                Assert.AreEqual(this.providerServiceCategoryList.Count, response.Count);

                Assert.AreEqual(this.providerServiceCategoryList[0].Crime, response[0].Crime);
                Assert.AreEqual(this.providerServiceCategoryList[0].Description, response[0].Description);
                Assert.AreEqual(this.providerServiceCategoryList[0].ID, response[0].Id);
                Assert.AreEqual(this.providerServiceCategoryList[0].Name, response[0].Name);

                Assert.AreEqual(this.providerServiceCategoryList[1].Crime, response[1].Crime);
                Assert.AreEqual(this.providerServiceCategoryList[1].Description, response[1].Description);
                Assert.AreEqual(this.providerServiceCategoryList[1].ID, response[1].Id);
                Assert.AreEqual(this.providerServiceCategoryList[1].Name, response[1].Name);
            }
        }

        /// <summary>
        /// The convert database category test.
        /// </summary>
        [TestMethod]
        [TestCategory("ServiceTypesLogicsTests")]
        public void ConvertDatabaseCategoriesTest()
        {
            var response = this.target.ConvertDatabaseCategories(this.providerServiceCategoryList);

            Assert.IsNotNull(response);
            Assert.AreEqual(this.providerServiceCategoryList.Count, response.Count);

            Assert.AreEqual(this.providerServiceCategoryList[0].Crime, response[0].Crime);
            Assert.AreEqual(this.providerServiceCategoryList[0].Description, response[0].Description);
            Assert.AreEqual(this.providerServiceCategoryList[0].ID, response[0].Id);
            Assert.AreEqual(this.providerServiceCategoryList[0].Name, response[0].Name);

            Assert.AreEqual(this.providerServiceCategoryList[1].Crime, response[1].Crime);
            Assert.AreEqual(this.providerServiceCategoryList[1].Description, response[1].Description);
            Assert.AreEqual(this.providerServiceCategoryList[1].ID, response[1].Id);
            Assert.AreEqual(this.providerServiceCategoryList[1].Name, response[1].Name);
        }

        [TestMethod]
        [TestCategory("ServiceTypesLogicsTests")]
        public void GetServiceTypesTest()
        {
            using (ShimsContext.Create())
            {
                ShimServiceTypesRepo.AllInstances.GetServiceTypes = repo => this.serviceTypeList;

                var response = this.target.GetServiceTypes();

                Assert.IsNotNull(response);
                Assert.AreEqual(this.serviceTypeList.Count, response.Count);

                Assert.IsTrue(response.ContainsKey(this.serviceTypeList[0].ID));
                Assert.AreEqual(this.serviceTypeList[0].Name, response[this.serviceTypeList[0].ID]);

                Assert.IsTrue(response.ContainsKey(this.serviceTypeList[1].ID));
                Assert.AreEqual(this.serviceTypeList[1].Name, response[this.serviceTypeList[1].ID]);
            }
        }

        [TestMethod]
        [TestCategory("ServiceTypesLogicsTests")]
        public void ConvertDatabaseTypesTests()
        {
            var response = this.target.ConvertDatabaseTypes(this.serviceTypeList);

            Assert.IsNotNull(response);
            Assert.AreEqual(this.serviceTypeList.Count, response.Count);

            Assert.IsTrue(response.ContainsKey(this.serviceTypeList[0].ID));
            Assert.AreEqual(this.serviceTypeList[0].Name, response[this.serviceTypeList[0].ID]);

            Assert.IsTrue(response.ContainsKey(this.serviceTypeList[1].ID));
            Assert.AreEqual(this.serviceTypeList[1].Name, response[this.serviceTypeList[1].ID]);
        }
    }
}
