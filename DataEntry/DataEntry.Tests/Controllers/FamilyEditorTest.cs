using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using Website.Models;
using Models;
using Microsoft.QualityTools.Testing.Fakes;
using Website.BL.Fakes;
using Website.Controllers;
using System.Web.Mvc;

namespace DataEntry.Tests.BL
{
    [TestClass]
    public class FamilyEditorTest
    {
        private List<FamilyEditor> familyList;
        private FamilyEditor familyUpdate;
        private FamilyEditor familyCreate;
        private FamilyEditor familyDelete;
        private FamilyEditor familyDeactiv;
        private FamilyEditor familyActiv;
        private FamilyEditor familyRead;

        /// <summary>
        /// The my test initialize.
        /// </summary>
        [TestInitialize]
        public void MyTestInitialize()
        {
            familyList = new List<FamilyEditor>
            {
                new FamilyEditor { Id = 3, Name = "Name", Description = "Description" },
                new FamilyEditor { Name = "Name", Description = "Description", Active = true, State = ObjectStatus.ObjectState.Update },
            };

            familyCreate = new FamilyEditor { Name = "Name", Description = "Description", Active = false, State = ObjectStatus.ObjectState.Create };
            familyUpdate = new FamilyEditor { Name = "Name", Description = "Description", Active = true, State = ObjectStatus.ObjectState.Update };
            familyDelete = new FamilyEditor { Name = "Name", Description = "Description", Active = true, State = ObjectStatus.ObjectState.Delete };
            familyDeactiv = new FamilyEditor { Name = "Name", Description = "Description", Active = false, State = ObjectStatus.ObjectState.Update };
            familyActiv = new FamilyEditor { Name = "Name", Description = "Description", Active = true, State = ObjectStatus.ObjectState.Update };
            familyRead = new FamilyEditor { Name = "Name", Description = "Description", Active = true };
        }

        /// <summary>
        /// Index function test.
        /// </summary>
        [TestMethod]
        [TestCategory("FamilyEditorsControllerTests")]
        public void IndexTest()
        {
            using (ShimsContext.Create())
            {
                ShimFamiliesLogic.AllInstances.GetFamiliesEdit = access => familyList;
                FamilyEditorsController controller = new FamilyEditorsController();
                var result = controller.Index() as ViewResult;
                var resultModel = result.Model as List<FamilyEditor>;
                Assert.AreEqual(0, resultModel[0].Id);
            }
        }

        /// <summary>
        /// Details function test.
        /// </summary>
        [TestMethod]
        [TestCategory("FamilyEditorsControllerTests")]
        public void DetailsTest()
        {
            using (ShimsContext.Create())
            {
                ShimFamiliesLogic.AllInstances.GetFamiliesEdit = access => familyList;
                FamilyEditorsController controller = new FamilyEditorsController();
                var result = controller.Details(3) as ViewResult;
                var resultModel = result.Model as FamilyEditor;
                Assert.IsNotNull(result);
                Assert.AreEqual(3, resultModel.Id);
            }
        }

        /// <summary>
        /// Edit function test.
        /// </summary>
        [TestMethod]
        [TestCategory("FamilyEditorsControllerTests")]
        public void EditHelperTest()
        {
            FamilyEditorsController controller = new FamilyEditorsController();

            FamilyEditor sampleFamily = new FamilyEditor
            {
                Name = "Name",
                Description = "Description",
                Active = true,
                State = ObjectStatus.ObjectState.Read //test to see that this changed to Update
            };

            var result = controller.EditHelper(sampleFamily);

            Assert.IsNotNull(result);
            Assert.AreEqual(familyUpdate.State, result.State);  
        }


        /// <summary>
        /// Deactivate function test.
        /// </summary>
        [TestMethod]
        [TestCategory("FamilyEditorsControllerTests")]
        public void DeactivateHelperTest()
        {
            FamilyEditorsController controller = new FamilyEditorsController();

            FamilyEditor sampleFamily = new FamilyEditor
            {
                Name = "Name",
                Description = "Description",
                Active = true,  //test to see that this changed to false
                State = ObjectStatus.ObjectState.Read //test to see that this is changed to Update
            };
            var result = controller.DeactivateHelper(sampleFamily);

            Assert.IsNotNull(result);
            Assert.AreEqual(familyDeactiv.Active, sampleFamily.Active);
            Assert.AreEqual(familyDeactiv.State, sampleFamily.State);
        }

        /// <summary>
        /// Activate function test.
        /// </summary>
        [TestMethod]
        [TestCategory("FamilyEditorsControllerTests")]
        public void ActivateHelperTest()
        {
            FamilyEditorsController controller = new FamilyEditorsController();

            FamilyEditor sampleFamily = new FamilyEditor
            {
                Name = "Name",
                Description = "Description",
                Active = false,  //test to see that this changed to true
                State = ObjectStatus.ObjectState.Read //test to see that this is changed to Update
            };
            var result = controller.ActivateHelper(sampleFamily);

            Assert.IsNotNull(result);
            Assert.AreEqual(familyActiv.Active, sampleFamily.Active);
            Assert.AreEqual(familyActiv.State, sampleFamily.State);
        }

        /// <summary>
        /// Delete function test.
        /// </summary>
        [TestMethod]
        [TestCategory("FamilyEditorsControllerTests")]
        public void DeleteHelperTest()
        {
            FamilyEditorsController controller = new FamilyEditorsController();

            FamilyEditor sampleFamily = new FamilyEditor
            {
                Name = "Name",
                Description = "Description",
                State = ObjectStatus.ObjectState.Read //test to see that this is changed to Delete
            };
            var result = controller.DeleteHelper(sampleFamily);

            Assert.IsNotNull(result);
            Assert.AreEqual(familyDelete.State, sampleFamily.State);
        }

        /// <summary>
        /// Create function test.
        /// </summary>
        [TestMethod]
        [TestCategory("FamilyEditorsControllerTests")]
        public void CreateHelperTest()
        {
            FamilyEditorsController controller = new FamilyEditorsController();

            FamilyEditor sampleFamily = new FamilyEditor
            {
                Name = "Name",
                Description = "Description",
                Active = true,
                State = ObjectStatus.ObjectState.Read //test to see that this changed to Update
            };

            var result = controller.CreateHelper(sampleFamily);

            Assert.IsNotNull(result);
            Assert.AreEqual(familyCreate.State, result.State);
        }

        /// <summary>
        /// Search function test
        /// </summary>
        [TestMethod]
        [TestCategory("FamilyEditorsControllerTests")]
        public void SearchTest()
        {
            using (ShimsContext.Create())
            {
                FamilyEditorsController controller = new FamilyEditorsController();

                var familyList = new List<FamilyEditor>
                {
                    new FamilyEditor
                    {
                        Name = "My Name",
                        Description = "My Description"
                    },
                    new FamilyEditor
                    {
                        Name = "My Name 2",
                        Description = "My Description 2"
                    }
                };

                ShimFamiliesLogic.AllInstances.GetFamiliesEditByNameString = (logics, s) => familyList;

                var result = controller.Search("test test");
                var resultModel = result.Model as List<FamilyEditor>;
                Assert.IsNotNull(resultModel);
                Assert.AreEqual(2, resultModel.Count);
            }
        }


    }
}
