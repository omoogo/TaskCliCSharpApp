using FluentAssertions;
using NSubstitute;
using TaskCliCSharp.Data;
using TaskCliCSharp.Services;

namespace TaskCliCSharp.Tests;

[TestFixture]
public class TaskServiceShould
{
    private ITaskRepository _taskRepository;
    private TaskService _taskService;
    private StringWriter _stringWriter;
    private TextWriter _originalConsoleOutput;

    [SetUp]
    public void SetUp()
    {
        _taskRepository = Substitute.For<ITaskRepository>();
        _taskService = new TaskService(_taskRepository);

        _originalConsoleOutput = Console.Out;

        _stringWriter = new StringWriter();
        Console.SetOut(_stringWriter);
    }

    [TearDown]
    public void TearDown()
    {
        _stringWriter.Dispose();
        Console.SetOut(_originalConsoleOutput);
    }

    [Test]
    public void PrintTaskAddedSuccessfullyAndTaskIdWithTheAddCommand()
    {
        var mockTasks = new List<Data.Entities.Task>
        {
            new() { Id = 1, Description = "Test Task", Status = "todo" }
        };
        var args = new string[] { "add", mockTasks.First().Description };
        _taskRepository.AddTask(mockTasks.First().Description).Returns(mockTasks.First().Id);

        _taskService.ExecuteCommand("add", args);

        _taskRepository.Received().AddTask(Arg.Is<string>(t => t == "Test Task"));
        _stringWriter.ToString().Should().Contain($"Task added successfully (ID: {mockTasks.First().Id})");
    }

    [Test]
    public void PrintAllTasksWithTheListCommand()
    {
        var mockTasks = new List<Data.Entities.Task>
        {
            new() { Id = 1, Description = "Test for list 1", Status = "todo" },
            new() { Id = 2, Description = "Test for list 2", Status = "todo" }
        };

        _taskRepository.GetTasks().Returns(mockTasks);

        var args = new string[] { "list" };
        _taskService.ExecuteCommand("list", args);

        _taskRepository.Received().GetTasks();
        _stringWriter.ToString().Should().Contain("Test for list 1");
        _stringWriter.ToString().Should().Contain("Test for list 2");
    }

    [Test]
    public void PrintNoTasksFoundWithTheListCommandIfNoTasks()
    {
        var args = new string[] { "list" };

        _taskService.ExecuteCommand("list", args);

        _stringWriter.ToString().Should().Contain("No tasks found");
    }

    [Test]
    public void PrintUnknownCommandIfCommandIsNotRecognized()
    {
        var args = new string[] { "unknown" };

        _taskService.ExecuteCommand("unknown", args);

        _stringWriter.ToString().Should().Contain("Unknown command");
    }

    [Test]
    public void PrintPleaseProvideDescriptionIfNoDescriptionIsProvidedWithNewTask()
    {
        var args = new string[] { "add" };

        _taskService.ExecuteCommand("add", args);

        _stringWriter.ToString().Should().Contain("Please provide a description for the task");
    }

    [Test]
    public void UpdateTaskDescriptionWithTheUpdateCommand()
    {
        var mockTasks = new List<Data.Entities.Task>
        {
            new() { Id = 1, Description = "old description", Status = "todo" }
        };
        _taskRepository.GetTasks().Returns(mockTasks);
        _taskRepository.TaskExists(1).Returns(true);

        _taskService.ExecuteCommand("update", ["update", "1", "new description"]);

        _taskRepository.Received(1).UpdateTaskDescription(
            Arg.Is<int>(i => i == 1), 
            Arg.Is<string>(d => d == "new description"
        ));
    }

    [Test]
    public void PrintPleaseProvideDescriptionIfNoDescriptionIsProvidedWithUpdateTask()
    {
        var args = new string[] { "update", "1" };

        _taskService.ExecuteCommand("update", args);

        _stringWriter.ToString().Should().Contain("Please provide a task id and a description");
    }

    [Test]
    public void PrintNoTasksFoundIfTaskDoesNotExist()
    {
        var args = new string[] { "update", "1", "new description" };

        _taskService.ExecuteCommand("update", args);

        _stringWriter.ToString().Should().Contain("Task not found");
    }

    [Test]
    public void DeleteTaskWithTheDeleteCommand()
    {
        var mockTasks = new List<Data.Entities.Task>
        {
            new() { Id = 1, Description = "Test for delete", Status = "todo" }
        };
        _taskRepository.GetTasks().Returns(mockTasks);
        _taskRepository.TaskExists(1).Returns(true);

        _taskService.ExecuteCommand("delete", ["delete", "1"]);

        _taskRepository.Received(1).DeleteTask(Arg.Is<int>(i => i == 1));
    }

