using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Website;
using Website.Controllers;

namespace DataEntry.Tests.Controllers
{
    using System.Configuration.Provider;

    using DataEntry_Helpers;
    using DataEntry_Helpers.Repositories;

    using Website.BL;

    [TestClass]
    public class HomeControllerTest
    {
        private DataEntry_Helpers.IServiceProviderRepo repo;

        private ServiceProviderQueryLogics logics;

        [TestMethod]
        public void Index()
        {
            // Arrange
            WebController controller = new WebController();

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void About()
        {
            // Arrange
            WebController controller = new WebController();

            // Act
            ViewResult result = controller.About() as ViewResult;

            // Assert
            Assert.AreEqual("Your application description page.", result.ViewBag.Message);
        }

        [TestMethod]
        public void Contact()
        {
            // Arrange
            WebController controller = new WebController();

            // Act
            ViewResult result = controller.Contact() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void ProviderSearchTest()
        {
            this.logics = new ServiceProviderQueryLogics();
            var test = this.logics.ServiceProviderQuery(new List<int> { 57 }, new List<int> { 1, 2 });
            this.repo = new ServiceProviderRepo();
            var result = this.repo.GetServiceProviders(new List<int> { 57 }, new List<int> { 1 , 25 });
        }
    }
}
