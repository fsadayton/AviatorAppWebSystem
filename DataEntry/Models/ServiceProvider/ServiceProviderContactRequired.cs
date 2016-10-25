// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ServiceProviderContactRequired.cs" company="UDRI">
//   Service Provider Contact that has a required phone number.
// </copyright>
// <summary>
//   The service provider contact with required fields.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Models.ServiceProvider
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// The service provider contact with required fields.
    /// </summary>
    public class ServiceProviderContactRequired
    {

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the phone number.
        /// </summary>
        [Required(ErrorMessage = "A phone number is required.")]
        [RegularExpressionAttribute(@"^(?:(?:\+?1\s*(?:[.-]\s*)?)?(?:\(\s*([2-9]1[02-9]|[2-9][02-8]1|[2-9][02-8][02-9])\s*\)|([2-9]1[02-9]|[2-9][02-8]1|[2-9][02-8][02-9]))\s*(?:[.-]\s*)?)?([2-9]1[02-9]|[2-9][02-9]1|[2-9][02-9]{2})\s*(?:[.-]\s*)?([0-9]{4})(?:\s*(?:#|x\.?|ext\.?|extension)\s*(\d+))?$", ErrorMessage = "Please input a phone number in correct format")]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the website.
        /// </summary>
        public string Website { get; set; }

        /// <summary>
        /// Gets or sets the help line.
        /// </summary>
        public string HelpLine { get; set; }

        /// <summary>
        /// Gets or sets the state of the object.
        /// </summary>
        public ObjectStatus.ObjectState State { get; set; }
    }
}
