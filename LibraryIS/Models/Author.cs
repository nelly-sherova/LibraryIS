namespace LibraryIS.Models
{
	public class Author
	{
		public int Id { get; set; }	
		public string? FullName { get; set; }	
		public string? Description { get; set; }	
		public ICollection<BookAuthor>? BookAuthor { get; set; }	

	}
}
