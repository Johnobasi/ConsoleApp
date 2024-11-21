using ConsoleApp.Services;
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

            string content = await new InputFileReader().ReadFileAsync(inputFile);
            var frequencyDictionary = await new FrequencyProcessor().GetWordFrequenciesAsync(content);
            await  new OutputFileWriter().WriteFrequenciesAsync(outputFile, frequencyDictionary);

            Console.WriteLine($"Frequency dictionary created successfully. Output copy saved to output.txt...\n{JsonConvert.SerializeObject(frequencyDictionary, Formatting.Indented)}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }

    }
}