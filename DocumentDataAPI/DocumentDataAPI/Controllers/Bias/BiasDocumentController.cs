using DocumentDataAPI.Data.Repositories;
using DocumentDataAPI.Models;

using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace DocumentDataAPI.Controllers;

[ApiController]
[Route(RoutePrefixHelper.Prefix + "/bias_documents")]
[Produces(MediaTypeNames.Application.Json)]
public class BiasDocumentController : ControllerBase
{
    private readonly ILogger<BiasDocumentController> _logger;
    private readonly IBiasDocumentRepository _repository;

    public BiasDocumentController(ILogger<BiasDocumentController> logger, IBiasDocumentRepository repository)
    {
        _logger = logger;
        _repository = repository;
    }

    /// <summary>
    /// Adds the documents from the content body to the database and returns a sequential list of IDs for the inserted documents.
    /// </summary>
    /// <response code="200">Success: A list of IDs for the added documents (i.e., the last inserted ID is last in the list).</response>
    /// <response code="500">Internal Server Error: a <see cref="ProblemDetails"/> describing the error.</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<List<long>>> InsertDocuments([FromBody] List<BiasDocumentModel> documents)
    {
        try
        {
            IEnumerable<long> insertedIds = await _repository.AddBatch(documents);
            return Ok(insertedIds);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unable to add document.");
            return Problem(e.Message);
        }
    }

    /// <summary>
    /// Persists the changes to the given <paramref name="documentModel"/> in the database.
    /// </summary>
    /// <response code="200">Success: The updated document.</response>
    /// <response code="204">No Content: Nothing is returned.</response>
    /// <response code="500">Internal Server Error: a <see cref="ProblemDetails"/> describing the error.</response>
    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<BiasDocumentModel>> UpdateDocument([FromBody] BiasDocumentModel documentModel)
    {
        try
        {
            return await _repository.Update(documentModel) == 1
                ? Ok(_repository.Get(documentModel.Id))
                : NoContent();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unable to update document with id: {id}", documentModel.Id);
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
    [Route("{documentId:long}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> DeleteDocument(long documentId)
    {
        try
        {
            return await _repository.Delete(documentId) == 1
                ? Ok()
                : NoContent();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unable to delete document with id: {documentId}", documentId);
            return Problem(e.Message);
        }
    }

    /// <summary>
    /// Retrieves a list of all documents from the database.
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
    public async Task<ActionResult<IEnumerable<BiasDocumentModel>>> GetAll(int? limit = null, int? offset = null)
    {
        try
        {
            IEnumerable<BiasDocumentModel> result = await _repository.GetAll(limit, offset);
            return result.Any()
                ? Ok(result)
                : NoContent();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unable to get documents.");
            return Problem(e.Message);
        }
    }
}