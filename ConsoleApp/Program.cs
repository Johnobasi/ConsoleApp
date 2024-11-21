using ConsoleApp;
using ConsoleApp.Abstracts;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

public class Program
{
    public static async Task Main(string[] args)
    {
        try
        {
            if (args.Length != 2)
            {
                Console.WriteLine("Usage: FrequencyDictionary <inputFile> <outputFile>");
                return;
            }

            string inputFile = args[0];
            string outputFile = args[1];

            #region Dependency Injection
            /// Set up the DI container
            var serviceProvider = DependencyInjection.ConfigureServices();

            // Resolve dependencies
            var inputReader = serviceProvider.GetRequiredService<IInputFileReader>();
            var frequencyAnalyzer = serviceProvider.GetRequiredService<IFrequencyProcessor>();
            var outputWriter = serviceProvider.GetRequiredService<IOutputFileWriter>();

            #endregion
            
            // Use the resolved dependencies
            var fileStream =  inputReader.ReadFileInChunksAsync(inputFile);
            var frequencyDictionary = await frequencyAnalyzer.GetWordFrequenciesAsync(fileStream);
            await outputWriter.WriteFrequenciesAsync(outputFile, frequencyDictionary);

            Console.WriteLine($"Frequency dictionary created successfully. Otput copy saved to output.txt...\n{JsonConvert.SerializeObject(frequencyDictionary, Formatting.Indented)}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }

    }
}