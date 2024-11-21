using ConsoleApp.Abstracts;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace ConsoleApp.Services
{
    public class FrequencyProcessor : IFrequencyProcessor
    {
        public async Task<Dictionary<string, int>> GetWordFrequenciesAsync(string fileContent)
        {

            try
            {
                //Check if file content is nul.
                if (string.IsNullOrEmpty(fileContent))
                {
                    throw new ArgumentException("File content is empty");
                }

                //Regex pattern to match words 
                var wordPattern = @"^[A-Za-z]";

                //Split content by spaces and newlines, normalize case
                var words = fileContent
                    .Split([' ', '\n', '\r', '\t'], StringSplitOptions.RemoveEmptyEntries) //use collection expression
                    .Select(word => word.Trim().ToLowerInvariant());

                //ConcurrentDictionary is used here for thread-safety
                var wordFrequencies = new ConcurrentDictionary<string, int>();

                await Parallel.ForEachAsync(words, async (word, CancellationToken) =>
                {

                    if (Regex.IsMatch(word, wordPattern))
                    {
                        Debug.WriteLine($"Processing word: {word}");
                        wordFrequencies.AddOrUpdate(word, 1, (_, count) => count + 1);
                    }
                    await Task.CompletedTask;
                });

                // Convert to dictionary, sort by descending order
                return wordFrequencies
                    .OrderByDescending(kvp => kvp.Value)
                    .ThenBy(kvp => kvp.Key)
                    .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to successfully process word frequencies..!", ex);
            }
            
        }
    }
}
