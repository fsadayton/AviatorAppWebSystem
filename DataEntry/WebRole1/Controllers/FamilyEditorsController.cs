using Models;
using Models.Editors;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Website.BL;
using Website.Models;

namespace Website.Controllers
{
    [Authorize(Roles = "1")]
    public class FamilyEditorsController : BaseController
    {
        public FamiliesLogic famLogic;
        public CategoriesLogic catLogic;

        /// <summary>
        /// FamilyEditorsController constructor
        /// </summary>
        public FamilyEditorsController() 
        { 
            this.famLogic = new FamiliesLogic();
            this.catLogic = new CategoriesLogic();
        }

        /// <summary>
        ///  GET: CatagoryEditor
        /// </summary>
        public ActionResult Index()
        {
            List<FamilyEditor> families = this.famLogic.GetFamiliesEdit();
            families = FamilySort(families); 
            return this.View(families);
        }

        /// <summary>
        /// Sorts the families to be displayed by Name and Active
        /// </summary>
        /// <param name="families"></param>
        /// <returns></returns>
        private List<FamilyEditor> FamilySort(List<FamilyEditor> families)
        {
            families = families.OrderByDescending(fam => fam.Active)
                .ThenBy(fam => fam.Name)
                .ToList();
            return families;
        }

        /// <summary>
        /// GET: FamilyEditor/Details/5
        /// </summary>
        public ActionResult Details(int id)
        {
            this.SetupViewBag();
            
            var familyDetail = this.famLogic.GetFamilyEditById(id);
            return this.View(familyDetail);
        }

        /// <summary>
        /// GET: FamilyEditor/Create
        /// </summary>
        public ActionResult Create()
        {
            this.SetupViewBag();
            //Inactive family by default, renders empty list of checkboxes
            FamilyEditor famEditor = new FamilyEditor { Active = false, CategoryIds = new List<int>() }; 
            return this.View(famEditor);
        }

        /// <summary>
        /// POST: FamilyEditor/Create
        /// </summary>
        /// <param name="famEditor">FamilyEditor object to be created</param>
        /// <returns>Returns the Create view</returns>
        [HttpPost]
        public ActionResult Create(FamilyEditor famEditor)
        {
            
            try
            {
                if (ModelState.IsValid) { 
                famEditor = CreateHelper(famEditor);
                famLogic.EditFamily(famEditor);

                return this.RedirectToAction("Index");
                }
                this.resetForInvalidModel(famEditor);
                TempData["Error"] = "Entry invalid, please include a category";
                return View(famEditor);
            }
            catch
            {
                    this.resetForInvalidModel(famEditor);
                    TempData["Error"] = "Entry invalid, please include a category";
                    return View(famEditor);
            }
        }

       
        /// <summary>
        /// Sets the FamilyEditor famEditor state to Create and updates the Category
        /// </summary>
        /// <param name="famEditor">FamilyEditor object to have it's State updated</param>
        /// <returns>FamilyEditor object with a state set to Create</returns>
        public FamilyEditor CreateHelper(FamilyEditor famEditor)
        {
            famEditor.State = ObjectStatus.ObjectState.Create; //Set the object's state to Update
            return famEditor;
        }

        /// <summary>
        /// GET: FamilyEditor/Edit/5
        /// </summary>
        /// <param name="id">Index of the FamilyEditor model to display in a view</param>
        /// <returns>View of the FamilyEditor to be edited</returns>
        public ActionResult Edit(int id)
        {
            this.SetupViewBag();

            var editedFamily = this.famLogic.GetFamilyEditById(id);
            return this.View(editedFamily);
        }

        /// <summary>
        /// POST: FamilyEditor/Edit/5
        /// </summary>
        /// <param name="famEditor">FamilyEditor to be edited</param>
        /// <returns>Returns the FamilyEiditor to be edited</returns>
        [HttpPost]
        public ActionResult Edit(FamilyEditor famEditor)
        {
            try
            {
                if (ModelState.IsValid) { 
                var editedFamily = this.EditHelper(famEditor);
                this.famLogic.EditFamily(editedFamily);

                return this.RedirectToAction("Details", new { id = famEditor.Id });
                }
                this.resetForInvalidModel(famEditor);
                TempData["Error"] = "Entry invalid, please include a category";
                return View(famEditor);
            }
            catch
            {
                Console.WriteLine("Error updating FamilyEditor " + famEditor.Id);
                return this.View();
            }
        }

        /// <summary>
        /// Sets the FamilyEditor famEditor state to Update and updates the Category
        /// </summary>
        /// <param name="famEditor">FamilyEditor to have its State set to Edit</param>
        /// <returns>Returns the updated FamilyEditor</returns>
        public FamilyEditor EditHelper(FamilyEditor famEditor)
        {
            famEditor.State = ObjectStatus.ObjectState.Update; //Set the object's state to Update
            return famEditor;
        }

        /// <summary>
        /// GET: FamilyEditor/Deactivate/5
        /// </summary>
        /// <param name="id">ID of the FamilyEdior to be deactivated</param>
        /// <returns>Returns the deactivation confirmation view</returns>
        public ActionResult Deactivate(int id)
        {
            var deactivatedFamily = this.famLogic.GetFamilyEditById(id);
            return this.View(deactivatedFamily);
        }

