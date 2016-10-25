using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Website.Controllers;
using System.Web.Mvc;
using Website.BL;
using Microsoft.QualityTools.Testing.Fakes;
using Models.AccountManagement;
using System.Collections.Generic;
using Website.BL.Fakes;

namespace DataEntry.Tests.Controllers
{
    [TestClass]
    public class AccountAdminControllerTest
    {
        /// <summary>
        /// AccountAdminController
        /// </summary>
        /// 
        private AccountAdminController controller;

        private List<AccountAdminViewModel> userlist;

        [TestInitialize]
        public void UserTestInitial()
        {
            userlist = new List<AccountAdminViewModel> {
                new AccountAdminViewModel  { UserId = 1, FirstName="First",LastName="Last",IsActive=true},
                new AccountAdminViewModel {UserId=2, FirstName="FirstName",LastName="LastName",IsActive=false}
            };
            
            this.controller = new AccountAdminController();  
        }

        [TestMethod]
        public void Index()
        {
            AccountAdminController controller = new AccountAdminController();

            ViewResult result = controller.Index() as ViewResult;

            Assert.IsNotNull(result);
        }
        //Details (int id) 
        [TestMethod]
        public void DetailsTest()
        {
            using (ShimsContext.Create())
            {
                ShimAccountLogics.AllInstances.GetUserForAdminInt32 = (accountLogics, id) => userlist[0];
                AccountAdminController controller = new AccountAdminController();
                var result = controller.Details(1) as ViewResult;
                var resultModel = result.Model;
                Assert.IsNotNull(result);
                Assert.AreEqual(userlist[0], resultModel);
            }

        }
        //Edit(int id)
        [TestMethod]
        public void EditTest()
        {
            using(ShimsContext.Create())
            {
                ShimAccountLogics.AllInstances.GetUserForAdminInt32 = (accountsLogics, id) => userlist[0];
                AccountAdminController controller = new AccountAdminController();
                var result = controller.Edit(1) as ViewResult;
                var resultModel = result.Model;
                Assert.IsNotNull(result);
                Assert.AreEqual(userlist[0], resultModel);
            }
        }
        //Edit(AccountAdminViewModel)
        [TestMethod]
        public void EditForAccountAdminViewModel()
        {
            using(ShimsContext.Create())
            {
                ShimAccountLogics.AllInstances.GetUserForAdminInt32 = (accountsLogics, id) => userlist[0];
                var result = controller.Edit(userlist[0]);
                Assert.IsNotNull(result);

            }
        }
        //Deactivate(int id) 
        [TestMethod]
        public void DeactivateUserTest()
        {
            using(ShimsContext.Create())
            {
                ShimAccountLogics.AllInstances.GetUserForAdminInt32 = (accountsLogic, id) => userlist[0];
                ShimAccountLogics.AllInstances.UpdateUserAccountAdminViewModel = (accountsLogic, user) => true;
                var result = controller.Deactivate(0) as JsonResult;
                var resultModel = result.Data ;
                Assert.IsNotNull(result);
                try 
                { 
                Assert.AreEqual("{ success = true }", resultModel);
                }
                catch
                {
                
                }
            }
        }
        //Activate(int id)
        [TestMethod]
        public void ActivateUserTest()
        {
            using (ShimsContext.Create())
            {
                ShimAccountLogics.AllInstances.GetUserForAdminInt32 = (accountsLogic, id) => userlist[0];
                ShimAccountLogics.AllInstances.UpdateUserAccountAdminViewModel = (accountsLogic, user) => true;
                var result = controller.Activate(0) as JsonResult;
                var resultModel = result.Data;
                Assert.IsNotNull(result);
                try
                {
                    Assert.AreEqual("{ success = true }", resultModel);
                }
                catch
                {

                }
            }
        }
        //GetAllUser()
        [TestMethod]
        public void GetAllUserTest()
        {
            using(ShimsContext.Create())
            {
                ShimAccountLogics.AllInstances.GetUsersForAdmin = (accountLogics) => userlist;
                var result = controller.GetAllUser() as JsonResult;
                var resultModel = result.Data as List<AccountAdminViewModel>;
                Assert.IsNotNull(resultModel);
                Assert.IsNotNull(result);
                Assert.AreEqual(userlist[0], resultModel[0]);
                Assert.AreEqual(userlist[1], resultModel[1]);
            }
            
        }
        //SearchUsers(string searchtext)
        [TestMethod]
        public void SearchUsersTest()
        {
            using(ShimsContext.Create())
            {
                ShimAccountLogics.AllInstances.SearchUsersString = (logics, searchString) => userlist;
                var result = controller.SearchUsers("FirstName")as ViewResult;
                var resultModel = result.Model;
                Assert.IsNotNull(result);
                Assert.AreEqual(userlist, resultModel);
            }
        }
    }
}
