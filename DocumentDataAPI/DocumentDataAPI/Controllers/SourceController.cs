using System.Net.Mime;
using DocumentDataAPI.Models;
using Microsoft.AspNetCore.Mvc;
using DocumentDataAPI.Data.Repositories;

namespace DocumentDataAPI.Controllers;

[ApiController]
[Route(RoutePrefixHelper.Prefix + "/sources")]
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

    /// <summary>
    /// Retrieves all sources.
    /// </summary>
    /// <response code="200">Success: A list of all sources.</response>
    /// <response code="204">No Content: Nothing is returned.</response>
    /// <response code="500">Internal Server Error: A <see cref="ProblemDetails"/> describing the error.</response>
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

    /// <summary>
    /// Retrieves the source with the given id.
    /// </summary>
    /// <response code="200">Success: The source.</response>
    /// <response code="404">Not Found: Nothing is returned.</response>
    /// <response code="500">Internal Server Error: A <see cref="ProblemDetails"/> describing the error.</response>
    [HttpGet]
    [Route("{id:long}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<SourceModel> GetById(long id)
    {
        try
        {
            SourceModel? result = _repository.Get(id).Result;
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

    /// <summary>
    /// Retrieves all sources with the given name.
    /// </summary>
    /// <response code="200">Success: A list of sources with the given name.</response>
    /// <response code="404">Not Found: Nothing is returned.</response>
    /// <response code="500">Internal Server Error: A <see cref="ProblemDetails"/> describing the error.</response>
    [HttpGet]
    [Route("{name}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<IEnumerable<SourceModel>> GetByName(string name)
    {
        try
        {
            IEnumerable<SourceModel> result = _repository.GetByName(name).Result;
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

    /// <summary>
    /// Retrieves the total number of documents for the given source id.
    /// </summary>
    /// <response code="200">Success: The total number of documents for the given source.</response>
    /// <response code="500">Internal Server Error: A <see cref="ProblemDetails"/> describing the error.</response>
    [HttpGet]
    [Route("{id:long}/document-count")]
    [ProducesResponseType(StatusCodes.Status200OK)]
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

    /// <summary>
    /// Inserts a new source with the given name in the database.
    /// </summary>
    /// <response code="200">Success: The source that was inserted.</response>
    /// <response code="400">Bad Request: A message.</response>
    /// <response code="500">Internal Server Error: A <see cref="ProblemDetails"/> describing the error.</response>
    [HttpPut]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<SourceModel?> PutSource(string name)
    {
        try
        {
            return _repository.Add(new SourceModel { Name = name }).Result == 1
                ? Ok(_repository.GetByName(name).Result.Last())
                : BadRequest("Could not add the source with name: " + name);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unable to add source with name: {name})", name);
            return Problem(e.Message);
        }
    }

    /// <summary>
    /// Updates the source given in the request body in the database.
    /// </summary>
    /// <response code="200">Success: The source that was updated.</response>
    /// <response code="404">Not Found: A message.</response>
    /// <response code="500">Internal Server Error: A <see cref="ProblemDetails"/> describing the error.</response>
    [HttpPost]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<SourceModel?> UpdateSource([FromBody] SourceModel source)
    {
        try
        {
            return _repository.Update(source).Result == 1
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
