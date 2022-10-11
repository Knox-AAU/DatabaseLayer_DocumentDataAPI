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
    public IActionResult Get(long? documentId)
    {
        try
        {
            if (documentId.HasValue)
            {
                DocumentContentModel? result = _repository.Get(documentId.Value);
                return result == null
                    ? NotFound()
                    : Ok(result);
            }
            else
            {
                IEnumerable<DocumentContentModel> result = _repository.GetAll();
                return result.Any()
                    ? Ok(result)
                    : NoContent();
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unable to fetch document contents");
            return Problem(e.Message);
        }
    }

    [HttpPut]
    [Route("")]
    public IActionResult PutDocumentContent([FromBody] DocumentContentModel documentContent)
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

    [HttpPost]
    [Route("")]
    public IActionResult UpdateDocumentContent([FromBody] DocumentContentModel documentContent)
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
