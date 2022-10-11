using DocumentDataAPI.Data.Repositories;
using DocumentDataAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.Common;

namespace DocumentDataAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class WordRatioController : ControllerBase
{
    private readonly IWordRatioRepository _repository;

    public WordRatioController(IWordRatioRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    [Route("GetAll/")]
    public ActionResult<IEnumerable<WordRatioModel>> GetAll()
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
    [Route("GetByDocumentIDAndWord/{id:int}/{word}")]
    public ActionResult<WordRatioModel> GetByDocumentIDAndWord(int documentid, string word)
    {
        throw new NotImplementedException();
    }

    [HttpGet]
    [Route("GetByWord/{word}")]
    public ActionResult<IEnumerable<WordRatioModel>> GetByWord(string word)
    {
        try
        {
            return Ok(_repository.GetByWord(word));
        }
        catch (DbException e)
        {
            return Problem(e.Message);
        }
    }

    [HttpGet]
    [Route("GetByWords/{wordlist}")]
    public ActionResult<int> GetByWords(IEnumerable<string> wordlist)
    {
        //TODO: Split word list by semicolon
        throw new NotImplementedException();
    }

    [HttpGet]
    [Route("PostWordRatios/{wordratios}")]
    public ActionResult<int> PostWordRatios(IEnumerable<WordRatioModel> wordratios)
    {
        //TODO: Split word ratios by semicolon
        throw new NotImplementedException();
    }

}
