using ConsoleApp.Abstracts;
using System.Text;

namespace ConsoleApp.Services
{
    public class InputFileReader : IInputFileReader
    {
        public async IAsyncEnumerable<string> ReadFileInChunksAsync(string path)
        {
            StreamReader streamReader;

            try
            {
                if (!File.Exists(path))
                {
                    throw new FileNotFoundException($"Input file not found: {path}");
                }

                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                streamReader = new StreamReader(path, Encoding.GetEncoding("Windows-1252"));
            }
            catch (Exception ex)
            {
                throw new IOException($"Error initializing file read: {path}. Details: {ex.Message}", ex);
            }
            using(streamReader)
            {
                string? line;
                while ((line = await streamReader.ReadLineAsync()) != null)
                {
                    yield return line; // Yield lines one by one
                }
            }
        }

    }

}
