// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FamiliesLogic.cs" company="UDRI">
//   Copyright © 2015 University of Dayton Research Institute. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
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

    using Website.Models;

    /// <summary>
    /// Families logic.
    /// </summary>
    public class FamiliesLogic
    {
        /// <summary>
        /// The family repo.
        /// </summary>
        private readonly IFamilyRepo familyRepo;

        /// <summary>
        /// Initializes a new instance of the <see cref="FamiliesLogic"/> class.
        /// </summary>
        public FamiliesLogic()
        {
            this.familyRepo = new FamiliesRepo();
        }

        /// <summary>
        /// Get families for display.
        /// </summary>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<FamilyEditor> GetFamilies()
        {
            var databaseFamilies = this.familyRepo.GetFamilies();
            return this.CreateWebFamilies(databaseFamilies);
        }

        public List<FamilyEditor> GetSpecialPopulationFamilies()
        {
            var databaseFamilies = this.familyRepo.GetSpecialPopulationFamilies();
            return this.CreateWebFamilies(databaseFamilies);
        } 

        public FamilyEditor GetFamilyEditById(int id)
        {
            var databaseFamily = this.familyRepo.GetFamilyById(id);
            return  this.CreateWebFamily(databaseFamily);
        }

        /// <summary>
        /// Get families for admin.
        /// </summary>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<FamilyEditor> GetFamiliesEdit()
        {
            var databaseFamilies = this.familyRepo.GetFamiliesEdit();
            return this.CreateWebFamilies(databaseFamilies);
        }

        /// <summary>
        /// Get FamiliesEditor by name using search string.
        /// </summary>
        /// <param name="familyName">
        /// The provider Name.
        /// </param>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<FamilyEditor> GetFamiliesEditByName(string familyName)
        {
            List<Family> databaseFamilies;
            if (familyName.IsEmpty() || familyName.IsNullOrWhiteSpace())
            {
                databaseFamilies = this.familyRepo.GetFamiliesEdit();
            }
            else
            {
                databaseFamilies = this.familyRepo.GetFamiliesEditByName(familyName);
            }

            var list = new List<FamilyEditor>();

            if (databaseFamilies == null || databaseFamilies.Count == 0)
            {
                return list;
            }
            
            foreach (var databaseFamily in databaseFamilies)
            {
                var family = this.CreateWebFamily(databaseFamily);

                list.Add(family);
            }

            return list;
        }

        /// <summary>
        /// Edit family.
        /// </summary>
        /// <param name="editor">
        /// The editor.
        /// </param>
        /// <returns>
        /// The <see cref="FamilyEditor"/>.
        /// </returns>
        public bool EditFamily(FamilyEditor editor)
        {
            bool success;
            switch (editor.State)
            {
                case ObjectStatus.ObjectState.Update:
                    success = this.UpdateDatabaseFamily(editor);
                    break;
                case ObjectStatus.ObjectState.Delete:
                    success = this.DeleteDatabaseFamily(editor);
                    break;
                case ObjectStatus.ObjectState.Create:
                    success = this.CreateDatabaseFamily(editor);
                    break;
                default:
                    success = true;
                    break;
            }

            return success;
        }

        /// <summary>
        /// Delete database family.
        /// </summary>
        /// <param name="editor">
        /// The editor.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool DeleteDatabaseFamily(FamilyEditor editor)
        {
            var success = this.familyRepo.DeleteFamilyServices(editor.Id);
            return this.familyRepo.DeleteFamily(this.familyRepo.GetFamilyById(editor.Id));
        }

        /// <summary>
        /// Create families for display.
        /// </summary>
        /// <param name="databaseFamilies">
        /// The database families.
        /// </param>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<FamilyEditor> CreateWebFamilies(List<Family> databaseFamilies)
        {
            return databaseFamilies?.Select(databaseFamily => new FamilyEditor
            {
                Id = databaseFamily.ID,
                Name = databaseFamily.Name,
                Description = databaseFamily.Description,
                Active = databaseFamily.Active,
                State = ObjectStatus.ObjectState.Read,
                CategoryIds = this.CreateCategoryIds(this.familyRepo.GetFamilyServices(databaseFamily.ID)),
                IsSpecialPopulation = databaseFamily.SpecialPopulation
            }).ToList();
        }

        /// <summary>
        /// The create web family.
        /// </summary>
        /// <param name="databaseFamily">
        /// The database family.
        /// </param>
        /// <returns>
        /// The <see cref="FamilyEditor"/>.
        /// </returns>
        private FamilyEditor CreateWebFamily(Family databaseFamily)
        {
            return new FamilyEditor
            {
                Id = databaseFamily.ID,
                Name = databaseFamily.Name,
                Description = databaseFamily.Description,
                Active = databaseFamily.Active,
                State = ObjectStatus.ObjectState.Read,
                CategoryIds =
                    this.CreateCategoryIds(
                        this.familyRepo.GetFamilyServices(databaseFamily.ID)),
                IsSpecialPopulation =  databaseFamily.SpecialPopulation
            };
        }

        /// <summary>
        /// Update database family.
        /// </summary>
        /// <param name="editor">
        /// The editor.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool UpdateDatabaseFamily(FamilyEditor editor)
        {
            var editedFamily = this.CreateADatabaseFamily(editor);

            var success = this.familyRepo.UpdateFamily(editedFamily);
            return success && this.familyRepo.UpdateFamilyServices(editor.Id, editor.CategoryIds);
        }

        /// <summary>
        /// Create a database family.
        /// </summary>
        /// <param name="editor">
        /// The editor.
        /// </param>
        /// <returns>
        /// The <see cref="Family"/>.
        /// </returns>
        private Family CreateADatabaseFamily(FamilyEditor editor)
        {
            return new Family
            {
                Active = editor.Active,
                Description = editor.Description,
                ID = editor.Id,
                Name = editor.Name,
                SpecialPopulation = editor.IsSpecialPopulation
            };
        }

        /// <summary>
        /// Create database family.
        /// </summary>
        /// <param name="editor">
        /// The editor.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool CreateDatabaseFamily(FamilyEditor editor)
        {

            var family = new Family
            {
                Active = editor.Active,
                Description = editor.Description,
                Name = editor.Name,
                SpecialPopulation = editor.IsSpecialPopulation
            };

            var newFamily = this.familyRepo.CreateFamily(family);
            if (newFamily == null)
            {
                return false;
            }

            foreach (var categoryId in editor.CategoryIds)
            {
                var created = this.CreateFamilyService(newFamily.ID, categoryId);
            }

            return true;
        }

        /// <summary>
        /// Create family service.
        /// </summary>
        /// <param name="familyId">
        /// The family id.
        /// </param>
        /// <param name="categoryId">
        /// The category id.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool CreateFamilyService(int familyId, int categoryId)
        {
            var count = 0;
            var service = new FamilyService { FamilyID = familyId, ServiceID = categoryId };
            var success = this.familyRepo.CreateFamilyService(service);

            while (!success && count < 6)
            {
                count++;
                success = this.familyRepo.CreateFamilyService(service);
            }

            return success;
        }

        /// <summary>
        /// Create category ids for display.
        /// </summary>
        /// <param name="databaseFamilyServices">
        /// The database family services.
        /// </param>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<int> CreateCategoryIds(List<FamilyService> databaseFamilyServices)
        {
            return databaseFamilyServices.Select(familyService => familyService.ServiceID).ToList();
        }

    }
}