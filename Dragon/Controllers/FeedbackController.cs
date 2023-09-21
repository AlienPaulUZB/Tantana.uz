using Dragon.Models;
using Dragon.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace Dragon.Controllers
{
    public class FeedbackController : Controller
    {
        // GET: Feedback
        public ActionResult ViewFeedbacks(int id)
        {
            
            FeedbackRepository repo=new FeedbackRepository();
            var sRepo = new SupplierRepository();
            IList<Feedback> feedbacks = repo.GetSupplierFeedbacks(id);
            ViewBag.Supplier=sRepo.GetById(id);

            var uRepo = new UsersRepository();

            ViewBag.UserList=uRepo.GetAllUsers();
            return View(feedbacks);
        }


        // GET: Feedback/Create
        public ActionResult Create(int id)
        {
            var sRepo=new SupplierRepository();
            ViewBag.Supplier=sRepo.GetById(id);
            return View();
        }

        // POST: Feedback/Create
        [HttpPost]
        public ActionResult Create(int id, Feedback feedback)
        {
           var supRepo=new SupplierRepository();
            var feedbackRepo=new FeedbackRepository();
            feedback.SupplierId=id;
            feedback.CreatedTime=DateTime.Now;
            feedback.UserID = User.Identity.GetUserId();
            feedback.Rating = Int32.Parse(Request.Form["stars"]);
            if (feedback.Rating == 0)
            {
                feedback.Rating = 3;
            }
           
            feedbackRepo.Create(feedback);
            var supplierID = id;

            return RedirectToAction("Details/" + supplierID, "Supplier");
        }

        // GET: Feedback/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Feedback/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Feedback/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Feedback/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
