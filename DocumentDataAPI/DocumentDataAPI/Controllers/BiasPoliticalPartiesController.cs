using System.Data.Common;
using System.Net.Mime;
using DocumentDataAPI;
using DocumentDataAPI.Data.Repositories;
using DocumentDataAPI.Exceptions;
using DocumentDataAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace PoliticalPartiesDataAPI.Controllers;

[ApiController]
[Route(RoutePrefixHelper.Prefix + "/bias_political_parties")]
[Produces(MediaTypeNames.Application.Json)]
public class BiasPoliticalPartiesController : ControllerBase
{
    private readonly ILogger<BiasPoliticalPartiesController> _logger;
    private readonly IBiasPoliticalPartiesRepository _repository;

    public BiasPoliticalPartiesController(ILogger<BiasPoliticalPartiesController> logger, IBiasPoliticalPartiesRepository repository)
    {
        _logger = logger;
        _repository = repository;
    }

    /// <summary>
    /// Retrieves a list of all political parties from the database.
    /// </summary>
    /// <param name="limit">The maximum number of rows to get.</param>
    /// <param name="offset">The number of rows to skip (previous offset + previous limit).</param>
    /// <response code="200">Success: A list of all political parties</response>
    /// <response code="204">No Content: Nothing is returned.</response>
    /// <response code="500">Internal Server Error: a <see cref="ProblemDetails"/> describing the error.</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<BiasPoliticalPartiesModel>>> GetAll(int? limit = 100, int? offset = null)
    {
        try
        {
            IEnumerable<BiasPoliticalPartiesModel> result = await _repository.GetAll(limit, offset);
            return result.Any()
                ? Ok(result)
                : NoContent();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unable to get political parties");
            return Problem(e.Message);
        }
    }

    /// <summary>
    /// Deletes an existing political party from the database matching the provided id.
    /// </summary>
    /// <response code="200">Success: Nothing is returned.</response>
    /// <response code="204">No Content: Nothing is returned.</response>
    /// <response code="500">Internal Server Error: A <see cref="ProblemDetails"/> describing the error.</response>
    [HttpDelete]
    [Route("{partyId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> DeleteDocument(int partyId)
    {
        try
        {
            return await _repository.Delete(partyId) == 1
                ? Ok()
                : NoContent();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unable to delete party with id: {partyId}", partyId);
            return Problem(e.Message);
        }
    }

    /// <summary>
    /// Persists the changes to the given <paramref name="partyModel"/> in the database.
    /// </summary>
    /// <response code="200">Success: The updated political party.</response>
    /// <response code="204">No Content: Nothing is returned.</response>
    /// <response code="500">Internal Server Error: a <see cref="ProblemDetails"/> describing the error.</response>
    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<BiasPoliticalPartiesModel>> UpdateDocument([FromBody] BiasPoliticalPartiesModel partyModel)
    {
        try
        {
            return await _repository.Update(partyModel) == 1
                ? Ok(_repository.Get(partyModel.Id))
                : NoContent();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unable to update political party with id: {partyId}", partyModel.Id);
            return Problem(e.Message);
        }
    }

    /// <summary>
    /// Adds the political parties from the content body to the database and returns a sequential list of IDs for the inserted parties.
    /// </summary>
    /// <response code="200">Success: A list of IDs for the added parties (i.e., the last inserted ID is last in the list).</response>
    /// <response code="500">Internal Server Error: a <see cref="ProblemDetails"/> describing the error.</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<List<long>>> InsertDocuments([FromBody] List<BiasPoliticalPartiesModel> parties)
    {
        try
        {
            IEnumerable<long> insertedIds = await _repository.AddBatch(parties);
            return Ok(insertedIds);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unable to add political party.");
            return Problem(e.Message);
        }
    }
}