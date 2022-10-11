using System.Data.Common;
using DocumentDataAPI.Models;
using Microsoft.AspNetCore.Mvc;
using DocumentDataAPI.Data.Repositories;
using Npgsql;

namespace DocumentDataAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class SourceController : ControllerBase
{
    private readonly SourceRepository _sourceRepo;

    public SourceController(IConfiguration configuration)
    {
        _sourceRepo = new SourceRepository(configuration);
    }

    [HttpGet(Name = "SourceGetAll")]
    [Route("GetAll")]
    public ActionResult<IEnumerable<SourceModel>> GetAll()
    {
        try
        {
            return Ok(_sourceRepo.GetAll());
        }
        catch (DbException e)
        {
            return Problem(e.Message);
        }
    }

    [HttpGet(Name = "SourceGetById")]
    [Route("GetById/{id:int?}")]
    public ActionResult<SourceModel> GetById(int id)
    {
        try
        {
            return Ok(_sourceRepo.Get(id));
        }
        catch (DbException e)
        {
            return Problem(e.Message);
        }
    }

    [HttpGet(Name = "SourceGetDocumentCount")]
    [Route("GetDocumentCount/{id:int?}")]
    public ActionResult<int> GetDocumentCount(int id)
    {
        try
        {
            return Ok(_sourceRepo.GetCountFromId(id));
        }
        catch (DbException e)
        {
            return Problem(e.Message);
        }
    }

    [HttpPost(Name = "SourcePostSource")]
    [Route("PostSource/{name}")]
    public ActionResult PostSource(string name)
    {
        try
        {
            int status = _sourceRepo.Add(new SourceModel() { Name = name });
            if (status == 0)
            {
                return BadRequest($"Rows affected: {status}");
            }

            return Ok($"Rows affected: {status}");
        }
        catch (DbException e)
        {
            return Problem(e.Message);
        }
    }
}
