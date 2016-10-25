using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataEntry_Helpers.Repositories
{
    using System.Data.Entity;

    using DataEntry_Helpers.RepositoryInterfaces;

    /// <summary>
    /// The category repository.
    /// </summary>
    public class CategoryRepo : Repository, ICategoryRepo
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
        public ProviderServiceCategory GetCategoryById(int id)
        {
            ProviderServiceCategory response;
            try
            {
                response = this.db.ProviderServiceCategories.Single(r => r.ID == id);
            }
            catch (Exception e)
            {
                response = null;
            }

            return response;
        }

        /// <summary>
        /// The get category by name.
        /// </summary>
        /// <param name="categoryText">
        /// The categories.
        /// </param>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<ProviderServiceCategory> GetCategoryByName(string categoryText)
        {
            List<ProviderServiceCategory> response;
            try
            {
                response =
                    this.db.ProviderServiceCategories.Where(p => p.Name.ToLower().Contains(categoryText.ToLower()))
                        .ToList();
            }
            catch (Exception)
            {
                response = null;
            }

            return response;
        }


        /// <summary>
        /// The create category.
        /// </summary>
        /// <param name="category">
        /// The category.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool CreateCategory(ProviderServiceCategory category)
        {
            try
            {
                this.db.ProviderServiceCategories.Add(category);
                this.db.SaveChanges();


                foreach (var categoryType in category.CategoryTypes)
                {
                    categoryType.ID = category.ID;
                    this.db.CategoryTypes.Add(categoryType);
                }

                this.db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        /// <summary>
        /// The update category.
        /// </summary>
        /// <param name="category">
        /// The category.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool UpdateCategory(ProviderServiceCategory category)
        {
            try
            {
                this.db.Entry(category).State = EntityState.Modified;
                this.db.SaveChanges();

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        /// <summary>
        /// The deactivate category.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool DeactivateCategory(int id)
        {
            try
            {
                var category = this.db.ProviderServiceCategories.Single(s => s.ID == id);

                category.Active = false;

                this.db.Entry(category).State = EntityState.Modified;
                this.db.SaveChanges();

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        /// <summary>
        /// Delete a category.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool DeleteCategory(int id)
        {
            try
            {
                var category = this.db.ProviderServiceCategories.Single(s => s.ID == id);
                this.db.ProviderServiceCategories.Remove(category);
                this.db.SaveChanges();

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

    }
}
