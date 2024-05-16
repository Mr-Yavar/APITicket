using Microsoft.Extensions.DependencyInjection;
using Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Reflection;
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
                var interfaceService = service.GetInterface("I" + service.Name)?.GetType().ReflectedType;
               
                
                if (interfaceService != null)
                    services.AddScoped(interfaceService, service);


            });


            services.AddDistributedMemoryCache();
        }
    }
}
