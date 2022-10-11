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
    private readonly IRepository<DocumentContentModel> _repository;

    public DocumentContentController(ILogger<DocumentContentController> logger, IRepository<DocumentContentModel> repository)
    {
        _logger = logger;
        _repository = repository;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        try
        {
            IEnumerable<DocumentContentModel> result = _repository.GetAll().ToList();
            return result.Any() ? Ok(result) : NoContent();
        }
        catch (Exception e)
        {
            _logger.LogError("Unable to get document contents: {message}", e.Message);
            return Problem("Unable to fetch document contents");
        }
    }

    [HttpGet]
    [Route("{id:int}")]
    public IActionResult GetById(int id)
    {
        try
        {
            DocumentContentModel result = _repository.Get(id);
            return Ok(result);
        }
        catch (Exception e)
        {
            _logger.LogError("Unable to get document content with id {id}: {message}", id, e.Message);
            return Problem("Unable to fetch document content with id " + id);
        }
    }

    [HttpPost]
    public IActionResult PostDocumentContent([FromBody] DocumentContentModel documentContentModel)
    {
        _repository.Add(documentContentModel);
        return Ok();
    }
}
