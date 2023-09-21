using Dragon.Models;
using Dragon.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Dragon.Controllers
{
    public class ImageController : Controller
    {
     
        // GET: Image
        public ActionResult ViewImages(int id)
        {
            IList<Image> images = new List<Image>();
            var repo = new ImageRepository();
            var supRepo = new SupplierRepository();
            ViewBag.Supplier = supRepo.GetById(id);
            images = repo.GetImagesForSupplier(id);

            return View(images);
        }

        
        
        // GET: Image/Details/5
        public ActionResult SingleImage(int id)
        {
            var repo = new ImageRepository();
            var img = repo.GetById(id);
            return View(img);
        }

        
        
        // GET: Image/Create
        [Authorize(Roles ="Admin,Business")]
        public ActionResult Create(int id)
        {
            var sRepo = new SupplierRepository();
            ViewBag.Supplier = sRepo.GetById(id);
            return View();
        }

        
        
        // POST: Image/Create
        [Authorize(Roles ="Admin,Business")]
        [HttpPost]
        public ActionResult Create(int id, Image image, HttpPostedFileBase imageFile, bool backImg)
        {
            var supRepo = new SupplierRepository();
            var imgRepo = new ImageRepository();
            image.SupplierID = id;
            if (imageFile?.ContentLength > 0)
            {
                using (var stream = new MemoryStream())
                {
                    imageFile.InputStream.CopyTo(stream);
                    image.ImageContent = stream.ToArray();
                }
            }
            imgRepo.Create(image, backImg);
            

            
            var supplierID = id;
            int newImgID = image.ImageID;
            

            return RedirectToAction("Details/" + supplierID, "Supplier");
        }

        
        
        // GET: Image/Edit/5
        [Authorize(Roles = "Admin,Business")]
        public ActionResult ChooseAsBackground(int id)
        {
            var repo = new ImageRepository();
            var img = repo.GetById(id);
            return View(img);
        }

        
        
        
        // POST: Image/Edit/5
        [Authorize(Roles = "Admin,Business")]
        [HttpPost]
        public ActionResult ChooseAsBackground(int id, int supplierID)
        {
            try
            {
                // TODO: Add update logic here

                ChangeBackground(id, supplierID);

                return RedirectToAction("ViewImages/" + supplierID);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }
        }

        
        
        
        // GET: Image/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            return View();
        }

        
        
        
        // POST: Image/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                var repo=new ImageRepository();
                repo.Delete(id);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                
                return View();
            }
        }
        
        
        
        public FileResult ShowImage(int id)
        {

            var imgRepo = new ImageRepository();
            var image = imgRepo.GetById(id);
            if (image != null && image.ImageContent != null)
            {
                return File(image.ImageContent, "image/jpeg", image.ImageID + ".jpg");

            }
            return null;

        }

        
        
        
        public void ChangeBackground(int id, int supplierID)
        {
            var repo = new ImageRepository();
            repo.MakeImgBackground(id, supplierID);
        }

        
        
        
        // GET: Image/CreateCategoryImage
        [Authorize(Roles = "Admin")]
        public ActionResult CreateCategoryImage()
        {
            var repo = new CategoryRepository();
            
            ViewBag.CategoriesList = repo.GetAll();
            return View();
        }

        
        
        
        // POST: Image/CreateCategoryImage
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult CreateCategoryImage(CategoryImage catImg, HttpPostedFileBase imageFile)
        {
            var repo= new CategoryRepository();
            var imgRepo = new ImageRepository();
            
            if (imageFile?.ContentLength > 0)
            {
                using (var stream = new MemoryStream())
                {
                    imageFile.InputStream.CopyTo(stream);
                    catImg.ImageContent = stream.ToArray();
                }
            }
            imgRepo.CreateCategoryImage(catImg);
            

            return RedirectToAction("CreateCategoryImage", "Image");
        }
    }
}
