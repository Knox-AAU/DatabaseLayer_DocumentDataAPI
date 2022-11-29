using DocumentDataAPI.Data.Repositories;
using DocumentDataAPI.Models;

using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace DocumentDataAPI.Controllers;

[ApiController]
[Route(RoutePrefixHelper.Prefix + "/similarDocument")]
[Produces(MediaTypeNames.Application.Json)]
public class SimilarDocumentController : ControllerBase
{
    private readonly ILogger<SimilarDocumentController> _logger;
    private readonly ISimilarDocumentRepository _repository;

    public SimilarDocumentController(ILogger<SimilarDocumentController> logger, ISimilarDocumentRepository repository)
    {
        _logger = logger;
        _repository = repository;
    }

    /// <summary>
    /// Adds the SimilarDocument to the MainDocument and returns the ID of the MainDocument.
    /// </summary>
    /// <response code="200">Success: ID of the MainDocument.</response>
    /// <response code="500">Internal Server Error: a <see cref="ProblemDetails"/> describing the error.</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<List<long>>> SetSimilarDocument([FromBody] List<SimilarDocumentModel> mainDocument)
    {
        try
        {
            IEnumerable<long> insertedIds = await _repository.AddBatch(mainDocument);
            return Ok(insertedIds);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unable to add mainDocument.");
            return Problem(e.Message);
        }
    }

    /// <summary>
    /// Retrieves a list of all similar documents from the database.
    /// </summary>
    /// <param name="limit">The maximum number of rows to get.</param>
    /// <param name="offset">The number of rows to skip (previous offset + previous limit).</param>
    /// <response code="200">Success: A list of all similar documents</response>
    /// <response code="204">No Content: Nothing is returned.</response>
    /// <response code="500">Internal Server Error: a <see cref="ProblemDetails"/> describing the error.</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<SimilarDocumentModel>>> GetAll(int? limit = 100, int? offset = null)
    {
        try
        {
            IEnumerable<SimilarDocumentModel> result = await _repository.GetAll(limit, offset);
            return result.Any()
                ? Ok(result)
                : NoContent();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unable to get sources");
            return Problem(e.Message);
        }
    }

    /// <summary>
    /// Retrieves a list of similar document based on the mainDocument id.
    /// </summary>
    /// <response code="200">Success: A list of similar document for the given mainDocument id.</response>
    /// <response code="204">No Content: Nothing is returned.</response>
    /// <response code="500">Internal Server Error: a <see cref="ProblemDetails"/> describing the error.</response>
    [HttpGet]
    [Route("/{mainDocumentId:long}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<SimilarDocumentModel>>?> GetById(long mainDocumentId)
    {
        try
        {
            IEnumerable<SimilarDocumentModel>? result = await _repository.Get(mainDocumentId);
            return result.Any()
                ? Ok(result)
                : NoContent();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unable to get similarDocument with mainDocumentId: " +
                "{mainDocumentId}.", mainDocumentId);
            return Problem(e.Message);
        }
    }

    /// <summary>
    /// Deletes an existing similarDocument from the database matching the provided mainDocument id and similarDocument id.
    /// </summary>
    /// <response code="200">Success: Nothing is returned.</response>
    /// <response code="204">No Content: Nothing is returned.</response>
    /// <response code="500">Internal Server Error: A <see cref="ProblemDetails"/> describing the error.</response>
    [HttpDelete]
    [Route("/{mainDocumentId:long}/{similarDocumentId:long}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> DeleteSimilarDocument(long mainDocumentId, long similarDocumentId)
    {
        try
        {
            return await _repository.Delete(mainDocumentId, similarDocumentId) == 1
                ? Ok()
                : NoContent();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unable tt delete similarDocument with mainDocumentId: " +
                "{mainDocumentId} and {similarDocumentId}.", mainDocumentId, similarDocumentId);
            return Problem(e.Message);
        }
    }
}
