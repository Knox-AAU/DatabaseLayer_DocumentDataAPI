using System.Data.Common;
using DocumentDataAPI.Data.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace DocumentDataAPI.Controllers;

[ApiController]
[Route(RoutePrefixHelper.Prefix + "/tf-idf/update")]
public class WordRelevanceController : ControllerBase
{
    private readonly ILogger<WordRelevanceController> _logger;
    private readonly IWordRelevanceRepository _repository;

    public WordRelevanceController(IWordRelevanceRepository repository, ILogger<WordRelevanceController> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    /// <summary>
    /// Updates the Tf-Idf values of all word ratios.
    /// </summary>
    /// <response code="200">Success: All word ratios have been updated.</response>
    /// <response code="204">No Content: No word ratios were updated.</response>
    /// <response code="500">Internal Server Error: A <see cref="ProblemDetails"/> describing the error.</response>
    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<int>> UpdateTfIdfs()
    {
        try
        {
            int result = await _repository.UpdateTfIdfs();
            return result > 0
                ? Ok(result)
                : NoContent();
        }
        catch (DbException e)
        {
            return Problem(e.Message);
        }
    }
}
