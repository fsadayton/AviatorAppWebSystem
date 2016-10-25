// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CategoriesLogicTest.cs" company="UDRI">
//   Copyright © 2015 University of Dayton Research Institute. All rights reserved.
// </copyright>
// <summary>
//   The date logic tests.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Linq;
using Models.ServiceProvider;

namespace DataEntry.Tests.BL
{
    using System.Collections.Generic;

    using DataEntry_Helpers;
    using DataEntry_Helpers.Repositories.Fakes;

    using Microsoft.QualityTools.Testing.Fakes;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Models;
    using Models.Editors;

    using Website.BL;

    /// <summary>
    /// The date logic tests.
    /// </summary>
    [TestClass]
    public class CategoriesLogicTest
    {
        /// <summary>
        /// The target.
        /// </summary>
        private CategoriesLogic target;

        /// <summary>
        /// The website category.
        /// </summary>
        private CategoryEditor websiteCategory;

        private ProviderServiceCategory databaseCategory;

        private List<CategoryEditor> categoryList;

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
            this.target = new CategoriesLogic();

            this.websiteCategory = new CategoryEditor
                                       {
                                           Active = true,
                                           Crime = true,
                                           Description = "Test Cat",
                                           Id = 1,
                                           Name = "Edit Cat",
                                           CategoryTypes = new List<int> { 1 },
                                           State = ObjectStatus.ObjectState.Read
                                       };

            this.databaseCategory = new ProviderServiceCategory
                                        {
                                            Active = true,
                                            Crime = false,
                                            Description = "Test Cat",
                                            ID = 1,
                                            CategoryTypes = new List<CategoryType> {  new CategoryType
                                            {
                                                ID = 1,
                                                CategoryId = 1,
                                                ServiceTypeId = 1,
                                                ServiceType =  new ServiceType { ID = 1, Name = "General"}
                                            } },
                                            Name = "Edit Cat",
                                        };

            this.categoryList = new List<CategoryEditor> { this.websiteCategory };


        }

        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup()
        // {
        // }
        #endregion

        /// <summary>
        /// Get categories test.
        /// </summary>
        [TestMethod]
        [TestCategory("CategoriesLogicTests")]
        public void GetCategoriesTest()
        {
            var response = this.target.GetCategories();

            Assert.IsNotNull(response);
        }

        /// <summary>
        /// Get categories by name test.
        /// </summary>
        [TestMethod]
        [TestCategory("CategoriesLogicTests")]
        public void GetCategoriesByNameTest()
        {
            using (ShimsContext.Create())
            {
                ShimCategoryRepo.AllInstances.GetCategoryByNameString =
                    (repo, s) => new List<ProviderServiceCategory> { this.databaseCategory };

                ShimDataAccess.AllInstances.GetAllCategories =
                    access => new List<ProviderServiceCategory> { this.databaseCategory };

                var response = this.target.GetCategoriesByName("test");

                Assert.IsNotNull(response);
                Assert.AreEqual(1, response.Count);
                Assert.AreEqual("Test Cat", response[0].Description);
                Assert.AreEqual(1, response[0].Id);
                Assert.AreEqual("Edit Cat", response[0].Name);


                this.databaseCategory.Name = "2nd Test";

                response = this.target.GetCategoriesByName(null);

                Assert.IsNotNull(response);
                Assert.AreEqual(1, response.Count);
                Assert.AreEqual("Test Cat", response[0].Description);
                Assert.AreEqual(1, response[0].Id);
                Assert.AreEqual("2nd Test", response[0].Name);

                ShimCategoryRepo.AllInstances.GetCategoryByNameString =
                    (repo, s) => null;

                response = this.target.GetCategoriesByName("test 3");

                Assert.AreEqual(0, response.Count);

            }
        }

        /// <summary>
        /// The merge category test.
        /// </summary>
        [TestMethod]
        [TestCategory("CategoriesLogicTests")]
        public void MergeCategoryTest()
        {
            var response = this.target.MergeCategories(this.websiteCategory, this.databaseCategory);

            Assert.AreEqual(response.Active, this.websiteCategory.Active);
            Assert.AreEqual(response.Crime, this.websiteCategory.Crime);
            Assert.AreEqual(response.Description, this.websiteCategory.Description);
            Assert.AreEqual(response.ID, this.databaseCategory.ID);
            Assert.AreEqual(response.Name, this.websiteCategory.Name);
            Assert.AreEqual(response.CategoryTypes.ToList()[0].ServiceTypeId, this.websiteCategory.CategoryTypes[0]);
        }

        /// <summary>
        /// The update service provider areas update positive test.
        /// </summary>
        [TestMethod]
        [TestCategory("CategoriesLogicTests")]
        public void EditCategoriesUpdatePositiveTest()
        {
            using (ShimsContext.Create())
            {
                ShimCategoryRepo.AllInstances.UpdateCategoryProviderServiceCategory = (repo, category) => true;
                ShimCategoryRepo.AllInstances.GetCategoryByIdInt32 = (repo, i) => this.databaseCategory;

                this.websiteCategory.State = ObjectStatus.ObjectState.Update;

                var response = this.target.EditCategories(this.categoryList);

                Assert.IsTrue(response);
            }
        }