        /// <summary>
        /// POST: FamilyEditor/Family/5
        /// </summary>
        /// <param name="famEditor">FamilyEditor to be deactivated</param>
        /// <returns>Returns index view</returns>
        [HttpPost]
        public ActionResult Deactivate(FamilyEditor famEditor)
        {
            try
            {
                famEditor = DeactivateHelper(famEditor);

                this.famLogic.EditFamily(famEditor); //Sending the deactivate command to the FamiliesLogic object

                return this.RedirectToAction("Index");
            }
            catch
            {
                Console.WriteLine("Error deactivating FamilyEditor " + famEditor.Id);
                return this.View();
            }
        }

        /// <summary>
        /// Sets the FamilyEditor famEditor state to Deactivate and updates the Category
        /// </summary>
        /// <param name="famEditor">FamilyEditor to have its state changed</param>
        /// <returns>Returns the FamilyEditor with State set to Deactivate</returns>
        public FamilyEditor DeactivateHelper(FamilyEditor famEditor)
        {
            famEditor.State = ObjectStatus.ObjectState.Update;
            famEditor.Active = false;
            return famEditor;
        }

        /// <summary>
        /// GET: FamilyEditor/Activate/5
        /// </summary>
        /// <param name="id">ID of the FamilyEdior to be activated</param>
        /// <returns>Returns the activation confirmation view</returns>
        public ActionResult Activate(int id)
        {
            var activatedFamily = this.famLogic.GetFamilyEditById(id);
            return this.View(activatedFamily);
        }

        /// <summary>
        /// POST: FamilyEditor/Family/5
        /// </summary>
        /// <param name="famEditor">FamilyEditor to be activated</param>
        /// <returns>Returns index view</returns>
        [HttpPost]
        public ActionResult Activate(FamilyEditor famEditor)
        {
            try
            {
                famEditor = ActivateHelper(famEditor);

                this.famLogic.EditFamily(famEditor); //Sending the deactivate command to the FamiliesLogic object

                return this.RedirectToAction("Index");
            }
            catch
            {
                Console.WriteLine("Error activating FamilyEditor " + famEditor.Id);
                return this.View();
            }
        }

        /// <summary>
        /// Sets the FamilyEditor famEditor state to Deactivate and updates the Category
        /// </summary>
        /// <param name="famEditor">FamilyEditor to have its state changed</param>
        /// <returns>Returns the FamilyEditor with State set to Activate</returns>
        public FamilyEditor ActivateHelper(FamilyEditor famEditor)
        {
            famEditor.State = ObjectStatus.ObjectState.Update;
            famEditor.Active = true;
            return famEditor;
        }

        /// <summary>
        /// GET: FamilyEditor/Delete/5
        /// </summary>
        /// <param name="id">ID of the FamilyEdior to be permenantly delted</param>
        /// <returns>Returns the deletion confirmation view</returns>
        public ActionResult Delete(int id)
        {
            var deletedFamily = this.famLogic.GetFamilyEditById(id);
            return this.View(deletedFamily);
        }

        /// <summary>
        /// POST: FamilyEditor/Family/5
        /// </summary>
        /// <param name="famEditor">FamilyEditor to be delete</param>
        /// <returns>Returns index view</returns>
        [HttpPost]
        public ActionResult Delete(FamilyEditor famEditor)
        {
            try
            {
                famEditor = DeleteHelper(famEditor);

                this.famLogic.EditFamily(famEditor); //Sending the delete command to the FamiliesLogic object

                return this.RedirectToAction("Index");
            }
            catch
            {
                Console.WriteLine("Error activating FamilyEditor " + famEditor.Id);
                return this.View();
            }
        }

        /// <summary>
        /// Sets the familyEditor state to Delete
        /// </summary>
        /// <param name="familyEditor">Family editor to delete</param>
        /// <returns>familyEditor with an updated State</returns>
        public FamilyEditor DeleteHelper(FamilyEditor familyEditor)
        {
            familyEditor.State = ObjectStatus.ObjectState.Delete; //Set the object's state to Update
            return familyEditor;
        }

        /// <summary>
        /// Gets all the data needed for the Create view and puts it in the ViewBag
        /// </summary>
        private void SetupViewBag()
        {
            var categories = this.catLogic.GetCategories();
            categories.Sort((category1, category2) => category1.Name.CompareTo(category2.Name));
            ViewBag.AllCategories = categories;
        }

        /// <summary>
        /// Searches the providers and shows the result.
        /// </summary>
        /// <param name="searchText">Name search</param>
        /// <returns>View Result</returns>
        [HttpGet]
        public ViewResult Search(string searchText)
        {
            var foundFamilies = famLogic.GetFamiliesEditByName(searchText);
            return this.View("FamiliesList", foundFamilies);
        }
        private void resetForInvalidModel(FamilyEditor famEditor)
        {
            this.SetupViewBag();
            famEditor.CategoryIds = new List<int>();
        }

    }
}

