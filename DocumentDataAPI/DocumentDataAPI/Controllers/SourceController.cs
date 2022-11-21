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
    /// <param name="limit">The maximum number of rows to get.</param>
    /// <param name="offset">The number of rows to skip (previous offset + previous limit).</param>
    /// <response code="200">Success: A list of all sources.</response>
    /// <response code="204">No Content: Nothing is returned.</response>
    /// <response code="500">Internal Server Error: A <see cref="ProblemDetails"/> describing the error.</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<SourceModel>>> GetAll(int? limit, int? offset)
    {
        try
        {
            IEnumerable<SourceModel> result = await _repository.GetAll(limit, offset);
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
    /// <response code="204">No Content: Nothing is returned.</response>
    /// <response code="500">Internal Server Error: A <see cref="ProblemDetails"/> describing the error.</response>
    [HttpGet]
    [Route("{id:long}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<SourceModel>> GetById(long id)
    {
        try
        {
            SourceModel? result = await _repository.Get(id);
            return result == null
                ? NoContent()
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
    /// <response code="204">No Content: Nothing is returned.</response>
    /// <response code="500">Internal Server Error: A <see cref="ProblemDetails"/> describing the error.</response>
    [HttpGet]
    [Route("{name}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<SourceModel>>> GetByName(string name)
    {
        try
        {
            IEnumerable<SourceModel> result = await _repository.GetByName(name);
            return result.Any()
                ? Ok(result)
                : NoContent();
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
    public async Task<ActionResult<int>> GetDocumentCount(long id)
    {
        try
        {
            return Ok(await _repository.GetCountFromId(id));
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
    /// <response code="200">Success: The ID of the data source that was inserted.</response>
    /// <response code="400">Bad Request: A message indicating that the source could not be added.</response>
    /// <response code="500">Internal Server Error: A <see cref="ProblemDetails"/> describing the error.</response>
    [HttpPost]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<long>> InsertSource(string name)
    {
        try
        {
            long id = await _repository.Add(new SourceModel { Name = name });
            return Ok(id);
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
    /// <response code="204">No Content: Nothing is returned.</response>
    /// <response code="500">Internal Server Error: A <see cref="ProblemDetails"/> describing the error.</response>
    [HttpPut]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<SourceModel?>> UpdateSource([FromBody] SourceModel source)
    {
        try
        {
            return await _repository.Update(source) == 1
                ? Ok(await _repository.Get(source.Id))
                : NoContent();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unable to update source ({id}, {name})", source.Id, source.Name);
            return Problem(e.Message);
        }
    }

    /// <summary>
    /// Deletes an existing source from the database matching the provided id.
    /// </summary>
    /// <response code="200">Success: Nothing is returned.</response>
    /// <response code="204">No Content: Nothing is returned.</response>
    /// <response code="500">Internal Server Error: A <see cref="ProblemDetails"/> describing the error.</response>
    [HttpDelete]
    [Route("{sourceId:long}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> DeleteSource(long sourceId)
    {
        try
        {
            return await _repository.Delete(sourceId) == 1
                ? Ok()
                : NoContent();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unable to delete source with id: {sourceId}", sourceId);
            return Problem(e.Message);
        }
    }
}
