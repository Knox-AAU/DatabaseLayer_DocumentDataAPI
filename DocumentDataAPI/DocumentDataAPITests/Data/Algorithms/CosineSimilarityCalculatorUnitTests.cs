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
        result.Should().Be(expected);
    }
    
    public static IEnumerable<object[]> TestData =>
        new List<object[]>
        {
            new object[] { 
                new List<WordRatioModel>() {
                    new WordRatioModel() {Word = "test1", Amount = 1},
                    new WordRatioModel() {Word = "test2", Amount = 5},
                    new WordRatioModel() {Word = "test3", Amount = 10}},
                new List<string>()
                {
                    "test1"
                }, 
                1/Math.Sqrt(Math.Pow(1,2)+Math.Pow(5,2)+Math.Pow(10,2))},
            new object[] { 
                new List<WordRatioModel>() {
                    new WordRatioModel() {Word = "test1", Amount = 1},
                    new WordRatioModel() {Word = "test2", Amount = 5},
                    new WordRatioModel() {Word = "test3", Amount = 10}},
                new List<string>()
                {
                    "test2"
                }, 
                5/Math.Sqrt(Math.Pow(1,2)+Math.Pow(5,2)+Math.Pow(10,2))},
            new object[] { 
                new List<WordRatioModel>() {
                    new WordRatioModel() {Word = "test1", Amount = 1},
                    new WordRatioModel() {Word = "test2", Amount = 5},
                    new WordRatioModel() {Word = "test3", Amount = 10}},
                new List<string>()
                {
                    "test2", "test10"
                }, 
                5/(Math.Sqrt(Math.Pow(1,2)+Math.Pow(5,2)+Math.Pow(10,2)) * Math.Sqrt(2))},
            new object[] { 
                new List<WordRatioModel>() {
                    new WordRatioModel() {Word = "test1", Amount = 1},
                    new WordRatioModel() {Word = "test2", Amount = 5},
                    new WordRatioModel() {Word = "test3", Amount = 10}},
                new List<string>()
                {
                    "test3"
                }, 
                10/Math.Sqrt(Math.Pow(1,2)+Math.Pow(5,2)+Math.Pow(10,2))},
            new object[] { 
                new List<WordRatioModel>() {
                    new WordRatioModel() {Word = "test1", Amount = 1},
                    new WordRatioModel() {Word = "test2", Amount = 5},
                    new WordRatioModel() {Word = "test3", Amount = 10}},
                new List<string>()
                {
                    "test1", "test2"
                }, 
                6/(Math.Sqrt(Math.Pow(1,2)+Math.Pow(5,2)+Math.Pow(10,2)) * Math.Sqrt(2))},
            new object[] { 
                new List<WordRatioModel>() {
                    new WordRatioModel() {Word = "test1", Amount = 10},
                    new WordRatioModel() {Word = "test2", Amount = 15},
                    new WordRatioModel() {Word = "test3", Amount = 10},
                    new WordRatioModel() {Word = "test4", Amount = 25},
                    new WordRatioModel() {Word = "test5", Amount = 20}},
                new List<string>()
                {
                    "test3", "test4"
                }, 
                35/(Math.Sqrt(Math.Pow(10,2)+Math.Pow(15,2)+Math.Pow(10,2)+Math.Pow(25,2)+Math.Pow(20,2)) * Math.Sqrt(2))},
            new object[] { 
                new List<WordRatioModel>() {
                    new WordRatioModel() {Word = "test1", Amount = 10},
                    new WordRatioModel() {Word = "test2", Amount = 15},
                    new WordRatioModel() {Word = "test3", Amount = 10},
                    new WordRatioModel() {Word = "test4", Amount = 25},
                    new WordRatioModel() {Word = "test5", Amount = 20}},
                new List<string>()
                {
                    "test3", "test4", "test10", "test11", "test12"
                }, 
                35/(Math.Sqrt(Math.Pow(10,2)+Math.Pow(15,2)+Math.Pow(10,2)+Math.Pow(25,2)+Math.Pow(20,2)) * Math.Sqrt(5))},
            new object[] { 
                new List<WordRatioModel>() {
                    new WordRatioModel() {Word = "test1", Amount = 10}},
                new List<string>()
                {
                    "test1"
                }, 
                10/(Math.Sqrt(Math.Pow(10,2)) * Math.Sqrt(1))},
        };
        
    
}
