// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DisplayProviderCreator.cs" company="UDRI">
//   Copyright © 2015 University of Dayton Research Institute. All rights reserved.
// </copyright>
// <summary>
//   Creator for Display Providers.  Filters by county.
// </summary>
// --------------------------------------------------------------------------------------------------------------------


namespace Website.BL
{
    using System.Collections.Generic;
    using System.Linq;
    using DataEntry_Helpers;
    using Website.Models;

    /// <summary>
    /// Creator for Display Provider
    /// </summary>
    public class DisplayProviderCreator
    {
        /// <summary>
        /// The create display provider.
        /// </summary>
        /// <param name="providers"> The providers. </param>
        /// <param name="countiesToShow">List of counties to filter by</param>
        /// <returns> The <see cref="List{T}"/>. </returns>
        public List<DisplayServiceProvider> CreateDisplayProvider(List<ServiceProvider> providers, List<int> countiesToShow )
        {

            var listOfDisplayProviders = new List<DisplayServiceProvider>();

            foreach (var serviceProvider in providers)
            {
                foreach (var location in serviceProvider.Locations.Where(
                    location => location.Display && location.ProviderCoverages.Select(o => o.AreaID).ToList().Intersect(countiesToShow).ToList().Count > 0))
                {
                    var display = new DisplayServiceProvider
                    {
                        ServiceProviderId = serviceProvider.ID,
                        LocationId = location.ID,
                        Name = serviceProvider.ProviderName,
                        LocationName = location.Name,
                        Description = serviceProvider.Description,
                        PhoneNumber = location.Contact.Phone,
                        CrisisNumber = location.Contact.HelpLine,
                        Email = location.Contact.Email,
                        Website = location.Contact.Website,
                        DisplayRank = serviceProvider.DisplayRank,
                        Categories = serviceProvider.ProviderServices.Select(p => p.ServiceID).Distinct().ToList(),
                        Address = $"{location.Street}, {location.City}, {location.State.Abbreviation}, {location.Zip}"
                    };


                    listOfDisplayProviders.Add(display);
                }
            }

            return listOfDisplayProviders;
        }
    }
}