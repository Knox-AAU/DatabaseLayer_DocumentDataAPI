using DocumentDataAPI.Models.Lemmatizer;

namespace DocumentDataAPI.Data.Services;

public class LemmatizerService : ILemmatizerService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<LemmatizerService> _logger;
    private readonly string _lemmatizerApiUrl;
    private readonly string _defaultLanguage;

    public LemmatizerService(HttpClient httpClient, IConfiguration configuration, ILogger<LemmatizerService> logger)
    {
        _defaultLanguage = configuration.GetValue<string>("Lemmatizer:DefaultLanguage", defaultValue: "da") ??
                           throw new ArgumentNullException(nameof(configuration));
        _lemmatizerApiUrl = configuration.GetValue<string>("Lemmatizer:ApiUrl") ??
                            throw new ArgumentNullException(nameof(configuration));
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<string> GetLemmatizedString(string input) => await GetLemmatizedString(input, _defaultLanguage);

    public async Task<string> GetLemmatizedString(string input, string language)
    {
        try
        {
            LemmatizerRequestModel requestModel = new(input, language);
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync(_lemmatizerApiUrl, requestModel);
            LemmatizerResponseModel? lemmatizedResponse =
                await response.Content.ReadFromJsonAsync<LemmatizerResponseModel>();

            return lemmatizedResponse?.LemmatizedString
                   ?? throw new HttpRequestException("Could not parse the response from the Lemmatizer API");
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to lemmatize input, will use fallback value");
            return GetFallbackValue(input);
        }
    }

    private static string GetFallbackValue(string input)
    {
        return input.ToLowerInvariant();
    }
}