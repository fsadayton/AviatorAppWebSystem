// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ToolsRepo.cs" company="UDRI">
//   Copyright © 2016 University of Dayton Research Institute. All rights reserved.
// </copyright>
// <summary>
//   Model Conversions for Tools
// </summary>
// --------------------------------------------------------------------------------------------------------------------


using System;

namespace Website.BL.ModelConversions
{
    using System.Collections.Generic;
    using System.Linq;
    using DataEntry_Helpers;
    using Models;


    public enum PhoneType
    {
        Apple,
        Google
    }

    /// <summary>
    /// Convert Tools to and from DB models
    /// </summary>
    public class ToolConversions
    {
        /// <summary>
        /// Convert the db model to the view model
        /// </summary>
        /// <param name="dbTool"></param>
        /// <returns></returns>
        public static ToolViewModel ConvertDbToViewModel(Tool dbTool)
        {
            return new ToolViewModel
            {
                Id = dbTool.ID,
                AppleStore = dbTool.AppleStoreWebsite,
                Name =  dbTool.Name,
                Description = dbTool.Description,
                GooglePlayStore = dbTool.GooglePlayWebsite,
                WebsiteUrl =  dbTool.Website,
                IsActive = dbTool.Active
            };
        }

        /// <summary>
        /// Convert the View model to the db model
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        public static Tool ConvertViewModelToDb(ToolViewModel viewModel)
        {
            return new Tool
            {
                ID = viewModel.Id,
                AppleStoreWebsite = viewModel.AppleStore,
                Name = viewModel.Name,
                Description = viewModel.Description,
                GooglePlayWebsite = viewModel.GooglePlayStore,
                Website = viewModel.WebsiteUrl,
                Active = viewModel.IsActive
            };
        }


        /// <summary>
        /// Convert a list of db models to view models
        /// </summary>
        /// <param name="dbTools"></param>
        /// <returns></returns>
        public static List<ToolViewModel> ConvertDbToViewModels(List<Tool> dbTools)
        {
            return dbTools.Select(ConvertDbToViewModel).ToList();
        }

        /// <summary>
        /// Convert the db model to the API model of the tool
        /// </summary>
        /// <param name="dbTools">DB models</param>
        /// <param name="phoneType">Which phone to get the url for</param>
        /// <returns>List of API models</returns>
        public static List<ToolApiModel> ConvertDbModelToApiModel(List<Tool> dbTools, PhoneType phoneType)
        {
            var apiModels = new List<ToolApiModel>();
            foreach (var curTool in dbTools)
            {
                var apiModel = new ToolApiModel
                {
                    Name = curTool.Name,
                    Description = curTool.Description,
                    Id = curTool.ID,
                    WebsiteUrl = curTool.Website
                };

                switch (phoneType)
                {
                    case PhoneType.Apple:
                        apiModel.AppStoreUrl = curTool.AppleStoreWebsite;
                        break;
                    case PhoneType.Google:
                        apiModel.AppStoreUrl = curTool.GooglePlayWebsite;
                        break;
                    default:
                        apiModel.AppStoreUrl = "";
                        break;
                }
                apiModels.Add(apiModel);
            }
            return apiModels;
        }
    }
}