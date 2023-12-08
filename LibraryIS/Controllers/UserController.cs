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
            if (users.Count <= 0)
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
            foreach (var item in user.BookUsers)
            {
                item.Book = context.Books.FirstOrDefault(b => b.Id == item.BookId);
            }

            return View(user);
        }
        public ActionResult DetailsUser(int id)
        {
            var user = context.Users.Where(u => u.Id == id).FirstOrDefault();

            if (user == null)
                return BadRequest();
            user.Role = context.Roles.FirstOrDefault(r => r.Id == user.RoleId);
            user.BookUsers = context.BookUser.Where(bu => bu.UserId == id).ToList();
            foreach (var item in user.BookUsers)
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
            ViewBag.UserId = user.Id;

            return View(user);
        }

        // POST: UserController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(User user)
        {
            try
            {
                var data = context.Users.Where(u => u.Id == user.Id).FirstOrDefault();
                if (data != null)
                {
                    data.FirstName = user.FirstName;
                    data.LastName = user.LastName;
                    data.Email = user.Email;
                    data.MiddleName = user.MiddleName;
                    data.PhoneNumber = user.PhoneNumber;
                    data.DateOfBirth = user.DateOfBirth;
                    data.City = user.City;
                    data.Country = user.Country;
                    data.Region = user.Region;
                    data.Sex = user.Sex;
                    context.Users.Update(data);
                    context.SaveChanges();


                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        public ActionResult EditRole(int userId)
        {
            var roles = context.Roles.ToList();
            var user = context.Users.Where(u => u.Id == userId).FirstOrDefault();
            ViewBag.Roles = roles;
            ViewBag.Name = user.FirstName + " " + user.LastName + " " + user.MiddleName;
            ViewBag.UserId = userId;
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditRole(User user)
        {

            var data = context.Users.Where(u => u.Id == user.Id).FirstOrDefault();
            var role = context.Roles.FirstOrDefault(role => role.Id == user.RoleId);
            data.Role = role;
            data.RoleId = user.RoleId;
            context.Users.Update(data);
            context.SaveChanges();
            return RedirectToAction(nameof(Details), new RouteValueDictionary(new { id = user.Id }));
        }
        public ActionResult ChangePassword(int userId)
        {
            var user = context.Users.Where(u => u.Id == userId).FirstOrDefault();
            ViewBag.UserId = userId;
            ViewBag.Email = user.Email;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(User user)
        {

            var data = context.Users.Where(u => u.Id == user.Id).FirstOrDefault();
            data.Password = user.Password;
            data.Email = user.Email;
            context.Users.Update(data);
            context.SaveChanges();
            return RedirectToAction(nameof(DetailsSimpleUser), new RouteValueDictionary(new { id = user.Id }));
        }
        public ActionResult DeleteBasket(int id)
        {
            var data = context.Users.Where(u => u.Id == id).FirstOrDefault();
            data.Visible = false;
            context.Users.Update(data);
            context.SaveChanges();
            return RedirectToAction(nameof(UserBasket));

        }
        public ActionResult Restore(int id)
        {

            var data = context.Users.Where(u => u.Id == id).FirstOrDefault();
            data.Visible = true;
            context.Users.Update(data);
            context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }


        public ActionResult Delete(int id)
        {
            try
            {
                var data = context.Users.Where(u => u.Id == id).FirstOrDefault();
                context.Remove(data);
                context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        public ActionResult LogIn()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(User user)
        {
            try
            {
                var us = context.Users.Where(u => u.Email == user.Email && u.Password == user.Password).FirstOrDefault();
                if (us == null) {
                    return View();
                }
                us.Role = context.Roles.Where(r => r.Id == us.RoleId).FirstOrDefault();
                if(us.Role.Name == "Администратор")
                {
                    return RedirectToAction(nameof(AdminPanel));
                }
                else
                {
                    return RedirectToAction(nameof(DetailsSimpleUser), new RouteValueDictionary(new { id = us.Id }));
                }
            }
            catch
            {

            }
            return View();
        }
       
     
        public ActionResult DetailsSimpleUser(int id)
        {
            var user = context.Users.Where(u => u.Id == id).FirstOrDefault();

            if (user == null)
                return BadRequest();
            user.Role = context.Roles.FirstOrDefault(r => r.Id == user.RoleId);
            user.BookUsers = context.BookUser.Where(bu => bu.UserId == id).ToList();
            foreach (var item in user.BookUsers)
            {
                item.Book = context.Books.FirstOrDefault(b => b.Id == item.BookId);
            }

            return View(user);
        }
        public ActionResult AdminPanel()
        {
            return View();
        }
    }
}
