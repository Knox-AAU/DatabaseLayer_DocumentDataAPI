using DocumentDataAPI.Data.Repositories;
using DocumentDataAPI.Models;

using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using DocumentDataAPI.Data.Services;

namespace DocumentDataAPI.Controllers;

[ApiController]
[Route(RoutePrefixHelper.Prefix + "/search")]
[Produces(MediaTypeNames.Application.Json)]
public class SearchController : ControllerBase
{
    private readonly ILogger<SearchController> _logger;
    private readonly ISearchRepository _repository;
    private readonly ILemmatizerService _lemmatizerService;

    public SearchController(ILogger<SearchController> logger, ISearchRepository repository, ILemmatizerService lemmatizerService)
    {
        _logger = logger;
        _repository = repository;
        _lemmatizerService = lemmatizerService;
    }

    /// <summary>
    /// Retrieves a list of all documents relevant to a given search.
    /// </summary>
    /// <param name="limit">The maximum number of rows to get.</param>
    /// <param name="offset">The number of rows to skip (previous offset + previous limit).</param>
    /// <param name="words">A comma-separated list of words.</param>
    /// <param name="sourceId">A source ID used to delimit the search.</param>
    /// <param name="author">The name of an author, used to delimit the search.</param>
    /// <param name="categoryId">The ID of a category, used to delimit the search.</param>
    /// <param name="beforeDate">A minimum date for documents.</param>
    /// <param name="afterDate">A maximum date for documents.</param>
    /// <response code="200">Success: A list of documents with their relevance to the search.</response>
    /// <response code="204">No Content: Nothing is returned.</response>
    /// <response code="500">Internal Server Error: a <see cref="ProblemDetails"/> describing the error.</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<SearchResponseModel>>> Get(string words, [FromQuery] List<long> sourceIds, [FromQuery] List<string> authors, [FromQuery] List<int> categoryIds, DateTime? beforeDate, DateTime? afterDate, int? limit, int? offset)
    {
        try
        {
            string lemmatizerInput = words.Replace(',', ' ');
            string lemmatizedString = await _lemmatizerService.GetLemmatizedString(lemmatizerInput);
            List<string> processedWords = lemmatizedString.Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
                .ToList();

            DocumentSearchParameters parameters = new(sourceIds, authors, categoryIds, beforeDate, afterDate);

            IEnumerable<SearchResponseModel> result = await _repository.Get(processedWords, parameters, limit, offset);
            return result.Any()
                ? Ok(result)
                : NoContent();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unable to get search results");
            return Problem(e.Message);
        }
    }
}
