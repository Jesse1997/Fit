using Fit.Services;

namespace Fit.Tests
{
    public class CommandServiceTests
    {
        private readonly CommandService _sut;


        public CommandServiceTests()
        {
            _sut = new CommandService();
        }

        [Fact]
        public void FitInitExecute_ShouldCreateSecretInitDirectory()
        {
            // Arrange
            const string correctPath = "C:\\Users\\jesse\\OneDrive\\Documents\\Fit - git clone\\Fit\\Fit.Tests\\TestEnv";
            if (Directory.Exists(correctPath + "/.fit")) Directory.Delete(correctPath + "/.fit");
            _sut.SetPath(correctPath);

            // Act
            Assert.False(Directory.Exists(correctPath + "/.fit"));

            _sut.FitInitExecute();

            // Assert
            Assert.True(Directory.Exists(correctPath + "/.fit"));
            var directoryInfo = new DirectoryInfo("C:\\Users\\jesse\\OneDrive\\Documents\\Fit - git clone\\Fit\\Fit.Tests\\TestEnv\\.fit");
            Assert.True(directoryInfo.Attributes.Equals(FileAttributes.Directory | FileAttributes.Hidden));
        }

        [Fact]
        public void HandleCommand_ShouldNotCreateInitDirectory_WhenIncorrectPathGiven()
        {
            // Arrange
            const string incorrectPath = "/////// path";
            _sut.SetPath(incorrectPath);

            // Act
            _sut.FitInitExecute();

            // Assert
            Assert.False(Directory.Exists(incorrectPath + "/.fit"));
        }
    }
}