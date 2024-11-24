using Fit.Services;

namespace Fit
{
    public class Main
    {
        private readonly IConsoleService _consoleService;

        public Main(IConsoleService consoleService)
        {
            _consoleService = consoleService;
        }

        public void Start()
        {
            string path = GetWorkingDirectoryPathFromUser();

            string command;

            do
            {
                command = _consoleService.Read();
                HandleCommand(path, command);
            }
            while (command != "fit quit");
        }

        public void HandleCommand(string path, string command)
        {
            if (command != "fit init" && command != "fit quit")
            {
                _consoleService.Write($"'{command}' is not an existing command...");
                return;
            }

            if (command == "fit init")
            {
                try
                {
                    var createdDirectory = Directory.CreateDirectory(path + "/.fit");


                    createdDirectory.Attributes = FileAttributes.Directory | FileAttributes.Hidden;

                    _consoleService.Write(createdDirectory.FullName + " is created");
                }
                catch
                {
                    _consoleService.Write("Something went wrong trying to create a fit repository");
                    return;
                }
            }
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
