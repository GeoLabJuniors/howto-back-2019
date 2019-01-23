using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HowToWebApplication.Models;

    namespace HowToWebApplication.Controllers
    {
    public class CategoriesController : Controller
    {
            HowToDbEntities _db = new HowToDbEntities();
            CategoriesDataProvider CategoriesData = new CategoriesDataProvider();
            

            // GET: Users
            
            public ActionResult Index(string name = "")
            {
                
                var result = CategoriesData.AllCategories();
                return View(result);
            }


            // GET: Users/Details/5
            public ActionResult Details(int id)
            {
                var result = CategoriesData.GetCategoriesById(id);

                if (result == null)
                {
                    return HttpNotFound();
                }
                return View(result);
            }

            // GET: Users/Create
            public ActionResult Create()
            {
                return View();
            }

            // POST: Users/Create
            [HttpPost]
            [ValidateAntiForgeryToken]
            public ActionResult Create( CategoriesCustomClass model)
            {

                if (ModelState.IsValid)
                {
                    CategoriesData.CreateCategories(model);
                    return RedirectToAction("Index");
                }
                else
                {

                    return View(model);
                }
            }

            //// GET: Users/Edit/5
            public ActionResult Edit(int id)
            {
                var result = CategoriesData.GetCategoriesById(id);

                var customCategory = new CategoriesCustomClass()
                {
                    Name = result.name,
                    Id = result.Id
                };
                return View(customCategory);
            }


            //// POST: Users/Edit/5
            [HttpPost]
            [ValidateAntiForgeryToken]
            public ActionResult Edit( CategoriesCustomClass model)
            {
                if (ModelState.IsValid)
                {
                    CategoriesData.EditCategories(model);
                    return RedirectToAction("Index");
                }
                return View(model);
            }

            // GET: Users/Delete/5
            public ActionResult Delete(int id)
            {
                var result = CategoriesData.GetCategoriesById(id);
                if (result == null)
                {
                    return HttpNotFound();
                }
                return View(result);
            }

            //POST: Users/Delete/5
            [HttpPost]
            [ValidateAntiForgeryToken]
            public ActionResult Delete(categories model)
            {
                try
                {
                    CategoriesData.FullDeleteCategories(model);
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