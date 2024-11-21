namespace ConsoleApp.Abstracts
{
    public interface IInputFileReader 
    {
       Task<string> ReadFileAsync(string path);
    }
}
