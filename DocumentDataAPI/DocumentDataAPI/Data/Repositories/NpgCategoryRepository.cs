using System.Data;
using Dapper;
using DocumentDataAPI.Data.Mappers;
using DocumentDataAPI.Models;

namespace DocumentDataAPI.Data.Repositories;

public class NpgCategoryRepository : ICategoryRepository
{
    private readonly IDbConnectionFactory _connectionFactory;
    private readonly ILogger<NpgCategoryRepository> _logger;

    public NpgCategoryRepository(IDbConnectionFactory connectionFactory, ILogger<NpgCategoryRepository> logger)
    {
        _connectionFactory = connectionFactory;
        _logger = logger;
    }

    public async Task<IEnumerable<CategoryModel>> GetAll()
    {
        _logger.LogDebug("Retrieving all categories");
        using IDbConnection connection = _connectionFactory.CreateConnection();
        return await connection.QueryAsync<CategoryModel>("select * from categories");
    }

    public async Task<long> Add(CategoryModel entity)
    {
        _logger.LogDebug("Inserting category with name: {Name}", entity.Name);
        using IDbConnection connection = _connectionFactory.CreateConnection();
        return await connection.QuerySingleAsync<long>($"insert into categories({CategoryMap.Name}) values (@Name) returning {CategoryMap.Id}",
            new { entity.Name });
    }

    public async Task<int> Update(CategoryModel entity)
    {
        _logger.LogDebug("Updating name of category: ({Id}, {Name})", entity.Id, entity.Name);
        using IDbConnection connection = _connectionFactory.CreateConnection();
        return await connection.ExecuteAsync($"update categories set {CategoryMap.Name} = @Name where {CategoryMap.Id} = @Id",
            new { entity.Name, entity.Id });
    }

    public async Task<CategoryModel?> Get(int id)
    {
        _logger.LogDebug("Retrieving category with ID: {Id}", id);
        using IDbConnection connection = _connectionFactory.CreateConnection();
        return await connection.QuerySingleOrDefaultAsync<CategoryModel>($"select * from categories where {CategoryMap.Id} = @Id",
            new { id });
    }

    public async Task<int> Delete(int id)
    {
        _logger.LogDebug("Deleting category with ID: {Id}", id);
        using IDbConnection connection = _connectionFactory.CreateConnection();
        return await connection.ExecuteAsync($"delete from categories where {CategoryMap.Id} = @Id",
            new { id });
    }
}
