namespace ConsoleApp.Abstracts
{
    public interface IFrequencyProcessor 
    {
        Task<Dictionary<string, int>> GetWordFrequenciesAsync(string fileContent);
    }
}
