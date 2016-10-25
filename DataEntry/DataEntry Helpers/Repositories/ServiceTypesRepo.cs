using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataEntry_Helpers.Repositories
{
    using DataEntry_Helpers.RepositoryInterfaces;

    using Models;

    /// <summary>
    /// The service types repo.
    /// </summary>
    public class ServiceTypesRepo : Repository, IServiceTypes
    {
        /// <summary>
        /// The create new category.
        /// </summary>
        /// <param name="newCategory">
        /// The new category.
        /// </param>
        /// <returns>
        /// The <see cref="CrimeCategory"/>.
        /// </returns>
        public ProviderServiceCategory CreateNewCategory(Category newCategory)
        {
            var databaseCategory = ConvertWebsiteCategory(newCategory);
            var responseCategory = this.db.ProviderServiceCategories.Add(databaseCategory);

            return responseCategory;
        }

        /// <summary>
        /// The get service types.
        /// </summary>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<ServiceType> GetServiceTypes()
        {
            List<ServiceType> types;
            try
            {
                types = this.db.ServiceTypes.ToList();
            }
            catch (Exception e)
            {
                return null;
            }

            return types;
        }

        /// <summary>
        /// The convert website category.
        /// </summary>
        /// <param name="category">
        /// The category.
        /// </param>
        /// <returns>
        /// The <see cref="CrimeCategory"/>.
        /// </returns>
        private static ProviderServiceCategory ConvertWebsiteCategory(Category category)
        {
            var databaseCategory = new ProviderServiceCategory { Description = category.Description, Name = category.Name, Active = true };

            return databaseCategory;
        }
    }
}
