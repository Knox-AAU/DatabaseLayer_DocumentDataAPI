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
    /// <response code="200">Success: A list of document contents.</response>
    /// <response code="204">No Content: Nothing is returned.</response>
    /// <response code="500">Internal Server Error: A <see cref="ProblemDetails"/> describing the error.</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<IEnumerable<DocumentContentModel>> GetAll()
    {
        try
        {
            IEnumerable<DocumentContentModel> result = _repository.GetAll();
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
    /// <response code="404">Not Found: Nothing is returned.</response>
    /// <response code="500">Internal Server Error: A <see cref="ProblemDetails"/> describing the error.</response>
    [HttpGet]
    [Route("{documentId:long}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<DocumentContentModel?> GetByDocumentId(long documentId)
    {
        try
        {
            DocumentContentModel? result = _repository.Get(documentId);
            return result == null
                ? NotFound()
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
    [HttpPut]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<DocumentContentModel?> PutDocumentContent([FromBody] DocumentContentModel documentContent)
    {
        try
        {
            return _repository.Add(documentContent) == 1
                ? Ok(_repository.Get(documentContent.DocumentId))
                : Problem("No rows were added");
        }
        catch (Exception e)
        {
            _logger.LogError("Unable to insert document content with documents_id: {id}: {message}",
                documentContent.DocumentId, e.Message);
            return Problem(e.Message);
        }
    }

    /// <summary>
    /// Updates an existing document content from the request body in the database.
    /// </summary>
    /// <response code="200">Success: The document content that was updated.</response>
    /// <response code="500">Internal Server Error: A <see cref="ProblemDetails"/> describing the error.</response>
    [HttpPost]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<DocumentContentModel?> UpdateDocumentContent([FromBody] DocumentContentModel documentContent)
    {
        try
        {
            return _repository.Update(documentContent) == 1
                ? Ok(_repository.Get(documentContent.DocumentId))
                : NotFound();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unable to update document content with documents_id: {id}", documentContent.DocumentId);
            return Problem(e.Message);
        }
    }
}
