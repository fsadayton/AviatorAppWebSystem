using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataEntry_Helpers.RepositoryInterfaces
{
    public interface IVeteransRepo
    {
        List<ServiceProvider> GetServiceProviders(List<int> counties);
    }
}
