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
    [Route("GetById/{id:int?}")]
    public ActionResult<SourceModel> GetById(int id)
    {
        try
        {
            return Ok(_repository.Get(id));
        }
        catch (DbException e)
        {
            return Problem(e.Message);
        }
    }

    [HttpGet]
    [Route("GetDocumentCount/{id:int?}")]
    public ActionResult<int> GetDocumentCount(int id)
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
