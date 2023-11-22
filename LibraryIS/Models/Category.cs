namespace LibraryIS.Models
{
	public class Category
	{
		public int Id { get; set; }	
		public string Name { get; set; }	
		public string? Description { get; set; }	
		public IEnumerable<BookCategory> BookCategories { get; set; }


	}
}
