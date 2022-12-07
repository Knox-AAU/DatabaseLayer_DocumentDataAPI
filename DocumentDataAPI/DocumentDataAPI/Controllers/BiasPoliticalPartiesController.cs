using System.Data.Common;
using System.Net.Mime;
using DocumentDataAPI;
using DocumentDataAPI.Data.Repositories;
using DocumentDataAPI.Exceptions;
using DocumentDataAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace PoliticalPartiesDataAPI.Controllers;

[ApiController]
[Route(RoutePrefixHelper.Prefix + "/bias_PoliticalParties")]
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
    /// Deletes an existing document from the database matching the provided id.
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
    /// <response code="200">Success: The updated document.</response>
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
            _logger.LogError(e, "Unable to update document with id: {id}", partyModel.Id);
            return Problem(e.Message);
        }
    }

    /// <summary>
    /// Adds the documents from the content body to the database and returns a sequential list of IDs for the inserted documents.
    /// </summary>
    /// <response code="200">Success: A list of IDs for the added document (i.e., the last inserted ID is last in the list).</response>
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
            _logger.LogError(e, "Unable to add party.");
            return Problem(e.Message);
        }
    }
}