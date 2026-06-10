using System.ComponentModel.DataAnnotations;

namespace TaskManagerApi
{
	public class TaskItem
	{
		public int Id { get; set; }

		[Required(ErrorMessage = "The Title is too Long ,Please make it Shorter !!!")]
		public string Title { get; set; }

		public string? Description { get; set; }
		public bool IsCompleted { get; set; } = false;
		public DateTime? CreatedAt {set; get;} = DateTime.Now;

	}
}
