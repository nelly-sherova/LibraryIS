﻿namespace LibraryIS.Models
{
	public class Role
	{
		public int Id { get; set; }	
		public string Name { get; set; }	
		public string? Description { get; set; }
		public bool? Visible { get; set; }	
		
		public ICollection<User> Users { get; set; }

	}
}
