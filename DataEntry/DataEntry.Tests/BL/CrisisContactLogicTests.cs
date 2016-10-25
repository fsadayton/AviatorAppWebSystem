// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CrisisContactLogicTests.cs" company="UDRI">
//   Copyright © 2016 University of Dayton Research Institute. All rights reserved.
// </copyright>
// <summary>
//  Crisis Contact Logic tests
// </summary>
// --------------------------------------------------------------------------------------------------------------------


using Website.Models;

namespace DataEntry.Tests.BL
{
    using System;
    using System.Collections.Generic;
    using DataEntry_Helpers;
    using DataEntry_Helpers.Repositories.Fakes;
    using Microsoft.QualityTools.Testing.Fakes;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Website.BL;

    [TestClass]
    public class CrisisContactLogicTests
    {

        private CrisisContactLogic target;
        private List<CrisisContact> dbContacts;

        private CrisisContactDisplay sampleDisplay;

        [TestInitialize]
        public void TestInitialize()
        {
            target = new CrisisContactLogic();

            dbContacts = new List<CrisisContact>
            {
                new CrisisContact { ID = 1, Contact =  new Contact { ID = 1, Phone = "9375555555"}, Name = "Test 1" },

                new CrisisContact { ID = 2, Contact =  new Contact { ID = 2,  Phone = "9375555556"}, Name = "Test 2" },
                
            };

            sampleDisplay = new CrisisContactDisplay
            {
                ContactId = 1,
                ID = 1,
                Name = "Test 1",
                PhoneNumber = "(937) 555-1234"
            };
        }

        [TestMethod]
        public void GetAllCrisisContactsTest()
        {
            using (ShimsContext.Create())
            {
                ShimCrisisContactRepo.AllInstances.GetCrisisContacts = repo => dbContacts; 
                var result = target.GetCrisisContactDisplays();
                Assert.AreEqual("Test 1", result[0].Name);
                Assert.AreEqual("(937) 555-5555", result[0].PhoneNumber);

                Assert.AreEqual("Test 2", result[1].Name);
                Assert.AreEqual("(937) 555-5556", result[1].PhoneNumber);
            }
        }


        [TestMethod]
        public void GetAllCrisisContactsNoneFoundTest()
        {
            using (ShimsContext.Create())
            {
                ShimCrisisContactRepo.AllInstances.GetCrisisContacts = repo => null;
                var result = target.GetCrisisContactDisplays();
                Assert.IsNull(result);
            }
        }



        [TestMethod]
        public void GetAllCrisisContactsSearchTest()
        {
            using (ShimsContext.Create())
            {
                ShimCrisisContactRepo.AllInstances.GetCrisisContactsByNameString = (repo, s) => dbContacts;
                var result = target.GetCrisisContactDisplays("test search");
                Assert.AreEqual("Test 1", result[0].Name);
                Assert.AreEqual("(937) 555-5555", result[0].PhoneNumber);

                Assert.AreEqual("Test 2", result[1].Name);
                Assert.AreEqual("(937) 555-5556", result[1].PhoneNumber);
            }
        }
        
        [TestMethod]
        public void GetAllCrisisContactsSearchNoneFoundTest()
        {
            using (ShimsContext.Create())
            {
                ShimCrisisContactRepo.AllInstances.GetCrisisContactsByNameString = (repo, s) => null;
                var result = target.GetCrisisContactDisplays("search");
                Assert.IsNull(result);
            }
        }

        [TestMethod]
        public void GetCrisisContactDisplayByIdTest()
        {
            using (ShimsContext.Create())
            {
                ShimCrisisContactRepo.AllInstances.GetCrisisContactInt32 = (repo, i) => dbContacts[0];
                var result = target.GetCrisisContactDisplay(1);
                Assert.AreEqual("Test 1", result.Name);
                Assert.AreEqual("(937) 555-5555", result.PhoneNumber);
            }
        }

        [TestMethod]
        public void GetCrisisContactDisplayByIdNotFoundTest()
        {
            using (ShimsContext.Create())
            {
                ShimCrisisContactRepo.AllInstances.GetCrisisContactInt32 = (repo, i) => null;
                var result = target.GetCrisisContactDisplay(1);
                Assert.IsNull(result);
            }
        }

        [TestMethod]
        public void CreateCrisisContactSuccessfulTest()
        {
            using (ShimsContext.Create())
            {
                sampleDisplay.ID = 0;
                ShimCrisisContactRepo.AllInstances.CreateCrisisContactCrisisContact = (repo, contact) => 1;
                var result = target.CreateCrisisContact(sampleDisplay);
                Assert.AreEqual(1, result);
            }
        }


        [TestMethod]
        public void CreateCrisisContactFailTest()
        {
            using (ShimsContext.Create())
            {
                sampleDisplay.ID = 0;
                ShimCrisisContactRepo.AllInstances.CreateCrisisContactCrisisContact = (repo, contact) => null;
                var result = target.CreateCrisisContact(sampleDisplay);
                Assert.IsNull(result);
            }
        }



        [TestMethod]
        public void UpdateCrisisContactSuccessfulTest()
        {
            using (ShimsContext.Create())
            {
                sampleDisplay.ID = 1;
                ShimCrisisContactRepo.AllInstances.UpdateCrisisContactCrisisContact = (repo, contact) => true;
                ShimCrisisContactRepo.AllInstances.GetCrisisContactInt32 = (repo, i) => this.dbContacts[0];
                var result = target.UpdateCrisisContact(sampleDisplay);
                Assert.IsTrue(result);
            }
        }


        [TestMethod]
        public void UpdateCrisisContactFailTest()
        {
            using (ShimsContext.Create())
            {
                sampleDisplay.ID = 1;
                ShimCrisisContactRepo.AllInstances.UpdateCrisisContactCrisisContact = (repo, contact) => false;
                ShimCrisisContactRepo.AllInstances.GetCrisisContactInt32 = (repo, i) => this.dbContacts[0];
                var result = target.UpdateCrisisContact(sampleDisplay);
                Assert.IsFalse(result);
            }
        }
        

        [TestMethod]
        public void DeleteCrisisContactSuccessfulTest()
        {
            using (ShimsContext.Create())
            {
                sampleDisplay.ID = 1;
                ShimCrisisContactRepo.AllInstances.DeleteCrisisContactInt32 = (repo, i) => true;
                ShimCrisisContactRepo.AllInstances.GetCrisisContactInt32 = (repo, i) => this.dbContacts[0];
                var result = target.DeleteCrisisContact(1);
                Assert.IsTrue(result);
            }
        }


        [TestMethod]
        public void DeleteCrisisContactFailTest()
        {
            using (ShimsContext.Create())
            {
                sampleDisplay.ID = 1;
                ShimCrisisContactRepo.AllInstances.DeleteCrisisContactInt32 = (repo, i) => false;
                ShimCrisisContactRepo.AllInstances.GetCrisisContactInt32 = (repo, i) => this.dbContacts[0];
                var result = target.DeleteCrisisContact(1);
                Assert.IsFalse(result);
            }
        }

    }
}
