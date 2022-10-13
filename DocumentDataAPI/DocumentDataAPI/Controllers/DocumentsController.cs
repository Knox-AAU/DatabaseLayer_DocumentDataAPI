using DocumentDataAPI.Data.Repositories;
using DocumentDataAPI.Models;

using Microsoft.AspNetCore.Mvc;

using System.Text.Json;

namespace DocumentDataAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DocumentsController : ControllerBase
    {
        private readonly ILogger<DocumentsController> _logger;
        private readonly IDocumentRepository _repository;

        public DocumentsController(ILogger<DocumentsController> logger, IConfiguration config, IDocumentRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        /*TODO
         * Use ActionResult instead of IActionResult
         * Documentation
         * Unit tests
        */

        [HttpPut]
        [Route("PutDocument")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult PutDocument(string document)
        {
            try
            {
                var model = JsonSerializer.Deserialize<DocumentModel>(document);

                _repository.Add(model!);
                return model == null?
                    NotFound() :
                    Ok(model);
            }
            catch (Exception e)
            {
                _logger.LogError($"Unable to add document.\n{e.Message}");
                return Problem(e.Message);
            }
        }

        [HttpGet]
        [Route("Get")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<IEnumerable<DocumentModel>> GetAll()
        {
            try
            {
                List<DocumentModel> result = _repository.GetAll().ToList();
                return result == null ?
                    NotFound() :
                    Ok(result);
            }
            catch (Exception e)
            {
                _logger.LogError($"Unable to fetch documents\n{e.Message}");
                return Problem(e.Message);
            }
            
        }

        [HttpGet]
        [Route("Get/{id:int?}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<DocumentModel> GetById(int id)
        {
            try
            {
                DocumentModel? result = _repository.Get(id);
                return result == null ?
                    NotFound() :
                    Ok(result);
            }
            catch (Exception e)
            {
                _logger.LogError($"Unable to fetch document with id: {id}.\n{e.Message}");
                return Problem(e.Message);
            }
        }

        [HttpGet]
        [Route("GetTotalDocumentCount")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<int> GetTotalDocumentCount()
        {
            try
            {
                int result = _repository.GetTotalDocumentCount();
                return Ok(result);
            }
            catch (Exception e)
            {
                return Problem(e.Message);
            }
        }

        [HttpGet]
        [Route("GetBySourceId/{id:int?}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<IEnumerable<DocumentModel>> GetBySourceId(int id)
        {
            try
            {
                List<DocumentModel> result = _repository.GetBySource(id).ToList();
                return result == null ?
                    NotFound() :
                    Ok(result);
            }
            catch (Exception e)
            {
                _logger.LogError($"Unable to fetch document by source id: {id}.\n{e.Message}");
                return Problem(e.Message);
            }
        }

        [HttpGet]
        [Route("GetByAuthor")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<IEnumerable<DocumentModel>> GetByAuthor(string author)
        {
            try
            {
                List<DocumentModel> result = _repository.GetByAuthor(author).ToList();
                return result == null ?
                    NotFound() :
                    Ok(result);
            }
            catch (Exception e)
            {
                _logger.LogError($"Unable to fetch documents by author: {author}.\n{e.Message}");
                return Problem(e.Message);
            }
        }

        [HttpGet]
        [Route("GetByDate")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<IEnumerable<DocumentModel>> GetByDate(string date)
        {
            try
            {
                bool isDate = DateTime.TryParse(date, out DateTime dateTime);
                if (!isDate)
                {
                    return BadRequest($"Wrong date format: {date}\nUse format: DD:MM:YYYY HH:MM:SS");
                }

                List<DocumentModel> result = _repository.GetByDate(dateTime).ToList();
                return result == null ?
                    NotFound() :
                    Ok(result);
            }
            catch (Exception e)
            {
                _logger.LogError($"Unable to fetch documents by date {date}.\n{e.Message}");
                return Problem(e.Message);
            }
        }
    }
}
