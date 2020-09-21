using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Raccoon.DevKits.Wpf.DependencyInjection
{
    public static class Extensions
    {
        public static IDIApplication? AsDIApplication(this Application application) =>
            application as IDIApplication;

        public static void OnStartupProxy<TStartupWindow>(this IDIApplication application)
            where TStartupWindow:Window
        {
            IServiceCollection services = new ServiceCollection();

            application.Configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings", true, true)
                .Build();
            services.AddSingleton(application.Configuration);

            application.ConfigureServices(services);
            application.ServiceProvider = services.BuildServiceProvider();

            application.ServiceProvider.GetRequiredService<TStartupWindow>().Show();
        }

        public static TWindow GetWindow<TWindow>(this IDIApplication application)where TWindow:Window =>
            application.ServiceProvider.GetRequiredService<TWindow>();
    }
}
