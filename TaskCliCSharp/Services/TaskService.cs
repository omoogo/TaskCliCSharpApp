using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using TaskCliCSharp.Data;

namespace TaskCliCSharp.Services;

public class TaskService
{
    private readonly ITaskRepository _taskRepository;

    public TaskService(ITaskRepository repository)
    {
        _taskRepository = repository;
    }

    public void ExecuteCommand(string command, string[] args)
    {
        switch (command)
        {
            case "add":
                AddTask(args);
                break;
            case "list":
                ListAllTasks(args);
                break;
            case "update":
                UpdateTask(args);
                break;
            case "delete":
                DeleteTask(args);
                break;
            case "mark-in-progress":
                UpdateTaskStatus(args, "in-progress");
                break;
            case "mark-done":
                UpdateTaskStatus(args, "done");
                break;
            default:
                Console.WriteLine("Unknown command");
                break;
        }
    }

    private void UpdateTaskStatus(string[] args, string status)
    {
        if (args.Length < 2)
        {
            Console.WriteLine("Please provide a task id");
            return;
        }

        var taskId = int.Parse(args[1]);

        if (!_taskRepository.TaskExists(taskId))
        {
            Console.WriteLine("Task not found");
            return;
        }

        _taskRepository.UpdateTaskStatus(taskId, status);
    }

    private void DeleteTask(string[] args)
    {
        if (args.Length < 2)
        {
            Console.WriteLine("Please provide a task id");
            return;
        }

        var taskId = int.Parse(args[1]);

        if (!_taskRepository.TaskExists(taskId))
        {
            Console.WriteLine("Task not found");
            return;
        }

        _taskRepository.DeleteTask(taskId);
    }

    private void UpdateTask(string[] args)
    {
        if (args.Length < 3)
        {
            Console.WriteLine("Please provide a task id and a description");
            return;
        }

        var taskId = int.Parse(args[1]);

        if (!_taskRepository.TaskExists(taskId))
        {
            Console.WriteLine("Task not found");
            return;
        }

        var description = string.Join(" ", args.Skip(2));
        _taskRepository.UpdateTaskDescription(taskId, description);
    }

    public void AddTask(string[] args)
    {
        if (args.Length < 2)
        {
            Console.WriteLine("Please provide a description for the task");
            return;
        }
        var description = string.Join(" ", args.Skip(1));
        int taskId = _taskRepository.AddTask(description);
        Console.WriteLine($"Task added successfully (ID: {taskId})");
    }

    private void ListAllTasks(string[] args)
    {
        string status = "";
        if (args.Length > 1)
        {
            status = args[1];
        }
        if (status is not "" and not "todo" and not "in-progress" and not "done")
        {
            Console.WriteLine("Invalid status");
            return;
        }

        List<Data.Entities.Task> tasks;

        if (args.Length > 1)
        {
            tasks = _taskRepository.GetTasks(args[1]);
        }
        else
        {
            tasks = _taskRepository.GetTasks();
        }

        if (tasks is null || tasks.Count == 0)
        {
            Console.WriteLine("No tasks found");
            return;
        }

        foreach (var task in tasks)
        {
            Console.WriteLine($"{task.Id} - {task.Description} - {task.Status}");
        }
    }


}
