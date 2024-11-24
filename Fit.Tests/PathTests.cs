using Fit.Services;
using Moq;

namespace Fit.Tests
{
    public class PathTests
    {
        private readonly Mock<IConsoleService> _consoleServiceMock;

        public PathTests()
        {
            _consoleServiceMock = new Mock<IConsoleService>();
        }

        [Fact]
        public void GetWorkingDirectoryPathFromUser_ShouldReturnPath_WhenCorrectPathGiven()
        {
            // Arrange
            const string correctPath = "C:\\Users\\jesse\\OneDrive\\Documents\\Fit - git clone\\MyTestSpace\\fit";
            _consoleServiceMock.Setup(x => x.Read()).Returns(correctPath);

            var sut = new Main(_consoleServiceMock.Object);

            // Act
            var resultPath = sut.GetWorkingDirectoryPathFromUser();

            // Assert
            Assert.Equal(correctPath, resultPath);
        }

        [Fact]
        public void GetWorkingDirectoryPathFromUser_ShouldAskUserForPath()
        {
            // Arrange
            const string correctPath = "C:\\Users\\jesse\\OneDrive\\Documents\\Fit - git clone\\MyTestSpace\\fit";
            _consoleServiceMock.Setup(x => x.Read()).Returns(correctPath);

            var sut = new Main(_consoleServiceMock.Object);

            // Act
            sut.GetWorkingDirectoryPathFromUser();


            // Assert
            _consoleServiceMock.Verify(x => x.Write("Give the correct path of the working directory: "));
        }

        [Fact]
        public void GetWorkingDirectoryPathFromUser_ShouldAskUserAgain_WhenIncorrectPathGiven()
        {
            // Arrange
            const string incorrectPath = "C:\\Users\\jesse\\OneDrive\\Documents\\Fit - git clone\\Fit\\Fit.Tests\\TestEnv\\IncorrectPath";
            const string correctPath = "C:\\Users\\jesse\\OneDrive\\Documents\\Fit - git clone\\MyTestSpace\\fit";
            _consoleServiceMock.SetupSequence(x => x.Read()).Returns(incorrectPath).Returns(correctPath);

            var sut = new Main(_consoleServiceMock.Object);

            // Act
            var resultPath = sut.GetWorkingDirectoryPathFromUser();

            // Assert
            _consoleServiceMock.Verify(x => x.Write("Given path is not an existing working directory!"));
            _consoleServiceMock.Verify(x => x.Write("Give the correct path of the working directory: "), Times.Exactly(2));
        }
    }
}