using LibraryIS.Models;

namespace LibraryIS.Data
{
	public class Seed
	{
		private DataContext context; 

		public Seed(DataContext context)
		{
			this.context = context;	
		}

		public void SeedData()
		{
			if(!context.Books.Any()) 
			{
				
					var bookCategory = new BookCategory
					{
						Book = new Book
						{
							Name = "7 навыков высокоэффективных людей",
							Description = "Мощные инструменты развития личности ",
							PublicationDate = new DateTime(2019, 01, 01),
							Publication = "Альпина Паблишер",
							PublicationCity = "Москва",
							Language = "Русский",
							BookAuthor = new List<BookAuthor>
							{
								new BookAuthor
								{
									Author = new Author
									{
										FullName = "Стивен Кови",
										Description = "Современный писатель"
									}
								}
							},
							
							BookUsers = new List<BookUser>
							{
								new BookUser
								{
									User = new User
									{
										FirstName = "Sherova",
										LastName = "Nilufar",
										MiddleName = "Umedjonovna",
										Email = "nilufar@mail.ru",
										Password = "00000000",
										DateOfBirth = new DateTime(2000,07,08),
										PhoneNumber = "1234567890",
										Role = new Role
										{
											Name = "Administrator"
										}
									}
								}
							}
							
						},
						Category = new Category
						{
							Name = "Зарубежная психология"
						}
					};
				context.BookCategory.AddRange(bookCategory);
				context.SaveChanges();


			}
		}

	}
}
