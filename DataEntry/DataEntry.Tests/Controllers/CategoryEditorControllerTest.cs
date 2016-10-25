using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models.Editors;
using System.Collections.Generic;
using Models;
using Website.Controllers;
using System.Web.UI.WebControls;
using System.Web.Mvc;
using DataEntry_Helpers;
using DataEntry_Helpers.Repositories.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using Website.BL;
using Website.BL.Fakes;
using Website.Models;

namespace DataEntry.Tests.Controllers
{
    [TestClass]
    public class CategoryEditorControllerTest
    {

        /// <summary>
        /// The target.
        /// </summary>
        private View target;

        private List<CategoryEditor> categoryList;
        private CategoryEditor categoryEdit;

        /// <summary>
        /// The my test initialize.
        /// </summary>
        [TestInitialize]
        public void MyTestInitialize()
        {
            categoryList = new List<CategoryEditor>
            {
                new CategoryEditor { Id = 3, Name = "Name", Description = "Description" },
                new CategoryEditor { Name = "Name", Description = "Description", Crime = true, Active = true, State = ObjectStatus.ObjectState.Update },
            };

            categoryEdit = new CategoryEditor { Name = "Name", Description = "Description", Crime = true, Active = true, State = ObjectStatus.ObjectState.Update };
        }

        /// <summary>
        /// Index function test.
        /// </summary>
        [TestMethod]
        [TestCategory("CategoriesEditorControllerTests")]
        public void IndexTest()
        {
            //compare view from CagatoriesLogic .getCategeories vs view from Index
            using (ShimsContext.Create())
            {
                ShimCategoriesLogic.AllInstances.GetCategories = access => categoryList;
                CategoryEditorController controller = new CategoryEditorController();
                var result = controller.Index() as ViewResult;
                var resultModel = result.Model as List<CategoryEditor>;
                Assert.AreEqual(3, resultModel[0].Id);
            }
            
        }

        /// <summary>
        /// Create function test. 
        /// </summary>
        /// (Additional) checks key-value sets for viewbag
        [TestMethod]
        [TestCategory("CategoriesEditorControllerTests")]
        public void CreateTest()
        {
            var target = new ServiceTypesLogics();
            var serviceType1 = new ServiceType {  ID = 1, Name = "Type 1" };
            var serviceType2 = new ServiceType {  ID = 2, Name = "Type 2" };
            var serviceTypeList = new List<ServiceType> { serviceType1, serviceType2 };
            using (ShimsContext.Create())
            {
                ShimServiceTypesRepo.AllInstances.GetServiceTypes = repo => serviceTypeList;

                var response = target.GetServiceTypes();

                Assert.IsNotNull(response);
                Assert.AreEqual(serviceTypeList.Count, response.Count);

                Assert.IsTrue(response.ContainsKey(serviceTypeList[0].ID));
                Assert.AreEqual(serviceTypeList[0].Name, response[serviceTypeList[0].ID]);

                Assert.IsTrue(response.ContainsKey(serviceTypeList[1].ID));
                Assert.AreEqual(serviceTypeList[1].Name, response[serviceTypeList[1].ID]);
            }
            using (ShimsContext.Create())
            {
                ShimCategoriesLogic.AllInstances.GetCategories = access => categoryList;
                ShimCategoriesLogic.AllInstances.EditCategoriesListOfCategoryEditor = (access, cat) => true;
                CategoryEditorController controller = new CategoryEditorController();
                
               
                CategoryEditor sampleCategory = new CategoryEditor
                {
                    Name = "Name",
                    Description = "Description",
                    Active = true,
                    Crime = true
                };
                
                var result = controller.Create(sampleCategory);
                Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
                RedirectToRouteResult routeResult = result as RedirectToRouteResult;
                Assert.IsNotNull(routeResult);
                Assert.AreEqual(routeResult.RouteValues["action"], "Index");
            }

        }

        /// <summary>
        /// Edit function test.
        /// </summary>
        [TestMethod]
        [TestCategory("CategoriesEditorControllerTests")]
        public void EditHelperTest()
        {
            using (ShimsContext.Create())
            {
                ShimCategoriesLogic.AllInstances.GetCategories = access => categoryList;
                CategoryEditorController controller = new CategoryEditorController();

                CategoryEditor sampleCategory = new CategoryEditor
                {
                    Name = "Name", 
                    Description = "Description", 
                    Crime = true, 
                    Active = true,
                    State = ObjectStatus.ObjectState.Read //test to see that this changed to Update
                };

                var result = controller.EditHelper(sampleCategory);

                var firstResult = result[0];

                Assert.IsNotNull(result);
                Assert.AreEqual(categoryEdit.State, firstResult.State);
            }

        }

        /// <summary>
        /// Search function test
        /// </summary>
        [TestMethod]
        [TestCategory("CategoriesEditorControllerTests")]
        public void SearchTest()
        {
            using (ShimsContext.Create())
            {
                CategoryEditorController controller = new CategoryEditorController();

                var categoryList = new List<CategoryEditor>
                {
                    new CategoryEditor
                    {
                        Name = "My Name",
                        Description = "My Description"
                    },
                    new CategoryEditor
                    {
                        Name = "My Name 2",
                        Description = "My Description 2"
                    }
                };

                ShimCategoriesLogic.AllInstances.GetCategoriesByNameString = (logics, s) => categoryList;

                var result = controller.Search("test test");
                var resultModel = result.Model as List<CategoryEditor>;
                Assert.IsNotNull(resultModel);
                Assert.AreEqual(2, resultModel.Count);
            }
        }

    }
}
