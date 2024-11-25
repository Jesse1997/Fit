using Fit;
using Fit.Services;

IConsoleService consoleService = new ConsoleService();
ICommandService commandService = new CommandService();


var main = new Main(consoleService, commandService);

main.Start();