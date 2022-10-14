global using Xunit;
global using Moq;
global using FluentAssertions;


using DocumentDataAPI.Controllers;

//Tests:
//1. Does it connect properly?
//2. Do they throw if null?
//3. Post document
//4. GetAll
//5. GetID
//6. GetTotalDocumentCount
//7. GetBySourceID
//8. GetByAuthor
//9. GetByDate


namespace UnitTests;

public class DocumentsControllerTests
{
    [Fact]
    public void Check_If_GetAll_Throws_Error_Message()
    {
        var mock = new Mock<DocumentsController> { };






        //Arrange
      /*  var DocumentsController = new DocumentDataAPI.Controllers.DocumentsController(logger, config, repository);

        //Act
        var actual = DocumentsController.PutDocument("hello there");

        //Assert
        Assert.IsAssignableFrom<NotFoundObjectResult>(actual);*/
    } 

}