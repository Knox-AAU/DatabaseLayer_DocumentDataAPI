using System.Net.Mime;
using DocumentDataAPI.Models;
using Microsoft.AspNetCore.Mvc;
using DocumentDataAPI.Data.Repositories;

namespace DocumentDataAPI.Controllers;

[ApiController]
[Route(RoutePrefixHelper.Prefix + "/data-sources")]
[Produces(MediaTypeNames.Application.Json)]
public class DataSourceController : ControllerBase
{
    private readonly IDataSourceRepository _repository;
    private readonly ILogger<DataSourceController> _logger;

    public DataSourceController(IDataSourceRepository repository, ILogger<DataSourceController> logger)
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
    public async Task<ActionResult<IEnumerable<DataSourceModel>>> GetAll()
    {
        try
        {
            IEnumerable<DataSourceModel> result = await _repository.GetAll();
            return result.Any()
                ? Ok(result)
                : NoContent();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unable to get data sources");
            return Problem(e.Message);
        }
    }

    /// <summary>
    /// Retrieves the data source with the given id.
    /// </summary>
    /// <response code="200">Success: The data source.</response>
    /// <response code="404">Not Found: Nothing is returned.</response>
    /// <response code="500">Internal Server Error: A <see cref="ProblemDetails"/> describing the error.</response>
    [HttpGet]
    [Route("{id:long}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<DataSourceModel>> GetById(long id)
    {
        try
        {
            DataSourceModel? result = await _repository.Get(id);
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
    /// Retrieves all data sources with the given name.
    /// </summary>
    /// <response code="200">Success: A list of data sources with the given name.</response>
    /// <response code="404">Not Found: Nothing is returned.</response>
    /// <response code="500">Internal Server Error: A <see cref="ProblemDetails"/> describing the error.</response>
    [HttpGet]
    [Route("{name}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<DataSourceModel>>> GetByName(string name)
    {
        try
        {
            IEnumerable<DataSourceModel> result = await _repository.GetByName(name);
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
            long id = await _repository.Add(new DataSourceModel { Name = name });
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
    public async Task<ActionResult<DataSourceModel?>> UpdateDataSource([FromBody] DataSourceModel dataSource)
    {
        try
        {
            return await _repository.Update(dataSource) == 1
                ? Ok(await _repository.Get(dataSource.Id))
                : NotFound("Could not find data source with id: " + dataSource.Id);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unable to update data source ({id}, {name})", dataSource.Id, dataSource.Name);
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
    public async Task<ActionResult<DataSourceModel?>> DeleteDataSource([FromBody] DataSourceModel dataSource)
    {
        try
        {
            return await _repository.Delete(dataSource) == 1
                ? Ok(dataSource)
                : NotFound("Could not find data source with id: " + dataSource.Id);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unable to delete data source ({id}, {name})", dataSource.Id, dataSource.Name);
            return Problem(e.Message);
        }
    }
}
