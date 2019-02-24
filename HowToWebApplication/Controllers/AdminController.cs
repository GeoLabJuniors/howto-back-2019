using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HowToWebApplication.Models;
using HowToWebApplication.Helpers;
using static HowToWebApplication.Helpers.PasswordHelper;
using HowToWebApplication.Filters;

namespace HowToWebApplication.Controllers
{
    [AdminFilter]
    public class AdminController : Controller
    {
        HowToDbEntities _db = new HowToDbEntities();
        UsersDataProvider UserData = new UsersDataProvider();
        CategoriesDataProvider CategoriesData = new CategoriesDataProvider();
        

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }


        #region user
        // GET: Users

        public ActionResult UserList()
        {
            var result = UserData.AllUsers();
            return View(result);
        }


        // GET: Users/Details/5
        public ActionResult UserDetails(int id)
        {
            var result = UserData.GetUserById(id);

            if (result == null)
            {
                return HttpNotFound();
            }
            return View(result);
        }


        // GET: Users/Create
        public ActionResult CreateUser()
        {
            //var categories = data.GetUserCategories();
            ViewBag.CategoriesId = new SelectList(_db.usersCategories.ToList(), "Id", "Name");
            return View();
        }

        // POST: Users/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateUser(UsersCustomClass model)
        {
            ViewBag.CategoriesId = new SelectList(_db.usersCategories.ToList(), "Id", "Name");
            if (ModelState.IsValid)
            {
                UserData.CreateUser(model);
                return RedirectToAction("UserList");
            }
            return View(model);
        }

        //// GET: Users/Edit/5  
        public ActionResult EditUser(int id)
        {
            var result = UserData.GetUserById(id);
            //var categories = data.GetUserCategories();
            ViewBag.CategoriesId = new SelectList(_db.usersCategories.ToList(), "Id", "Name", result.categoriesId);
            var customUser = new UsersCustomeEditClass()
            {
                Name = result.name,
                Surname = result.surname,
                Email = result.email,
                CategoriesId = result.categoriesId,
                IsActive = result.isActive
            };
            return View(customUser);
        }


        //// POST: Users/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditUser(UsersCustomeEditClass model)
        {
            ViewBag.CategoriesId = new SelectList(_db.usersCategories.ToList(), "Id", "Name");
            if (ModelState.IsValid)
            {
                UserData.EditUser(model);
                return RedirectToAction("UserList");
            }
            return View(model);
        }

        // GET: Users/Delete/5
        public ActionResult DeleteUser(int id)
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
        public ActionResult DeleteUser(users model)
        {
            try
            {
                UserData.deleteUser(model);
            }
            catch
            {
                return View(model);
            }
            return RedirectToAction("UserList");
        }

        // GET: Users/fullDelete/5
        public ActionResult fullDeleteUser(int id)
        {
            var result = UserData.GetUserById(id);


            if (result == null)
            {
                return HttpNotFound();
            }
            return View(result);
        }

        //POST: Users/fullDelete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult fullDeleteUser(users model)
        {
            try
            {
                UserData.FullDeleteUser(model);
            }
            catch
            {
                return View(model);
            }
            return RedirectToAction("UserList");
        }

        #endregion

        #region  Categories 

        // GET: Categories

        public ActionResult CategoriesList()
        {
            var result = CategoriesData.AllCategories();
            return View(result);
        }


        // GET: Categories/Details/5
        public ActionResult CategoriesDetails(int id)
        {
            var result = CategoriesData.GetCategoriesById(id);

            if (result == null)
            {
                return HttpNotFound();
            }
            return View(result);
        }

        // GET: Categories/Create
        public ActionResult CreateCategories()
        {
            var list = _db.categories.ToList();
            list.Insert(list.Count, new categories() { Id = 0, name = "სხვა" });
            ViewBag.ParentCategoryId = new SelectList(list, "Id", "Name");
            return View();
        }

        // POST: Categories/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateCategories(CategoriesCustomClass model)
        {
            //var CategoriesList = _db.categories.ToList();
            //CategoriesList.Insert(0, new categories() { Id = 0, name = "სხვა" });           
            //ViewBag.ParentId = new SelectList(CategoriesList, "Id", "Name");
            if (ModelState.IsValid)
            {
                CategoriesData.CreateCategories(model);
                return RedirectToAction("CategoriesList");
            }
            else
            {

                return View(model);
            }
        }

