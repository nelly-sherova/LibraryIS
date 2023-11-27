using LibraryIS.Data;
using LibraryIS.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using static Azure.Core.HttpHeader;

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
            var users = context.Users.Where(u => u.Visible == true).ToList();
            if (users.Count <= 0 )
                return BadRequest();
            foreach (var user in users)
            {
                user.Role = context.Roles.FirstOrDefault(r => r.Id == user.RoleId);
            }
          
            return View(users);
        }

        // GET: UserController/Details/5
        public ActionResult Details(int id)
        {
            var user = context.Users.Where(u => u.Id == id).FirstOrDefault();

            if (user == null)
                return BadRequest();
            user.Role = context.Roles.FirstOrDefault(r => r.Id == user.RoleId);
            user.BookUsers = context.BookUser.Where(bu => bu.UserId == id).ToList();
            foreach(var item in user.BookUsers)
            {
                item.Book = context.Books.FirstOrDefault(b => b.Id == item.BookId);
            }
                 
            return View(user);
        }
        public ActionResult UserBasket()
        {
            var users = context.Users.Where(u => u.Visible == false).ToList();
            if (users.Count <= 0)
                return BadRequest();
            foreach (var user in users)
            {
                user.Role = context.Roles.FirstOrDefault(r => r.Id == user.RoleId);
            }
            return View(users);
        }
      

        // GET: UserController/Create
        public ActionResult Create()
        {
            var roles = context.Roles.ToList();
            string[] strings = new string[roles.Count];
            for(int i = 0; i < roles.Count; i++)
            {
                strings[i] = roles[i].Name; 
            }
            ViewBag.Names = strings;
            ViewBag.Roles = roles;  
            return View();
        }

        // POST: UserController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(User user)
        {
            try
            {
                user.Role = context.Roles.Where(r => r.Id == user.RoleId).FirstOrDefault();
                user.Visible = true;

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
