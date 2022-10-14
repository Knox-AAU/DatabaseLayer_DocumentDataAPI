using DocumentDataAPI.Data.Repositories;
using DocumentDataAPI.Models;

using Microsoft.AspNetCore.Mvc;

using System.Net.Mime;

namespace DocumentDataAPI.Controllers
{
    [ApiController]
    [Route("document")]
    [Produces(MediaTypeNames.Application.Json)]
    public class DocumentController : ControllerBase
    {
        private readonly ILogger<DocumentController> _logger;
        private readonly IDocumentRepository _repository;

        public DocumentController(ILogger<DocumentController> logger, IDocumentRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        /// <summary>
        /// Adds the document from the content body.
        /// </summary>
        /// <response code="200">Success: The document that was added to the database.</response>
        /// <response code="500">Internal Server Error: a <see cref="ProblemDetails"/> describing the error.</response>
        [HttpPut]
        [Route("PutDocument")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<DocumentModel> PutDocument([FromBody] DocumentModel document)
        {
            try
            {
                return _repository.Add(document).Result == 0?
                    Problem("No rows were added") :
                    Ok(_repository.Get(document.Id));
            }
            catch (Exception e)
            {
                _logger.LogError($"Unable to add document.\n{e.Message}");
                return Problem(e.Message);
            }
        }

        /// <summary>
        /// Retrieves a list of all documents from the database.
        /// </summary>
        /// <response code="200">Success: A list of all documents</response>
        /// <response code="404">Not Found: Nothing is returned.</response>
        /// <response code="500">Internal Server Error: a <see cref="ProblemDetails"/> describing the error.</response>
        [HttpGet]
        [Route("Get")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<IEnumerable<DocumentModel>> GetAll()
        {
            try
            {
                List<DocumentModel> result = _repository.GetAll().Result.ToList();
                return result.Count == 0 ?
                    NotFound() :
                    Ok(result);
            }
            catch (Exception e)
            {
                _logger.LogError($"Unable to fetch documents\n{e.Message}");
                return Problem(e.Message);
            }

        }

        /// <summary>
        /// Retrieves a document based on the document id.
        /// </summary>
        /// <response code="200">Success: A document for the given id.</response>
        /// <response code="404">Not Found: Nothing is returned.</response>
        /// <response code="500">Internal Server Error: a <see cref="ProblemDetails"/> describing the error.</response>
        [HttpGet]
        [Route("Get/{id:int?}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<DocumentModel> GetById(int id)
        {
            try
            {
                DocumentModel? result = _repository.Get(id).Result;
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

        /// <summary>
        /// Retrieves the number of documents in the database.
        /// </summary>
        /// <response code="200">Success: A number</response>
        /// <response code="404">Not Found: Nothing is returned.</response>
        /// <response code="500">Internal Server Error: a <see cref="ProblemDetails"/> describing the error.</response>
        [HttpGet]
        [Route("GetTotalDocumentCount")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<int> GetTotalDocumentCount()
        {
            try
            {
                int result = _repository.GetTotalDocumentCount().Result;
                return Ok(result);
            }
            catch (Exception e)
            {
                return Problem(e.Message);
            }
        }

        /// <summary>
        /// Retrieves a list of documents based on the source id.
        /// </summary>
        /// <response code="200">Success: A list of documents for the given source id.</response>
        /// <response code="404">Not Found: Nothing is returned.</response>
        /// <response code="500">Internal Server Error: a <see cref="ProblemDetails"/> describing the error.</response>
        [HttpGet]
        [Route("GetBySourceId/{id:int?}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<IEnumerable<DocumentModel>> GetBySourceId(int id)
        {
            try
            {
                List<DocumentModel> result = _repository.GetBySource(id).Result.ToList();
                return result.Count == 0 ?
                    NotFound() :
                    Ok(result);
            }
            catch (Exception e)
            {
                _logger.LogError($"Unable to fetch document by source id: {id}.\n{e.Message}");
                return Problem(e.Message);
            }
        }

        /// <summary>
        /// Retrieves a list of documents based on the author.
        /// </summary>
        /// <response code="200">Success: A list of documents by the given author.</response>
        /// <response code="404">Not Found: Nothing is returned.</response>
        /// <response code="500">Internal Server Error: a <see cref="ProblemDetails"/> describing the error.</response>
        [HttpGet]
        [Route("GetByAuthor")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<IEnumerable<DocumentModel>> GetByAuthor(string author)
        {
            try
            {
                List<DocumentModel> result = _repository.GetByAuthor(author).Result.ToList();
                return result.Count == 0 ?
                    NotFound() :
                    Ok(result);
            }
            catch (Exception e)
            {
                _logger.LogError($"Unable to fetch documents by author: {author}.\n{e.Message}");
                return Problem(e.Message);
            }
        }

        /// <summary>
        /// Retrieves a list of documents created at the specified date.
        /// </summary>
        /// <response code="200">Success: A list of documents by the given author.</response>
        /// <response code="404">Not Found: Nothing is returned.</response>
        /// <response code="500">Internal Server Error: a <see cref="ProblemDetails"/> describing the error.</response>
        [HttpGet]
        [Route("GetByDate")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<IEnumerable<DocumentModel>> GetByDate(DateTime date)
        {
            try
            {
                List<DocumentModel> result = _repository.GetByDate(date).Result.ToList();
                return result.Count == 0 ?
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
