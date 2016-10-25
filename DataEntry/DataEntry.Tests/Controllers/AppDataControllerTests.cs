using System;
using System.Collections.Generic;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models.ServiceProvider;
using Website.BL.Fakes;
using Website.Controllers;

namespace DataEntry.Tests.Controllers
{
    [TestClass]
    public class AppDataControllerTests
    {
        /// <summary>
        /// The controller to be tested.
        /// </summary>
        private AppDataController target;

        /// <summary>
        /// The test initializer
        /// </summary>
        [TestInitialize]
        public void TestInitialize()
        {
            this.target = new AppDataController();
        }


        [TestMethod]
        public void GetCategoriesByFamilyTest()
        {
            using (ShimsContext.Create())
            {
                var categories = new List<WebsiteCategory>
                {
                    new WebsiteCategory
                    {
                        Crime = false,
                        Description = "Description",
                        Name = "Name",
                        Id = 1,
                        ServiceTypes = new List<WebsiteCategoryType> { new WebsiteCategoryType {  Id = 1, Name = "General" } }
                    },
                    new WebsiteCategory
                    {
                        Crime = false,
                        Description = "Description2",
                        Name = "Name 2",
                        Id = 2,
                        ServiceTypes = new List<WebsiteCategoryType> { new WebsiteCategoryType {  Id = 1, Name = "General" } }
                    }
                };

                ShimDataLogics.AllInstances.GetWebsiteCategoriesInt32 = (logics, i) => categories;

                var result = this.target.GetCategoriesByFamily(1);
                Assert.AreEqual(2, result.Count);
            }
        }
    }
}
