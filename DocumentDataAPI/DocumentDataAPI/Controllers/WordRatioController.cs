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
    private readonly ILogger<WordRatioController> _logger;

    public WordRatioController(IWordRatioRepository repository, ILogger<WordRatioController> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    /// <summary>
    /// Retrieves all word ratios.
    /// </summary>
    /// <response code="200">Success: A list of word ratios.</response>
    /// <response code="204">No Content: Nothing is returned.</response>
    /// <response code="500">Internal Server Error: A <see cref="ProblemDetails"/> describing the error.</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<WordRatioModel>>> GetAll()
    {
        try
        {
            IEnumerable<WordRatioModel> result = await _repository.GetAll();
            return result.Any()
                ? Ok(result)
                : NoContent();
        }
        catch (DbException e)
        {
            return Problem(e.Message);
        }
    }

    /// <summary>
    /// Retrieves a word ratio with a specific word and document id.
    /// </summary>
    /// <response code="200">Success: The word ratio.</response>
    /// <response code="204">No Content: Nothing is returned.</response>
    /// <response code="500">Internal Server Error: A <see cref="ProblemDetails"/> describing the error.</response>
    [HttpGet]
    [Route("documents/{documentId:int}/{word}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<WordRatioModel>> GetByDocumentIdAndWord(int documentId, string word)
    {
        try
        {
            WordRatioModel? result = await _repository.Get(documentId, word);
            return result != null
                ? Ok(result)
                : NoContent();
        }
        catch (DbException e)
        {
            return Problem(e.Message);
        }
    }

    /// <summary>
    /// Retrieves all word ratios for a specific document.
    /// </summary>
    /// <response code="200">Success: All word ratios for the specified document.</response>
    /// <response code="204">No Content: Nothing is returned.</response>
    /// <response code="500">Internal Server Error: A <see cref="ProblemDetails"/> describing the error.</response>
    [HttpGet]
    [Route("documents/{documentId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<WordRatioModel>>> GetByDocumentId(int documentId)
    {
        try
        {
            IEnumerable<WordRatioModel> result = await _repository.GetByDocumentId(documentId);
            return result.Any()
                ? Ok(result)
                : NoContent();
        }
        catch (DbException e)
        {
            return Problem(e.Message);
        }
    }

    /// <summary>
    /// Retrieves all word ratios that contain a word in the given <paramref name="wordListString"/>, which is a comma-separated string of words.
    /// </summary>
    /// <response code="200">Success: All word ratios with the specified word.</response>
    /// <response code="204">No Content: Nothing is returned.</response>
    /// <response code="500">Internal Server Error: A <see cref="ProblemDetails"/> describing the error.</response>
    [HttpGet]
    [Route("words/{wordListString}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<WordRatioModel>>> GetByWord(string wordListString)
    {
        List<string> wordList = wordListString.Split(',').ToList();
        try
        {
            IEnumerable<WordRatioModel> result = await _repository.GetByWords(wordList);
            return result.Any()
                ? Ok(result)
                : NoContent();
        }
        catch (DbException e)
        {
            return Problem(e.Message);
        }
    }

    /// <summary>
    /// Deletes the word ratio that is given in the request body.
    /// </summary>
    /// <response code="200">Success: The deleted word ratio.</response>
    /// <response code="204">No Content: Nothing is returned.</response>
    /// <response code="500">Internal Server Error: A <see cref="ProblemDetails"/> describing the error.</response>
    [HttpDelete]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<int>> DeleteWordRatio([FromBody] WordRatioModel wordRatio)
    {
        try
        {
            int result = await _repository.Delete(wordRatio);
            return result == 1
                ? Ok(result)
                : NoContent();
        }
        catch (DbException e)
        {
            return Problem(e.Message);
        }
    }

    /// <summary>
    /// Updates the values of the word ratio that is given in the request body.
    /// </summary>
    /// <response code="200">Success: The updated word ratio.</response>
    /// <response code="204">No Content: Nothing is returned.</response>
    /// <response code="500">Internal Server Error: A <see cref="ProblemDetails"/> describing the error.</response>
    [HttpPut]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<int>> UpdateWordRatio([FromBody] WordRatioModel wordRatio)
    {
        try
        {
            int result = await _repository.Update(wordRatio);
            return result == 1
                ? Ok(result)
                : NoContent();
        }
        catch (DbException e)
        {
            return Problem(e.Message);
        }
    }

    /// <summary>
    /// Inserts the given list of word ratios in the database.
    /// </summary>
    /// <response code="200">Success: The number of rows inserted.</response>
    /// <response code="204">No Content: Nothing is returned.</response>
    /// <response code="500">Internal Server Error: A <see cref="ProblemDetails"/> describing the error.</response>
    [HttpPost]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<int>> InsertWordRatios([FromBody] List<WordRatioModel> wordRatios)
    {
        try
        {
            int result = await _repository.AddBatch(wordRatios);
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

    /// <summary>
    /// Deletes an existing word ratio from the database matching the provided document id and word.
    /// </summary>
    /// <response code="200">Success: Nothing is returned.</response>
    /// <response code="204">No Content: Nothing is returned.</response>
    /// <response code="500">Internal Server Error: A <see cref="ProblemDetails"/> describing the error.</response>
    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> DeleteWordRatio(long documentId, string word)
    {
        try
        {
            return await _repository.Delete(documentId, word) == 1
                ? Ok()
                : NoContent();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unable to delete word ratio with document id: {documentId} and word: {word}", documentId, word);
            return Problem(e.Message);
        }
    }
}
