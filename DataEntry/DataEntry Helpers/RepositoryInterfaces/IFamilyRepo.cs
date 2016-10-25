using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataEntry_Helpers.RepositoryInterfaces
{
    public interface IFamilyRepo
    {
        /// <summary>
        /// Get database families that are active.
        /// </summary>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        List<Family> GetFamilies();


        /// <summary>
        /// Get the database families that are active and are special populations.
        /// </summary>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        List<Family> GetSpecialPopulationFamilies();

            /// <summary>
        /// Get all database families.
        /// </summary>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        List<Family> GetFamiliesEdit();

        /// <summary>
        /// Get families by name using search string.
        /// </summary>
        /// <param name="familyName">
        /// The family name.
        /// </param>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        List<Family> GetFamiliesEditByName(string familyName);
            
        /// <summary>
        /// Get family edit by id.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="Family"/>.
        /// </returns>
        Family GetFamilyById(int id);

        /// <summary>
        /// Get database family services.
        /// </summary>
        /// <param name="familyId">
        /// The family id.
        /// </param>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        List<FamilyService> GetFamilyServices(int familyId);

        /// <summary>
        /// Create database family service.
        /// </summary>
        /// <param name="familyService">
        /// The family service.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        bool CreateFamilyService(FamilyService familyService);

        /// <summary>
        /// Delete all family services for a family.
        /// </summary>
        /// <param name="familyId">
        /// The family id.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        bool DeleteFamilyServices(int familyId);

        /// <summary>
        /// Create database family.
        /// </summary>
        /// <param name="family">
        /// The family.
        /// </param>
        /// <returns>
        /// The <see cref="Family"/>.
        /// </returns>
        Family CreateFamily(Family family);

        /// <summary>
        /// Update database family.
        /// </summary>
        /// <param name="family">
        /// The family.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        bool UpdateFamily(Family family);

        /// <summary>
        /// Delete database family.
        /// </summary>
        /// <param name="family">
        /// The family.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        bool DeleteFamily(Family family);

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
        bool UpdateFamilyServices(int familyId, List<int> newList);
    }
}
