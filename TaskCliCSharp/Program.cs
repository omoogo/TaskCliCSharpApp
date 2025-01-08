using TaskCliCSharp.Data;
using TaskCliCSharp.Services;

if (args.Length == 0)
{
    return;
}

var command = args[0];

var taskService = new TaskService(new TaskRepository());

taskService.ExecuteCommand(command, args);