using LibraryIS.Data;
using LibraryIS.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryIS.Controllers
{
    public class UserController : Controller
    {
        private DataContext context;

        public UserController(DataContext context) 
        {
            this.context = context; 
        }
        // GET: UserController
        public ActionResult Index()
        {
            var users = context.Users.ToList(); 
            if (!users.Any())
                return BadRequest();
            return View(users);
        }

        // GET: UserController/Details/5
        public ActionResult Details(int id)
        {
            var user = context.Users.Where(u => u.Id == id); 
            if (!user.Any())   
                return BadRequest();    
            return View(user);
        }

        // GET: UserController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UserController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(User user)
        {
            try
            {
                context.Users.Add(user);
                context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: UserController/Edit/5
        public ActionResult Edit(int id)
        {
            var user = context.Users.Where(u => u.Id == id).FirstOrDefault();   
            if (user == null)
                return BadRequest();

            return View(user);
        }

        // POST: UserController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, User user)
        {
            try
            {
                var data = context.Users.Where(u => u.Id == id).FirstOrDefault();
                if(data != null)
                {
                    data.FirstName = user.FirstName;
                    data.LastName = user.LastName;
                    data.Email = user.Email;    
                    data.MiddleName = user.MiddleName;  
                    data.PhoneNumber = user.PhoneNumber;
                    data.DateOfBirth = user.DateOfBirth;
                    context.SaveChanges();
                   

                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: UserController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: UserController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, User user)
        {
            try
            {
                var data = context.Users.Where(u => u.Id == id);
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
