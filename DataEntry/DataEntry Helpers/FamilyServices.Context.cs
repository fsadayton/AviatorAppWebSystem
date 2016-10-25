﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class ArmorAppEntities : DbContext
    {
        public ArmorAppEntities()
            : base("name=ArmorAppEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<ServiceProviderEdit> ServiceProviderEdits { get; set; }
        public virtual DbSet<CategoryType> CategoryTypes { get; set; }
        public virtual DbSet<Contact> Contacts { get; set; }
        public virtual DbSet<ContactPerson> ContactPersons { get; set; }
        public virtual DbSet<County> Counties { get; set; }
        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<CrisisContact> CrisisContacts { get; set; }
        public virtual DbSet<Family> Families { get; set; }
        public virtual DbSet<FamilyService> FamilyServices { get; set; }
        public virtual DbSet<Invite> Invites { get; set; }
        public virtual DbSet<Location> Locations { get; set; }
        public virtual DbSet<ProviderCoverage> ProviderCoverages { get; set; }
        public virtual DbSet<ProviderServiceCategory> ProviderServiceCategories { get; set; }
        public virtual DbSet<ProviderService> ProviderServices { get; set; }
        public virtual DbSet<RoleType> RoleTypes { get; set; }
        public virtual DbSet<ServiceProvider> ServiceProviders { get; set; }
        public virtual DbSet<ServiceType> ServiceTypes { get; set; }
        public virtual DbSet<State> States { get; set; }
        public virtual DbSet<Tool> Tools { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserCredential> UserCredentials { get; set; }
        public virtual DbSet<UserRole> UserRoles { get; set; }
    }
}
