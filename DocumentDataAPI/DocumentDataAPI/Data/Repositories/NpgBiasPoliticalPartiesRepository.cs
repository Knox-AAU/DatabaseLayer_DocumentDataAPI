using System.Data;
using Dapper;
using DocumentDataAPI.Data.Mappers;
using DocumentDataAPI.Data.Repositories.Helpers;
using DocumentDataAPI.Models;

namespace DocumentDataAPI.Data.Repositories;

public class NpgBiasPoliticalPartiesRepository : IBiasPoliticalPartiesRepository
{
    private readonly IDbConnectionFactory _connectionFactory;
    private readonly ILogger<NpgBiasDocumentRepository> _logger;
    private readonly ISqlHelper _sqlHelper;

    public NpgBiasPoliticalPartiesRepository(IDbConnectionFactory connectionFactory, ILogger<NpgBiasDocumentRepository> logger, ISqlHelper sqlHelper)
    {
        _connectionFactory = connectionFactory;
        _logger = logger;
        _sqlHelper = sqlHelper;
    }

    public async Task<int> Add(BiasPoliticalPartiesModel entity)
    {
        _logger.LogDebug("Adding Political Party with id {Id} to database", entity.Id);
        _logger.LogTrace("Political Party: {Party}", entity);
        using IDbConnection con = _connectionFactory.CreateConnection();
        return await con.ExecuteAsync(
            $"instert into political_parties ({BiasPoliticalPartiesMap.Id}, {BiasPoliticalPartiesMap.PartyName}, {BiasPoliticalPartiesMap.PartyBias}) " +
            "values (@Id, @PartyName, @PartyBias",
                new
                {
                    entity.Id,
                    entity.PartyName,
                    entity.PartyBias
                });
    }

    public async Task<int> Delete(int Id)
    {
        _logger.LogDebug("Deleting PoliticalParty with id {Id} from database", Id);
        using IDbConnection con = _connectionFactory.CreateConnection();
        return await con.ExecuteAsync(
            $"delete from political_parties where {BiasPoliticalPartiesMap.Id} = @Id",
            new { Id });
    }

    public async Task<IEnumerable<BiasPoliticalPartiesModel>> GetAll(int? limit = null, int? offset = null)
    {
        _logger.LogDebug("Retrieving all PoliticalParties from database");
        string sql = _sqlHelper.GetPaginatedQuery("select * from political_parties", limit, offset,
            BiasPoliticalPartiesMap.Id);
        using IDbConnection con = _connectionFactory.CreateConnection();
        return await con.QueryAsync<BiasPoliticalPartiesModel>(sql);
    }

    public async Task<int> Update(BiasPoliticalPartiesModel entity)
    {
        _logger.LogDebug("Updating Political Party with id {Id} in database", entity.Id);
        _logger.LogTrace("PoliticalParty: {PoliticalParty}", entity);
        using IDbConnection con = _connectionFactory.CreateConnection();
        return await con.ExecuteAsync(
            $"update political_parties set {BiasPoliticalPartiesMap.PartyBias} = @PartyBias " +
            $"where {BiasPoliticalPartiesMap.Id} = @Id",
            new
            {
                entity.Id,
                entity.PartyName,
                entity.PartyBias
            });
    }
}