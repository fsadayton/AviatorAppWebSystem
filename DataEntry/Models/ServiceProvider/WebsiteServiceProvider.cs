using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    using Models.ServiceProvider;

    /// <summary>
    /// The website service provider.
    /// </summary>
    public class WebsiteServiceProvider
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        [Required (ErrorMessage="A name is required")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        [Required (ErrorMessage="A description is required")]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the display rank.
        /// </summary>
        public int DisplayRank { get; set; }

        /// <summary>
        /// Gets or sets the services.
        /// </summary>
        [Required(ErrorMessage = "At least one category is required.")]
        public WebServiceAreas Services { get; set; }
        
        /// <summary>
        /// Gets or sets the Locations.
        /// </summary>
        public List<ServiceProviderLocation> Locations { get; set; }

        /// <summary>
        /// Gets or sets the provider type.
        /// </summary>
        [Required]
        public int Type { get; set; }

        /// <summary>
        /// Gets or sets the type description.
        /// </summary>
        public string TypeName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the provider is active.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets the state of the object.
        /// </summary>
        public ObjectStatus.ObjectState State { get; set; }
    }
}
