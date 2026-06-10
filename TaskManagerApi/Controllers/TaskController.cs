/*
Refernces:
https://learn.microsoft.com/en-us/dotnet/standard/data/sqlite
I used this refernce to understand the microsoft sqlite package and use its function to interact with the db which is stored locally
*/

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;

namespace TaskManagerApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class TasksController : ControllerBase
	{
		// connection path to connect to local db sqlite
		private readonly string _connectionString;
		// constractor of our main Task controller class that intilize the connection if we triggered the object of the controller or any function under controller class
		public TasksController(IConfiguration configuration)
		{
			_connectionString = configuration.GetConnectionString("SqliteConnection")
								?? "Data Source=Tasks.db";

			//build the table if it does not exist
			InitializeDatabase();
		}
		private void InitializeDatabase()
		{
			// we wraped the function of connect to log or print any error for debugging
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
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex + "something went wrong");
			}

		}
		[HttpGet]
		public async Task<ActionResult<IEnumerable<TaskItem>>> GetAllTasks()
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

			return Ok(tasks); // returns 200 ok status
		}
		// create New task endpoint 
		[HttpGet("{id}")]
		public async Task<ActionResult<IEnumerable<TaskItem>>> GetTaskById(int id)
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
				return Ok(task);
			}
			return NotFound();
		}
		[HttpPost]
		public async Task<ActionResult<IEnumerable<TaskItem>>> CreateTask([FromBody] TaskItem formTask)
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
			
			return CreatedAtAction(nameof(GetTaskById) , new {id = formTask.Id} ,formTask);
		}
		// how we check the input 
		// how we create logger and error handling dependency
		// how to send db messages for invalid input for db insertaion
	}
}
