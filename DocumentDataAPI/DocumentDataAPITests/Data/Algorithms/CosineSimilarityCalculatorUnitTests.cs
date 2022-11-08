using DocumentDataAPI.Data.Algorithms;
using DocumentDataAPI.Models;

namespace DocumentDataAPITests.Data.Algorithms;

[Collection("DocumentDataApiUnitTests")]
public class CosineSimilarityCalculatorUnitTests
{
    [Theory]
    [MemberData(nameof(TestData))]
    public void CalculateCosineSimilarityReturnsCorrectCosineSimilarity(IEnumerable<WordRatioModel> docWordRatios, List<string> query, double expected)
    {
        //Arrange and Act
        double result = new CosineSimilarityCalculator().CalculateRelevance(docWordRatios, query);
        
        //Assert
        result.Should().BeApproximately(expected, 0.001, "because the values may vary by a very tiny amount");
    }
    
    public static IEnumerable<object[]> TestData =>
        new List<object[]>
        {
            new object[] { 
                new List<WordRatioModel>() {
                    new WordRatioModel() {Word = "test1", TfIdf = 0.5F},
                    new WordRatioModel() {Word = "test2", TfIdf = 0.8F},
                    new WordRatioModel() {Word = "test3", TfIdf = 0.2F}},
                new List<string>()
                {
                    "test1"
                }, 
                0.5F/Math.Sqrt(Math.Pow(0.5F,2)+Math.Pow(0.8F,2)+Math.Pow(0.2F,2))},
            new object[] { 
                new List<WordRatioModel>() {
                    new WordRatioModel() {Word = "test1", TfIdf = 0.5F},
                    new WordRatioModel() {Word = "test2", TfIdf = 0.8F},
                    new WordRatioModel() {Word = "test3", TfIdf = 0.2F}},
                new List<string>()
                {
                    "test2"
                }, 
                0.8F/Math.Sqrt(Math.Pow(0.5F,2)+Math.Pow(0.8F,2)+Math.Pow(0.2F,2))},
            new object[] { 
                new List<WordRatioModel>() {
                    new WordRatioModel() {Word = "test1", TfIdf = 0.5F},
                    new WordRatioModel() {Word = "test2", TfIdf = 0.8F},
                    new WordRatioModel() {Word = "test3", TfIdf = 0.2F}},
                new List<string>()
                {
                    "test2", "test10"
                }, 
                0.8F/(Math.Sqrt(Math.Pow(0.5F,2)+Math.Pow(0.8F,2)+Math.Pow(0.2F,2)) * Math.Sqrt(2))},
            new object[] { 
                new List<WordRatioModel>() {
                    new WordRatioModel() {Word = "test1", TfIdf = 0.5F},
                    new WordRatioModel() {Word = "test2", TfIdf = 0.8F},
                    new WordRatioModel() {Word = "test3", TfIdf = 0.2F}},
                new List<string>()
                {
                    "test3"
                }, 
                0.2F/Math.Sqrt(Math.Pow(0.5F,2)+Math.Pow(0.8F,2)+Math.Pow(0.2F,2))},
            new object[] { 
                new List<WordRatioModel>() {
                    new WordRatioModel() {Word = "test1", TfIdf = 0.5F},
                    new WordRatioModel() {Word = "test2", TfIdf = 0.8F},
                    new WordRatioModel() {Word = "test3", TfIdf = 0.2F}},
                new List<string>()
                {
                    "test1", "test2"
                }, 
                1.3F/(Math.Sqrt(Math.Pow(0.5F,2)+Math.Pow(0.8F,2)+Math.Pow(0.2F,2)) * Math.Sqrt(2))},
            new object[] { 
                new List<WordRatioModel>() {
                    new WordRatioModel() {Word = "test1", TfIdf = 0.1F},
                    new WordRatioModel() {Word = "test2", TfIdf = 0.2F},
                    new WordRatioModel() {Word = "test3", TfIdf = 1.1F},
                    new WordRatioModel() {Word = "test4", TfIdf = 1.5F},
                    new WordRatioModel() {Word = "test5", TfIdf = 2F}},
                new List<string>()
                {
                    "test3", "test4"
                }, 
                2.6F/(Math.Sqrt(Math.Pow(0.1F,2)+Math.Pow(0.2F,2)+Math.Pow(1.1F,2)+Math.Pow(1.5F,2)+Math.Pow(2F,2)) * Math.Sqrt(2))},
            new object[] { 
                new List<WordRatioModel>() {
                    new WordRatioModel() {Word = "test1", TfIdf = 0.1F},
                    new WordRatioModel() {Word = "test2", TfIdf = 0.2F},
                    new WordRatioModel() {Word = "test3", TfIdf = 1.1F},
                    new WordRatioModel() {Word = "test4", TfIdf = 1.5F},
                    new WordRatioModel() {Word = "test5", TfIdf = 2F}},
                new List<string>()
                {
                    "test3", "test4", "test10", "test11", "test12"
                }, 
                2.6F/(Math.Sqrt(Math.Pow(0.1F,2)+Math.Pow(0.2F,2)+Math.Pow(1.1F,2)+Math.Pow(1.5F,2)+Math.Pow(2F,2)) * Math.Sqrt(5))},
            new object[] { 
                new List<WordRatioModel>() {
                    new WordRatioModel() {Word = "test1", TfIdf = 1.3F}},
                new List<string>()
                {
                    "test1"
                }, 
                1.3F/(Math.Sqrt(Math.Pow(1.3F,2)) * Math.Sqrt(1))},
        };
        
    
}