    [Test]
    public void PrintTaskNotFoundIfTaskDoesNotExistWithTheDeleteCommand()
    {
        var args = new string[] { "delete", "1" };

        _taskService.ExecuteCommand("delete", args);

        _stringWriter.ToString().Should().Contain("Task not found");
    }

    [Test]
    public void PrintPleaseProvideTaskIdIfNoTaskIdIsProvidedWithDeleteCommand()
    {
        var args = new string[] { "delete" };

        _taskService.ExecuteCommand("delete", args);

        _stringWriter.ToString().Should().Contain("Please provide a task id");
    }

    [Test]
    public void UpdateTaskStatusToInProgressWithTheMarkInProgressCommand()
    {
        var mockTasks = new List<Data.Entities.Task>
        {
            new() { Id = 1, Description = "Test for mark in progress", Status = "todo" }
        };
        _taskRepository.GetTasks().Returns(mockTasks);
        _taskRepository.TaskExists(1).Returns(true);

        _taskService.ExecuteCommand("mark-in-progress", ["mark-in-progress", "1"]);

        _taskRepository.Received(1).UpdateTaskStatus(
            Arg.Is<int>(i => i == 1),
            Arg.Is<string>(s => s == "in-progress"
        ));
    }

    [Test]
    public void PrintTaskNotFoundIfTaskDoesNotExistWithTheMarkInProgressCommand()
    {
        var args = new string[] { "mark-in-progress", "1" };

        _taskService.ExecuteCommand("mark-in-progress", args);

        _stringWriter.ToString().Should().Contain("Task not found");
    }

    [Test]
    public void PrintPleaseProvideTaskIdIfNoTaskIdIsProvidedWithMarkInProgressCommand()
    {
        var args = new string[] { "mark-in-progress" };

        _taskService.ExecuteCommand("mark-in-progress", args);

        _stringWriter.ToString().Should().Contain("Please provide a task id");
    }

    [Test]
    public void UpdateTaskStatusToDoneWithTheMarkDoneCommand()
    {
        var mockTasks = new List<Data.Entities.Task>
        {
            new() { Id = 1, Description = "Test for mark done", Status = "todo" }
        };
        _taskRepository.GetTasks().Returns(mockTasks);
        _taskRepository.TaskExists(1).Returns(true);

        _taskService.ExecuteCommand("mark-done", ["mark-done", "1"]);

        _taskRepository.Received(1).UpdateTaskStatus(
            Arg.Is<int>(i => i == 1),
            Arg.Is<string>(s => s == "done"
        ));
    }

    [Test]
    public void PrintTaskNotFoundIfTaskDoesNotExistWithTheMarkDoneCommand()
    {
        var args = new string[] { "mark-done", "1" };

        _taskService.ExecuteCommand("mark-done", args);

        _stringWriter.ToString().Should().Contain("Task not found");
    }

    [Test]
    public void PrintPleaseProvideTaskIdIfNoTaskIdIsProvidedWithMarkDoneCommand()
    {
        var args = new string[] { "mark-done" };

        _taskService.ExecuteCommand("mark-done", args);

        _stringWriter.ToString().Should().Contain("Please provide a task id");
    }

    [Test]
    [TestCase("todo")]
    [TestCase("in-progress")]
    [TestCase("done")]
    public void PrintTasksWithMatchingStatusWithTheListCommand(string status)
    {
        // Arrange
        var mockTasks = new List<Data.Entities.Task>
        {
            new() { Id = 1, Description = "Test for list todo", Status = "todo" },
            new() { Id = 2, Description = "Test for list in-progress", Status = "in-progress" },
            new() { Id = 3, Description = "Test for list done", Status = "done" }
        };
        _taskRepository
            .GetTasks(status)
            .Returns(mockTasks.Where(t => t.Status == status)
            .ToList());
        var args = new string[] { "list", status };

        // Act
        _taskService.ExecuteCommand("list", args);

        // Assert(s)
        _taskRepository.Received(1).GetTasks(status);

        var consoleOutput = _stringWriter.ToString();

        consoleOutput
            .Should().Contain(mockTasks.Where(t => t.Status == status).First().Description);

        foreach (var task in mockTasks.Where(t => t.Status != status))
        {
            consoleOutput.Should().NotContain(task.Description);
        }
    }

    [Test]
    public void PrintInvalidStatusIfStatusIsNotValidWithTheListCommand()
    {
        var args = new string[] { "list", "invalid-status" };

        _taskService.ExecuteCommand("list", args);

        _stringWriter.ToString().Should().Contain("Invalid status");
    }


}
