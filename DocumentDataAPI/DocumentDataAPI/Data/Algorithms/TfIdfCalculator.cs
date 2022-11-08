using DocumentDataAPI.Models;

namespace DocumentDataAPI.Data.Algorithms;

//Not in use
[Obsolete("Use NpgWordRelevanceRepository to update TF-IDF values in db", false)]
public class TfIdfCalculator
{
    private readonly int _docCount;
    private readonly IEnumerable<WordRatioModel> _wordRatios;

    public TfIdfCalculator(int docCount, IEnumerable<WordRatioModel> wordRatios)
    {
        _docCount = docCount;
        _wordRatios = wordRatios;
    }

    public double CalculateTfIdf(WordRatioModel wordRatio)
    {
        int termdocCount = _wordRatios.Where(x => x.Word == wordRatio.Word).Count();

        return wordRatio.Percent * IdfCalculator(termdocCount);
    }

    private double IdfCalculator(int termdocCount)
    {
        return Math.Log(_docCount / termdocCount);
    }
}
