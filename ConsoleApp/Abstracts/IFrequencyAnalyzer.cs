namespace ConsoleApp.Abstracts
{
    public interface IFrequencyAnalyzer
    {
        Dictionary<string, int> GetWordFrequencies(string fileContent);
    }
}
