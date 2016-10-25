using System.Linq;
using System.Web.Mvc;
using Models;
using Website.BL;
using Website.BL.ModelConversions;

namespace Website.Controllers
{
    public class SpecialPopulationsController : Controller
    {
        // GET: PersonalResources

        /// <summary>
        /// Get all the personal resources for display.
        /// </summary>
        /// <returns> The <see cref="ActionResult"/>. </returns>
        public ActionResult Index(string family, string category)
        {
            if (!string.IsNullOrEmpty(family))
            {
                return this.View("ServiceProviderList");
            }

            var familiesLogic = new FamiliesLogic();
            var viewModel = familiesLogic.GetSpecialPopulationFamilies().OrderBy(fam => fam.Name).ToList();

            return this.View(viewModel);
        }

        /// <summary>
        /// Shows a service provider
        /// </summary>
        /// <param name="id">id of the provider to be shown</param>
        /// <param name="locationId">Which location to show. (optional)</param>
        /// <returns></returns>
        public ActionResult Details(int id, int? locationId)
        {
            var dataLogics = new DataLogics();
            var categories = dataLogics.GetWebsiteCategories();
            categories.Sort((category1, category2) => category1.Name.CompareTo(category2.Name));
            ViewBag.AllCategories = categories;

            var dbConverter = new DatabaseToWebServiceProvider();
            return this.View(dbConverter.GetServiceProvider(id));
        }

    }
}