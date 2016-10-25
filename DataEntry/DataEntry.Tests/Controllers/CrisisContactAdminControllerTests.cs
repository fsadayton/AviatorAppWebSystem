// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CrisisContactAdminControllerTests.cs" company="UDRI">
//   Copyright © 2016 University of Dayton Research Institute. All rights reserved.
// </copyright>
// <summary>
//   The crisis contact controller tests.
// </summary>
// --------------------------------------------------------------------------------------------------------------------


namespace DataEntry.Tests.Controllers
{
    using System.Web.Mvc;
    using Microsoft.QualityTools.Testing.Fakes;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Website.BL.Fakes;
    using Website.Controllers;
    using Website.Models;
    
    /// <summary>
    /// Crisis contact controller tests
    /// </summary>
    [TestClass]
    public class CrisisContactAdminControllerTests
    {
        private CrisisContactAdminController target;
        private CrisisContactDisplay sampleDisplay;

        /// <summary>
        /// Test initializer
        /// </summary>
        [TestInitialize]
        public void TestInitialize()
        {
            target = new CrisisContactAdminController();
            sampleDisplay = new CrisisContactDisplay
            {
                ID = 1,
                ContactId = 1,
                Name = "test name",
                PhoneNumber = "(555) 555-5555"
            };
        }

        /// <summary>
        /// The Get index test
        /// </summary>
        [TestMethod]
        public void GetIndexTest()
        {
            var result = target.Index();
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof (ViewResult));
        }

        /// <summary>
        /// The get create view test
        /// </summary>
        [TestMethod]
        public void GetCreateTest()
        {
            var result = target.Create();
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof (ViewResult));
            Assert.IsNotNull((result as ViewResult).Model);
        }

        /// <summary>
        /// The post to create test
        /// </summary>
        [TestMethod]
        public void PostCreateTest()
        {
            using (ShimsContext.Create())
            {
                ShimCrisisContactLogic.AllInstances.CreateCrisisContactCrisisContactDisplay = (logic, display) => 1;
                var result = target.Create(this.sampleDisplay);
                Assert.IsNotNull(result);
                Assert.IsInstanceOfType(result, typeof (RedirectToRouteResult));
                var routeResult = result as RedirectToRouteResult;
                Assert.IsNotNull(routeResult);
                Assert.AreEqual(routeResult.RouteValues["action"], "Index");
                Assert.IsTrue(this.target.TempData.ContainsKey("Info"));
            }
        }

        /// <summary>
        /// The post to create test where insert fails
        /// </summary>
        [TestMethod]
        public void PostCreateFailureTest()
        {
            using (ShimsContext.Create())
            {
                ShimCrisisContactLogic.AllInstances.CreateCrisisContactCrisisContactDisplay = (logic, display) => null;
                var result = target.Create(this.sampleDisplay);
                Assert.IsNotNull(result);
                Assert.IsInstanceOfType(result, typeof (ViewResult));
                Assert.IsNotNull((result as ViewResult).Model);
                Assert.IsTrue(this.target.TempData.ContainsKey("Error"));
            }
        }

        /// <summary>
        /// The get the edit view test
        /// </summary>
        [TestMethod]
        public void GetEditTest()
        {
            using (ShimsContext.Create())
            {
                ShimCrisisContactLogic.AllInstances.GetCrisisContactDisplayInt32 = (logic, i) => sampleDisplay;
                var result = target.Edit(1);
                Assert.IsNotNull(result);
                Assert.IsInstanceOfType(result, typeof (ViewResult));
                Assert.IsNotNull((result as ViewResult).Model);
            }
        }

        /// <summary>
        /// The post updated changes test
        /// </summary>
        [TestMethod]
        public void PostEditTest()
        {
            using (ShimsContext.Create())
            {
                ShimCrisisContactLogic.AllInstances.UpdateCrisisContactCrisisContactDisplay = (logic, display) => true;
                var result = target.Edit(this.sampleDisplay);
                Assert.IsNotNull(result);
                Assert.IsInstanceOfType(result, typeof (RedirectToRouteResult));
                var routeResult = result as RedirectToRouteResult;
                Assert.IsNotNull(routeResult);
                Assert.AreEqual(routeResult.RouteValues["action"], "Index");
                Assert.IsTrue(this.target.TempData.ContainsKey("Info"));
            }
        }

        /// <summary>
        /// The delete crisis contact test.
        /// </summary>
        [TestMethod]
        public void DeleteTest()
        {
            using (ShimsContext.Create())
            {
                ShimCrisisContactLogic.AllInstances.DeleteCrisisContactInt32 = (logic, i) => true;
                var result = target.Delete(1);
                Assert.IsNotNull(result);
                Assert.IsInstanceOfType(result, typeof (RedirectToRouteResult));
                var routeResult = result as RedirectToRouteResult;
                Assert.IsNotNull(routeResult);
                Assert.AreEqual(routeResult.RouteValues["action"], "Index");
                Assert.IsTrue(this.target.TempData.ContainsKey("Info"));
            }
        }

    }
}
