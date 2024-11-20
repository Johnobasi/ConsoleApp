using ConsoleApp.Abstracts;
using ConsoleApp.Services;

public class Program
{
    public static void Main(string[] args)
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

            // Dependency injection
            IInputReader inputReader = new InputReader();
            IFrequencyAnalyzer frequencyAnalyzer = new FrequencyAnalyzer();

            // Process
            string content = inputReader.ReadFile(inputFile);
            var frequencyDictionary = frequencyAnalyzer.GetWordFrequencies(content);
            new OutputWriter().WriteFrequencies(outputFile, frequencyDictionary);

            Console.WriteLine("Frequency dictionary created successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }

    }
}