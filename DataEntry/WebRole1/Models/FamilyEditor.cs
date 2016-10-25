
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Website.Models
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    using global::Models;

    /// <summary>
    /// The family.
    /// </summary>
    [DataContract]
    public class FamilyEditor
    {
        /// <summary>
        /// Gets or sets the database id.
        /// </summary>
        [DataMember(Name = "id")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        [Required(ErrorMessage = "Family name should be entered.")]
        [DataMember(Name = "name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        [Required(ErrorMessage = "Family description should be entered.")]
        [DataMember(Name = "description")]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the category ids.
        /// </summary>
        [DataMember(Name = "categoryIds")]
        [EnsureOneElement (ErrorMessage = "Family description should be entered.")]
        public List<int> CategoryIds { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the category is active.
        /// </summary>
        [DataMember(Name = "active")]
        public bool Active { get; set; }

        /// <summary>
        /// Gets or sets if the family is a special population
        /// </summary> 
        [DisplayName("Special Population")]
        public bool IsSpecialPopulation { get; set; }

        /// <summary>
        /// Gets or sets the state of the object.
        /// </summary>
        public ObjectStatus.ObjectState State { get; set; }
    }
}
