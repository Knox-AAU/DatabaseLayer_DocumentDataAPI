using System.Linq;
using DocumentDataAPI.Models;

namespace DocumentDataAPI.Data.Algorithms;

public class TfIdfCalculator
{

    private readonly int _docCount;
    private readonly IEnumerable<WordRatioModel> _wordRatios;

    public TfIdfCalculator(int docCount, IEnumerable<WordRatioModel> wordRatios)
    {
        _docCount = docCount;
        _wordRatios = wordRatios;
    }

    public void CalculateTfIdf()
    {
        double wordRatioTfIdf = 0;
        foreach (WordRatioModel wordRatio in _wordRatios)
        {
            wordRatioTfIdf = CalculateSingleTfIdf(wordRatio);

            //Insert into db

        }

    }

    private double CalculateSingleTfIdf(WordRatioModel wordRatio)
    {
        int termdocCount = _wordRatios.Where(x => x.Word == wordRatio.Word).Count();

        return wordRatio.Percent * IdfCalculator(termdocCount);
    }

    private double IdfCalculator(int termdocCount)
    {
        return Math.Log(_docCount / termdocCount, 10);
    }

}