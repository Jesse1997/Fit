using Fit.Services;
using Moq;

namespace Fit.Tests
{
    public class MainTests
    {
        private readonly Mock<IConsoleService> _consoleServiceMock;
        private readonly Mock<ICommandService> _commandServiceMock;
        private readonly Main _sut;


        public MainTests()
        {
            _consoleServiceMock = new Mock<IConsoleService>();
            _commandServiceMock = new Mock<ICommandService>();
            _sut = new Main(_consoleServiceMock.Object, _commandServiceMock.Object);
        }

        [Fact]
        public void Start_ShouldSetCommandPath_WhenCorrectPathGiven()
        {
            // Arrange
            const string correctPath = "C:\\Users\\jesse\\OneDrive\\Documents\\Fit - git clone\\MyTestSpace\\fit";
            const string quitCommand = "fit quit";
            _consoleServiceMock.SetupSequence(x => x.Read()).Returns(correctPath).Returns(quitCommand);

            // Act
            _sut.Start();

            // Assert
            _commandServiceMock.Verify(x => x.SetPath(correctPath));
        }

        [Fact]
        public void Start_ShouldAskUserForCommandAgain_WhenFitQuitCommandNotGiven()
        {
            // Arrange
            const string correctPath = "C:\\Users\\jesse\\OneDrive\\Documents\\Fit - git clone\\MyTestSpace\\fit";
            const string knownCommand = "fit known";
            const string quitCommand = "fit quit";
            _consoleServiceMock.SetupSequence(x => x.Read()).Returns(correctPath).Returns(knownCommand).Returns(quitCommand);
            _commandServiceMock.Setup(x => x.CommandExists(It.IsAny<string>())).Returns(true);

            // Act
            _sut.Start();

            // Assert
            _commandServiceMock.Verify(x => x.ExecuteCommand(It.IsAny<string>()), Times.Exactly(2));
        }

        [Fact]
        public void HandleCommand_ShouldShowError_WhenUnknownCommandGiven()
        {
            // Arrange
            const string unknownCommand = "fit unknown";
            _commandServiceMock.Setup(x => x.CommandExists(It.IsAny<string>())).Returns(false);

            // Act
            _sut.HandleCommand(unknownCommand);

            // Assert
            _consoleServiceMock.Verify(x => x.Write($"'{unknownCommand}' is not an existing command..."));
        }

        [Fact]
        public void HandleCommand_ShouldExecuteCommand_WhenKnownCommandGiven()
        {
            // Arrange
            const string knownCommand = "fit init";
            _commandServiceMock.Setup(x => x.CommandExists(It.IsAny<string>())).Returns(true);

            // Act
            _sut.HandleCommand(knownCommand);

            // Assert
            _commandServiceMock.Verify(x => x.ExecuteCommand(knownCommand));
        }


        [Fact]
        public void GetWorkingDirectoryPathFromUser_ShouldReturnPath_WhenCorrectPathGiven()
        {
            // Arrange
            const string correctPath = "C:\\Users\\jesse\\OneDrive\\Documents\\Fit - git clone\\MyTestSpace\\fit";
            _consoleServiceMock.Setup(x => x.Read()).Returns(correctPath);

            // Act
            var resultPath = _sut.GetWorkingDirectoryPathFromUser();

            // Assert
            Assert.Equal(correctPath, resultPath);
        }

        [Fact]
        public void GetWorkingDirectoryPathFromUser_ShouldAskUserForPath()
        {
            // Arrange
            const string correctPath = "C:\\Users\\jesse\\OneDrive\\Documents\\Fit - git clone\\MyTestSpace\\fit";
            _consoleServiceMock.Setup(x => x.Read()).Returns(correctPath);

            // Act
            _sut.GetWorkingDirectoryPathFromUser();


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

            // Act
            var resultPath = _sut.GetWorkingDirectoryPathFromUser();

            // Assert
            _consoleServiceMock.Verify(x => x.Write("Given path is not an existing working directory!"));
            _consoleServiceMock.Verify(x => x.Write("Give the correct path of the working directory: "), Times.Exactly(2));
        }
    }
}