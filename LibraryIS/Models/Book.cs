namespace LibraryIS.Models
{
	public class Book
	{
		public int Id { get; set; }
		public string? Name { get; set; }
		public string? Description { get; set; }
		public DateTime PublicationDate { get; set; }
		public string? Publication { get; set; }
		public string? PublicationCity { get; set; }
		public string? Language { get; set; }
		public ICollection<BookAuthor>? BookAuthor { get; set; }
		public ICollection<BookCategory>? BookCategory { get; set; }	
		public ICollection<BookUser>? BookUsers { get; set; }	

		

	}
}
