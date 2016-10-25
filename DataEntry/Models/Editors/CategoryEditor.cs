// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CategoryEditor.cs" company="UDRI">
//   Copyright © 2015 University of Dayton Research Institute. All rights reserved.
// </copyright>
// <summary>
//   The category editor.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.ComponentModel;
using Models.ServiceProvider;

namespace Models.Editors
{
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;

    /// <summary>
    /// The category editor.
    /// </summary>
    public class CategoryEditor
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        [DataMember(Name = "id")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        [DataMember(Name = "name")]
        [Required (ErrorMessage="Name is required")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        [DataMember(Name = "description")]
        [Required (ErrorMessage="Description is required")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the category is active.
        /// </summary>
        [DataMember(Name = "active")]
        [DefaultValue(true)]
        public bool Active { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether crime.
        /// </summary>
        [DataMember(Name = "crime")]
        public bool Crime { get; set; }
        
        /// <summary>
        /// Gets or sets the category type.
        /// </summary>
        [EnsureOneElement(ErrorMessage = "Select one type or more")]
        public List<int> CategoryTypes { get; set; }

        /// <summary>
        /// Full Category Types with ID and Name
        /// </summary>
        public List<WebsiteCategoryType> CategoryTypesNames { get; set; }

        /// <summary>
        /// Gets or sets the state of the object.
        /// </summary>
        public ObjectStatus.ObjectState State { get; set; }

    }
}
