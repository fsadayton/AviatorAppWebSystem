using System;
using System.Collections.Generic;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models.ServiceProvider;
using Website.BL.Fakes;
using Website.Controllers;

namespace DataEntry.Tests.Controllers
{
    /// <summary>
    /// App Compensation Controller tests
    /// </summary>
    [TestClass]
    public class AppCompensationControllerTests
    {
        /// <summary>
        /// The controller to be tested.
        /// </summary>
        private AppCompensationController target;

        /// <summary>
        /// The test initializer
        /// </summary>
        [TestInitialize]
        public void TestInitialize()
        {
           this.target = new AppCompensationController();
        }

        /// <summary>
        /// Get categories test
        /// </summary>
        [TestMethod]
        public void GetCategoriesTest()
        {
            var foundCategories = new List<WebsiteCategory>
            {
                new WebsiteCategory {
                    Crime = false,
                    Description = "test description",
                    Id = 1,
                    Name = "Category Name",
                    ServiceTypes = new List<WebsiteCategoryType> { new WebsiteCategoryType { Id=1, Name = "Victim Compensation"} } },
                new WebsiteCategory {
                    Crime = false,
                    Description = "test description",
                    Id = 1,
                    Name = "Category Name",
                    ServiceTypes = new List<WebsiteCategoryType>
                    {
                        new WebsiteCategoryType { Id=1, Name = "Victim Compensation"},
                        new WebsiteCategoryType { Id=2, Name = "General"}
                    }
                },
                new WebsiteCategory {
                    Crime = false,
                    Description = "test description",
                    Id = 1,
                    Name = "Category Name",
                    ServiceTypes = new List<WebsiteCategoryType> { new WebsiteCategoryType { Id=2, Name = "General"} }
                }
            };

            using (ShimsContext.Create())
            {
                ShimDataLogics.AllInstances.GetWebsiteCategories = logics => foundCategories;
                var result = this.target.GetCategories();
                Assert.IsNotNull(result);
                Assert.AreEqual(2, result.Count);
            }
        }
    }
}
