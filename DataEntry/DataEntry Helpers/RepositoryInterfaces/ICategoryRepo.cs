using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataEntry_Helpers.RepositoryInterfaces
{
    public interface ICategoryRepo
    {
        /// <summary>
        /// The get category by id.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="ProviderServiceCategory"/>.
        /// </returns>
        ProviderServiceCategory GetCategoryById(int id);

        /// <summary>
        /// The get all categories by name.
        /// </summary>
        /// <param name="categoryText">
        /// The category text.
        /// </param>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        List<ProviderServiceCategory> GetCategoryByName(string categoryText);

        /// <summary>
        /// Create a category.
        /// </summary>
        /// <param name="category">
        /// The category.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        bool CreateCategory(ProviderServiceCategory category);

        /// <summary>
        /// Update a category.
        /// </summary>
        /// <param name="category">
        /// The category.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        bool UpdateCategory(ProviderServiceCategory category);

        /// <summary>
        /// Deactivate a category.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        bool DeactivateCategory(int id);

        /// <summary>
        /// Delete a category.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        bool DeleteCategory(int id);
    }
}
