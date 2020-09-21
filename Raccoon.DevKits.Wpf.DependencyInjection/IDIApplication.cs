using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raccoon.DevKits.Wpf.DependencyInjection
{
    public interface IDIApplication
    {
        IServiceProvider ServiceProvider { get; set; }
        IConfiguration Configuration { get; set; }
        void ConfigureServices(IServiceCollection services);
    }
}
