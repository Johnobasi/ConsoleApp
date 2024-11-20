using ConsoleApp.Abstracts;
using System.Text;

namespace ConsoleApp.Services
{
    public class OutputWriter : IOutputWriter
    {
        public void WriteFrequencies(string path, Dictionary<string, int> frequenciesDictionary)
        {

            using var writer = new StreamWriter(path, false, Encoding.GetEncoding("Windows-1252"));
            foreach (var kvp in frequenciesDictionary)
            {
                writer.WriteLine($"{kvp.Key}: {kvp.Value}");
            }
        }
    }
}
