using Dragon.Models;
using Dragon.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Dragon.Controllers
{
    public class CategoryController : Controller
    {
        [Authorize(Roles ="Admin")]
        public ActionResult Index()
        {
            IList<Category> categories = new List<Category>();
            CategoryRepository categoryRepo = new CategoryRepository();
            categories = categoryRepo.GetAll();
            return View(categories);
        }

        [Authorize(Roles = "Admin")]
        // GET: Category/Create
        public ActionResult Create()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]
        // POST: Category/Create
        [HttpPost]
        public ActionResult Create(Category category)
        {
            CategoryRepository categoryRepo = new CategoryRepository();
            categoryRepo.Create(category);

            return RedirectToAction("Index");
        }
        [Authorize(Roles = "Admin")]
        // GET: Category/Edit/5
        public ActionResult Edit(int id)
        {
            var categoryRepo = new CategoryRepository();
            var category = categoryRepo.GetById(id);
            return View(category);
        }
        [Authorize(Roles = "Admin")]
        // POST: Category/Edit/5
        [HttpPost]
        public ActionResult Edit(Category category)
        {
            var categoryRepo = new CategoryRepository();
            try
            {
                // TODO: Add update logic here
                categoryRepo.Update(category);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }
        }

        // GET: Category/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            var repo = new CategoryRepository();
            var category = repo.GetById(id);

            return View(category);
        }

        // POST: Category/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                var repo = new CategoryRepository();
                repo.Delete(id);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }
        }
    }
}
