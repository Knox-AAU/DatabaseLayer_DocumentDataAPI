using DocumentDataAPI.Models.Lemmatizer;

namespace DocumentDataAPI.Data.Services;

public class LemmatizerService : ILemmatizerService
{
    private readonly HttpClient _httpClient;
    private readonly string _lemmatizerApiUrl;
    private readonly string _defaultLanguage;

    public LemmatizerService(HttpClient httpClient, IConfiguration configuration)
    {
        _defaultLanguage = configuration.GetValue<string>("Lemmatizer:DefaultLanguage", defaultValue: "da");
        _lemmatizerApiUrl = configuration.GetValue<string>("Lemmatizer:ApiUrl");
        _httpClient = httpClient;
    }

    public async Task<string> GetLemmatizedString(string input) => await GetLemmatizedString(input, _defaultLanguage);

    public async Task<string> GetLemmatizedString(string input, string language)
    {
        LemmatizerRequestModel requestModel = new(input, language);
        HttpResponseMessage response = await _httpClient.PostAsJsonAsync(_lemmatizerApiUrl, requestModel);
        LemmatizerResponseModel? lemmatizedResponse = await response.Content.ReadFromJsonAsync<LemmatizerResponseModel>();

        return lemmatizedResponse?.LemmatizedString
               ?? throw new HttpRequestException("Could not parse the response from the Lemmatizer API");
    }
}
