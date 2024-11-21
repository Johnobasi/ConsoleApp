using ConsoleApp.Abstracts;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace ConsoleApp.Services
{
    public class FrequencyProcessor : IFrequencyProcessor
    {
        public async Task<Dictionary<string, int>> GetWordFrequenciesAsync(IAsyncEnumerable<string> fileLines)
        {
            try
            {
                var wordPattern = @"\b\p{L}+\b"; // Include Unicode letters
                var wordFrequencies = new ConcurrentDictionary<string, int>();
                var unsupportedCharacters = new List<string>();
                await Parallel.ForEachAsync(fileLines, async (line, _) =>
                {
                    var words = Regex.Matches(line.ToLowerInvariant(), wordPattern)
                                     .Cast<Match>()
                                     .Select(match => match.Value);

                    foreach (var word in words)
                    {
                        Debug.WriteLine($"Processing word: {word}");
                        wordFrequencies.AddOrUpdate(word, 1, (_, count) => count + 1);
                    }

                    await Task.CompletedTask;
                });

                foreach (var unsupported in unsupportedCharacters)
                {
                    Debug.WriteLine($"Excluded unsupported chars: {unsupported}");
                }

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
