/*
Refernces:
https://learn.microsoft.com/en-us/dotnet/standard/data/sqlite
I used this refernce to understand the microsoft sqlite package and use its function to interact with the db which is stored locally
*/

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;

// commit the first commit with the controller only 
// create the correct db table with the correct setting
// push the commit a new repo

namespace TaskManagerApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class TaskController : ControllerBase
	{
		// connection path to connect to local db sqlite
		private readonly string _connectionString;
		// constractor of our main Task controller that intilize the connection if we triggered the object of the controller or any function under controller class
		public TaskController(IConfiguration configuration)
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
		// create fetch all tasks endpoint
		// create New task endpoint 
		// how we write into the db 
		// how we check the input 
		// how we spacify the http response code and error handling 
		// how we create logger and error handling dependency
		// how we access or create data in json 
		// how to send db messages for invalid input for db insertaion
	}
}
