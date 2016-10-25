namespace Website.BL
{
    using System.Collections.Generic;
    using System.Linq;

    using DataEntry_Helpers;
    using DataEntry_Helpers.Repositories;
    using DataEntry_Helpers.RepositoryInterfaces;

    using global::Models;

    /// <summary>
    /// The service types logics.
    /// </summary>
    public class ServiceTypesLogics
    {
        /// <summary>
        /// The DataAccess Repository.
        /// </summary>
        private readonly IDataAccess dataAccess;

        /// <summary>
        /// The ServiceTypes Repository.
        /// </summary>
        private readonly IServiceTypes typesAccess;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceTypesLogics"/> class.
        /// </summary>
        public ServiceTypesLogics()
        {
            this.dataAccess = new DataAccess();
            this.typesAccess = new ServiceTypesRepo();
        }

        /// <summary>
        /// The create new category.
        /// </summary>
        /// <param name="newCategory">
        /// The new category.
        /// </param>
        /// <returns>
        /// The <see cref="Category"/>.
        /// </returns>
        public Category CreateNewCategory(Category newCategory)
        {
            var databaseCategory = this.typesAccess.CreateNewCategory(newCategory);
            var category = this.ConvertDatabaseCategory(databaseCategory);

            return category;
        }

        /// <summary>
        /// The get categories.
        /// </summary>
        /// <returns>
        /// The <see cref="CreateCategories"/>.
        /// </returns>
        public List<Category> GetCategories()
        {
            var databaseCategories = this.dataAccess.GetCategories();
            var websiteCategories = this.ConvertDatabaseCategories(databaseCategories);

            return websiteCategories;
        }

        /// <summary>
        /// The get service types.
        /// </summary>
        /// <returns>
        /// The <see cref="Dictionary"/>.
        /// </returns>
        public Dictionary<int, string> GetServiceTypes()
        {
            var databaseTypes = this.typesAccess.GetServiceTypes();
            var websiteTypes = this.ConvertDatabaseTypes(databaseTypes);

            return websiteTypes;
        }

        /// <summary>
        /// The convert database types.
        /// </summary>
        /// <param name="databaseTypes">
        /// The database types.
        /// </param>
        /// <returns>
        /// The <see cref="Dictionary"/>.
        /// </returns>
        public Dictionary<int, string> ConvertDatabaseTypes(List<ServiceType> databaseTypes)
        {
            return databaseTypes.ToDictionary(databaseType => databaseType.ID, databaseType => databaseType.Name);
        }

        /// <summary>
        /// The convert database categories.
        /// </summary>
        /// <param name="databaseCategory">
        /// The database categories.
        /// </param>
        /// <returns>
        /// The <see cref="CreateCategories"/>.
        /// </returns>
        public Category ConvertDatabaseCategory(ProviderServiceCategory databaseCategory)
        {
            var category = new Category
                               {
                                   Id = databaseCategory.ID,
                                   Name = databaseCategory.Name,
                                   Description = databaseCategory.Description
                               };

            return category;
        }

        /// <summary>
        /// The convert database categories.
        /// </summary>
        /// <param name="databaseCategories">
        /// The database categories.
        /// </param>
        /// <returns>
        /// The <see cref="CreateCategories"/>.
        /// </returns>
        public List<Category> ConvertDatabaseCategories(List<ProviderServiceCategory> databaseCategories)
        {
            var list =
                databaseCategories.Select(
                    databaseCategory =>
                    new Category
                    {
                        Id = databaseCategory.ID,
                        Name = databaseCategory.Name,
                        Description = databaseCategory.Description,
                        Crime = databaseCategory.Crime
                    }).ToList();

            return list;
        }
    }
}