using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Website.Models
{
    using System.Runtime.Serialization;

    /// <summary>
    /// The display service provider.
    /// </summary>
    [DataContract]
    public class DisplayServiceProvider
    {

        /// <summary>
        /// Gets or sets the service provider id
        /// </summary>
        [DataMember(Name = "serviceProviderId")]
        public int ServiceProviderId { get; set; }


        /// <summary>
        /// Gets or sets the service provider id
        /// </summary>
        [DataMember(Name = "locationId")]
        public int LocationId { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        [DataMember(Name = "name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the sub-name to be shown if there are multiple locations.
        /// </summary>
        [DataMember(Name = "locationName")]
        public string LocationName { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        [DataMember(Name = "description")]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the address.
        /// </summary>
        [DataMember(Name = "address")]
        public string Address { get; set; }

        /// <summary>
        /// Gets or sets the phone number.
        /// </summary>
        [DataMember(Name = "phoneNumber")]
        [Required (ErrorMessage = "Phone number is required")]
        [RegularExpressionAttribute(@"^(?:(?:\+?1\s*(?:[.-]\s*)?)?(?:\(\s*([2-9]1[02-9]|[2-9][02-8]1|[2-9][02-8][02-9])\s*\)|([2-9]1[02-9]|[2-9][02-8]1|[2-9][02-8][02-9]))\s*(?:[.-]\s*)?)?([2-9]1[02-9]|[2-9][02-9]1|[2-9][02-9]{2})\s*(?:[.-]\s*)?([0-9]{4})(?:\s*(?:#|x\.?|ext\.?|extension)\s*(\d+))?$", ErrorMessage = "Please input a phone number in correct format")]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Gets or sets the crisis number.
        /// </summary>
        [DataMember(Name = "crisisNumber")]
        public string CrisisNumber { get; set; }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        [DataMember(Name = "email")]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the website.
        /// </summary>
        [DataMember(Name = "website")]
        public string Website { get; set; }

        /// <summary>
        /// Gets or sets the categories.
        /// </summary>
        [DataMember(Name = "categories")]
        public List<int> Categories { get; set; }

        /// <summary>
        /// Gets or sets the display rank.
        /// </summary>
        [DataMember(Name = "displayRank")]
        public int DisplayRank { get; set; }
    }
}