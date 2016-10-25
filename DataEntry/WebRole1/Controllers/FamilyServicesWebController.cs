using System.Collections.Generic;
using System.Web.Mvc;
using DataEntry_Helpers;


namespace Website.Controllers
{
    using System.Web.Http.Cors;

    using DataEntry_Helpers.Repositories;
    [EnableCors("*", "*", "*")]
    public class FamilyServicesWebController : BaseController
    {
        private readonly DataAccess data;

        public FamilyServicesWebController()
        {
            data = new DataAccess();
        }
       
        [HttpGet]
        public List<County> GetCountiesByStateId(int stateId)
        {
            return data.GetCountiesList(stateId);
        }

        [HttpGet]
        public List<State> GetStatesByCountryId(int countryId)
        {
            return data.GetStateList(countryId);
        }

        [HttpGet]
        public ServiceProvider GetServiceProviderById(int providerId)
        {
            return data.GetServiceProviders(providerId);
        }
    }
}
