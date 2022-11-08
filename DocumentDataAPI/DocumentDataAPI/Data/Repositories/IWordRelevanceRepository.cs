namespace DocumentDataAPI.Data.Repositories
{
    public interface IWordRelevanceRepository
    {
        Task<int> UpdateWordRelevances();
    }
}
