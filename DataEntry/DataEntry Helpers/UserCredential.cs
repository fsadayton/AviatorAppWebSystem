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
    
    public partial class UserCredential
    {
        public UserCredential()
        {
            this.Users = new HashSet<User>();
        }
    
        public int ID { get; set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public string ResetToken { get; set; }
    
        public virtual ICollection<User> Users { get; set; }
    }
}
