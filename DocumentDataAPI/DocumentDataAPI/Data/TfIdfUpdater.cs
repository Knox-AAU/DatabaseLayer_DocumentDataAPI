using DocumentDataAPI.Data.Algorithms;
using DocumentDataAPI.Models;

namespace DocumentDataAPI.Data;

//Not in use
[Obsolete("Use NpgWordRelevanceRepository to update TF-IDF values in db", true)]
public class TfIdfUpdater
{
    private readonly TfIdfCalculator _calc;

    private readonly int _docCount;
    private readonly IEnumerable<WordRatioModel> _wordRatios;

    public TfIdfUpdater(int docCount, IEnumerable<WordRatioModel> wordRatios)
    {
        _docCount = docCount;
        _wordRatios = wordRatios;
        _calc = new TfIdfCalculator(docCount, wordRatios);
    }

    public void UpdateTfIdfValues()
    {
        double wordRatioTfIdf = 0;
        foreach (WordRatioModel wordRatio in _wordRatios)
        {
            wordRatioTfIdf = _calc.CalculateTfIdf(wordRatio);

            //Insert into db
        }
    }
}
