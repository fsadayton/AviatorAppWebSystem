using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ServiceProvider
{
    public class WebsiteCategory
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

        public bool Crime { get; set; }

        public List<WebsiteCategoryType> ServiceTypes { get; set; }
    }
}
