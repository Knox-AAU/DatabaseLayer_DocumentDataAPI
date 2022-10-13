using System.Data.Common;
using DocumentDataAPI.Data.Repositories;
using DocumentDataAPI.Exceptions;
using DocumentDataAPI.Models;
using Microsoft.AspNetCore.Mvc;

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
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<IEnumerable<WordRatioModel>> GetAll()
    {
        try
        {
            IEnumerable<WordRatioModel> result = _repository.GetAll();
            return result.Any()
                ? Ok(result)
                : NoContent();
        }
        catch (DbException e)
        {
            return Problem(e.Message);
        }
    }


    [HttpGet]
    [Route("GetByDocumentIDAndWord/{id:int}/{word}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<WordRatioModel> GetByDocumentIdAndWord(int documentId, string word)
    {
        try
        {
            WordRatioModel? result = _repository.GetByDocumentIdAndWord(documentId, word);
            return result != null
                ? Ok(result)
                : NoContent();
        }
        catch (DbException e)
        {
            return Problem(e.Message);
        }
    }

    [HttpGet]
    [Route("GetByWord/{word}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<IEnumerable<WordRatioModel>> GetByWord(string word)
    {
        try
        {
            IEnumerable<WordRatioModel> result = _repository.GetByWord(word);
            return result.Any()
                ? Ok(result)
                : NoContent();
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
        try
        {
            IEnumerable<WordRatioModel> result = _repository.GetByWords(wordlist);
            return result.Any()
                ? Ok(result)
                : NoContent();
        }
        catch (DbException e)
        {
            return Problem(e.Message);
        }
    }

    [HttpDelete]
    [Route("DeleteWordRatio/{wordRatio}")]
    public ActionResult<int> DeleteWordRatio(WordRatioModel wordRatio)
    {
        try
        {
            int result = _repository.Delete(wordRatio);
            return result == 1
                ? Ok(result)
                : NoContent();
        }
        catch (DbException e)
        {
            return Problem(e.Message);
        }
    }


    [HttpPost]
    [Route("UpdateWordRatio/{wordratio}")]
    public ActionResult<int> UpdateWordRatio(WordRatioModel wordRatio)
    {
        try
        {
            int result = _repository.Update(wordRatio);
            return result == 1
                ? Ok(result)
                : NoContent();
        }
        catch (DbException e)
        {
            return Problem(e.Message);
        }
    }
    
    
    [HttpPut]
    [Route("PutWordRatio/{wordratio}")]
    public ActionResult<int> PutWordRatio(WordRatioModel wordRatio)
    {
        try
        {
            int result = _repository.Add(wordRatio);
            return result == 1
                ? Ok(result)
                : NoContent();
        }
        catch (DbException e)
        {
            return Problem(e.Message);
        }
    }

    [HttpPut]
    [Route("PutWordRatios/{wordRatios}")]
    public ActionResult<int> PutWordRatios(IEnumerable<WordRatioModel> wordRatios)
    {
        try
        {
            int result = _repository.AddWordRatios(wordRatios);
            return result > 0
                ? Ok(result)
                : NoContent();
        }
        catch (RowsAffectedMismatchException e)
        {
            return Problem(e.Message);
        }
        catch (DbException e)
        {
            return Problem(e.Message);
        }
    }
}