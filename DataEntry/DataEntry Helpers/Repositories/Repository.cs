// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Repository.cs" company="UDRI">
//   Copyright © 2015 University of Dayton Research Institute. All rights reserved.
// </copyright>
// <summary>
//   Defines the Repository type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DataEntry_Helpers.Repositories
{
    using System;

    using DataEntry_Helpers.RepositoryInterfaces;

    /// <summary>
    /// The repository.
    /// </summary>
    public class Repository : IRepository
    {
        #region Fields

        /// <summary>
        /// The EF context.
        /// </summary>
        protected ArmorAppEntities db;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Repository" /> class.
        /// </summary>
        protected Repository()
        {
            this.db = new ArmorAppEntities();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Repository"/> class.
        /// </summary>
        /// <param name="entities">
        /// The entities.
        /// </param>
        protected Repository(ArmorAppEntities entities)
        {
            this.db = entities;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The dispose.
        /// </summary>
        public void Dispose()
        {
            this.db.Dispose();
            GC.SuppressFinalize(this);
        }

        #endregion

        /// <summary>
        /// The get context.
        /// </summary>
        /// <returns>
        /// The <see cref="ArmorAppEntities"/>.
        /// </returns>
        public ArmorAppEntities GetContext()
        {
            return this.db;
        }

        /// <summary>
        /// The set context.
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
        public void SetContext(ArmorAppEntities context)
        {
            this.db = context;
        }
    }
}
