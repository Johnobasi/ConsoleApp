using ConsoleApp.Abstracts;
using System.Text;

namespace ConsoleApp.Services
{
    public class InputFileReader : IInputFileReader
    {
        public async IAsyncEnumerable<string> ReadFileInChunksAsync(string path)
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException($"Input file not found: {path}");
            }
            StreamReader streamReader;

            try
            {
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                try
                {
                    streamReader = new StreamReader(path, Encoding.GetEncoding("Windows-1252"));
                }
                catch (ArgumentException)
                {
                    // Fallback to UTF-8 encoding if Windows-1252 is not supported
                    streamReader = new StreamReader(path, Encoding.UTF8);
                }                
            }
            catch (Exception ex)
            {
                throw new IOException($"Error while initializing file read: {path}. Message: {ex.Message}", ex);
            }
            using(streamReader)
            {
                string? line;
                while ((line = await streamReader.ReadLineAsync()) != null)
                {
                    yield return line;
                }
            }
        }
    }
}
