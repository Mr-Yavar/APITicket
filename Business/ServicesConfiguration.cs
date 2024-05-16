using Microsoft.Extensions.DependencyInjection;
using Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public static class ServicesConfiguration
    {

        public static void RegisterServices(this IServiceCollection services)
        {
            (typeof(ServiceBase)).Assembly.GetTypes().Where(x => x.IsSubclassOf(typeof(ServiceBase)) && !x.IsAbstract).ToList().ForEach(service =>
            {
                var t =service.GetInterface("I"+service.Name);


                var x = 5;
            });
        }
    }
}
