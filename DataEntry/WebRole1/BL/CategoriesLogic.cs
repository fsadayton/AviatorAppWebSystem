// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CategoriesLogic.cs" company="UDRI">
//   Copyright © 2015 University of Dayton Research Institute. All rights reserved.
// </copyright>
// <summary>
//   The categories logic.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using Models.ServiceProvider;

namespace Website.BL
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.WebPages;

    using DataEntry_Helpers;
    using DataEntry_Helpers.Repositories;
    using DataEntry_Helpers.RepositoryInterfaces;

    using Microsoft.Ajax.Utilities;

    using global::Models;

    using global::Models.Editors;

    /// <summary>
    /// The categories logic.
    /// </summary>
    public class CategoriesLogic
    {
        /// <summary>
        /// The data access.
        /// </summary>
        private readonly IDataAccess dataAccess;

        /// <summary>
        /// Category repo.
        /// </summary>
        private readonly ICategoryRepo categoryRepo;

        /// <summary>
        /// Initializes a new instance of the <see cref="CategoriesLogic"/> class.
        /// </summary>
        public CategoriesLogic()
        {
            this.dataAccess = new DataAccess();
            this.categoryRepo = new CategoryRepo();
        }

        /// <summary>
        /// Get categories for editor.
        /// </summary>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<CategoryEditor> GetCategories()
        {
            var databaseCategories = this.dataAccess.GetAllCategories();
            var editorCategories = this.ConvertDatabaseCategories(databaseCategories);

            return editorCategories;
        }

        /// <summary>
        /// The get categories by name.
        /// </summary>
        /// <param name="categoriesText">
        /// The categories text.
        /// </param>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<CategoryEditor> GetCategoriesByName(string categoriesText)
        {
            List<ProviderServiceCategory> databaseFamilies;
            if (categoriesText.IsEmpty() || categoriesText.IsNullOrWhiteSpace())
            {
                databaseFamilies = this.dataAccess.GetAllCategories();
            }
            else
            {
                databaseFamilies = this.categoryRepo.GetCategoryByName(categoriesText);
            }

            var list = new List<CategoryEditor>();

            if (databaseFamilies == null || databaseFamilies.Count == 0)
            {
                return list;
            }

            list = this.ConvertDatabaseCategories(databaseFamilies);

            return list;
        }

        /// <summary>
        /// Edit categories .
        /// </summary>
        /// <param name="categories">
        /// The categories.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool EditCategories(List<CategoryEditor> categories)
        {
            return this.CheckCategories(categories);
        }

        /// <summary>
        /// Merge database and edit categories.
        /// </summary>
        /// <param name="categoryEditor">
        /// The category editor.
        /// </param>
        /// <param name="databaseCategory">
        /// The database category.
        /// </param>
        /// <returns>
        /// The <see cref="ProviderServiceCategory"/>.
        /// </returns>
        public ProviderServiceCategory MergeCategories(CategoryEditor categoryEditor, ProviderServiceCategory databaseCategory)
        {
            databaseCategory.Active = categoryEditor.Active;
            databaseCategory.Crime = categoryEditor.Crime;
            databaseCategory.Description = categoryEditor.Description;
            databaseCategory.Name = categoryEditor.Name;
            databaseCategory.CategoryTypes = new List<CategoryType>();
            foreach (var categoryId in categoryEditor.CategoryTypes)
            {
                var foundCategory = databaseCategory.CategoryTypes.Where(categoryType => categoryType.ServiceType != null && categoryType.ServiceType.ID == categoryId).ToList();
                if (foundCategory.Count == 0 || foundCategory[0].ID == 0)
                {
                    databaseCategory.CategoryTypes.Add(new CategoryType
                    {
                        CategoryId = categoryEditor.Id,
                        ServiceTypeId = categoryId
                    });
                }
            }

            return databaseCategory;
        }

        /// <summary>
        /// Routes the categories based on objectState.
        /// </summary>
        /// <param name="categories">
        /// The categories.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        private bool CheckCategories(List<CategoryEditor> categories)
        {
            foreach (var categoryEditor in categories)
            {
                bool response;
                switch (categoryEditor.State)
                {
                    case ObjectStatus.ObjectState.Update:
                        response = this.UpdateCategory(categoryEditor);
                        break;
                    case ObjectStatus.ObjectState.Delete:
                        response = this.DeactivateCategory(categoryEditor);
                        break;
                    case ObjectStatus.ObjectState.Create:
                        response = this.CreateCategory(categoryEditor);
                        break;
                    case ObjectStatus.ObjectState.Read:
                        response = true;
                        break;
                    default:
                        response = false;
                        break;
                }

                if (!response)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Create a category.
        /// </summary>
        /// <param name="categoryEditor">
        /// The category editor.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        private bool CreateCategory(CategoryEditor categoryEditor)
        {
            var newCategory = this.MergeCategories(categoryEditor, new ProviderServiceCategory());
            var databaseResponse = this.categoryRepo.CreateCategory(newCategory);

            return databaseResponse;
        }

        /// <summary>
        /// Deactivates a category.
        /// </summary>
        /// <param name="categoryEditor">
        /// The category editor.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        private bool DeactivateCategory(CategoryEditor categoryEditor)
        {
            var databaseResponse = this.categoryRepo.DeactivateCategory(categoryEditor.Id);

            return databaseResponse;
        }

        /// <summary>
        /// Update a category.
        /// </summary>
        /// <param name="categoryEditor">
        /// The category editor.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        private bool UpdateCategory(CategoryEditor categoryEditor)
        {
            var databaseCategory = this.categoryRepo.GetCategoryById(categoryEditor.Id);
            if (databaseCategory == null)
            {
                return false;
            }

            var categoryUpdate = this.MergeCategories(categoryEditor, databaseCategory);
            var repositoryResponse = this.categoryRepo.UpdateCategory(categoryUpdate);

            return repositoryResponse;
        }

        /// <summary>
        /// Converts database categories into object for editor.
        /// </summary>
        /// <param name="databaseCategories">
        /// The database categories.
        /// </param>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        private List<CategoryEditor> ConvertDatabaseCategories(List<ProviderServiceCategory> databaseCategories)
        {
            var list = new List<CategoryEditor>();
            
            foreach (var databaseCategory in databaseCategories)
            {
                list.Add(new CategoryEditor
               {
                   Id = databaseCategory.ID,
                   Name = databaseCategory.Name,
                   Description = databaseCategory.Description,
                   Crime = databaseCategory.Crime,
                   Active = databaseCategory.Active,
                   CategoryTypes = databaseCategory.CategoryTypes.Select(categoryType => categoryType.ServiceTypeId).ToList(),
                   CategoryTypesNames = databaseCategory.CategoryTypes.Select(categoryType => new WebsiteCategoryType
                   {
                       Id = categoryType.ServiceTypeId,
                       Name = categoryType.ServiceType.Name
                   }).ToList(),
                   State = ObjectStatus.ObjectState.Read
               });
            }
            return list;
        }
    }
}