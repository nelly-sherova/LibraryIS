using LibraryIS.Data;
using LibraryIS.Models;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;

namespace LibraryIS.Controllers
{
    public class CategoryController : Controller
    {
        private readonly DataContext context;
        public CategoryController(DataContext context)
        {
            this.context = context; 
        }
        // GET: CategoryController
        public ActionResult Index()
        {
            var categories = context.Categories.Where(c => c.Visible == true).ToList();
            if(!categories.Any())
            {
                return BadRequest();  
            }
            return View(categories);
        }

        // GET: CategoryController/Details/5
        public ActionResult Details(int id)
        {
            var category = context.Categories.FirstOrDefault(c => c.Id == id);
            if(category == null)
            {
                return NotFound(id);
            }
            return View(category);
        }

        // GET: CategoryController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CategoryController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Category category)
        {
           
            try
            {
                category.Visible = true;
                context.Categories.Add(category);
                context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CategoryController/Edit/5
        public ActionResult Edit(int id)
        {
            var category = context.Categories.Where(c => c.Id == id).FirstOrDefault();
            if(category == null)
            {
                return BadRequest();
            }
            ViewBag.CategoryId = category.Id;
            return View(category);
           
          
           
        }

        // POST: CategoryController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Category category)
        {
            try
            {
                var data = context.Categories.FirstOrDefault(c => c.Id == category.Id);
                if(data == null)
                {
                    return BadRequest();
                }
                data.Name = category.Name;
                data.Description = category.Description;
                context.Update(data);
                context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        
        public ActionResult CategoryBasket() 
        {
            var categories = context.Categories.Where(c => c.Visible == false).ToList();
            return View(categories);
        }
        public ActionResult DeleteBasket(int id)
        {
            var category = context.Categories.Where(c => c.Id == id).FirstOrDefault();
            category.Visible = false;
            context.Categories.Update(category);
            context.SaveChanges();
            return RedirectToAction(nameof(CategoryBasket));
        }
        public ActionResult Restore(int id)
        {
            var category = context.Categories.Where(c => c.Id == id).FirstOrDefault();
            category.Visible = true;
            context.Categories.Update(category);
            context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }


      
        public ActionResult Delete(int id)
        {
            try
            {
                var data = context.Books.Where(b => b.Id == id).FirstOrDefault();
                context.Books.Remove(data);
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
