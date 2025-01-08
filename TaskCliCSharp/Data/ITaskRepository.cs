namespace TaskCliCSharp.Data;

public interface ITaskRepository
{
    int AddTask(string description);

    List<Entities.Task> GetTasks(string? status = null);
    bool TaskExists(int id);

    void UpdateTaskStatus(int id, string status);
    void UpdateTaskDescription(int id, string description);

    void DeleteTask(int id);
}
