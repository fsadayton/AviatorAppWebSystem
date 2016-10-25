using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Website.Controllers;

namespace DataEntry.Tests.Controllers
{
    [TestClass]
    public class AdministrationControllerTests
    {
        /// <summary>
        /// The get index test.
        /// </summary>
        [TestMethod]
        public void GetIndexTest()
        {
            var controller = new AdministrationController();
            var result = controller.Index();
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }
    }
}
