using FluentAssertions;
using TaskCliCSharp.Data;

namespace TaskCliCSharp.Tests;

[TestFixture]
public class TaskRepositoryShould
{
    private string _testFilePath;

    [SetUp]
    public void SetUp()
    {
        _testFilePath = Path.Combine(Path.GetTempPath(), "test_tasks.json");

        if (File.Exists(_testFilePath))
        {
            File.Delete(_testFilePath);
        }
    }

    [Test, Order(1)]
    public void ReturnEmptyListWhenNoTasks()
    {
        var repo = new TaskRepository(_testFilePath);

        var tasks = repo.GetTasks();

        tasks.Should().BeEmpty();
    }

    [Test]
    public void AddTask()
    {
        var repo = new TaskRepository(_testFilePath);

        repo.AddTask("Test 1");

        var tasks = repo.GetTasks();
        tasks.Should().ContainSingle(t => t.Description == "Test 1");
    }

    [Test]
    public void AddMultipleTasks()
    {
        var repo = new TaskRepository(_testFilePath);

        repo.AddTask("Test 2");
        repo.AddTask("Test 3");

        var tasks = repo.GetTasks();
        tasks.Should().ContainSingle(t => t.Description == "Test 2");
        tasks.Should().ContainSingle(t => t.Description == "Test 3");
    }

    [Test]
    public void DeleteTask()
    {
        var taskDescription = "Testing Delete";

        var repo = new TaskRepository(_testFilePath);
        repo.AddTask(taskDescription);

        var tasks = repo.GetTasks();
        tasks.Should().ContainSingle(t => t.Description == taskDescription);

        var taskToDelete = tasks.Where(tasks => tasks.Description == taskDescription).First();

        repo.DeleteTask(taskToDelete.Id);

        tasks = repo.GetTasks();
        tasks.Should().NotContain(t => t.Description == taskDescription);
    }

    [Test]
    public void UpdateTaskDescription()
    {
        var orginalTaskDescription = "Testing Update";
        var updatedTaskDescription = "Updated Task";

        var repo = new TaskRepository(_testFilePath);
        repo.AddTask(orginalTaskDescription);

        var tasks = repo.GetTasks();
        tasks.Should().ContainSingle(t => t.Description == orginalTaskDescription);
        var taskToUpdate = tasks.Where(tasks => tasks.Description == orginalTaskDescription).First();

        repo.UpdateTaskDescription(taskToUpdate.Id, updatedTaskDescription);
        tasks = repo.GetTasks();
        tasks.Should().ContainSingle(t => t.Description == updatedTaskDescription);
    }

    [Test]
    public void CheckIfTaskExists()
    {
        var taskDescription = "Testing Task Exists";
        var repo = new TaskRepository(_testFilePath);
        repo.AddTask(taskDescription);

        var tasks = repo.GetTasks();
        tasks.Should().ContainSingle(t => t.Description == taskDescription);
        var taskToCheck = tasks.Where(tasks => tasks.Description == taskDescription).First();

        var result = repo.TaskExists(taskToCheck.Id);

        result.Should().BeTrue();
    }

    [Test]
    public void CheckIfTaskDoesNotExist()
    {
        var repo = new TaskRepository(_testFilePath);
        var tasks = repo.GetTasks();

        var result = repo.TaskExists(999);

        result.Should().BeFalse();
    }

    [Test]
    [TestCase("Testing Done", "done")]
    [TestCase("Testing In Progress", "in-progress")]
    public void UpdateTaskStatus(string description, string status)
    {
        var repo = new TaskRepository(_testFilePath);
        repo.AddTask(description);

        var tasks = repo.GetTasks();
        tasks.Should().ContainSingle(t => t.Description == description);
        var taskToCheck = tasks.Where(tasks => tasks.Description == description).First();

        repo.UpdateTaskStatus(taskToCheck.Id, status);

        tasks = repo.GetTasks();
        tasks.Should().ContainSingle(t => t.Description == description && t.Status == status);
    }

    [Test]
    public void ReturnDoneTasks()
    {
        var repo = new TaskRepository(_testFilePath);

        repo.AddTask("Done 1");
        repo.AddTask("Done 2");
        repo.AddTask("Done 3");
        repo.UpdateTaskStatus(1, "done");
        repo.UpdateTaskStatus(3, "done");

        var tasks = repo.GetTasks("done");

        tasks.Should().ContainSingle(t => t.Description == "Done 1" && t.Status == "done");
        tasks.Should().ContainSingle(t => t.Description == "Done 3" && t.Status == "done");
        tasks.Should().NotContain(t => t.Description == "Done 2");
    }

    [Test]
    public void ReturnTodoTasks()
    {
        var repo = new TaskRepository(_testFilePath);
        repo.AddTask("Todo 1");
        repo.AddTask("Todo 2");
        repo.AddTask("Todo 3");
        repo.UpdateTaskStatus(1, "done");

        var tasks = repo.GetTasks("todo");

        tasks.Should().ContainSingle(t => t.Description == "Todo 2");
        tasks.Should().ContainSingle(t => t.Description == "Todo 3");
        tasks.Should().NotContain(t => t.Description == "Todo 1");
    }

    [Test]
    public void ReturnInProgressTasks()
    {
        var repo = new TaskRepository(_testFilePath);
        repo.AddTask("In Progress 1");
        repo.AddTask("In Progress 2");
        repo.AddTask("In Progress 3");
        repo.UpdateTaskStatus(1, "done");
        repo.UpdateTaskStatus(3, "in-progress");

        var tasks = repo.GetTasks("in-progress");

        tasks.Should().ContainSingle(t => t.Description == "In Progress 3" && t.Status == "in-progress");
        tasks.Should().NotContain(t => t.Description == "In Progress 1");
        tasks.Should().NotContain(t => t.Description == "In Progress 2");
    }

}
