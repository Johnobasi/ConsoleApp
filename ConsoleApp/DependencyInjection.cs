using ConsoleApp.Abstracts;
using ConsoleApp.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ConsoleApp
{
    public static class DependencyInjection
    {
        public static ServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();
            services.AddTransient<IInputFileReader, InputFileReader>();
            services.AddTransient<IFrequencyProcessor, FrequencyProcessor>();
            services.AddTransient<IOutputFileWriter, OutputFileWriter>();
                       
            return services.BuildServiceProvider();
        }
    }
}
