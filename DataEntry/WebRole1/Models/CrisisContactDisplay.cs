// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CrisisContactDisplay.cs" company="UDRI">
//   Copyright © 2016 University of Dayton Research Institute. All rights reserved.
// </copyright>
// <summary>
//   Crisis contact display to be used by views.
// </summary>
// --------------------------------------------------------------------------------------------------------------------


namespace Website.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Runtime.Serialization;
    using DataEntry_Helpers;
    using Website.BL;
    /// <summary>
    /// The Crisis Contact Display class
    /// </summary>
    [DataContract]
    public class CrisisContactDisplay
    {
        /// <summary>
        /// Crisis contact display constructor
        /// </summary>
        public CrisisContactDisplay()
        {
            this.ID = 0;
        }

        /// <summary>
        /// Crisis Contact Display Constructor
        /// </summary>
        /// <param name="dbContact">Db contact to fill in values with</param>
        public CrisisContactDisplay(CrisisContact dbContact)
        {
            this.ID = dbContact.ID;
            this.Name = dbContact.Name;
            this.ContactId = dbContact.ContactId;
            this.PhoneNumber = formatPhoneNumber(dbContact.Contact.Phone);
        }

        /// <summary>
        /// ID of the crisis contact
        /// </summary>
        [DataMember(Name = "id")]
        public int ID { get; set; }
        
        /// <summary>
        /// Name of the crisis contact
        /// </summary>
        [Required(ErrorMessage = "Name is required")]
        [DataMember(Name = "name")]
        public string Name { get; set; }

        /// <summary>
        /// Contact ID in the database
        /// </summary>
        [DataMember(Name = "contactId")]
        public int ContactId { get; set; }

        /// <summary>
        /// Phone number for the crisis contact
        /// </summary>
        [Required(ErrorMessage = "Phone number is required")]
        [DataMember(Name = "phoneNumber")]
        [RegularExpressionAttribute(@"^(?:(?:\+?1\s*(?:[.-]\s*)?)?(?:\(\s*([2-9]1[02-9]|[2-9][02-8]1|[2-9][02-8][02-9])\s*\)|([2-9]1[02-9]|[2-9][02-8]1|[2-9][02-8][02-9]))\s*(?:[.-]\s*)?)?([2-9]1[02-9]|[2-9][02-9]1|[2-9][02-9]{2})\s*(?:[.-]\s*)?([0-9]{4})(?:\s*(?:#|x\.?|ext\.?|extension)\s*(\d+))?$", ErrorMessage = "Please input a phone number in correct format")]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Converts this display crisis contact to a database contact.
        /// </summary>
        /// <returns>DB version of the contact</returns>
        public CrisisContact ToDbCrisisContact()
        {
            CrisisContact dbContact;

            if (ID == 0)
            {
                //New crisis contact
                dbContact = new CrisisContact
                {
                    Name = this.Name,
                    Contact = new Contact {Phone = new string(this.PhoneNumber.Where(c => char.IsDigit(c)).ToArray())}
                };
            }
            else
            {
                var logics = new CrisisContactLogic();
                //Go find the existing one and set changes
                dbContact               = logics.GetDbCrisisContact(this.ID);
                dbContact.Name          = this.Name;
                dbContact.Contact.Phone = this.PhoneNumber;
            }
            return dbContact;
        }

        /// <summary>
        /// Formats a string of numbers to a phone number string.
        /// </summary>
        /// <param name="numbersOnly">String of numbers</param>
        /// <returns>phone number formatted string</returns>
        private static string formatPhoneNumber(string numbersOnly)
        {
            switch (numbersOnly.Length)
            {
                case 11:
                    return String.Format("{0:#+ (###) ###-####}", Convert.ToInt64(numbersOnly));
                case 10:
                    return String.Format("{0:(###) ###-####}", Convert.ToInt64(numbersOnly));
                case 7:
                    return String.Format("{0: ###-####}", Convert.ToInt64(numbersOnly));
                default:
                    return numbersOnly;
            }
        }
    }
}