using LibraryIS.Data;
using LibraryIS.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

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
            
            if (!context.Books.Any())
            {
                return BadRequest();
            }
 
            var books = context.Books
                .Where(b => b.Visible == true)
                .GroupBy(b => new { b.Name})
                .Select(group => new BookIndexViewModel
                {
                   
                    Name = group.Key.Name,
                    BookCount = group.Count(),
                  
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
        public IActionResult DetailsCollection(string name)
        {
            var books = context.Books.Where(b => b.Visible == true && b.Name == name).ToList();
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
            
            return View();
        }


		// POST: BookController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Book book)
        {
            try
            {
                context.Books.Add(book);
                context.SaveChanges();
                return RedirectToAction(nameof(AddCategories), new RouteValueDictionary(new { bookid = book.Id }));
            }
            catch (Exception ex)
            {

            }
            return View();
           
        }



        public ActionResult AddCategories(int bookid)
        {

            var book = context.Books.Where(b => b.Id == bookid).FirstOrDefault();
            
            var categories = context.Categories.ToList();
            ViewBag.BookId = bookid;
            ViewBag.Book = book;
            ViewBag.Categories = categories;    
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddCategories(BookCategory bookCategory)
        {
            try
            {
                bookCategory.Category = context.Categories.Where(Category => Category.Id == bookCategory.CategoryId).FirstOrDefault();
                bookCategory.Book = context.Books.Where(book => book.Id == bookCategory.BookId).FirstOrDefault();


                context.BookCategory.Add(bookCategory);
                context.SaveChanges();

                return RedirectToAction(nameof(AddAuthors), new RouteValueDictionary(new { bookid = bookCategory.BookId }));
            }
            catch(Exception ex)
            {

            }
            return View();
        }
        public ActionResult AddAuthors(int bookid)
        {
            var book = context.Books.Where(b=> b.Id == bookid).FirstOrDefault();
            ViewBag.BookId = bookid;
            ViewBag.Book = book;
            var authors = context.Authors.ToList();
            ViewBag.Authors = authors;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddAuthors(BookAuthor bookAuthor)
        {
            try
            {
                bookAuthor.Author = context.Authors.Where(a => a.Id == bookAuthor.AuthorId).FirstOrDefault();
                bookAuthor.Book = context.Books.Where(b => b.Id == bookAuthor.BookId).FirstOrDefault();
                bookAuthor.Book.Visible = true;
                context.BookAuthor.Add(bookAuthor);
                context.SaveChanges();

            }
            catch (Exception ex)
            {

            }
           
            return RedirectToAction(nameof(Index));
        }
        public ActionResult AddUser(int bookid)
        {
            try
            {
                var book = context.Books.Where(b => b.Id == bookid).FirstOrDefault();
                ViewBag.BookId = book.Id;
                ViewBag.Book = book;
                var users = context.Users.ToList();
                ViewBag.Users = users; 
            }
            catch(Exception ex)
            {

            }

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddUser(BookUser bookUser)
        {
            try
            {
                bookUser.User = context.Users.Where(u => u.Id == bookUser.UserId).FirstOrDefault();
                bookUser.Book = context.Books.Where(b => b.Id == bookUser.BookId).FirstOrDefault();
                bookUser.Book.IssueDate = DateTime.Now;
                bookUser.Book.RetunDate = bookUser.Book.IssueDate.AddDays(30);
                context.BookUser.Add(bookUser);
                context.SaveChanges();
            }
            catch(Exception ex)
            {

            }
           
           // return RedirectToAction(nameof(UserController.Details), new RouteValueDictionary(bookUser.BookId));
           

            return RedirectToAction(nameof(UserController.Details), new RouteValueDictionary(new { id = bookUser.BookId }));// он возвращает book 



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
