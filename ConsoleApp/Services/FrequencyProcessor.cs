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
                var wordPattern = @"\b[a-zA-Z]+\b";
                var wordFrequencies = new ConcurrentDictionary<string, int>();
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
