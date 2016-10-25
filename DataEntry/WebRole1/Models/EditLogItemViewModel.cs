using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Website.Models
{
    /// <summary>
    /// An edit log item.
    /// </summary>
    public class EditLogItemViewModel
    {
        /// <summary>
        /// Gets or sets the provider id.
        /// </summary>
        public int? EditedProviderId { get; set; }

        /// <summary>
        /// Gets or sets the provider name.
        /// </summary>
        public string EditedProviderName { get; set; }

        /// <summary>
        /// Gets or sets the user id.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets the user first name.
        /// </summary>
        public string UserFirstName { get; set; }

        /// <summary>
        /// Gets or sets the user last name.
        /// </summary>
        public string UserLastName { get; set; }

        /// <summary>
        /// Gets or sets the date and time the provider was edited.
        /// </summary>
        public DateTime EditedDateTime { get; set; }

        /// <summary>
        /// Action taken by the user
        /// </summary>
        public string Action { get; set; }
    }
}