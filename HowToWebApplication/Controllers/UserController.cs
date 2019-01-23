using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HowToWebApplication.Models;
using HowToWebApplication.Helpers;
using static HowToWebApplication.Helpers.PasswordHelper;

namespace HowToWebApplication.Controllers
{
    public class UserController : Controller
    {
        HowToDbEntities  _db = new HowToDbEntities();
        UsersDataProvider UserData = new UsersDataProvider();
    

        // GET: Users
       
        public ActionResult Index()
        {
            var result = UserData.AllUsers();
            return View(result);
        }
 

        // GET: Users/Details/5
        public ActionResult Details(int id)
        {
            var result = UserData.GetUserById(id);

            if (result == null)
            {
                return HttpNotFound();
            }
            return View(result);
        }


        // GET: Users/Create
        public ActionResult Create()
        {
            //var categories = data.GetUserCategories();
            ViewBag.CategoriesId = new SelectList(_db.usersCategories.ToList(), "Id", "Name");
            return View();
        }

        // POST: Users/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(UsersCustomClass model)
        {
            ViewBag.CategoriesId = new SelectList(_db.usersCategories.ToList(), "Id", "Name");
            if (ModelState.IsValid)
            {
                UserData.CreateUser(model);
                return RedirectToAction("Index");
            }
            return View(model);
        }

        //// GET: Users/Edit/5  
        public ActionResult Edit(int id)
        {
            var result = UserData.GetUserById(id);
            //var categories = data.GetUserCategories();
            ViewBag.CategoriesId = new SelectList(_db.usersCategories.ToList(), "Id", "Name", result.categoriesId);
            var customUser = new UsersCustomClass()
            {
                Name = result.name,
                Surname = result.surname,
                Email = result.email,
                Password = result.password,
                CategoriesId = result.categoriesId,
                IsActive = result.isActive
            };
            return View(customUser);
        }


        //// POST: Users/Edit/5
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( UsersCustomClass model)
        {
            ViewBag.CategoriesId = new SelectList(_db.usersCategories.ToList(), "Id", "Name");
            if (ModelState.IsValid)
            {
                UserData.EditUser(model);
                return RedirectToAction("Index");
            }
            return View(model);
        }

        // GET: Users/Delete/5
        public ActionResult Delete(int id)
        {
            var result = UserData.GetUserById(id);


            if (result == null)
            {
                return HttpNotFound();
            }
            return View(result);
        }

        //POST: Users/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(users model)
        {
            try
            {
                UserData.FullDeleteUser(model);
            }
            catch
            {
                return View(model);
            }
            return RedirectToAction("index");
        }
        //[ValidateAntiForgeryToken]
        //public ActionResult Delete(Users model)
        //{
        //    try
        //    {
        //        UserData.deleteUser(model);
        //    }
        //    catch
        //    {
        //        return View(model);
        //    }
        //    return RedirectToAction("index");
        //}
        [HttpPost]
        [ValidateAntiForgeryToken]
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}