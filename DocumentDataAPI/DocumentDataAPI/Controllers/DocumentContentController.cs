using DocumentDataAPI.Data.Repositories;
using DocumentDataAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace DocumentDataAPI.Controllers;

[ApiController]
[Route("[controller]")]
[Produces("application/json")]
public class DocumentContentController : ControllerBase
{
    private readonly ILogger<DocumentContentController> _logger;
    private readonly IDocumentContentRepository _repository;

    public DocumentContentController(ILogger<DocumentContentController> logger, IDocumentContentRepository repository)
    {
        _logger = logger;
        _repository = repository;
        _repository = repository;
    }

    [HttpGet]
    [Route("")]
    public IActionResult GetAll()
    {
        try
        {
            IEnumerable<DocumentContentModel> result = _repository.GetAll().ToList();
            return result.Any() ? Ok(result) : NoContent();
        }
        catch (Exception e)
        {
            _logger.LogError("Unable to fetch document contents: {message}", e.Message);
            return Problem("Unable to fetch document contents");
        }
    }

    [HttpGet]
    [Route("{documentId:long}")]
    public IActionResult GetById(long documentId)
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
            _logger.LogError("Unable to get document content with document_id {id}: {message}", documentId, e.Message);
            return Problem("Unable to fetch document content with document_id " + documentId);
        }
    }

    [HttpPut]
    [Route("")]
    public IActionResult PutDocumentContent([FromBody] DocumentContentModel documentContent)
    {
        try
        {
            return _repository.Add(documentContent) == 1
                ? Ok()
                : Problem("No rows were added");
        }
        catch (Exception e)
        {
            _logger.LogError("Unable to insert document content with document_id: {id}: {message}",
                documentContent.DocumentId, e.Message);
            return Problem("Unable to insert document content with document_id " + documentContent.DocumentId);
        }
    }

    [HttpPost]
    [Route("{documentId:long}")]
    public IActionResult UpdateDocumentContent(long documentId, [FromBody] DocumentContentModel documentContent)
    {
        try
        {
            return _repository.Update(documentContent) == 1
                ? Ok(_repository.Get(documentId))
                : NotFound();
        }
        catch (Exception e)
        {
            _logger.LogError("Unable to update document content with document_id: {id}: {message}",
                documentContent.DocumentId, e.Message);
            return Problem("Unable to update document content with document_id " + documentContent.DocumentId);
        }
    }
}
