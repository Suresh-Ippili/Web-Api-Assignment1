using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using TaskApi.DataAccess;
using TaskApi.Models;
using Task = TaskApi.Models.Task;

namespace TaskApi.DataAccess
{
    public class TaskRepository
    {
        private readonly string _connectionString;

        public TaskRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddTransient<TaskRepository>();
        }

        // Create a new Task
        public void AddTask(Task task)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand("INSERT INTO Task (TaskName, Description, Duration, AssignedBy) VALUES (@TaskName, @Description, @Duration, @AssignedBy)", connection);
                command.Parameters.AddWithValue("@TaskName", task.TaskName);
                command.Parameters.AddWithValue("@Description", task.Description);
                command.Parameters.AddWithValue("@Duration", task.Duration);
                command.Parameters.AddWithValue("@AssignedBy", task.AssignedBy);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        // Retrieve all Tasks
        public IEnumerable<Task> GetTasks()
        {
            List<Task> tasks = new List<Task>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand("SELECT * FROM Task", connection);
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Task task = new Task
                    {
                        TaskID = Convert.ToInt32(reader["TaskID"]),
                        TaskName = reader["TaskName"].ToString(),
                        Description = reader["Description"].ToString(),
                        Duration = Convert.ToInt32(reader["Duration"]),
                        AssignedBy = reader["AssignedBy"].ToString()
                    };
                    tasks.Add(task);
                }
            }

            return tasks;
        }

        // Retrieve a Task by ID
        public Task GetTaskById(int taskId)
        {
            Task task = null;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand("SELECT * FROM Task WHERE TaskID = @TaskID", connection);
                command.Parameters.AddWithValue("@TaskID", taskId);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    task = new Task
                    {
                        TaskID = Convert.ToInt32(reader["TaskID"]),
                        TaskName = reader["TaskName"].ToString(),
                        Description = reader["Description"].ToString(),
                        Duration = Convert.ToInt32(reader["Duration"]),
                        AssignedBy = reader["AssignedBy"].ToString()
                    };
                }
            }

            return task;
        }

        // Update an existing Task
        public void UpdateTask(Task task)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand("UPDATE Task SET TaskName = @TaskName, Description = @Description, Duration = @Duration, AssignedBy = @AssignedBy WHERE TaskID = @TaskID", connection);
                command.Parameters.AddWithValue("@TaskID", task.TaskID);
                command.Parameters.AddWithValue("@TaskName", task.TaskName);
                command.Parameters.AddWithValue("@Description", task.Description);
                command.Parameters.AddWithValue("@Duration", task.Duration);
                command.Parameters.AddWithValue("@AssignedBy", task.AssignedBy);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        // Delete a Task
        public void DeleteTask(int taskId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand("DELETE FROM Task WHERE TaskID = @TaskID", connection);
                command.Parameters.AddWithValue("@TaskID", taskId);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }
}
