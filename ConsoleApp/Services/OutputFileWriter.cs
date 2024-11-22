using ConsoleApp.Abstracts;
using System.Text;

namespace ConsoleApp.Services
{
    /// <summary>
    /// This class writes word frequencies to an output file in a key value pair format
    /// </summary>
    public class OutputFileWriter : IOutputFileWriter
    {
        public async Task WriteFrequenciesAsync(string path, Dictionary<string, int> frequenciesDictionary)
        {
            try
            {
                // Batch size for concurrent writes operation => can be any size depending on the file size
                const int batchSize = 20;
                var tasks = new List<Task>();

                using var writer = new StreamWriter(path, false, Encoding.GetEncoding("Windows-1252"));
                var batch = new List<string>();
                foreach (var kvp in frequenciesDictionary)
                {
                    batch.Add($"{kvp.Key},{kvp.Value}");

                    if (batch.Count >= batchSize)
                    {
                        tasks.Add(WriteBatchAsync(writer, batch));
                        batch = new List<string>();
                    }
                }
                if (batch.Any())
                {
                    tasks.Add(WriteBatchAsync(writer, batch));
                }
                await Task.WhenAll(tasks);
            }
            catch (UnauthorizedAccessException)
            {
                throw new IOException($"Cannot write to file: {path}. Check user's file permissions.");
            }
        }
        private async Task WriteBatchAsync(StreamWriter writer, List<string> batch)
        {
            foreach (var line in batch)
            {
                await writer.WriteLineAsync(line);
            }
        }
    }
}
