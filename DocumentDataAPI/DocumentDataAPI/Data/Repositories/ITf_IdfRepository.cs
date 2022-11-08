namespace DocumentDataAPI.Data.Repositories
{
    public interface ITfIdfRepository
    {
        Task<int> UpdateTfIdfs();
    }
}