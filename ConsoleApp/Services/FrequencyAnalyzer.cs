using ConsoleApp.Abstracts;

namespace ConsoleApp.Services
{
    public class FrequencyAnalyzer : IFrequencyAnalyzer
    {
        public Dictionary<string, int> GetWordFrequencies(string fileContent)
        {

            // check if the file content is empty
            if (string.IsNullOrEmpty(fileContent))
            {
                throw new ArgumentException("File content is empty");
            }

            // sSplit file content  by spaces and newlines, remove empty entries, and normalize case
            var words = fileContent
                .Split(new[] { ' ', '\n', '\r', '\t' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(word => word.Trim().ToLowerInvariant());

            // count the frequency of each word
            var wordFrequencies = words
                .GroupBy(word => word)
                .ToDictionary(group => group.Key, group => group.Count());

            // Sort by descending frequency
            return wordFrequencies
                            .OrderByDescending(kvp => kvp.Value)
                            .ThenBy(kvp => kvp.Key) // Optional: Sort alphabetically for ties
                            .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        }
    }
}
