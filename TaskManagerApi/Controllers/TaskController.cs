/*
Refernces:
https://learn.microsoft.com/en-us/dotnet/standard/data/sqlite
I used this refernce to understand the microsoft sqlite package and use its function to interact with the db which is stored locally
*/
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Logging;

namespace TaskManagerApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class TasksController : ControllerBase
	{
		// connection path to connect to local db sqlite
		private readonly string _connectionString;

		private readonly ILogger<TasksController> _logger;

		// constractor of our main Task controller class that intilize the connection if we triggered the object of the controller or any function under controller class
		public TasksController(IConfiguration configuration, ILogger<TasksController> logger)
		{
			_connectionString = configuration.GetConnectionString("SqliteConnection")
								?? "Data Source=Tasks.db";
			_logger = logger;
			//build the table if it does not exist
			InitializeDatabase();
		}
		private void InitializeDatabase()
		{
			// we wraped the function of connect to log or print any error for debugging
			_logger.LogInformation("Starting Database Connection !!");
			try
			{
				// using microsoft package to create the db 
				using var connection = new SqliteConnection(_connectionString);
				connection.Open();
				var command = connection.CreateCommand();
				command.CommandText = @"
                CREATE TABLE IF NOT EXISTS Tasks (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Title VARCHAR(100) NOT NULL,
  					Description TEXT NULL,
  					IsCompleted BOOLEAN NOT NULL DEFAULT FALSE,
  					CreatedAt TEXT  NOT NULL DEFAULT (datetime('now'))
                );";
				command.ExecuteNonQuery();
				_logger.LogInformation("Datebase Connected Successfully !!");
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error Creating or Starting Datebase Connection");
			}

		}
		// fetch all tasks 
		[HttpGet]
		public async Task<ActionResult<IEnumerable<TaskItem>>> GetAllTasks()
		{
			try
			{
				var tasks = new List<TaskItem>();
				using var connection = new SqliteConnection(_connectionString);
				await connection.OpenAsync();
				var command = connection.CreateCommand();
				command.CommandText = "SELECT * FROM Tasks";

				using var reader = command.ExecuteReader();
				while (await reader.ReadAsync())
				{
					tasks.Add(new TaskItem
					{
						Id = reader.GetInt32(0),
						Title = reader.GetString(1),
						Description = reader.IsDBNull(2) ? null : reader.GetString(2),
						IsCompleted = reader.GetBoolean(3),
						CreatedAt = reader.GetDateTime(4)
					});
				}
				_logger.LogInformation("All Tasks fetched with success !!");
				return Ok(tasks); // returns 200 ok status

			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error Fetching  Tasks from  database !!");
				return StatusCode(500, "internal server error");
			}

		}
		// create New task endpoint 
		[HttpGet("{id}")]
		public async Task<ActionResult<IEnumerable<TaskItem>>> GetTaskById(int id)
		{
			try
			{
				var task = new TaskItem();
				using var connection = new SqliteConnection(_connectionString);
				await connection.OpenAsync();
				var command = connection.CreateCommand();
				command.CommandText = "SELECT * FROM Tasks WHERE Id = @id";
				command.Parameters.AddWithValue("@id", id);
				using var reader = command.ExecuteReader();
				if (await reader.ReadAsync())
				{
					task.Id = reader.GetInt32(0);
					task.Title = reader.GetString(1);
					task.Description = reader.IsDBNull(2) ? null : reader.GetString(2);
					task.IsCompleted = reader.GetBoolean(3);
					task.CreatedAt = reader.GetDateTime(4);

					_logger.LogInformation($"task found Task : {task.Title}");

					return Ok(task);
				}
				return NotFound(new { message = $"Task with ID {id} was not found." });
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error please check your database");
				return StatusCode(500, "internal server error");
			}

		}
		[HttpPost]
		public async Task<ActionResult<IEnumerable<TaskItem>>> CreateTask([FromBody] TaskItem formTask)
		{
			try
			{
				// connect 
				using var connection = new SqliteConnection(_connectionString);
				await connection.OpenAsync();
				// command
				var command = connection.CreateCommand();
				command.CommandText = "INSERT INTO Tasks (Title, Description, IsCompleted) VALUES (@title ,@Description, @complete ); SELECT last_insert_rowid();";
				command.Parameters.AddWithValue("@title", formTask.Title);
				command.Parameters.AddWithValue("@Description", (object?)formTask.Description ?? DBNull.Value);
				command.Parameters.AddWithValue("@complete", formTask.IsCompleted);
				// run command 
				var newId = Convert.ToInt32(await command.ExecuteScalarAsync());
				formTask.Id = newId;
				_logger.LogInformation($"Task Created With Id : {newId}");
				return CreatedAtAction(nameof(GetTaskById), new { id = formTask.Id }, formTask);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error please check your database");
				return StatusCode(500, "internal server error");
			}

		}
		// Update endpoint 
		[HttpPut("{id}")]
		public async Task<ActionResult<IEnumerable<TaskItem>>> UpdateTask(int id, [FromBody] TaskItem task)
		{
			try
			{
				// connect 
				using var connection = new SqliteConnection(_connectionString);
				await connection.OpenAsync();
				// important idea get the orignal task before update to make sure nothing happpes to the origianl task
				// write command 
				var command = connection.CreateCommand();
				command.CommandText = "UPDATE Tasks SET Title = @title , Description = @desc , IsCompleted = @comp WHERE Id = @id ;";
				command.Parameters.AddWithValue("@title", task.Title);
				command.Parameters.AddWithValue("@desc", task.Description);
				command.Parameters.AddWithValue("@comp", task.IsCompleted);
				command.Parameters.AddWithValue("@id", id);
				// execute
				int rowsAffected = await command.ExecuteNonQueryAsync();
				if (rowsAffected == 0)
				{
					_logger.LogError("No Record found with this task id");
					return NotFound(new { message = $"Task with ID {id} was not found." });
				}
				// return new values
				_logger.LogInformation($"Task Updated With Id : {id}");
				return Ok(new { message = "Record updated successfully." });
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error please check your database");
				return StatusCode(500, "internal server error");
			}

		}
		[HttpDelete("{id}")]
		public async Task<ActionResult<IEnumerable<TaskItem>>> DeleteTask(int id)
		{
			try
			{   // connect 
				using var connection = new SqliteConnection(_connectionString);
				await connection.OpenAsync();
				// write command
				var command = connection.CreateCommand();
				command.CommandText = "DELETE FROM Tasks WHERE Id = @id";
				command.Parameters.AddWithValue("@id", id);
				// execute command 
				int rowEffected = await command.ExecuteNonQueryAsync();
				if (rowEffected == 0)
				{
					_logger.LogError("No Record found with this task id");
					return NotFound(new { message = $"Task with ID {id} was not found." });
				}
				// return response
				_logger.LogInformation($"Task Deleted With Id : {id}");
				return Ok(new { message = "Record Deleted successfully." });

			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error please check your database");
				return StatusCode(500, "internal server error");
			}
		}
	}
}
