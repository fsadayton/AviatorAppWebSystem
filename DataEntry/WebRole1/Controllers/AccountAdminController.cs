using Models.AccountManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Website.BL;
using Website.Helpers;

namespace Website.Controllers
{

    [AuthorizeRedirect(Roles = "1")]
    public class AccountAdminController : Controller
    {
        public AccountLogics userAccounts = new AccountLogics();

        // GET: AccountAdmin
        public ActionResult Index()
        {   
            return View(userAccounts.GetUsersForAdmin());
        }

        // GET: AccountAdmin/Details/5
        public ActionResult Details(int id)
        {
            var user = userAccounts.GetUserForAdmin(id);
            return View(user);
        }

        /// GET: AccountAdmin/Edit/5
        /// <summary>
        /// Gets an account for editing.
        /// </summary>
        /// <param name="id"> The id. </param>
        /// <returns> The <see cref="ActionResult"/>. </returns>
        public ActionResult Edit(int id)
        {
            var logics = new DataLogics();
            //ViewBag.ServiceProviders = new SelectList(logics.GetServiceProviders(string.Empty), "Id", "Name");
            var user = this.userAccounts.GetUserForAdmin(id);
            return this.View(user);
        }

        // POST: AccountAdmin/Edit/5
        [HttpPost]
        public ActionResult Edit(AccountAdminViewModel accountadminvm)
        {
            try
            {
                var result = userAccounts.UpdateUser(accountadminvm);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: AccountAdmin/Deactivate/5
        [HttpPost]
        public ActionResult Deactivate(int id)
        {
            var user = userAccounts.GetUserForAdmin(id);
            user.IsActive = false;
            var isSuccessful = userAccounts.UpdateUser(user);
            return this.Json(new { success = isSuccessful });
        }
        
        // GET: AccountAdmin/Activate
        [HttpPost]
        public ActionResult Activate(int id)
        {
            //Activate the user
            var user = userAccounts.GetUserForAdmin(id);
            user.IsActive = true;
            userAccounts.UpdateUser(user);
            return this.Json(new{ success=true } );
        }
 
        public ActionResult GetAllUser()
        {
            var foundUsers = this.userAccounts.GetUsersForAdmin();
            return this.Json(foundUsers, JsonRequestBehavior.AllowGet);
        }
        public ViewResult SearchUsers(string searchText)
        {
            var foundUser = this.userAccounts.SearchUsers(searchText);
            return this.View("UserList",foundUser);
        }
    }
}
