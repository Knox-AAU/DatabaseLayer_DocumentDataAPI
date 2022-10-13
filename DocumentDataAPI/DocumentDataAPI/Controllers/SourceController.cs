using System.Net.Mime;
using DocumentDataAPI.Models;
using Microsoft.AspNetCore.Mvc;
using DocumentDataAPI.Data.Repositories;

namespace DocumentDataAPI.Controllers;

[ApiController]
[Route("sources")]
[Produces(MediaTypeNames.Application.Json)]
public class SourceController : ControllerBase
{
    private readonly ISourceRepository _repository;
    private readonly ILogger<SourceController> _logger;

    public SourceController(ISourceRepository repository, ILogger<SourceController> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<IEnumerable<SourceModel>> GetAll()
    {
        try
        {
            return Ok(_repository.GetAll());
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unable to get sources");
            return Problem(e.Message);
        }
    }

    [HttpGet]
    [Route("{id:long}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<SourceModel> GetById(long id)
    {
        try
        {
            SourceModel? result = _repository.Get(id);
            return result == null
                ? NotFound()
                : Ok(result);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unable to get source with id: {id}", id);
            return Problem(e.Message);
        }
    }

    [HttpGet]
    [Route("{name}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<IEnumerable<SourceModel>> GetByName(string name)
    {
        try
        {
            IEnumerable<SourceModel> result = _repository.GetByName(name);
            return result.Any()
                ? Ok(result)
                : NotFound("No source exists with name: " + name);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unable to get source with name: {name}", name);
            return Problem(e.Message);
        }
    }

    [HttpGet]
    [Route("{id:long}/document-count")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<int> GetDocumentCount(long id)
    {
        try
        {
            return Ok(_repository.GetCountFromId(id));
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unable to get document count for source with id: {id}", id);
            return Problem(e.Message);
        }
    }

    [HttpPut]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<SourceModel?> PutSource(string name)
    {
        try
        {
            return _repository.Add(new SourceModel { Name = name }) == 1
                ? Ok(_repository.GetByName(name).Last())
                : BadRequest("Could not add the source with name: " + name);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unable to add source with name: {name})", name);
            return Problem(e.Message);
        }
    }

    [HttpPost]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<SourceModel?> UpdateSource([FromBody] SourceModel source)
    {
        try
        {
            return _repository.Update(source) == 1
                ? Ok(_repository.Get(source.Id))
                : NotFound("Could not find source with id: " + source.Id);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unable to update source ({id}, {name})", source.Id, source.Name);
            return Problem(e.Message);
        }
    }
}
