using DocumentDataAPI.Data.Repositories;
using DocumentDataAPI.Models;

using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace DocumentDataAPI.Controllers;

[ApiController]
[Route(RoutePrefixHelper.Prefix + "/similar-documents")]
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
    /// Adds all provided similar documents entities, returning the ids of the documents they are related to.
    /// </summary>
    /// <param name="similarDocuments">A list of similar documents objects containing: mainDocument id, similarDocument id and their similarity.</param>
    /// <response code="200">Success: ID of the MainDocuments.</response>
    /// <response code="500">Internal Server Error: a <see cref="ProblemDetails"/> describing the error.</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<List<long>>> InsertSimilarDocuments([FromBody] List<SimilarDocumentModel> similarDocuments)
    {
        try
        {
            IEnumerable<int> result = await _repository.AddBatch(similarDocuments);
            return result.Any()
                 ? Ok(result)
                 : NoContent();

        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unable to add similar documents.");
            return Problem(e.Message);
        }
    }

    /// <summary>
    /// Retrieves a list of all similar documents entities from the database.
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
            _logger.LogError(e, "Unable to get similar documents");
            return Problem(e.Message);
        }
    }

    /// <summary>
    /// Retrieves a list of similar document entities for the given document id.
    /// </summary>
    /// <param name="mainDocumentId">The id of the mainDocument.</param>
    /// <response code="200">Success: A list of similar documents for the given document id.</response>
    /// <response code="204">No Content: Nothing is returned.</response>
    /// <response code="500">Internal Server Error: a <see cref="ProblemDetails"/> describing the error.</response>
    [HttpGet]
    [Route("/{mainDocumentId:long}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<SimilarDocumentModel>>> GetById(long mainDocumentId)
    {
        try
        {
            IEnumerable<SimilarDocumentModel> result = await _repository.Get(mainDocumentId);
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
    /// Deletes all existing similarDocuments from the database.
    /// </summary>
    /// <response code="200">Success: Nothing is returned.</response>
    /// <response code="204">No Content: Nothing is returned.</response>
    /// <response code="500">Internal Server Error: A <see cref="ProblemDetails"/> describing the error.</response>
    [HttpDelete]
    [Route("/delete-all")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> DeleteAll()
    {
        try
        {
            return await _repository.DeleteAll() == 1
                ? Ok()
                : NoContent();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unable to delete all similarDocument with mainDocumentId");
            return Problem(e.Message);
        }
    }
}
