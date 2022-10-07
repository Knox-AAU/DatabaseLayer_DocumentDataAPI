using DocumentDataAPI.Models;
using Microsoft.AspNetCore.Mvc;
using DocumentDataAPI.Options;
using DocumentDataAPI.Data.Repositories;

namespace DocumentDataAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class SourceController
{
    private readonly DatabaseOptions _databaseOptions;
    private readonly SourceRepository _sourceRepo;

    public SourceController(IConfiguration configuration)
    {
        _databaseOptions = configuration.GetSection(DatabaseOptions.Key).Get<DatabaseOptions>();
        _sourceRepo = new SourceRepository(configuration);
    }

    [HttpGet(Name = "SourceGetAll")]
    [Route("GetAll")]
    public IEnumerable<SourceModel> GetAll()
    {
        return _sourceRepo.GetAll();
    }

    [HttpGet(Name = "SourceGetById")]
    [Route("GetById/{id:int?}")]
    public SourceModel GetById(int id)
    {
        return _sourceRepo.Get(id);
    }

    [HttpGet(Name = "SourceGetDocumentCount")]
    [Route("GetDocumentCount/{id:int?}")]
    public int GetDocumentCount(int id)
    {
        return _sourceRepo.GetCountFromId(id);
    }

    [HttpPost(Name = "SourcePostSource")]
    [Route("PostSource/{name:string?}")]
    public void PostSource(string name)
    {
        _sourceRepo.Add(new SourceModel() { Name = name });
    }
}
