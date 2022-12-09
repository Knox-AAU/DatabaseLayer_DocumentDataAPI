using System.Data.Common;
using System.Net.Mime;
using DocumentDataAPI.Data.Repositories;
using DocumentDataAPI.Exceptions;
using DocumentDataAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace DocumentDataAPI.Controllers;

[ApiController]
[Route(RoutePrefixHelper.Prefix + "/bias_word_count")]
[Produces(MediaTypeNames.Application.Json)]
public class BiasWordCountController : ControllerBase
{
    private readonly ILogger<BiasWordCountController> _logger;
    private readonly IBiasWordCountRepository _repository;

    public BiasWordCountController(ILogger<BiasWordCountController> logger, IBiasWordCountRepository repository)
    {
        _logger = logger;
        _repository = repository;
    }

    /// <summary>
    /// Adds the word count entries from the content body to the database and returns a sequential list of IDs for the inserted entries.
    /// </summary>
    /// <response code="200">Success: A list of IDs for the added entries (i.e., the last inserted ID is last in the list).</response>
    /// <response code="500">Internal Server Error: a <see cref="ProblemDetails"/> describing the error.</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<List<long>>> InsertDocuments([FromBody] List<BiasWordCountModel> entries)
    {
        try
        {
            IEnumerable<long> insertedIds = await _repository.AddBatch(entries);
            return Ok(insertedIds);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unable to add entry.");
            return Problem(e.Message);
        }
    }

    /// <summary>
    /// Retrieves a list of all word count entries from the database.
    /// </summary>
    /// <param name="limit">The maximum number of rows to get.</param>
    /// <param name="offset">The number of rows to skip (previous offset + previous limit).</param>
    /// <response code="200">Success: A list of all word count entries</response>
    /// <response code="204">No Content: Nothing is returned.</response>
    /// <response code="500">Internal Server Error: a <see cref="ProblemDetails"/> describing the error.</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<BiasWordCountModel>>> GetAll(int? limit = null, int? offset = null)
    {
        try
        {
            IEnumerable<BiasWordCountModel> result = await _repository.GetAll(limit, offset);
            return result.Any()
                ? Ok(result)
                : NoContent();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unable to get entries.");
            return Problem(e.Message);
        }

    }

    /// <summary>
    /// Deletes all existing word count entries from the database.
    /// </summary>
    /// <response code="200">Success: Nothing is returned.</response>
    /// <response code="204">No Content: Nothing is returned.</response>
    /// <response code="500">Internal Server Error: A <see cref="ProblemDetails"/> describing the error.</response>
    [HttpDelete]
    [Route("delete-all")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> DeleteAll()
    {
        try
        {
            return await _repository.DeleteAll() > 0
                ? Ok()
                : NoContent();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unable to delete all word count entries");
            return Problem(e.Message);
        }
    }
}