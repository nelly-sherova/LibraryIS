namespace LibraryIS.Models
{
	public class User
	{
		public int Id { get; set; }
		public string? FirstName { get; set; }
		public string? LastName { get; set; } 
		public string? MiddleName { get; set; } 
		public string? Email { get; set; }		
		public string? Password { get; set; }
		public DateTime DateOfBirth { get; set; }
		public string? PhoneNumber { get; set; }
		public string Sex { get; set; }
		public string City { get; set; }
		public string Region { get; set; }
		public string Country { get; set; }
		public bool Visible { get; set; }
		public string PicUrl { get; set; }
		
		public int RoleId { get; set; }
		public Role? Role { get; set; }
	

		public ICollection<BookUser>? BookUsers { get; set; }	



	}
}
