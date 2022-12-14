using System.Data;
using System.Net.NetworkInformation;
using DocumentDataAPI.Data;
using Microsoft.AspNetCore.Mvc;

namespace DocumentDataAPI.Controllers;

[ApiController]
[Route(RoutePrefixHelper.Prefix + "/status")]
public class StatusController : ControllerBase
{
    private readonly IDbConnectionFactory _connectionFactory;
    private readonly ILogger<StatusController> _logger;

    public StatusController(IDbConnectionFactory connectionFactory, ILogger<StatusController> logger)
    {
        _connectionFactory = connectionFactory;
        _logger = logger;
    }

    /// <summary>
    /// Tests the connection to the database.
    /// </summary>
    /// <response code="200">Success: The database is reachable.</response>
    /// <response code="500">Internal Server Error: A <see cref="ProblemDetails"/> describing the error.</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult Status()
    {
        IDbConnection con = _connectionFactory.CreateConnection();
        try
        {
            con.Open();
            if (con.State is not ConnectionState.Open)
            {
                return Problem($"Problems connecting to database, current state: {con.State}", statusCode: 500);
            }

            return Ok();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Connection to the database could not be established");
            return Problem("Connection to the database could not be established: " + e.Message);
        }
        finally
        {
            con.Close();
        }
    }
}
