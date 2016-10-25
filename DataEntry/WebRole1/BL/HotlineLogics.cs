//----------------------------------------------------------------------------------------------
//<copyright file="HotlineLogics.cs" company="UDRI"
//   Copyright © 2015 University of Dayton Research Institute. All rights reserved.
// </copyright>
// <summary>
//   The invite logics.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataEntry_Helpers;
using DataEntry_Helpers.Repositories;
using Website.Models;

namespace Website.BL
{

    //Crisis Contacts Buisiness Logic 
    public class HotlineLogics
    {

        //Method for getting provider numbers for hotlines 
        /// <summary>
        /// Returns providers that include a helpline , that is not 911 
        /// </summary>
        /// <returns>Return crisis contact providers</returns>
        public List<HotLineProviderViewModel> GetHotlines()
        {
            
            var hotLineProviders = new List<HotLineProviderViewModel>();
            var accessData = new ServiceProviderRepo();
            var serviceProviders = accessData.GetAllActiveServiceProviders();
            foreach (ServiceProvider t in serviceProviders)
            {
                foreach (var location in t.Locations)
                {
                    if (   location.Display 
                        && location.Contact.HelpLine != null 
                        && location.Contact.HelpLine.Trim() != "911")
                    {
                        var hotLineProvider = new HotLineProviderViewModel
                        {
                            ProviderName = t.ProviderName,
                            CrisisNumber = location.Contact.HelpLine,
                            ProviderLocation = location.Name
                        };
                        hotLineProviders.Add(hotLineProvider);

                    }
                }

            }
            return hotLineProviders;
        } 
    }
}