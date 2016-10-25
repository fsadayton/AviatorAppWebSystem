// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FamiliesRepo.cs" company="UDRI">
//   Copyright © 2015 University of Dayton Research Institute. All rights reserved.
// </copyright>
// <summary>
//   The families repo.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DataEntry_Helpers.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using RepositoryInterfaces;

    /// <summary>
    /// The families repo.
    /// </summary>
    public class FamiliesRepo : Repository, IFamilyRepo
    {
        /// <summary>
        /// Get database families that are active and not special populations.
        /// </summary>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<Family> GetFamilies()
        {
            List<Family> families;
            try
            {
                families = this.db.Families.Where(f => f.Active && !f.SpecialPopulation).ToList();
            }
            catch (Exception e)
            {
                families = null;
            }

            return families;
        }


        /// <summary>
        /// Get database families that are active and are special populations.
        /// </summary>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<Family> GetSpecialPopulationFamilies()
        {
            List<Family> families;
            try
            {
                families = this.db.Families.Where(f => f.Active && f.SpecialPopulation).ToList();
            }
            catch (Exception e)
            {
                families = null;
            }

            return families;
        }

        /// <summary>
        /// Get all database families.
        /// </summary>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<Family> GetFamiliesEdit()
        {
            List<Family> families;
            try
            {
                families = this.db.Families.ToList();
            }
            catch (Exception e)
            {
                families = null;
            }

            return families;
        }

        /// <summary>
        /// Get families by name using search string.
        /// </summary>
        /// <param name="familyName">
        /// The family name.
        /// </param>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<Family> GetFamiliesEditByName(string familyName)
        {
            try
            {
                var families = this.db.Families.Where(p => p.Name.ToLower().Contains(familyName.ToLower())).ToList();

                return families;
            }
            catch (Exception)
            {
                return new List<Family>();
            }
        }

        /// <summary>
        /// Get database family with id.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public Family GetFamilyById(int id)
        {
            Family families;
            try
            {
                families = this.db.Families.Single(f => f.ID == id);
            }
            catch (Exception e)
            {
                families = null;
            }

            return families;
        }

        /// <summary>
        /// Get database family services.
        /// </summary>
        /// <param name="familyId">
        /// The family id.
        /// </param>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<FamilyService> GetFamilyServices(int familyId)
        {
            List<FamilyService> familyServices;
            try
            {
                familyServices = this.db.FamilyServices.Where(f => f.FamilyID == familyId).ToList();
            }
            catch (Exception e)
            {
                familyServices = null;
            }

            return familyServices;
        }

        /// <summary>
        /// Create database family service.
        /// </summary>
        /// <param name="familyService">
        /// The family service.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool CreateFamilyService(FamilyService familyService)
        {
            try
            {
                this.db.FamilyServices.Add(familyService);
                this.db.SaveChanges();

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        /// <summary>
        /// Delete all family services for a family.
        /// </summary>
        /// <param name="familyId">
        /// The family id.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool DeleteFamilyServices(int familyId)
        {
            var familyServices = this.GetFamilyServices(familyId);
            if (familyServices == null)
            {
                return false;
            }

            try
            {
                foreach (var familyService in familyServices)
                {
                    this.db.FamilyServices.Remove(familyService);
                    this.db.SaveChanges();
                }
                

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        /// <summary>
        /// Create database family.
        /// </summary>
        /// <param name="family">
        /// The family.
        /// </param>
        /// <returns>
        /// The <see cref="Family"/>.
        /// </returns>
        public Family CreateFamily(Family family)
        {
            try
            {
                var newFamily = this.db.Families.Add(family);
                this.db.SaveChanges();

                return newFamily;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        /// <summary>
        /// Update database family.
        /// </summary>
        /// <param name="family">
        /// The family.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool UpdateFamily(Family family)
        {
            try
            {
                var databaseFamily = this.db.Families.Single(d => d.ID == family.ID);
                var mergedFamily = this.MergeFamily(family, databaseFamily);

                this.db.Entry(mergedFamily).State = EntityState.Modified;
                this.db.SaveChanges();

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        /// <summary>
        /// Delete database family.
        /// </summary>
        /// <param name="family">
        /// The family.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool DeleteFamily(Family family)
        {
            try
            {
                this.db.Families.Attach(family);
                this.db.Entry(family).State = EntityState.Deleted;
                this.db.SaveChanges();

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        /// <summary>
        /// Update database family services.
        /// </summary>
        /// <param name="familyId">
        /// The family id.
        /// </param>
        /// <param name="newList">
        /// The new list.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool UpdateFamilyServices(int familyId, List<int> newList)
        {
            if (newList == null)
            {
                return true;
            }

            try
            {
                var currentCategories = this.db.FamilyServices.Where(c => c.FamilyID == familyId).ToList();
                var oldList = currentCategories.Select(currentCategory => currentCategory.ServiceID).ToList();

                var fistCheck = newList.Except(oldList);

                foreach (var i in fistCheck)
                {
                    var serviceId = i;
                    var providerService = new FamilyService { FamilyID = familyId, ServiceID = serviceId };
                    this.db.FamilyServices.Add(providerService);
                    this.db.SaveChanges();
                }

                var secondCheck = oldList.Except(newList);

                foreach (var i in secondCheck)
                {
                    var serviceId = i;
                    var service = this.db.FamilyServices.Single(x => x.FamilyID == familyId && x.ServiceID == serviceId);
                    this.db.FamilyServices.Remove(service);
                    this.db.SaveChanges();
                }
            }
            catch (Exception e)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Merge family and editor.
        /// </summary>
        /// <param name="editor">
        /// The editor.
        /// </param>
        /// <param name="databaseFamily">
        /// The database family.
        /// </param>
        /// <returns>
        /// The <see cref="Family"/>.
        /// </returns>
        public Family MergeFamily(Family editor, Family databaseFamily)
        {
            databaseFamily.Active = editor.Active;
            databaseFamily.Description = editor.Description ?? databaseFamily.Description;
            databaseFamily.Name = editor.Name ?? databaseFamily.Name;
            databaseFamily.SpecialPopulation = editor.SpecialPopulation;
            
            return databaseFamily;
        }
    }
}
