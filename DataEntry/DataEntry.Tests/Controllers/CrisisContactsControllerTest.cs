
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Website.BL;
using Website.BL.Fakes;
using Website.Controllers;
using Website.Models;


namespace DataEntry.Tests.Controllers
{

    [TestClass]
    public class CrisisContactsControllerTest
    {
        private CrisisContactsController controller;
        private List<HotLineProviderViewModel> crisisContactsList ;
        private HotlineLogics hotLogics;

        [TestInitialize]
        public void CrisisContactsInitialize()
        {
            crisisContactsList = new List<HotLineProviderViewModel>
            {
                new HotLineProviderViewModel
                {
                    ProviderName = "NineOneOneNumber",
                    CrisisNumber = "911",
                    ProviderLocation = "SomeArea"
                },
                new HotLineProviderViewModel
                {
                    ProviderName = "HotLineNumber",
                    CrisisNumber = "777-777-7777",
                    ProviderLocation = "Around"
                }
            };
            this.controller = new CrisisContactsController();

        }

        [TestMethod]
        public void Index()
        {
            CrisisContactsController controller= new CrisisContactsController();
            ViewResult result = controller.Index() as ViewResult;
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetHotLineProvidersTest()
        {
            using (ShimsContext.Create())
            {
                ShimHotlineLogics.AllInstances.GetHotlines = access => crisisContactsList;
                CrisisContactsController controller = new CrisisContactsController();
                var result = controller.GetHotlineProviders() as JsonResult;
                var resultModel = result.Data;
                Assert.IsNotNull(result);
                


            }
        }

    }
}
