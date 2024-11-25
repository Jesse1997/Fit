using Fit.Services;

namespace Fit
{
    public class Main
    {
        private readonly IConsoleService _consoleService;
        private readonly ICommandService _commandService;

        public Main(IConsoleService consoleService, ICommandService commandService)
        {
            _consoleService = consoleService;
            _commandService = commandService;
        }

        public void Start()
        {
            string path = GetWorkingDirectoryPathFromUser();
            _commandService.SetPath(path);

            string command;

            do
            {
                command = _consoleService.Read();
                HandleCommand(command);
            }
            while (command != "fit quit");
        }

        public void HandleCommand(string command)
        {
            if (!_commandService.CommandExists(command) && command != "fit quit")
            {
                _consoleService.Write($"'{command}' is not an existing command...");
                return;
            }

            _commandService.ExecuteCommand(command);
        }

        public string GetWorkingDirectoryPathFromUser()
        {
            string path;

            do
            {
                _consoleService.Write("Give the correct path of the working directory: ");
                path = _consoleService.Read();

                var directoryExists = Directory.Exists(path);

                if (directoryExists)
                {
                    _consoleService.Write("Working directory path updated!");
                    return path;
                }
                else
                {
                    _consoleService.Write("Given path is not an existing working directory!");
                }

            } while (true);
        }
    }
}
