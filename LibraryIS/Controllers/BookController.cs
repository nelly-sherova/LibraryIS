using LibraryIS.Data;
using LibraryIS.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using System.Linq;
using System.Net;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;


namespace LibraryIS.Controllers
{
	public class BookController : Controller
	{

		private DataContext context;
        IWebHostEnvironment _appEnvironment;

        public BookController(DataContext context, IWebHostEnvironment _appEnvironment)
		{
			this.context = context;
            this._appEnvironment = _appEnvironment;
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
        public ActionResult IndexUser()
        {
            if (!context.Books.Any())
            {
                return BadRequest();
            }

            var books = context.Books
                .Where(b => b.Visible == true)
                .GroupBy(b => new { b.Name })
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
        public IActionResult DetailsCollectionUser(string name)
        {
            var books = context.Books.Where(b => b.Visible == true && b.Name == name).ToList();
            if (books == null)
                return NotFound();

            foreach (var item in books)
            {
                item.BookAuthor = context.BookAuthor.Where(bu => bu.BookId == item.Id).ToList();
                foreach (var author in item.BookAuthor)
                {
                    author.Author = context.Authors.FirstOrDefault(a => a.Id == author.AuthorId);
                }
            }
            foreach (var item in books)
            {
                item.BookCategory = context.BookCategory.Where(bc => bc.BookId == item.Id).ToList();
                foreach (var category in item.BookCategory)
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
        public ActionResult DetailsUser(int id)
        {
            var book = context.Books.Where(b => b.Id == id).FirstOrDefault();
            if (book == null) return NotFound();
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
        public async Task<IActionResult> Create(AddBookViewModel bookView)
        {
            try
            {
                
                Book book = new Book();
                if (bookView.FileStream != null)
                {
                    string path = "/img/" + bookView.FileStream.FileName;
                    using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                    {
                        await bookView.FileStream.CopyToAsync(fileStream);
                    }
                    book.PicUrl = path;
                }
             
              if(bookView.FileStream != null)
                          
                      

                        book.Name = bookView.Name;
                        book.State = bookView.State;
                        book.CountOfPages = bookView.CountOfPages;
                        book.Language = bookView.Language;
                        book.Description = bookView.Description;
                        book.PublicationCity = bookView.PublicationCity;
                        book.PublicationDate = bookView.PublicationDate;
                        book.Publication = bookView.Publication;
                        book.Binding = bookView.Binding;
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
        public ActionResult AddUser(BookUserVM bookUserVM)
        {
            BookUser bookUser = new BookUser();
            try
            {
               
                bookUser.UserId = bookUserVM.UserId;
                bookUser.BookId = bookUserVM.BookId;

                bookUser.User = context.Users.Where(u => u.Id == bookUser.UserId).FirstOrDefault();
                bookUser.Book = context.Books.Where(b => b.Id == bookUser.BookId).FirstOrDefault();
                bookUser.Book.IssueDate = DateTime.Now;
                bookUser.Book.RetunDate = bookUserVM.ReturnDate;
               
                bookUser.Book.IsFramed = true;
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
            AddBookViewModel bookwm = new AddBookViewModel();
            bookwm.Id = id;
            bookwm.Name = book.Name;
            bookwm.Publication = book.Publication;
            bookwm.Description = book.Description;
            bookwm.PublicationCity = book.PublicationCity;
            bookwm.PublicationDate = book.PublicationDate;
            bookwm.Language = book.Language;
            bookwm.Binding = book.Binding;
            bookwm.CountOfPages = book.CountOfPages;
            bookwm.State =  book.State;
            ViewBag.Book = book;
            ViewBag.Id = book.Id;
			return View(bookwm);
		}

		// POST: BookController/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(AddBookViewModel bookwm)
		{
			try
			{
                var data = context.Books.Where(b => b.Id == bookwm.Id).FirstOrDefault();
                var file = bookwm.FileStream;
                var name = file.FileName;
                var path = "/img/" + bookwm.FileStream.FileName;
                if (bookwm.FileStream != null)
                {
                    using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                    {
                        await bookwm.FileStream.CopyToAsync(fileStream);
                    }




                    data.PicUrl = path;
                }
               
				if(data != null)
				{
					data.Name = bookwm.Name;
					data.Description = bookwm.Description;
					data.PublicationCity = bookwm.PublicationCity;	
					data.Publication = bookwm.Publication;
					data.Language = bookwm.Language;
                    data.PublicationDate = bookwm.PublicationDate;
                    data.CountOfPages = bookwm.CountOfPages;
                    data.Binding = bookwm.Binding;
                    data.State = bookwm.State;
				}
                context.Update(data);
                context.SaveChanges();
                return RedirectToAction(nameof(DetailsCollection), new RouteValueDictionary(new { name = bookwm.Name }));
            }
			catch
			{
				return View();
			}
		}




        public ActionResult DeleteForBasket(int id)
        {
            var book = context.Books.Where(b => b.Id == id).FirstOrDefault();
            book.Visible = false;
            context.Update(book);
            context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        public ActionResult Restore(int id)
        {
            var book = context.Books.Where(b => b.Id == id).FirstOrDefault();
            book.Visible = true;
            context.Update(book);
            context.SaveChanges();
            return RedirectToAction(nameof(DetailsCollection), new RouteValueDictionary(new {name = book.Name}));
        }



       
		
		public ActionResult Delete(int id)
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

        public ActionResult Overdue()
        {
            try
            {
                var books = context.Books.Where(b => b.RetunDate <= DateTime.UtcNow 
                && b.Visible == true
                && b.RetunDate >= new DateTime(2000,01,01)
                && b.IsFramed == true
                ).ToList();

                foreach (var item in books)
                {
                    item.BookAuthor = context.BookAuthor.Where(bu => bu.BookId == item.Id).ToList();

                    foreach (var author in item.BookAuthor)
                    {
                        author.Author = context.Authors.FirstOrDefault(a => a.Id == author.AuthorId);
                    }
                }
                foreach (var item in books)
                {
                    item.BookCategory = context.BookCategory.Where(bc => bc.BookId == item.Id).ToList();
                    foreach (var category in item.BookCategory)
                    {
                        category.Category = context.Categories.FirstOrDefault(c => c.Id == category.CategoryId);
                    }
                }
                foreach(var item in books)
                {
                    item.BookUsers = context.BookUser.Where(bu => bu.BookId == item.Id).ToList();
                    foreach(var user in item.BookUsers)
                    {
                        user.User = context.Users.FirstOrDefault(u => u.Id ==  user.UserId);

                    }    
                }
               
                return View(books);
            }
            catch
            {
                return View();
            }
          
        }

     
        public ActionResult Return(int bookid)
        {
            try
            {
                var book = context.Books.Where(b => b.Id == bookid).FirstOrDefault();
               
                var bookusers = context.BookUser.Where(bu => bu.BookId == book.Id).ToList();
                book.IsFramed = false;
                context.Books.Update(book);
                context.RemoveRange(bookusers);
                context.SaveChanges();
                return RedirectToAction("Overdue");   
            }
            catch
            {
                return View();
            }
            
            
        }



    }
}
