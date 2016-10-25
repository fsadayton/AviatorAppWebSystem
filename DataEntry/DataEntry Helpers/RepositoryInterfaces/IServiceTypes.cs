using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataEntry_Helpers.RepositoryInterfaces
{
    using System.Security.Cryptography.X509Certificates;

    using Models;

    /// <summary>
    /// The ServiceTypes interface.
    /// </summary>
    public interface IServiceTypes
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
        ProviderServiceCategory CreateNewCategory(Category newCategory);

        /// <summary>
        /// The get service types.
        /// </summary>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        List<ServiceType> GetServiceTypes();
    }
}