        //add new category 
        public ActionResult AddNewCategory(string categoryName)
        {
            var categories = _db.categories;
            
                if (categories.Count(e => e.name == categoryName) == 0)
                {
                    _db.categories.Add(new categories() { Id=0, name = categoryName });
                    _db.SaveChanges();
                }               
           
                var list= _db.categories.ToList();
                list.Insert(list.Count, new categories() { Id = 0, name = "სხვა" });
                var result = list.OrderByDescending(e => e.Id).Select(c => new { Id = c.Id, name = c.name });
                return Json(result, JsonRequestBehavior.AllowGet);
        }



        //// GET: Categories/Edit/5
        public ActionResult EditCategories(int id)
        {
            var result = CategoriesData.GetCategoriesById(id);
            var list = _db.categories.ToList();
           // list.Insert(0, new categories() { name = "-- Please Select --" });
            list.Insert(list.Count, new categories() { Id =0, name = "სხვა" });
            ViewBag.ParentCategoryId = new SelectList(list, "Id", "Name", result.parentId);

            var customCategory = new CategoriesCustomClass()
            {
                Name = result.name,
                Id = result.Id,
                ParentId = result.parentId
            };
            return View(customCategory);
        }


        //// POST: Categories/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditCategories(CategoriesCustomClass model)
        {
            var list = _db.categories.ToList();
            ViewBag.ParentId = new SelectList(list, "Id", "Name");

            if (ModelState.IsValid)
            {
                CategoriesData.EditCategories(model);
                return RedirectToAction("CategoriesList");
            }
            return View(model);
        }




        // GET: Categories/Delete/5
        public ActionResult DeleteCategories(int id)
        {
            var result = CategoriesData.GetCategoriesById(id);
            if (result == null)
            {
                return HttpNotFound();
            }
            return View(result);
        }

        //POST: Categories/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteCategories(categories model)
        {
            try
            {
                CategoriesData.FullDeleteCategories(model);
            }
            catch
            {
                return View(model);
            }
            return RedirectToAction("CategoriesList");
        }
        //[ValidateAntiForgeryToken]
        //public ActionResult Delete(categories model)
        //{
        //    try
        //    {
        //        UserData.deleteCategories(model);
        //    }
        //    catch
        //    {
        //        return View(model);
        //    }
        //    return RedirectToAction("index");
        //}

        #endregion

        #region Parent Categories
        //// GET: Categories

        //public ActionResult ParentCategoriesList()
        //{
        //    var result = ParentCategoriesData.AllParentCategories();
        //    return View(result);
        //}


        //// GET: Categories/Details/5
        //public ActionResult ParentCategoriesDetails(int id)
        //{
        //    var result = ParentCategoriesData.GetParentCategoriesById(id);

        //    if (result == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(result);
        //}

        //// GET: Categories/Create
        //public ActionResult CreateParentCategories()
        //{ 
        //    return View();
        //}

        //// POST: Categories/Create
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult CreateParentCategories(ParentCategoriesCustomClass model)
        //{
           
        //    if (ModelState.IsValid)
        //    {
        //        ParentCategoriesData.CreateParentCategories(model);
        //        return RedirectToAction("ParentCategoriesList");
        //    }
        //    else
        //    {

        //        return View(model);
        //    }
        //}

        ////// GET: Categories/Edit/5
        //public ActionResult EditParentCategories(int id)
        //{
        //    var result = ParentCategoriesData.GetParentCategoriesById(id);
            
        //    var customCategory = new ParentCategoriesCustomClass()
        //    {
        //        Name = result.name,
        //        Id = result.Id,  
        //    };
        //    return View(customCategory);
        //}


        ////// POST: Categories/Edit/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult EditParentCategories(ParentCategoriesCustomClass model)
        //{
            
        //    if (ModelState.IsValid)
        //    {
        //        ParentCategoriesData.EditParentCategories(model);
        //        return RedirectToAction("ParentCategoriesList");
        //    }
        //    return View(model);
        //}

        //// GET: Categories/Delete/5
        //public ActionResult DeleteParentCategories(int id)
        //{
        //    var result = ParentCategoriesData.GetParentCategoriesById(id);
        //    if (result == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(result);
        //}

        ////POST: Categories/Delete/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteParentCategories(parentCategories model)
        //{
        //    try
        //    {
        //        ParentCategoriesData.FullDeleteParentCategories(model);
        //    }
        //    catch
        //    {
        //        return View(model);
        //    }
        //    return RedirectToAction("ParentCategoriesList");
        //}
        ////[ValidateAntiForgeryToken]
        ////public ActionResult Delete(categories model)
        ////{
        ////    try
        ////    {
        ////        UserData.deleteCategories(model);
        ////    }
        ////    catch
        ////    {
        ////        return View(model);
        ////    }
        ////    return RedirectToAction("index");
        ////}

        #endregion


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