# Task Tracker

![.NET Version](https://img.shields.io/badge/.NET-9-blue)

Task Tracker is a .NET 9 console application designed to help you track and manage your tasks efficiently. This project is based on the [Task Tracker CLI](https://roadmap.sh/projects/task-tracker) from [roadmap.sh](https://roadmap.sh).

## Features

- **Add, Update, and Delete Tasks**: Manage your task list with ease.
- **Task Status Management**: Mark tasks as "in-progress" or "done."
- **List Filtering**: View tasks based on their status (`done`, `todo`, or `in-progress`).
- **Comprehensive Task Overview**: Quickly list all tasks for better organization.

## Usage

After building the project, run the application from the directory containing the compiled executable (`TaskCliCsharp.exe`) and provide commands as arguments. Available commands include:

- `add [description]`: Adds a new task with the given description.
  - **Example**: `./TaskCliCsharp.exe add Buy groceries`
- `update [id] [description]`: Updates a task's description by its ID.
  - **Example**: `./TaskCliCsharp.exe update 2 Finish report`
- `delete [id]`: Deletes the task with the specified ID.
  - **Example**: `./TaskCliCsharp.exe delete 3`
- `mark-in-progress [id]`: Marks a task as "in-progress" by its ID.
  - **Example**: `./TaskCliCsharp.exe mark-in-progress 1`
- `mark-done [id]`: Marks a task as "done" by its ID.
  - **Example**: `./TaskCliCsharp.exe mark-done 4`
- `list`: Lists all tasks.
  - **Example**: `./TaskCliCsharp.exe list`
- `list [status]`: Lists tasks filtered by status (`done`, `todo`, `in-progress`).
  - **Example**: `./TaskCliCsharp.exe list done`

### Notes:
- The `TaskCliCsharp.exe` file will be generated in the `bin/Debug/net9.0/` directory after building the project.
- For Linux/macOS users, the command may simply be `./TaskCliCsharp` (omit the `.exe`).
- Ensure the terminal is in the same directory as the executable or provide the full path to the file.

## Installation

Follow these steps to set up and run the project on your local machine:

### Prerequisites

Make sure you have the following installed on your system:
- [Git](https://git-scm.com/downloads)
- [.NET 9 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/9.0)

### Steps

1. Clone the repository:

    Clone the project repository and navigate to the application directory:

    ```bash
    git clone https://github.com/your-username/task-tracker.git
    cd TaskCliCsharpApp/TaskCliCSharp
    ```

2. Restore Dependencies

    Use the dotnet CLI to restore dependencies:

    ```bash
    dotnet restore
    ```

3. Build the Project

    Compile the application to ensure everything is set up correctly:

    ```bash
    dotnet build
    ```

4. Run the Application

    Start the application using the dotnet CLI:

    ```bash
    dotnet run
    ```

5. (Optional) Run the Tests

    Verify that all tests pass:

    - Navigate to the root directory:

    ```bash
    cd TaskCliCsharpApp
    ```

    - Run the tests:

    ```bash
    dotnet test
    ```

## Contributions

Contributions are welcome! This project is a learning exercise, but your feedback, suggestions, and enhancements can make it better. Feel free to open an issue or submit a pull request.
