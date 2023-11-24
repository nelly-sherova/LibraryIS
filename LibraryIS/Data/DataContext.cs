using LibraryIS.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryIS.Data
{
	public class DataContext : DbContext
	{
		public DataContext(DbContextOptions<DataContext> options) : base(options)
		{

		}
		public DbSet<Author> Authors { get; set; }
		public DbSet<Category> Categories { get; set; }
		public DbSet<BookAuthor> BookAuthor { get; set; }
		public DbSet<BookUser> BookUser { get; set; }
		public DbSet<Role> Roles { get; set; }
		public DbSet<User> Users { get; set; }
		public DbSet<Book> Books { get; set; }
		public DbSet<BookCategory> BookCategory { get; set; }



		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<BookAuthor>()
				.HasKey(ba => new { ba.BookId, ba.AuthorId });
			modelBuilder.Entity<BookAuthor>()
				.HasOne(b => b.Book)
				.WithMany(ba => ba.BookAuthor)
				.HasForeignKey(b => b.BookId);
			modelBuilder.Entity<BookAuthor>()
				.HasOne(a => a.Author)
				.WithMany(ba => ba.BookAuthor)
				.HasForeignKey(a => a.AuthorId);



			modelBuilder.Entity<BookUser>()
				.HasKey(bu => new { bu.BookId, bu.UserId });
			modelBuilder.Entity<BookUser>()
				.HasOne(b => b.Book)
				.WithMany(bu => bu.BookUsers)
				.HasForeignKey(b => b.BookId);
			modelBuilder.Entity<BookUser>()
				.HasOne(u => u.User)
				.WithMany(bu => bu.BookUsers)
				.HasForeignKey(u => u.UserId);


			modelBuilder.Entity<BookCategory>()
				.HasKey(bc => new { bc.BookId, bc.CategoryId });
			modelBuilder.Entity<BookCategory>()
				.HasOne(b => b.Book)
				.WithMany(bc => bc.BookCategory)
				.HasForeignKey(b => b.BookId);
			modelBuilder.Entity<BookCategory>()
				.HasOne(c => c.Category)
				.WithMany(bc => bc.BookCategories)
				.HasForeignKey(c => c.CategoryId);
		}
	}
}
		