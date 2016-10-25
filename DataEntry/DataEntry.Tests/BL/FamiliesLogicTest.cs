// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CategoriesLogicTest.cs" company="UDRI">
//   Copyright © 2015 University of Dayton Research Institute. All rights reserved.
// </copyright>
// <summary>
//   The date logic tests.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

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
    using Website.Models;

    /// <summary>
    /// The date logic tests.
    /// </summary>
    [TestClass]
    public class FamiliesLogicTest
    {
        /// <summary>
        /// The target.
        /// </summary>
        private FamiliesLogic target;

        /// <summary>
        /// The website category.
        /// </summary>
        private FamilyEditor websiteFamily;

        private Family databaseFamily;

        private List<FamilyEditor> websiteFamilyList;

        private List<Family> databaseFamilyList;

        private FamilyService databaseFamilyServices1;

        private FamilyService databaseFamilyServices2;

        private List<FamilyService> databaseFamilyServicesList;

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
            this.target = new FamiliesLogic();

            this.websiteFamily = new FamilyEditor
                                       {
                                           Active = true,
                                           Description = "Test Family",
                                           CategoryIds = new List<int> { 1, 2 },
                                           Id = 1,
                                           Name = "Edit Family",
                                           State = ObjectStatus.ObjectState.Read
                                       };

            this.databaseFamily = new Family
                                        {
                                            Active = true,
                                            Description = "Test database family",
                                            ID = 1,
                                            Name = "Edit database family",
                                        };

            this.databaseFamilyServices1 = new FamilyService { FamilyID = 1, ID = 1, ServiceID = 1 };
            this.databaseFamilyServices2 = new FamilyService { FamilyID = 1, ID = 2, ServiceID = 2 };
            this.databaseFamilyServicesList = new List<FamilyService>
                                                  {
                                                      this.databaseFamilyServices1,
                                                      this.databaseFamilyServices2
                                                  };

            this.websiteFamilyList = new List<FamilyEditor> { this.websiteFamily };

            this.databaseFamilyList = new List<Family> { this.databaseFamily };
        }

        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup()
        // {
        // }
        #endregion

        /// <summary>
        /// Get families test.
        /// </summary>
        [TestMethod]
        [TestCategory("FamiliesLogicTests")]
        public void GetFamiliesTest()
        {
            using (ShimsContext.Create())
            {
                ShimFamiliesRepo.AllInstances.GetFamilies = repo => this.databaseFamilyList;
                ShimFamiliesRepo.AllInstances.GetFamilyServicesInt32 = (repo, i) => this.databaseFamilyServicesList;
                var response = this.target.GetFamilies();

                Assert.IsNotNull(response);
                Assert.AreEqual(1, response.Count);
                Assert.AreEqual(this.databaseFamily.Active, response[0].Active);
                Assert.AreEqual(this.databaseFamily.Name, response[0].Name);
                Assert.AreEqual(this.databaseFamily.Description, response[0].Description);
                Assert.AreEqual(this.databaseFamily.ID, response[0].Id);
                Assert.AreEqual(ObjectStatus.ObjectState.Read, response[0].State);
            }
        }

        /// <summary>
        /// The merge category test.
        /// </summary>
        [TestMethod]
        [TestCategory("FamiliesLogicTests")]
        public void GetAllFamiliesTest()
        {
            using (ShimsContext.Create())
            {
                ShimFamiliesRepo.AllInstances.GetFamiliesEdit = repo => this.databaseFamilyList;
                ShimFamiliesRepo.AllInstances.GetFamilyServicesInt32 = (repo, i) => this.databaseFamilyServicesList;
                var response = this.target.GetFamiliesEdit();

                Assert.IsNotNull(response);
                Assert.AreEqual(1, response.Count);
                Assert.AreEqual(this.databaseFamily.Active, response[0].Active);
                Assert.AreEqual(this.databaseFamily.Name, response[0].Name);
                Assert.AreEqual(this.databaseFamily.Description, response[0].Description);
                Assert.AreEqual(this.databaseFamily.ID, response[0].Id);
                Assert.AreEqual(ObjectStatus.ObjectState.Read, response[0].State);
            }
        }

        [TestMethod]
        [TestCategory("FamiliesLogicTests")]
        public void EditFamilyPassTest()
        {
            using (ShimsContext.Create())
            {
                ShimFamiliesRepo.AllInstances.UpdateFamilyFamily = (repo, family) => true;
                ShimFamiliesRepo.AllInstances.UpdateFamilyServicesInt32ListOfInt32 = (repo, i, arg3) => true;

                ShimFamiliesRepo.AllInstances.DeleteFamilyFamily = (repo, family) => true;
                ShimFamiliesRepo.AllInstances.DeleteFamilyServicesInt32 = (repo, i) => true;

                ShimFamiliesRepo.AllInstances.CreateFamilyFamily = (repo, family) => this.databaseFamily;
                ShimFamiliesRepo.AllInstances.CreateFamilyServiceFamilyService = (repo, service) => true;

                this.websiteFamily.State = ObjectStatus.ObjectState.Update;
                var response = this.target.EditFamily(this.websiteFamily);

                Assert.IsTrue(response);

                this.websiteFamily.State = ObjectStatus.ObjectState.Delete;
                response = this.target.EditFamily(this.websiteFamily);

                Assert.IsTrue(response);

                this.websiteFamily.State = ObjectStatus.ObjectState.Create;
                response = this.target.EditFamily(this.websiteFamily);

                Assert.IsTrue(response);

                this.websiteFamily.State = ObjectStatus.ObjectState.Test;
                response = this.target.EditFamily(this.websiteFamily);

                Assert.IsTrue(response);
            }
        }

        [TestMethod]
        [TestCategory("FamiliesLogicTests")]
        public void EditFamilyFailTest()
        {
            using (ShimsContext.Create())
            {
                ShimFamiliesRepo.AllInstances.UpdateFamilyFamily = (repo, family) => false;
                ShimFamiliesRepo.AllInstances.UpdateFamilyServicesInt32ListOfInt32 = (repo, i, arg3) => false;

                ShimFamiliesRepo.AllInstances.DeleteFamilyFamily = (repo, family) => false;
                ShimFamiliesRepo.AllInstances.DeleteFamilyServicesInt32 = (repo, i) => false;

                ShimFamiliesRepo.AllInstances.CreateFamilyFamily = (repo, family) => this.databaseFamily;
                ShimFamiliesRepo.AllInstances.CreateFamilyServiceFamilyService = (repo, service) => false;

                this.websiteFamily.State = ObjectStatus.ObjectState.Update;
                var response = this.target.EditFamily(this.websiteFamily);

                Assert.IsFalse(response);

                this.websiteFamily.State = ObjectStatus.ObjectState.Delete;
                response = this.target.EditFamily(this.websiteFamily);

                Assert.IsFalse(response);

                this.websiteFamily.State = ObjectStatus.ObjectState.Create;
                response = this.target.EditFamily(this.websiteFamily);

                Assert.IsTrue(response);

                ShimFamiliesRepo.AllInstances.CreateFamilyFamily = (repo, family) => null;
                response = this.target.EditFamily(this.websiteFamily);

                Assert.IsFalse(response);

                this.websiteFamily.State = ObjectStatus.ObjectState.Test;
                response = this.target.EditFamily(this.websiteFamily);

                Assert.IsTrue(response);
            }
        }

    }
}
