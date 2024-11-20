namespace ConsoleApp.Abstracts
{
    public interface IOutputWriter
    {
        void WriteFrequencies(string path, Dictionary<string, int> frequenciesDictionary);
    }
}
