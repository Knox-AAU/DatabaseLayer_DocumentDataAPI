using System.Reflection.Metadata;
using DocumentDataAPI;

namespace DocumentDataAPITests.Relevance_Function;



[Collection("DocumentDataApiUnitTests")]
public class CosineSimilarityCalculatorUnitTests
{

    private readonly CosineSimilarityCalculator _calculator = new CosineSimilarityCalculator();

    [Theory]
    [MemberData(nameof(TestData))]
    public void CalculateCosineSimilarityReturnsCorrectCosineSimilarity(Dictionary<string, int> document, List<string> query, double expected)
    {
        //Arrange
        
        //Act
        double result = _calculator.CalculateCosineSimilarity(document, query);
        
        //Assert
        result.Should().Be(expected);
    }
    
    public static IEnumerable<object[]> TestData =>
        new List<object[]>
        {
            new object[] { 
                new Dictionary<string, int>() {
                    {"test1", 1},
                    {"test2", 5},
                    {"test3", 10}}, 
                new List<string>()
                {
                    "test1"
                }, 
                1/Math.Sqrt(Math.Pow(1,2)+Math.Pow(5,2)+Math.Pow(10,2))},
            new object[] { 
                new Dictionary<string, int>() {
                    {"test1", 1},
                    {"test2", 5},
                    {"test3", 10}}, 
                new List<string>()
                {
                    "test2"
                }, 
                5/Math.Sqrt(Math.Pow(1,2)+Math.Pow(5,2)+Math.Pow(10,2))},
            new object[] { 
                new Dictionary<string, int>() {
                    {"test1", 1},
                    {"test2", 5},
                    {"test3", 10}}, 
                new List<string>()
                {
                    "test2", "test10"
                }, 
                5/(Math.Sqrt(Math.Pow(1,2)+Math.Pow(5,2)+Math.Pow(10,2)) * Math.Sqrt(2))},
            new object[] { 
                new Dictionary<string, int>() {
                    {"test1", 1},
                    {"test2", 5},
                    {"test3", 10}}, 
                new List<string>()
                {
                    "test3"
                }, 
                10/Math.Sqrt(Math.Pow(1,2)+Math.Pow(5,2)+Math.Pow(10,2))},
            new object[] { 
                new Dictionary<string, int>() {
                    {"test1", 1},
                    {"test2", 5},
                    {"test3", 10}}, 
                new List<string>()
                {
                    "test1", "test2"
                }, 
                6/(Math.Sqrt(Math.Pow(1,2)+Math.Pow(5,2)+Math.Pow(10,2)) * Math.Sqrt(2))},
            new object[] { 
                new Dictionary<string, int>() {
                    {"test1", 10},
                    {"test2", 15},
                    {"test3", 10},
                    {"test4", 25},
                    {"test5", 20}},
                new List<string>()
                {
                    "test3", "test4"
                }, 
                35/(Math.Sqrt(Math.Pow(10,2)+Math.Pow(15,2)+Math.Pow(10,2)+Math.Pow(25,2)+Math.Pow(20,2)) * Math.Sqrt(2))},
            new object[] { 
                new Dictionary<string, int>() {
                    {"test1", 10},
                    {"test2", 15},
                    {"test3", 10},
                    {"test4", 25},
                    {"test5", 20}},
                new List<string>()
                {
                    "test3", "test4", "test10", "test11", "test12"
                }, 
                35/(Math.Sqrt(Math.Pow(10,2)+Math.Pow(15,2)+Math.Pow(10,2)+Math.Pow(25,2)+Math.Pow(20,2)) * Math.Sqrt(5))},

        };
        
    
}
