using LibraryIS.Data;
using LibraryIS.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryIS.Controllers
{
    public class AuthorController : Controller
    {
        private readonly DataContext context; 
        public AuthorController(DataContext context) 
        {
            this.context = context;
        }

        // GET: AuthorController
        public ActionResult Index()
        {
            var authors = context.Authors.Where(a => a.Visible == true).ToList();
            if(!authors.Any())
                return NotFound();  
            return View(authors);
        }

        // GET: AuthorController/Details/5
        public ActionResult Details(int id)
        {
            var author = context.Authors.Where(a => a.Id == id).FirstOrDefault();
            if(author == null) return NotFound();
            return View(author);
        }

        // GET: AuthorController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AuthorController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Author author)
        {
           
            try
            {
                author.Visible = true;
                context.Authors.Add(author);

                context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }

        }

        // GET: AuthorController/Edit/5
        public ActionResult Edit(int id)
        {
            var author = context.Authors.Where(author => author.Id == id).FirstOrDefault();
            if(author == null) return NotFound();
            ViewBag.AuthorId = author.Id;
            return View(author);
        }

        // POST: AuthorController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( Author author)
        {

            try
            {
                var data = context.Authors.Where(a => a.Id == author.Id).FirstOrDefault();
                if (data != null)
                {
                    data.FullName = author.FullName;
                    data.Description = author.Description;
                    context.Update(data);
                    context.SaveChanges();
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    

        // GET: AuthorController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AuthorController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                var data = context.Books.Where(b => b.Id == id).FirstOrDefault();   
                if(data == null) return NotFound();
                context.Books.Remove(data);
                context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public ActionResult AuthorBasket()
        {
            var authors = context.Authors.Where(a => a.Visible == false).ToList();
            
            return View(authors);
        }
    }
}
