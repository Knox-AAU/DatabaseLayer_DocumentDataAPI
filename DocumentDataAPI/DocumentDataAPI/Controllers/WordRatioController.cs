using System.Data.Common;
using System.Net.Mime;
using DocumentDataAPI.Data.Repositories;
using DocumentDataAPI.Exceptions;
using DocumentDataAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace DocumentDataAPI.Controllers;

[ApiController]
[Route(RoutePrefixHelper.Prefix + "/word-ratios")]
public class WordRatioController : ControllerBase
{
    private readonly IWordRatioRepository _repository;

    public WordRatioController(IWordRatioRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
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
    [Route("documents/{documentId:int}/{word}")]
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
    [Route("documents/{documentId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<IEnumerable<WordRatioModel>> GetByDocumentId(int documentId)
    {
        try
        {
            IEnumerable<WordRatioModel> result = _repository.GetByDocumentId(documentId);
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
    [Route("words/{wordListString}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<IEnumerable<WordRatioModel>> GetByWord(string wordListString)
    {
        List<string> wordList = wordListString.Split(',').ToList();
        try
        {
            IEnumerable<WordRatioModel> result = _repository.GetByWords(wordList);
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
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<int> DeleteWordRatio([FromBody] WordRatioModel wordRatio)
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
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<int> UpdateWordRatio([FromBody] WordRatioModel wordRatio)
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
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<int> PutWordRatios([FromBody] List<WordRatioModel> wordRatios)
    {
        try
        {
            int result = _repository.AddBatch(wordRatios);
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
