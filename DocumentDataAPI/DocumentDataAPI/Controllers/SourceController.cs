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
    public async Task<ActionResult<IEnumerable<SourceModel>>> GetAll()
    {
        try
        {
            IEnumerable<SourceModel> result = await _repository.GetAll();
            return result.Any()
                ? Ok(result)
                : NoContent();
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
    public async Task<ActionResult<SourceModel>> GetById(long id)
    {
        try
        {
            SourceModel? result = await _repository.Get(id);
            return result == null
                ? NotFound()
                : Ok(result);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unable to get data source with id: {id}", id);
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
    public async Task<ActionResult<IEnumerable<SourceModel>>> GetByName(string name)
    {
        try
        {
            IEnumerable<SourceModel> result = await _repository.GetByName(name);
            return result.Any()
                ? Ok(result)
                : NotFound("No data source exists with name: " + name);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unable to get data source with name: {name}", name);
            return Problem(e.Message);
        }
    }

    /// <summary>
    /// Retrieves the total number of documents for the given data source id.
    /// </summary>
    /// <response code="200">Success: The total number of documents for the given data source.</response>
    /// <response code="500">Internal Server Error: A <see cref="ProblemDetails"/> describing the error.</response>
    [HttpGet]
    [Route("{id:long}/document-count")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<int>> GetDocumentCount(long id)
    {
        try
        {
            return Ok(await _repository.GetCountFromId(id));
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unable to get document count for data source with id: {id}", id);
            return Problem(e.Message);
        }
    }

    /// <summary>
    /// Inserts a new data source with the given name in the database.
    /// </summary>
    /// <response code="200">Success: The ID of the data source that was inserted.</response>
    /// <response code="500">Internal Server Error: A <see cref="ProblemDetails"/> describing the error.</response>
    [HttpPut]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<long>> PutDataSource(string name)
    {
        try
        {
            long id = await _repository.Add(new SourceModel { Name = name });
            return Ok(id);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unable to add data source with name: {name})", name);
            return Problem(e.Message);
        }
    }

    /// <summary>
    /// Updates the data source given in the request body in the database.
    /// </summary>
    /// <response code="200">Success: The data source that was updated.</response>
    /// <response code="404">Not Found: A message.</response>
    /// <response code="500">Internal Server Error: A <see cref="ProblemDetails"/> describing the error.</response>
    [HttpPost]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<SourceModel?>> UpdateDataSource([FromBody] SourceModel source)
    {
        try
        {
            return await _repository.Update(source) == 1
                ? Ok(await _repository.Get(source.Id))
                : NotFound("Could not find data source with id: " + source.Id);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unable to update data source ({id}, {name})", source.Id, source.Name);
            return Problem(e.Message);
        }
    }

    /// <summary>
    /// Deletes the data source given in the request body in the database.
    /// </summary>
    /// <response code="200">Success: The data source that was deleted.</response>
    /// <response code="404">Not Found: A message.</response>
    /// <response code="500">Internal Server Error: A <see cref="ProblemDetails"/> describing the error.</response>
    [HttpDelete]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<SourceModel?>> DeleteDataSource([FromBody] SourceModel source)
    {
        try
        {
            return await _repository.Delete(source) == 1
                ? Ok(source)
                : NotFound("Could not find data source with id: " + source.Id);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unable to delete data source ({id}, {name})", source.Id, source.Name);
            return Problem(e.Message);
        }
    }
}
