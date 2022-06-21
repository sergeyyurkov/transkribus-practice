using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranskribusPractice.Services
{
    public class ServiceProvider : IServiceProvider
    {
        public object GetService(Type serviceType)
        {
            if (serviceType == typeof(IFileService))
            {
                return new FileService();
            }
            if (serviceType == typeof(IProjectService))
            {
                return new ProjectService();
            }
            return null;
        }
    }
}
