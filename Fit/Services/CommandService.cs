namespace Fit.Services
{
    public interface ICommandService
    {
        public void SetPath(string path);
        public void ExecuteCommand(string command);
        public bool CommandExists(string command);
    }
    public class CommandService : ICommandService
    {
        private readonly IConsoleService _consoleService;
        private string _path = "";

        public readonly IDictionary<string, Action> _commands = new Dictionary<string, Action>();

        public CommandService()
        {
            _consoleService = new ConsoleService();
            _commands.Add("fit init", FitInitExecute);
        }

        public void FitInitExecute()
        {
            try
            {
                var createdDirectory = Directory.CreateDirectory(_path + "/.fit");


                createdDirectory.Attributes = FileAttributes.Directory | FileAttributes.Hidden;

                _consoleService.Write(createdDirectory.FullName + " is created");
            }
            catch
            {
                _consoleService.Write("Something went wrong trying to create a fit repository");
                return;
            }
        }

        public void SetPath(string path)
        {
            _path = path;
        }

        public void ExecuteCommand(string command)
        {
            _commands[command].Invoke();
        }

        public bool CommandExists(string command)
        {
            return _commands.ContainsKey(command);
        }
    }
}
