namespace ConsoleApp.Abstracts
{
    public interface IInputFileReader 
    {
        IAsyncEnumerable<string> ReadFileInChunksAsync(string path);
    }
}
