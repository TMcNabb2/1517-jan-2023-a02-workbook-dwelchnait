using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

#region Additional Namespaces
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WestWindSystem.BLL;
using WestWindSystem.DAL;
#endregion

namespace WestWindSystem
{
    //this class needs to be static
    public static class WestWindExtensions
    {
        public static void WWExtensions(this IServiceCollection services,
                                        Action<DbContextOptionsBuilder> options)
        {
            //we will register all the services that will be available
            //   for used by any system (BLL services)
            //we will register the database connection to be used
            //   by this class library (context setup)

            //register the context service
            //the parameter options contains the connection string information
            services.AddDbContext<WestWindContext>(options);

            //register EACH service class (BLL class)
            //each service class will have an AddTransient<T>() method

            //use the AddTransient<T>() method where T is your BLL class name
            //AddTransient is called a factory
            services.AddTransient<AboutServices>((serviceProvider) =>
            {
                //get the Context class that was registered above in AddDbContext
                var context = serviceProvider.GetService<WestWindContext>();

                //create an instance of the service class (AboutServices)
                //  supplying the context reference to the service class constructor
                return new AboutServices(context);
            });

            services.AddTransient<RegionServices>((serviceProvider) =>
            {
                var context = serviceProvider.GetService<WestWindContext>();
                return new RegionServices(context);
            });
            services.AddTransient<CategoryServices>((serviceProvider) =>
            {
                var context = serviceProvider.GetService<WestWindContext>();
                return new CategoryServices(context);
            });
            services.AddTransient<SupplierServices>((serviceProvider) =>
            {
                var context = serviceProvider.GetService<WestWindContext>();
                return new SupplierServices(context);
            });
            services.AddTransient<ProductServices>((serviceProvider) =>
            {
                var context = serviceProvider.GetService<WestWindContext>();
                return new ProductServices(context);
            });
            services.AddTransient<ShipperServices>((serviceProvider) =>
            {
                var context = serviceProvider.GetService<WestWindContext>();
                return new ShipperServices(context);
            });
            services.AddTransient<ShipmentServices>((serviceProvider) =>
            {
                var context = serviceProvider.GetService<WestWindContext>();
                return new ShipmentServices(context);
            });

        }
    }
}
