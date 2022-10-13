using System.Data.Common;
using DocumentDataAPI.Models;
using Microsoft.AspNetCore.Mvc;
using DocumentDataAPI.Data.Repositories;

namespace DocumentDataAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class SourceController : ControllerBase
{
    private readonly ISourceRepository _repository;

    public SourceController(ISourceRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    [Route("GetAll/")]
    public ActionResult<IEnumerable<SourceModel>> GetAll()
    {
        try
        {
            return Ok(_repository.GetAll());
        }
        catch (DbException e)
        {
            return Problem(e.Message);
        }
    }

    [HttpGet]
    [Route("GetById/{id:long}")]
    public ActionResult<SourceModel> GetById(long id)
    {
        try
        {
            SourceModel? result = _repository.Get(id);
            return result == null
                ? NotFound()
                : Ok(result);
        }
        catch (DbException e)
        {
            return Problem(e.Message);
        }
    }

    [HttpGet]
    [Route("GetDocumentCount/{id:long}")]
    public ActionResult<int> GetDocumentCount(long id)
    {
        try
        {
            return Ok(_repository.GetCountFromId(id));
        }
        catch (DbException e)
        {
            return Problem(e.Message);
        }
    }

    [HttpPost]
    [Route("PostSource/{name}")]
    public ActionResult PostSource(string name)
    {
        try
        {
            int status = _repository.Add(new SourceModel() { Name = name });
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
