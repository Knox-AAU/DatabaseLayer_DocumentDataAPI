using System.Net.Mime;
using DocumentDataAPI.Data.Repositories;
using DocumentDataAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace DocumentDataAPI.Controllers;

[ApiController]
[Route(RoutePrefixHelper.Prefix + "/document-contents")]
[Produces(MediaTypeNames.Application.Json)]
public class DocumentContentController : ControllerBase
{
    private readonly ILogger<DocumentContentController> _logger;
    private readonly IDocumentContentRepository _repository;

    public DocumentContentController(ILogger<DocumentContentController> logger, IDocumentContentRepository repository)
    {
        _logger = logger;
        _repository = repository;
    }

    /// <summary>
    /// Retrieves all document contents.
    /// </summary>
    /// <param name="limit">The maximum number of rows to get.</param>
    /// <param name="offset">The number of rows to skip (previous offset + previous limit).</param>
    /// <response code="200">Success: A list of document contents.</response>
    /// <response code="204">No Content: Nothing is returned.</response>
    /// <response code="500">Internal Server Error: A <see cref="ProblemDetails"/> describing the error.</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<DocumentContentModel>>> GetAll(int? limit = 100, int? offset = null)
    {
        try
        {
            IEnumerable<DocumentContentModel> result = await _repository.GetAll(limit, offset);
            return result.Any()
                ? Ok(result)
                : NoContent();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unable to fetch document contents");
            return Problem(e.Message);
        }
    }

    /// <summary>
    /// Retrieves the document content for the given document id.
    /// </summary>
    /// <response code="200">Success: A document content for the given document id.</response>
    /// <response code="204">No Content: Nothing is returned.</response>
    /// <response code="500">Internal Server Error: A <see cref="ProblemDetails"/> describing the error.</response>
    [HttpGet]
    [Route("{documentId:long}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<DocumentContentModel?>> GetByDocumentIdAndIndex(long documentId, int index)
    {
        try
        {
            DocumentContentModel? result = await _repository.Get(documentId, index);
            return result == null
                ? NoContent()
                : Ok(result);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unable to fetch document content with documents_id: {id}", documentId);
            return Problem(e.Message);
        }
    }

    /// <summary>
    /// Adds the document contents from the request body to the database.
    /// </summary>
    /// <response code="200">Success: The document content that was added to the database.</response>
    /// <response code="500">Internal Server Error: A <see cref="ProblemDetails"/> describing the error.</response>
    [HttpPost]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<DocumentContentModel?>> InsertDocumentContent([FromBody] List<DocumentContentModel> documentContents)
    {
        try
        {
            return await _repository.AddBatch(documentContents) == documentContents.Count
                ? Ok(documentContents.Count)
                : Problem("No rows were added");
        }
        catch (Exception e)
        {
            _logger.LogError("Unable to insert document contents: {message}", e.Message);
            return Problem(e.Message);
        }
    }

    /// <summary>
    /// Updates an existing document content from the request body in the database.
    /// </summary>
    /// <response code="200">Success: The document content that was updated.</response>
    /// <response code="204">No Content: Nothing is returned.</response>
    /// <response code="500">Internal Server Error: A <see cref="ProblemDetails"/> describing the error.</response>
    [HttpPut]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<DocumentContentModel?>> UpdateDocumentContent([FromBody] DocumentContentModel documentContent)
    {
        try
        {
            return await _repository.Update(documentContent) == 1
                ? Ok(_repository.Get(documentContent.DocumentId, documentContent.Index))
                : NoContent();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unable to update document content with documents_id: {id}",
                documentContent.DocumentId);
            return Problem(e.Message);
        }
    }

    /// <summary>
    /// Deletes an existing document content from the database matching the provided id and index.
    /// </summary>
    /// <response code="200">Success: Nothing is returned.</response>
    /// <response code="204">No Content: Nothing is returned.</response>
    /// <response code="500">Internal Server Error: A <see cref="ProblemDetails"/> describing the error.</response>
    [HttpDelete]
    [Route("{documentId:long}/{documentIndex:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> DeleteDocumentContent(long documentId, int documentIndex)
    {
        try
        {
            return await _repository.Delete(documentId, documentIndex) == 1
                ? Ok()
                : NoContent();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unable to delete document content with document id: {documentId} and index: {documentIndex}", documentId, documentIndex);
            return Problem(e.Message);
        }
    }
}
