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
            //if(!context.Books.Any())
            //{
            //	return BadRequest();
            //}
            //var books = context.Books.ToList();

            //var bookAuthors = context.BookAuthor.ToList();
            //List<Author> author
            //s = new List<Author>();
            //return View(books);
            if (!context.Books.Any())
            {
                return BadRequest();
            }
            var books = context.Books
                .Where(b => b.Visible == true)
                .GroupBy(b => new { b.Collection, b.Name, b.Publication, b.Language })
                .Select(group => new BookIndexViewModel
                {
                    Collection = group.Key.Collection,
                    Name = group.Key.Name,
                    BookCount = group.Count(),
                    Publication = group.Key.Publication,
                    Language = group.Key.Language
                })
            .ToList();
            if (books == null)
            {
                return NotFound();
            }
          
            return View(books);
        }
        public IActionResult Basket()
        {
            return View();
        }
        public IActionResult BookBasket()
        {

            var books = context.Books.Where(b=> b.Visible == false).ToList();
            if(books == null)
            {
                return NotFound();
            }
            return View(books);
        }
        public IActionResult DetailsCollection(int Collection)
        {
            var books = context.Books.Where(b => b.Visible == true && b.Collection == Collection).ToList();
            if(books == null)
                return NotFound();

            foreach (var item in books)
            {
                item.BookAuthor = context.BookAuthor.Where(bu => bu.BookId == item.Id).ToList();
                foreach(var author in item.BookAuthor)
                {
                    author.Author = context.Authors.FirstOrDefault(a => a.Id == author.AuthorId);
                }
            }
            foreach(var item in books)
            {
                item.BookCategory = context.BookCategory.Where(bc=> bc.BookId == item.Id).ToList();
                foreach(var category in item.BookCategory)
                {
                    category.Category = context.Categories.FirstOrDefault(c => c.Id == category.CategoryId);
                }
            }
            return View(books);
        }
        // GET: BookController/Details/5
        public ActionResult Details(int id)
        {
            var book = context.Books.Where(b => b.Id == id).FirstOrDefault();
            if(book == null) return NotFound();
            book.BookAuthor = context.BookAuthor.Where(ba => ba.BookId == id).ToList();
            foreach (var author in book.BookAuthor)
            {
                author.Author = context.Authors.FirstOrDefault(a => a.Id == author.AuthorId);
            }
            book.BookCategory = context.BookCategory.Where(ba => ba.BookId == id).ToList();
            foreach (var category in book.BookCategory)
            {
                category.Category = context.Categories.FirstOrDefault(a => a.Id == category.CategoryId);
            }
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
