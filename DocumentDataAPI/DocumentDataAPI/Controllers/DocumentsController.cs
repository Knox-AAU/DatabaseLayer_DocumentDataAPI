using DocumentDataAPI.Data.Repositories;
using DocumentDataAPI.Models;

using Microsoft.AspNetCore.Mvc;

using System.Text.Json;

namespace DocumentDataAPI.Controllers
{
    [ApiController]
    [Route("DocumentDataAPI/[controller]")]
    public class DocumentsController : ControllerBase
    {
        private readonly ILogger<DocumentsController> _logger;
        private readonly DocumentRepository _repository;

        public DocumentsController(ILogger<DocumentsController> logger, IConfiguration config)
        {
            _logger = logger;
            _repository = new DocumentRepository(config);
        }

        [HttpPost]
        [Route("PostDocument")]
        public IActionResult PostDocument(string document)
        {
            try
            {
                var model = JsonSerializer.Deserialize<DocumentModel>(document);

                _repository.Add(model!);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        [Route("Get")]
        public IActionResult GetAll()
        {
            try
            {
                List<DocumentModel> result = _repository.GetAll().ToList();
                return Ok(result);
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }
            
        }

        [HttpGet]
        [Route("Get/{id:int?}")]
        public IActionResult GetById(int id)
        {
            try
            {
                DocumentModel result = _repository.Get(id);
                return Ok(result);
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpGet]
        [Route("GetTotalDocumentCount")]
        public int GetTotalDocumentCount()
        {
            return _repository.GetTotalDocumentCount();
        }

        [HttpGet]
        [Route("GetBySourceId/{id:int?}")]
        public IActionResult GetBySourceId(int id)
        {
            try
            {
                List<DocumentModel> result = _repository.GetBySource(id).ToList();
                return Ok(result);
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpGet]
        [Route("GetByAuthor")]
        public IActionResult GetByAuthor(string author)
        {
            try
            {
                List<DocumentModel> result = _repository.GetByAuthor(author).ToList();
                return Ok(result);
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpGet]
        [Route("GetByDate")]
        public IActionResult GetByDate(string date)
        {
            try
            {
                bool isDate = DateTime.TryParse(date, out DateTime dateTime);
                if (!isDate)
                {
                    return BadRequest($"Wrong date format: {date}\nUse format: DD:MM:YYYY HH:MM:SS");
                }

                List<DocumentModel> result = _repository.GetByDate(dateTime).ToList();
                return Ok(result);
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }
        }
    }
}
