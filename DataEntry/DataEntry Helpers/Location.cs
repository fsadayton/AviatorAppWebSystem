//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DataEntry_Helpers
{
    using System;
    using System.Collections.Generic;
    
    public partial class Location
    {
        public Location()
        {
            this.ProviderCoverages = new HashSet<ProviderCoverage>();
        }
    
        public int ID { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public int StateID { get; set; }
        public string Zip { get; set; }
        public int CountryID { get; set; }
        public int ProviderID { get; set; }
        public bool Display { get; set; }
        public int ContactID { get; set; }
        public Nullable<int> ContactPersonID { get; set; }
        public string Name { get; set; }
    
        public virtual Contact Contact { get; set; }
        public virtual ContactPerson ContactPerson { get; set; }
        public virtual Country Country { get; set; }
        public virtual State State { get; set; }
        public virtual ServiceProvider ServiceProvider { get; set; }
        public virtual ICollection<ProviderCoverage> ProviderCoverages { get; set; }
    }
}
