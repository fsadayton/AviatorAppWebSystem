// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BundleConfig.cs" company="UDRI">
//   Copyright © 2015 University of Dayton Research Institute. All rights reserved.
// </copyright>
// <summary>
//   Defines the BundleConfig type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Website
{
    using System.Web;
    using System.Web.Optimization;

    /// <summary>
    /// The bundle config.
    /// </summary>
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/jquery-ui-1.11.4.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js",
                      "~/Scripts/site.js",
                      "~/Scripts/bootstrap-toolkit.js",
                      "~/Scripts/Menus.js",
                      "~/Scripts/select2.js",
                      "~/Scripts/select2.full.js",
                      "~/Scripts/typeahead.js"));

            bundles.Add(new ScriptBundle("~/bundles/dataEntry").Include(
                        "~/Scripts/ServiceProvider.js",
                        "~/Scripts/CrisisContacts.js",
                        "~/Scripts/Invites.js"));

            bundles.Add(new ScriptBundle("~/bundles/Family").Include(
                        "~/Scripts/Family.js",
                        "~/Scripts/SearchItems.js"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/Category").Include(
                        "~/Scripts/Category.js",
                        "~/Scripts/SearchItems.js"
                        ));
            bundles.Add(new ScriptBundle("~/bundles/Users").Include(
                        "~/Scripts/User.js"
                        ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/Site.css",
                      "~/Content/select2.css",
                      "~/Content/font-awesome.css"));
            bundles.Add(new ScriptBundle("~/bundles/serviceProviderDisplay").Include(
                "~/Scripts/jquery.scrollTo.js",
                "~/Scripts/ServiceProviderDisplay.js",
                "~/Scripts/bingMap.js"
                ));
            bundles.Add(new ScriptBundle("~/bundles/ServiceProviderVeteran").Include(
                "~/Scripts/ServiceProviderVeteran.js"));
            bundles.Add(new ScriptBundle("~/bundles/ServiceProviderLaw").Include(
                "~/Scripts/ServiceProviderLaw.js"));
            bundles.Add(new ScriptBundle("~/bundles/serviceProviderSpecific").Include(
                "~/Scripts/jquery.scrollTo.js",
                "~/Scripts/ServiceProviderSpecific.js",
                 "~/Scripts/bingMap.js"
                ));
            bundles.Add(new ScriptBundle("~/bundles/HotLineScript").Include(
                "~/Scripts/HotLineScript.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/LandingPage").Include(
                "~/Scripts/jquery-2.1.4.js",
                "~/Scripts/bootstrap.js",
                "~/Scripts/LandingPage.js"
                ));
        }
    }
}
