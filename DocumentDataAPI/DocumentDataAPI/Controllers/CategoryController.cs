using DocumentDataAPI.Data.Repositories;
using DocumentDataAPI.Models;

using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace DocumentDataAPI.Controllers;

[ApiController]
[Route(RoutePrefixHelper.Prefix + "/categories")]
[Produces(MediaTypeNames.Application.Json)]
public class CategoryController : ControllerBase
{
    private readonly ILogger<CategoryController> _logger;
    private readonly ICategoryRepository _repository;

    public CategoryController(ILogger<CategoryController> logger, ICategoryRepository repository)
    {
        _logger = logger;
        _repository = repository;
    }

    /// <summary>
    /// Retrieves a list of all categories in the database.
    /// </summary>
    /// <response code="200">Success: A list of categories.</response>
    /// <response code="204">No Content: Nothing is returned.</response>
    /// <response code="500">Internal Server Error: a <see cref="ProblemDetails"/> describing the error.</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<CategoryModel>>> GetAll()
    {
        try
        {
            IEnumerable<CategoryModel> result = await _repository.GetAll();
            return result.Any()
                ? Ok(result)
                : NoContent();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unable to get categories.");
            return Problem(e.Message);
        }
    }

    /// <summary>
    /// Retrieves the category with the given id.
    /// </summary>
    /// <response code="200">Success: The category.</response>
    /// <response code="204">No Content: Nothing is returned.</response>
    /// <response code="500">Internal Server Error: A <see cref="ProblemDetails"/> describing the error.</response>
    [HttpGet]
    [Route("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<CategoryModel>> GetById(int id)
    {
        try
        {
            CategoryModel? result = await _repository.Get(id);
            return result == null
                ? NoContent()
                : Ok(result);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unable to get category with id: {id}", id);
            return Problem(e.Message);
        }
    }

    /// <summary>
    /// Inserts a new category with the given name in the database.
    /// </summary>
    /// <response code="200">Success: The ID of the category that was inserted.</response>
    /// <response code="500">Internal Server Error: A <see cref="ProblemDetails"/> describing the error.</response>
    [HttpPost]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<long>> InsertCategory(string name)
    {
        try
        {
            long id = await _repository.Add(new CategoryModel { Name = name });
            return Ok(id);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unable to add category with name: {name})", name);
            return Problem(e.Message);
        }
    }

    /// <summary>
    /// Updates the category given in the request body in the database.
    /// </summary>
    /// <response code="200">Success: The category that was updated.</response>
    /// <response code="204">No Content: Nothing is returned.</response>
    /// <response code="500">Internal Server Error: A <see cref="ProblemDetails"/> describing the error.</response>
    [HttpPut]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<CategoryModel?>> UpdateCategory([FromBody] CategoryModel category)
    {
        try
        {
            return await _repository.Update(category) == 1
                ? Ok(await _repository.Get(category.Id))
                : NoContent();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unable to update category ({id}, {name})", category.Id, category.Name);
            return Problem(e.Message);
        }
    }

    /// <summary>
    /// Deletes an existing category from the database matching the provided id.
    /// </summary>
    /// <response code="200">Success: Nothing is returned.</response>
    /// <response code="204">No Content: Nothing is returned.</response>
    /// <response code="500">Internal Server Error: A <see cref="ProblemDetails"/> describing the error.</response>
    [HttpDelete]
    [Route("{categoryId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> DeleteCategory(int categoryId)
    {
        try
        {
            return await _repository.Delete(categoryId) == 1
                ? Ok()
                : NoContent();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unable to delete category with id: {categoryId}", categoryId);
            return Problem(e.Message);
        }
    }
}
