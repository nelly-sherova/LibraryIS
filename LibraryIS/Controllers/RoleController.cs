﻿using LibraryIS.Data;
using LibraryIS.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace LibraryIS.Controllers
{
    public class RoleController : Controller
    {
        private DataContext context;

        public RoleController(DataContext context)
        {
            this.context = context;
        }


        // GET: RoleController
        public ActionResult Index()
        {
            var roles = context.Roles.Where(r => r.Visible == true).ToList();
            if (!roles.Any())
                return BadRequest();
            return View(roles);
        }

        // GET: RoleController/Details/5
        public ActionResult Details(int id)
        {
            var role = context.Roles.Where(r => r.Id == id).FirstOrDefault();   
            if (role == null)   
                return NotFound();    
            return View(role);
        }

        // GET: RoleController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: RoleController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Role role)
        {
           
            try
            {
                role.Visible = true;
                context.Roles.Add(role);
                context.SaveChanges();  
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: RoleController/Edit/5
        public ActionResult Edit(int id)
        {
            var role = context.Roles.Where(r => r.Id == id).FirstOrDefault();   
            if (role == null)
                return BadRequest();
            ViewBag.RoleId = role.Id;
            return View(role);
        }

        // POST: RoleController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Role role)
        {
            try
            {
                var data = context.Roles.Where(r => r.Id == role.Id).FirstOrDefault();
                data.Name = role.Name;
                data.Description = role.Description;
                context.Roles.Update(data);
                context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        public ActionResult RoleBasket()
        {
            var roles = context.Roles.Where(r => r.Visible == false).ToList();
            return View(roles); 
        }
        public ActionResult DeleteBasket(int id)
        {
            var role = context.Roles.Where(r => r.Id == id).FirstOrDefault();
            role.Visible = false;
            context.Update(role);
            context.SaveChanges();

            return RedirectToAction(nameof(RoleBasket));
        }
        public ActionResult Restore(int id)
        {
            var role = context.Roles.Where(r => r.Id == id).FirstOrDefault();
            role.Visible = true;
            context.Update(role);
            context.SaveChanges();
            return RedirectToAction(nameof(Index));

        }
        
        public ActionResult Delete(int id)
        {
            try
            {
                var data = context.Roles.Where(r => r.Id == id).FirstOrDefault();
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
