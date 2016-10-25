using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using DataEntry_Helpers;
using DataEntry_Helpers.Fakes;
using DataEntry_Helpers.Repositories;
using DataEntry_Helpers.Repositories.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Website.BL;
using Website.Controllers;
using Website.Models;

namespace DataEntry.Tests.BL
{
    [TestClass]
    public class HotLineLogicsTest
    {

        private HotlineLogics logics = new HotlineLogics();
       

        private List<ServiceProvider> serviceProviderList;
        private List<ServiceProvider> serviceProviderListLine;
        private List<ServiceProvider> serviceProviderDisplay;
        private List<HotLineProviderViewModel> crisisContactsList;
        
        [TestInitialize]
        public void Initialize()
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
                },
            new HotLineProviderViewModel
                {
                    ProviderName="NonDisplay",
                    CrisisNumber="1202020200",
                    ProviderLocation="Somewhere"

                }
            };
            Contact contact1 = new Contact() {HelpLine = "911"};
            Contact contact2 = new Contact() {HelpLine = "777-777-7777"};
            Contact contact3 = new Contact() {HelpLine = "1202020200" };

            Location local1 = new Location() {Contact = contact1, Name= "SomeArea", Display = true};
            Location local2 = new Location() {Contact = contact2, Name = "Around", Display = true};
            Location local3 = new Location() {Contact = contact3, Name= "Somewhere", Display = false};
            serviceProviderList = new List<ServiceProvider>()
            {
                
                new ServiceProvider()
                {
                    ProviderName = "NineOneOneNumber",
                    Active = true,
                    Locations = {local1}
                   
                },
                new ServiceProvider()
                {
                    ProviderName = "HotLineNumber",
                    Active = true,
                    Locations = {local2}
                },
                 new ServiceProvider()
                {
                    ProviderName = "NonDisplay",
                    Active = true,
                    Locations = {local3}
                }

            };
            serviceProviderListLine = new List<ServiceProvider>()
            { new ServiceProvider()
                {
                    ProviderName = "NineOneOneNumber",
                    Active = true,
                    Locations = {local1}

                },
                new ServiceProvider()
                {
                    ProviderName = "HotLineNumber",
                    Active = true,
                    Locations = {local2}
                },
                 new ServiceProvider()
                {
                    ProviderName = "NonDisplay",
                    Active = true,
                    Locations = {local3}
                }

            };
            serviceProviderDisplay = new List<ServiceProvider>()
            {
                new ServiceProvider()
                {
                    ProviderName = "HotLineNumber",
                    Active = true,
                    Locations = {local1}

                },
                new ServiceProvider()
                {
                    ProviderName = "NineOneOneNumber",
                    Active = true,
                    Locations = {local2}
                },
                 new ServiceProvider()
                {
                    ProviderName = "NonDisplay",
                    Active = true,
                    Locations = {local3}
                }
            };
             
        }
        [TestMethod]
        public void GetHotLinesTest()
        {
            
            using (ShimsContext.Create())
            {
                ShimServiceProviderRepo.AllInstances.GetAllActiveServiceProviders = repo => serviceProviderList;
                logics = new HotlineLogics();
                var hotLines = logics.GetHotlines();
                //assert 
                 Assert.IsNotNull(hotLines);
                 Assert.AreEqual(1,hotLines.Count);
                 Assert.AreEqual(hotLines[0].CrisisNumber, crisisContactsList[1].CrisisNumber);
            }
        }
        [TestMethod]
        public void GetHotLinesTestLine()
        {
            logics = new HotlineLogics();
            using (ShimsContext.Create())
            {
                ShimServiceProviderRepo.AllInstances.GetAllActiveServiceProviders = repo => serviceProviderListLine;

                var hotLines = logics.GetHotlines();

                //assert 
                Assert.IsNotNull(hotLines);
                Assert.AreEqual(hotLines[0].ProviderName, crisisContactsList[1].ProviderName);
                Assert.AreEqual(hotLines.Count,1);
            }
        }
        [TestMethod]
        public void GetHotLinesTestDisplay()
        {
            logics = new HotlineLogics();
            using (ShimsContext.Create())
            {
                ShimServiceProviderRepo.AllInstances.GetAllActiveServiceProviders = repo => serviceProviderDisplay;

                var hotLines = this.logics.GetHotlines();

                //assert 
                Assert.IsNotNull(hotLines);
                Assert.AreEqual(hotLines[0].ProviderLocation,crisisContactsList[1].ProviderLocation);
            }
        }
    }
}
