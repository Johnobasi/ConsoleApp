using ConsoleApp.Abstracts;
using ConsoleApp;
using Newtonsoft.Json;
using Microsoft.Extensions.DependencyInjection;

public class Program
{
    /// <summary>
    /// Main entry point for the application
    /// </summary>
    /// <param name="args"></param>
    /// <returns></returns>
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
            var inputReader = serviceProvider.GetRequiredService<IInputFileProcessor>();
            var outputWriter = serviceProvider.GetRequiredService<IOutputFileWriter>();

            #endregion
            Dictionary<string, int> frequencyDictionary = await inputReader.ProcessFileAsync(inputFile);
            await outputWriter.WriteFrequenciesAsync(outputFile, frequencyDictionary);

            Console.WriteLine($"Frequency dictionary created successfully. Otput copy saved to output.txt...\n{JsonConvert.SerializeObject(frequencyDictionary, Formatting.Indented)}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }

    }
}
