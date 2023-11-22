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
			return View();
		}

		// POST: BookController/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(IFormCollection collection)
		{
			try
			{
				return RedirectToAction(nameof(Index));
			}
			catch
			{
				return View();
			}
		}

		// GET: BookController/Edit/5
		public ActionResult Edit(int id)
		{
			return View();
		}

		// POST: BookController/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(int id, IFormCollection collection)
		{
			try
			{
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
				return RedirectToAction(nameof(Index));
			}
			catch
			{
				return View();
			}
		}
	}
}
