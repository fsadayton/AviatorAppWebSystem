using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models;
using Models.ServiceProvider;
using Website.BL.Fakes;
using Website.BL.ModelConversions.Fakes;
using Website.Controllers;
using Website.Models;

namespace DataEntry.Tests.Controllers
{
    [TestClass]
    public class SpecialPopulationsControllerTests
    {
        /// <summary>
        /// The get index test.
        /// </summary>
        [TestMethod]
        public void GetIndexTest()
        {
            var familyList = new List<FamilyEditor>
            {
                new FamilyEditor { Id = 1, Name = "Family 1", Description = "this is a description" },
                new FamilyEditor { Id = 1, Name = "Family 2", Description = "this is a description too" }
            };

            var controller = new SpecialPopulationsController();
            using (ShimsContext.Create())
            {
                ShimFamiliesLogic.AllInstances.GetFamilies = logic => familyList;
                var result = controller.Index(null, null) as ViewResult;
                var resultModel = result.Model as List<FamilyEditor>;
                Assert.AreEqual(2, resultModel.Count);
            }
        }

        /// <summary>
        /// Test for the index for a given family
        /// </summary>
        [TestMethod]
        public void GetIndexWithFamilyTest()
        {
            var controller = new SpecialPopulationsController();

            using (ShimsContext.Create())
            {
                var result = controller.Index("4", null) as ViewResult;
                Assert.AreEqual("ServiceProviderList", result.ViewName);
            }
        }

        /// <summary>
        /// Get Details for a provider.
        /// </summary>
        [TestMethod]
        public void GetDetailsTest()
        {
            var sampleCategory = new WebsiteCategory
            {
                Crime = false,
                Description = "description",
                Id = 1,
                Name = "Name",
                ServiceTypes = new List<WebsiteCategoryType> { new WebsiteCategoryType { Id = 2, Name = "Category1" } }
            };

            var sampleServiceProvider = new WebsiteServiceProvider
            {
                Description = "Service Provider",
                DisplayRank = 3,
                Id = 2,
                IsActive = true,
                Name = "My Name",
                Type = 1,
                Services = new WebServiceAreas { ServiceAreas = new List<int> { 1, 3 } },
                Locations = new List<ServiceProviderLocation>
                {
                    new ServiceProviderLocation
                    {
                        Id = 1,
                        Display = true,
                        Name = "Location Name",
                        City = "Dayton",
                        Zip = "12344"
                    }
                }
            };
            var categoryList = new List<WebsiteCategory> { sampleCategory };

            var controller = new SpecialPopulationsController();
            using (ShimsContext.Create())
            {
                ShimDataLogics.AllInstances.GetWebsiteCategories = logics => categoryList;
                ShimDatabaseToWebServiceProvider.AllInstances.GetServiceProviderInt32 =
                    (provider, i) => sampleServiceProvider;
                var result = controller.Details(2, null) as ViewResult;
                var resultModel = result.Model;
                Assert.IsNotNull(result);
                Assert.AreEqual(sampleServiceProvider, resultModel);
            }
        }
    }
}
