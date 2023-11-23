using LibraryIS.Data;
using LibraryIS.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryIS.Controllers
{
	public class BookController : Controller
	{

		private DataContext context; 
		public BookController(DataContext context)
		{
			this.context = context;	
		}
		// GET: BookController
		public ActionResult Index()
		{
			if(!context.Books.Any())
			{
				return BadRequest();
			}
			var books = context.Books.ToList();
				
			
			
			return View(books);
		}

		// GET: BookController/Details/5
		public ActionResult Details(int id)
		{
			var book = context.Books.Where(b => b.Id == id).FirstOrDefault();
			return View(book);
		}

		// GET: BookController/Create
		public ActionResult Create()
		{

            // Получить доступных авторов и категории
			 var authors = context.Authors.ToList();
            var categories = context.Categories.ToList();

            // Создать новый экземпляр класса BookViewModel
            var bookViewModel = new BookViewModel
            {
                Authors = authors,
                Categories = categories
            };

            return View(bookViewModel);
        }


		// POST: BookController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BookViewModel bookViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(bookViewModel);
            }

            // Получить выбранные идентификаторы авторов
            var selectedAuthorIds = bookViewModel.Authors.Select(author => author.Id).ToList();

            // Получить выбранные идентификаторы категорий
            var selectedCategoryIds = bookViewModel.Categories.Select(category => category.Id).ToList();

            // Создать новый экземпляр книги
            var book = new Book
            {
                Name = bookViewModel.Book.Name,
                Description = bookViewModel.Book.Description,
                PublicationDate = bookViewModel.Book.PublicationDate,
                Publication = bookViewModel.Book.Publication,
                PublicationCity = bookViewModel.Book.PublicationCity,
                Language = bookViewModel.Book.Language
            };

            // Добавить выбранных авторов в коллекцию BookAuthor книги
            foreach (var authorId in selectedAuthorIds)
            {
                var author = context.Authors.Find(authorId);
                book.BookAuthor.Add(new BookAuthor { Author = author, Book = book });
            }

            // Добавить выбранные категории в коллекцию BookCategory книги
            foreach (var categoryId in selectedCategoryIds)
            {
                var category = context.Categories.Find(categoryId);
                book.BookCategory.Add(new BookCategory { Category = category, Book = book });
            }

            // Добавить новую книгу в базу данных
            context.Books.Add(book);
            context.SaveChanges();

            return RedirectToAction("Index");
        }

        // GET: BookController/Edit/5
        public ActionResult Edit(int id)
		{
			var book = context.Books.Where(b=> b.Id == id).FirstOrDefault();
			if (book == null)
				return BadRequest();
			return View(book);
		}

		// POST: BookController/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(int id, Book book)
		{
			try
			{
				var data = context.Books.Where(b => b.Id == id).FirstOrDefault();
				if(data != null)
				{
					data.Name = book.Name;
					data.Description = book.Description;
					data.PublicationCity = book.PublicationCity;	
					data.Publication = book.Publication;
					data.Language = book.Language;
					context.SaveChanges();
				}
				return RedirectToAction(nameof(Index));
			}
			catch
			{
				return View();
			}
		}

		// GET: BookController/Delete/5
		public ActionResult Delete(int id)
		{
			return View();
		}

		// POST: BookController/Delete/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Delete(int id, IFormCollection collection)
		{
			try
			{
				var data = context.Books.Where(b => b.Id == id).FirstOrDefault();
				context.Remove(data);
				context.SaveChanges();
				return RedirectToAction(nameof(Index));
			}
			catch
			{
				return View();
			}
		}
	}
}
