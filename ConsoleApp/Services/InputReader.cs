using ConsoleApp.Abstracts;
using System.Text;

namespace ConsoleApp.Services
{
    public class InputReader : IInputReader
    {
        public string ReadFile(string path)
        {

            if (!File.Exists(path))
            {
                throw new FileNotFoundException($"Input file not found: {path}");
            }

            // Register the encoding provider
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            //read file using windows-1252 encoding
            return File.ReadAllText(path, Encoding.GetEncoding("windows-1252"));
        }
    }
}
