namespace DocumentDataAPI.Data.Services;

public interface ILemmatizerService
{
    Task<string> GetLemmatizedString(string input);
    Task<string> GetLemmatizedString(string input, string language);
}