        /// <summary>
        /// The update service provider areas update negative test.
        /// </summary>
        [TestMethod]
        [TestCategory("CategoriesLogicTests")]
        public void EditCategoriesUpdateNegativeTest()
        {
            using (ShimsContext.Create())
            {
                ShimCategoryRepo.AllInstances.UpdateCategoryProviderServiceCategory = (repo, category) => false;
                ShimCategoryRepo.AllInstances.GetCategoryByIdInt32 = (repo, i) => this.databaseCategory;

                this.websiteCategory.State = ObjectStatus.ObjectState.Update;

                var response = this.target.EditCategories(this.categoryList);

                Assert.IsFalse(response);
            }
        }

        /// <summary>
        /// The edit categories update null test.
        /// </summary>
        [TestMethod]
        [TestCategory("CategoriesLogicTests")]
        public void EditCategoriesUpdateNullTest()
        {
            using (ShimsContext.Create())
            {
                ShimCategoryRepo.AllInstances.UpdateCategoryProviderServiceCategory = (repo, category) => false;
                ShimCategoryRepo.AllInstances.GetCategoryByIdInt32 = (repo, i) => null;

                this.websiteCategory.State = ObjectStatus.ObjectState.Update;

                var response = this.target.EditCategories(this.categoryList);

                Assert.IsFalse(response);
            }
        }

        /// <summary>
        /// The edit categories delete positive test.
        /// </summary>
        [TestMethod]
        [TestCategory("CategoriesLogicTests")]
        public void EditCategoriesDeletePositiveTest()
        {
            using (ShimsContext.Create())
            {
                ShimCategoryRepo.AllInstances.DeactivateCategoryInt32 = (repo, i) => true;

                this.websiteCategory.State = ObjectStatus.ObjectState.Delete;

                var response = this.target.EditCategories(this.categoryList);

                Assert.IsTrue(response);
            }
        }

        /// <summary>
        /// The edit categories delete negative test.
        /// </summary>
        [TestMethod]
        [TestCategory("CategoriesLogicTests")]
        public void EditCategoriesDeleteNegativeTest()
        {
            using (ShimsContext.Create())
            {
                ShimCategoryRepo.AllInstances.DeactivateCategoryInt32 = (repo, i) => false;

                this.websiteCategory.State = ObjectStatus.ObjectState.Delete;

                var response = this.target.EditCategories(this.categoryList);

                Assert.IsFalse(response);
            }
        }

        /// <summary>
        /// The edit categories create positive test.
        /// </summary>
        [TestMethod]
        [TestCategory("CategoriesLogicTests")]
        public void EditCategoriesCreatePositiveTest()
        {
            using (ShimsContext.Create())
            {
                ShimCategoryRepo.AllInstances.CreateCategoryProviderServiceCategory = (repo, category) => true;

                this.websiteCategory.State = ObjectStatus.ObjectState.Create;

                var response = this.target.EditCategories(this.categoryList);

                Assert.IsTrue(response);
            }
        }

        /// <summary>
        /// The edit categories create negative test.
        /// </summary>
        [TestMethod]
        [TestCategory("CategoriesLogicTests")]
        public void EditCategoriesCreateNegativeTest()
        {
            using (ShimsContext.Create())
            {
                ShimCategoryRepo.AllInstances.CreateCategoryProviderServiceCategory = (repo, category) => false;

                this.websiteCategory.State = ObjectStatus.ObjectState.Create;

                var response = this.target.EditCategories(this.categoryList);

                Assert.IsFalse(response);
            }
        }

        /// <summary>
        /// The edit categories read positive test.
        /// </summary>
        [TestMethod]
        [TestCategory("CategoriesLogicTests")]
        public void EditCategoriesReadPositiveTest()
        {
            using (ShimsContext.Create())
            {
                ShimCategoryRepo.AllInstances.CreateCategoryProviderServiceCategory = (repo, category) => true;

                this.websiteCategory.State = ObjectStatus.ObjectState.Read;

                var response = this.target.EditCategories(this.categoryList);

                Assert.IsTrue(response);
            }
        }

        /// <summary>
        /// The edit categories default negative test.
        /// </summary>
        [TestMethod]
        [TestCategory("CategoriesLogicTests")]
        public void EditCategoriesDefaultNegativeTest()
        {
            using (ShimsContext.Create())
            {
                ShimCategoryRepo.AllInstances.CreateCategoryProviderServiceCategory = (repo, category) => false;

                this.websiteCategory.State = ObjectStatus.ObjectState.Test;

                var response = this.target.EditCategories(this.categoryList);

                Assert.IsFalse(response);
            }
        }
    }
}
