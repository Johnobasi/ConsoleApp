using ConsoleApp.Abstracts;
using System.Text;

namespace ConsoleApp.Services
{
    public class InputFileReader : IInputFileReader
    {
        public async Task<string> ReadFileAsync(string path)
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException($"Input file not found: {path}");
            }

            try
            {
                // Register the encoding provider
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

                using var streamReader = new StreamReader(path, Encoding.GetEncoding("Windows-1252"));
                return await ReadFileAsync(streamReader);

            }
            catch (Exception ex)
            {
                throw new IOException($"Error reading file: {path}. Details: {ex.Message}");
            }
        }

        private async Task<string> ReadFileAsync(StreamReader streamReader)
        {
            try
            {
                return await streamReader.ReadToEndAsync();
            }
            catch (Exception ex)
            {
                throw new IOException($"Error reading stream. Details: {ex.Message}");
            }
        }
    }
    
}
