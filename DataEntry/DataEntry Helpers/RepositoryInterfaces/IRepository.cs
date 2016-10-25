// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IRepository.cs" company="UDRI">
//   Copyright © 2015 University of Dayton Research Institute. All rights reserved.
// </copyright>
// <summary>
//   Defines the IRepository type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DataEntry_Helpers.RepositoryInterfaces
{
    using System;

    /// <summary>
    /// The Repository interface.
    /// </summary>
    public interface IRepository : IDisposable
    {
        /// <summary>
        /// The get context.
        /// </summary>
        /// <returns>
        /// The <see cref="ArmorAppEntities"/>.
        /// </returns>
        ArmorAppEntities GetContext();

        /// <summary>
        /// The set context.
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
        void SetContext(ArmorAppEntities context);
    }
}
