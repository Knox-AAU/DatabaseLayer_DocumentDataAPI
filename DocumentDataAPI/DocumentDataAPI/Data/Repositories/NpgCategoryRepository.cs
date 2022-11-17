using System.Data;
using Dapper;
using DocumentDataAPI.Data.Mappers;
using DocumentDataAPI.Data.Repositories.Helpers;
using DocumentDataAPI.Models;

namespace DocumentDataAPI.Data.Repositories;

public class NpgCategoryRepository : ICategoryRepository
{
    private readonly IDbConnectionFactory _connectionFactory;
    private readonly ILogger<NpgCategoryRepository> _logger;
    private readonly ISqlHelper _sqlHelper;

    public NpgCategoryRepository(IDbConnectionFactory connectionFactory, ILogger<NpgCategoryRepository> logger, ISqlHelper sqlHelper)
    {
        _connectionFactory = connectionFactory;
        _logger = logger;
        _sqlHelper = sqlHelper;
    }

    public async Task<IEnumerable<CategoryModel>> GetAll(int? limit = null, int? offset = null)
    {
        _logger.LogDebug("Retrieving all categories");
        string sql = _sqlHelper.GetPaginatedQuery("select * from categories", limit, offset, CategoryMap.Id);
        using IDbConnection connection = _connectionFactory.CreateConnection();
        return await connection.QueryAsync<CategoryModel>(sql);
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
