using DocumentDataAPI.Data.Repositories;
using DocumentDataAPI.Models;

using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace DocumentDataAPI.Controllers;

[ApiController]
[Route(RoutePrefixHelper.Prefix + "/documents")]
[Produces(MediaTypeNames.Application.Json)]
public class DocumentController : ControllerBase
{
    private readonly ILogger<DocumentController> _logger;
    private readonly IDocumentRepository _repository;

    public DocumentController(ILogger<DocumentController> logger, IDocumentRepository repository)
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
    public async Task<ActionResult<List<long>>> InsertDocuments([FromBody] List<DocumentModel> documents)
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
    /// Retrieves a list of all documents from the database.
    /// </summary>
    /// <response code="200">Success: A list of all documents</response>
    /// <response code="204">No Content: Nothing is returned.</response>
    /// <response code="500">Internal Server Error: a <see cref="ProblemDetails"/> describing the error.</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<DocumentModel>>> GetAll(int? sourceId, string? author, int? categoryId, DateTime? beforeDate, DateTime? afterDate)
    {
        try
        {
            DocumentSearchParameters parameters = new DocumentSearchParameters();
            if (sourceId is not null) parameters.AddSource(sourceId.Value);
            if (author is not null) parameters.AddAuthor(author);
            if (categoryId is not null) parameters.AddCategory(categoryId.Value);
            if (beforeDate is not null) parameters.AddBeforeDate(beforeDate.Value);
            if (afterDate is not null) parameters.AddAfterDate(afterDate.Value);

            IEnumerable<DocumentModel> result = await _repository.GetAll(parameters);
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

    /// <summary>
    /// Retrieves a document based on the document id.
    /// </summary>
    /// <response code="200">Success: A document for the given id.</response>
    /// <response code="204">No Content: Nothing is returned.</response>
    /// <response code="500">Internal Server Error: a <see cref="ProblemDetails"/> describing the error.</response>
    [HttpGet]
    [Route("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<DocumentModel>> GetById(int id)
    {
        try
        {
            DocumentModel? result = await _repository.Get(id);
            return result == null
                ? NoContent()
                : Ok(result);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unable to get document with id: {id}.", id);
            return Problem(e.Message);
        }
    }

    [HttpGet]
    [Route("authors")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<string>>> GetAuthors()
    {
        try
        {
            IEnumerable<string> result = await _repository.GetAuthors();
            return result.Any()
                ? Ok(result)
                : NoContent();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unable to get authors.");
            return Problem(e.Message);
        }
    }
    
    [HttpGet]
    [Route("categories")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<CategoryModel>>> GetCategories()
    {
        try
        {
            IEnumerable<CategoryModel> result = await _repository.GetCategories();
            return result.Any()
                ? Ok(result)
                : NoContent();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unable to get categories.");
            return Problem(e.Message);
        }
    }

    /// <summary>
    /// Retrieves the number of documents in the database.
    /// </summary>
    /// <response code="200">Success: The number of rows added.</response>
    /// <response code="500">Internal Server Error: a <see cref="ProblemDetails"/> describing the error.</response>
    [HttpGet]
    [Route("count")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<int>> GetTotalDocumentCount()
    {
        try
        {
            int result = await _repository.GetTotalDocumentCount();
            return Ok(result);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unable to get document count.");
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
    public async Task<ActionResult<DocumentModel>> UpdateDocument([FromBody] DocumentModel documentModel)
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
}
