using System.Data;
using System.Text;
using Dapper;
using Dapper.Transaction;
using DocumentDataAPI.Data.Mappers;
using DocumentDataAPI.Data.Repositories.Helpers;
using DocumentDataAPI.Models;

namespace DocumentDataAPI.Data.Repositories;

public class NpgBiasPoliticalPartiesRepository : IBiasPoliticalPartiesRepository
{
    private readonly IDbConnectionFactory _connectionFactory;
    private readonly ILogger<NpgBiasPoliticalPartiesRepository> _logger;
    private readonly ISqlHelper _sqlHelper;

    public NpgBiasPoliticalPartiesRepository(IDbConnectionFactory connectionFactory, ILogger<NpgBiasPoliticalPartiesRepository> logger, ISqlHelper sqlHelper)
    {
        _connectionFactory = connectionFactory;
        _logger = logger;
        _sqlHelper = sqlHelper;
    }

    public async Task<DocumentModel?> Get(int partyId)
    {
        _logger.LogDebug("Retrieving Document with id {id} from database", partyId);
        using IDbConnection con = _connectionFactory.CreateConnection();
        return await con.QueryFirstOrDefaultAsync<DocumentModel>($"select * from documents where {BiasPoliticalPartiesMap.Id} = @Id", new { id = partyId });
    }

    public async Task<IEnumerable<long>> AddBatch(List<BiasPoliticalPartiesModel> models)
    {
        IEnumerable<long> allInsertedIds = new List<long>();
        _logger.LogDebug("Adding {count} documents to database", models.Count);
        using IDbConnection con = _connectionFactory.CreateConnection();
        con.Open();
        using IDbTransaction transaction = con.BeginTransaction();
        try
        {
            // Divide the list of models into chunks to keep the INSERT statements from getting too large.
            foreach (BiasPoliticalPartiesModel[] chunk in models.Chunk(_sqlHelper.InsertStatementChunkSize))
            {
                string parameterString = _sqlHelper.GetBatchInsertParameters(chunk, out Dictionary<string, dynamic> parameters);
                IEnumerable<long> insertedIds = await transaction.QueryAsync<long>(
                    $"insert into documents ({BiasPoliticalPartiesMap.PartyName}, {BiasPoliticalPartiesMap.PartyBias}) " +
                    $"values {parameterString} returning {BiasPoliticalPartiesMap.Id}",
                    parameters);
                allInsertedIds = allInsertedIds.Concat(insertedIds);
            }

            transaction.Commit();
        }
        catch (Exception e)
        {
            transaction.Rollback();
            _logger.LogError(e, "Failed to insert documents");
            throw;
        }
        return allInsertedIds;
    }

    public async Task<int> Add(BiasPoliticalPartiesModel entity)
    {
        _logger.LogDebug("Adding Political Party with id {Id} to database", entity.Id);
        _logger.LogTrace("Political Party: {Party}", entity);
        using IDbConnection con = _connectionFactory.CreateConnection();
        return await con.ExecuteAsync(
            $"instert into political_parties ({BiasPoliticalPartiesMap.PartyName}, {BiasPoliticalPartiesMap.PartyBias}) " +
            $"values (@PartyName, @PartyBias) returning {BiasPoliticalPartiesMap.Id}",
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