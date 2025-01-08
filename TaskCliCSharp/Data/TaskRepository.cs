using System.Text.Json;

namespace TaskCliCSharp.Data;

public class TaskRepository : ITaskRepository
{
    private readonly string _filePath; 

    public TaskRepository(string? filePath = null)
    {
        _filePath = filePath ?? Path.Combine(AppContext.BaseDirectory, "tasks.json");

        if (!File.Exists(_filePath))
        {
            File.WriteAllText(_filePath, "{}");
        }
    }

    public int AddTask(string description)
    {
        var tasks = GetTasks();
        var lastTask = tasks.OrderByDescending(t => t.Id).FirstOrDefault();
        var id = lastTask?.Id + 1 ?? 1;

        var task = new Entities.Task
        {
            Id = id,
            Description = description,
            Status = "todo",
            CreatedAt = DateTime.UtcNow
        };

        tasks.Add(task);

        var json = JsonSerializer.Serialize(tasks);
        File.WriteAllText(_filePath, json);
        return id;
    }

    public void DeleteTask(int id)
    {
        var tasks = GetTasks();

        if (!tasks.Any(t => t.Id == id))
        {
            return;
        }

        tasks.Remove(tasks.First(t => t.Id == id));

        var json = JsonSerializer.Serialize(tasks);
        File.WriteAllText(_filePath, json);
    }

    public List<Entities.Task> GetTasks(string? status = null)
    {
        if (!File.Exists(_filePath))
        {
            return new List<Entities.Task>();
        }

        var tasksText = File.ReadAllText(_filePath);

        if (string.IsNullOrWhiteSpace(tasksText) || tasksText == "{}")
        {
            return new List<Entities.Task>();
        }

        var tasks = JsonSerializer.Deserialize<List<Entities.Task>>(tasksText) ?? new List<Entities.Task>();

        return !string.IsNullOrEmpty(status)
            ? tasks.Where(t => t.Status == status).ToList() 
            : tasks;
    }

    public bool TaskExists(int id)
    {
        var tasks = GetTasks();
        return tasks.Any(t => t.Id == id);
    }

    public void UpdateTaskDescription(int id, string description)
    {
        var tasks = GetTasks();

        if (!tasks.Any(t => t.Id == id))
        {
            return;
        }

        var taskToUpdate = tasks.First(t => t.Id == id);
        taskToUpdate.Description = description;
        taskToUpdate.UpdatedAt = DateTime.UtcNow;
            
        tasks.Remove(tasks.First(t => t.Id == id));
        tasks.Add(taskToUpdate);

        var json = JsonSerializer.Serialize(tasks);
        File.WriteAllText(_filePath, json);
    }

    public void UpdateTaskStatus(int id, string status)
    {
        var tasks = GetTasks();

        if (!tasks.Any(t => t.Id == id))
        {
            return;
        }

        var taskToUpdate = tasks.First(t => t.Id == id);
        taskToUpdate.Status = status;
        taskToUpdate.UpdatedAt = DateTime.UtcNow;

        tasks.Remove(tasks.First(t => t.Id == id));
        tasks.Add(taskToUpdate);

        var json = JsonSerializer.Serialize(tasks);
        File.WriteAllText(_filePath, json);
    }
}
