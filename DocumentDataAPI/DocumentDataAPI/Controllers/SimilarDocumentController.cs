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
    /// Adds the documents from the content body to the database and returns a sequential list of IDs for the inserted documents.
    /// </summary>
    /// <response code="200">Success: A list of IDs for the added document (i.e., the last inserted ID is last in the list).</response>
    /// <response code="500">Internal Server Error: a <see cref="ProblemDetails"/> describing the error.</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<List<long>>> SetSimilarDocument([FromBody] List<SimilarDocumentModel> similarDocument)
    {
        try
        {
            IEnumerable<long> insertedIds = await _repository.AddBatch(similarDocument);
            return Ok(insertedIds);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unable to add similarDocument.");
            return Problem(e.Message);
        }
    }

    /// <summary>
    /// Retrieves a list of all similar documents from the database.
    /// </summary>
    /// <param name="limit">The maximum number of rows to get.</param>
    /// <param name="offset">The number of rows to skip (previous offset + previous limit).</param>
    /// <response code="200">Success: A list of all documents</response>
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
    /// Retrieves a document based on the document id.
    /// </summary>
    /// <response code="200">Success: A document for the given id.</response>
    /// <response code="204">No Content: Nothing is returned.</response>
    /// <response code="500">Internal Server Error: a <see cref="ProblemDetails"/> describing the error.</response>
    [HttpGet]
    [Route("/{mainDocumentId:long}/{similarDocumentId:long}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<SimilarDocumentModel>> GetById(long mainDocumentId, long similarDocumentId)
    {
        try
        {
            SimilarDocumentModel? result = await _repository.Get(mainDocumentId, similarDocumentId);
            return result == null
                ? NoContent()
                : Ok(result);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unable to get similarDocument with mainDocumentId: " +
                "{mainDocumentId} and {similarDocumentId}.", mainDocumentId, similarDocumentId);
            return Problem(e.Message);
        }
    }

    /// <summary>
    /// Deletes an existing document from the database matching the provided id.
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
