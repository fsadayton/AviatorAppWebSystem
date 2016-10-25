//---------------------------------------------------------------------------------
//<copyright file="HotLineProviderViewModel" company="UDRI"
//   Copyright © 2015 University of Dayton Research Institute. All rights reserved.
// </copyright>
// <summary>
//   The invite logics.
// </summary>
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Website.Models
{
    public class HotLineProviderViewModel
    {
        /// <summary>
        /// HotLine Provider Name 
        /// </summary>
        public String ProviderName { get; set; }
        /// <summary>
        /// Crisis Number (non-formatted)
        /// </summary>
        public String CrisisNumber { get; set; }
        /// <summary>
        /// Provider Location Name 
        /// </summary>
        public String ProviderLocation { get; set; }
    }
}