using System.ComponentModel.DataAnnotations;

namespace TaskManagerApi
{
	public class TaskItem
	{
		public int Id { get; set; }

		[Required(ErrorMessage = "Please Insert correct title and make it short !!!")]
		public string Title { get; set; }

		public string? Description { get; set; }
		public bool IsCompleted { get; set; } = false;
		public DateTime? CreatedAt {set; get;} = DateTime.Now;

	}
}
