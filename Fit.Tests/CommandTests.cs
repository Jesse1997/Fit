using Fit.Services;
using Moq;

namespace Fit.Tests
{
    public class CommandTests
    {
        private readonly Mock<IConsoleService> _consoleServiceMock;

        public CommandTests()
        {
            _consoleServiceMock = new Mock<IConsoleService>();
        }

        [Fact]
        public void HandleCommand_ShouldShowError_WhenUnknownCommandGiven()
        {
            // Arrange
            const string correctPath = "C:\\Users\\jesse\\OneDrive\\Documents\\Fit - git clone\\MyTestSpace\\fit";
            const string unknownCommand = "fit unknown";

            var sut = new Main(_consoleServiceMock.Object);

            // Act
            sut.HandleCommand(correctPath, unknownCommand);

            // Assert
            _consoleServiceMock.Verify(x => x.Write($"'{unknownCommand}' is not an existing command..."));
        }

        [Fact]
        public void HandleCommand_ShouldCreateSecretInitDirectory_WhenFitInitCommandAndCorrectPathGiven()
        {
            // Arrange
            const string correctPath = "C:\\Users\\jesse\\OneDrive\\Documents\\Fit - git clone\\Fit\\Fit.Tests\\TestEnv";
            const string fitInitCommand = "fit init";
            if (Directory.Exists(correctPath + "/.fit")) Directory.Delete(correctPath + "/.fit");

            var sut = new Main(_consoleServiceMock.Object);

            // Act
            Assert.False(Directory.Exists(correctPath + "/.fit"));

            sut.HandleCommand(correctPath, fitInitCommand);

            // Assert
            Assert.True(Directory.Exists(correctPath + "/.fit"));
            var directoryInfo = new DirectoryInfo("C:\\Users\\jesse\\OneDrive\\Documents\\Fit - git clone\\Fit\\Fit.Tests\\TestEnv\\.fit");
            Assert.True(directoryInfo.Attributes.Equals(FileAttributes.Directory | FileAttributes.Hidden));
        }

        [Fact]
        public void HandleCommand_ShouldNotCreateInitDirectory_WhenFitInitCommandAndInCorrectPathGiven()
        {
            // Arrange
            const string incorrectPath = "/////// path";
            const string fitInitCommand = "fit init";

            var sut = new Main(_consoleServiceMock.Object);

            // Act
            sut.HandleCommand(incorrectPath, fitInitCommand);

            // Assert
            Assert.False(Directory.Exists(incorrectPath + "/.fit"));
            _consoleServiceMock.Verify(x => x.Write("Something went wrong trying to create a fit repository"));
        }
    }
}