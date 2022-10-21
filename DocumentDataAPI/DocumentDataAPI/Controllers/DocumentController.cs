using DocumentDataAPI.Data.Repositories;
using DocumentDataAPI.Models;

using Microsoft.AspNetCore.Mvc;

using System.Data.Common;
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
    /// Adds the document from the content body.
    /// </summary>
    /// <response code="200">Success: The document that was added to the database.</response>
    /// <response code="500">Internal Server Error: a <see cref="ProblemDetails"/> describing the error.</response>
    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<DocumentModel> PutDocument([FromBody] List<DocumentModel> documents)
    {
        try
        {
            return _repository.AddBatch(documents) == 0
                ? Problem("No rows were added")
                : Ok($"Added {documents.Count} documents.");
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
    /// <response code="404">Not Found: Nothing is returned.</response>
    /// <response code="500">Internal Server Error: a <see cref="ProblemDetails"/> describing the error.</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<IEnumerable<DocumentModel>> GetAll(int? sourceId, string? author, DateTime? beforeDate, DateTime? afterDate)
    {
        try
        {
            DocumentSearchParameters parameters = new DocumentSearchParameters();
            if (sourceId is not null) parameters.AddSource(sourceId.Value);
            if (author is not null) parameters.AddAuthor(author);
            if (beforeDate is not null) parameters.AddBeforeDate(beforeDate.Value);
            if (afterDate is not null) parameters.AddAfterDate(afterDate.Value);

            IEnumerable<DocumentModel> result = _repository.GetAll(parameters);
            return result.Any()
                ? Ok(result)
                : NotFound();
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
    /// <response code="404">Not Found: Nothing is returned.</response>
    /// <response code="500">Internal Server Error: a <see cref="ProblemDetails"/> describing the error.</response>
    [HttpGet]
    [Route("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<DocumentModel> GetById(int id)
    {
        try
        {
            DocumentModel? result = _repository.Get(id);
            return result == null
                ? NotFound()
                : Ok(result);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unable to get document with id: {id}.", id);
            return Problem(e.Message);
        }
    }

    /// <summary>
    /// Retrieves the number of documents in the database.
    /// </summary>
    /// <response code="200">Success: A number</response>
    /// <response code="404">Not Found: Nothing is returned.</response>
    /// <response code="500">Internal Server Error: a <see cref="ProblemDetails"/> describing the error.</response>
    [HttpGet]
    [Route("count")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<int> GetTotalDocumentCount()
    {
        try
        {
            int result = _repository.GetTotalDocumentCount();
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
    /// <response code="404">Not Found: Nothing is returned.</response>
    /// <response code="500">Internal Server Error: a <see cref="ProblemDetails"/> describing the error.</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<DocumentModel> UpdateDocument([FromBody] DocumentModel documentModel)
    {
        try
        {
            return _repository.Update(documentModel) == 1
                ? Ok(_repository.Get(documentModel.Id))
                : NotFound();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unable to update document with id: {id}", documentModel.Id);
            return Problem(e.Message);
        }
    }

    /// <summary>
    /// Deletes the given <paramref name="documentModel"/> from the database.
    /// </summary>
    /// <response code="200">Success: Nothing is returned.</response>
    /// <response code="404">Not Found: Nothing is returned.</response>
    /// <response code="500">Internal Server Error: a <see cref="ProblemDetails"/> describing the error.</response>
    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<DocumentModel> DeleteDocument([FromBody] DocumentModel documentModel)
    {
        try
        {
            return _repository.Delete(documentModel) == 1
                ? Ok()
                : NotFound();
        }
        catch (DbException e) when (e.SqlState != null && e.SqlState.StartsWith("23")) // integrity_constraint_violation, see https://docs.actian.com/ingres/11.0/index.html#page/OpenSQLRef/SQLSTATE_Values.htm
        {
            _logger.LogWarning(e, "Rejected attempt to delete document with id: {id} due to constraint violations", documentModel.Id);
            return Problem($"Could not delete document with id {documentModel.Id} due to constraint violations");
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unable to delete document with id: {id}", documentModel.Id);
            return Problem(e.Message);
        }
    }
}
