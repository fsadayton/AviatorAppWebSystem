using Models;
using Models.Editors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Website.BL;

namespace Website.Controllers
{
    using System.Web.Http.Cors;

    [EnableCors("*", "*", "*")]
    [Authorize(Roles = "1")]
    public class CategoryEditorController : BaseController
    {

        public readonly CategoriesLogic catLogic;

        public CategoryEditorController() 
        { 
            //TODO: change to global initialization
            this.catLogic = new CategoriesLogic();
        }

        // GET: CatagoryEditor
        public ActionResult Index()
        {
            CategoriesLogic ct = new CategoriesLogic();
            var categories = ct.GetCategories();
            return this.View(categories);
        }

        // GET: CatagoryEditor/Details/5
        public ActionResult Details(int id)
        {
            CategoriesLogic ct = new CategoriesLogic();
            List<CategoryEditor> catEditors = ct.GetCategories();
            CategoryEditor editedCategory = catEditors.Single(c => c.Id == id);
            return View(editedCategory);
        }

        // GET: CatagoryEditor/Create
        public ActionResult Create()
        {
                var serviceInfo = new ServiceTypesLogics();
                ViewBag.types = serviceInfo.GetServiceTypes().ToList().Select(x => new SelectListItem()
                {
                    Value = x.Key.ToString(),
                    Text = x.Value
                });
                CategoryEditor catEditor = new CategoryEditor();
                catEditor.Active = true;
                return this.View(catEditor);
        }

        // POST: CatagoryEditor/Create
        [HttpPost]
        public ActionResult Create(CategoryEditor catEditor)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var serviceInfo = new ServiceTypesLogics();
                    ViewBag.types = serviceInfo.GetServiceTypes().ToList().Select(x => new SelectListItem()
                    {
                        Value = x.Key.ToString(),
                        Text = x.Value
                    });
                    return View(catEditor);
                }

                CategoriesLogic catLogic = new CategoriesLogic();
                catEditor.State = ObjectStatus.ObjectState.Create; //Set the object's state to Create
                List<CategoryEditor> catEditList = new List<CategoryEditor>();
                catEditList.Add(catEditor);
                catLogic.EditCategories(catEditList);

                return this.RedirectToAction("Index");
            }
            catch
            {
                Console.WriteLine("Error creating new CategoryEditor");
                return View();
            }
        }

        // GET: CatagoryEditor/Edit/5
        public ActionResult Edit(int id)
        {
            CategoriesLogic ct = new CategoriesLogic();
            List<CategoryEditor> catEditors = ct.GetCategories();
            CategoryEditor editedCategory = catEditors.Single(c => c.Id == id);
            return View(editedCategory);
        }

        // POST: CatagoryEditor/Edit/5
        [HttpPost]
        public ActionResult Edit(CategoryEditor catEditor)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(catEditor);
                }
                CategoriesLogic catLogic = new CategoriesLogic();

                var catEditList = this.EditHelper(catEditor);
                catLogic.EditCategories(catEditList);

                return this.RedirectToAction("Index");
            }
            catch
            {
                Console.WriteLine("Error updating CategoryEditor " + catEditor.Id);
                return View();
            }
        }

        //Sets the CateogryEditor catEditor state to Update and updates the Category
        public List<CategoryEditor> EditHelper(CategoryEditor catEditor)
        {
            catEditor.State = ObjectStatus.ObjectState.Update; //Set the object's state to Update
            List<CategoryEditor> catEditList = new List<CategoryEditor>();
            catEditList.Add(catEditor);
            return catEditList;
        }

        // GET: CatagoryEditor/Delete/5
        public ActionResult Deactivate(int id)
        {
            CategoriesLogic ct = new CategoriesLogic();
            List<CategoryEditor> catEditors = ct.GetCategories();
            CategoryEditor deletedCategory = catEditors.Single(c => c.Id == id);
            return View(deletedCategory);
        }

        // POST: CatagoryEditor/Delete/5
        [HttpPost]
        public ActionResult Deactivate(CategoryEditor catEditor)
        {
            try
            {
                CategoriesLogic catLogic = new CategoriesLogic();
                catEditor.State = ObjectStatus.ObjectState.Delete; //Set the object's state to Delete
                List<CategoryEditor> catDelList = new List<CategoryEditor>();
                catDelList.Add(catEditor);

                catLogic.EditCategories(catDelList); //Sending the delete command to the CategoriesLogic object

                return this.RedirectToAction("Index");
            }
            catch
            {
                Console.WriteLine("Error deleting CategoryEditor " + catEditor.Id);
                return View();
            }
        }

        /// <summary>
        /// Searches the providers and shows the result.
        /// </summary>
        /// <param name="searchText">Name search</param>
        /// <returns>View Result</returns>
        [HttpGet]
        public ViewResult Search(string searchText)
        {
            var foundCategories = catLogic.GetCategoriesByName(searchText); 
            return this.View("CategoriesList", foundCategories);
        }
    }
}
