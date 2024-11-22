using ConsoleApp.Abstracts;
using ConsoleApp.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    public static class DependencyInjection
    {
        public static ServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();
            services.AddTransient<IInputFileProcessor, InputFileProcessor>();
            services.AddTransient<IOutputFileWriter, OutputFileWriter>();

            return services.BuildServiceProvider();
        }
    }
}
