using DocumentDataAPI.Models;

namespace DocumentDataAPI.Data.Algorithms;

public interface IRelevanceFunction
{
    public double CalculateRelevance(IEnumerable<WordRatioModel> docWordRatios, IEnumerable<string> query);
}