using Dragon.Models;
using Dragon.Repositories;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;
using X.PagedList.Mvc;
using System.Net.Mail;

namespace Dragon.Controllers
{
    public class SupplierController : Controller
    {
        ApplicationDbContext context = new ApplicationDbContext();
        
        
        
        
        
        
        // GET: Supplier
        public ActionResult Index(string id, string supplierName, int? page, string cityFilter)
        {
            IList<Supplier> supplierList = new List<Supplier>();
            SupplierRepository repo = new SupplierRepository();
            CategoryRepository categoryRepo = new CategoryRepository();
            FeedbackRepository feedbackRepository = new FeedbackRepository();
            
            supplierList = repo.GetAll();

            //Search function
            int pageNumber = page ?? 1;
            int totalCount;
            int pageSize = 10;
            supplierList = repo.Search(supplierName, cityFilter, pageNumber, pageSize, out totalCount);
            var pagedList = new StaticPagedList<Supplier>(supplierList, pageNumber, pageSize, totalCount);

            ViewBag.CategoriesList = categoryRepo.GetAll();
            if (isAdminUser() || isBusinessUser())
            {
                ViewBag.DisplayCreate = "Yes";
            }
            
            ViewBag.Cities=repo.GetAll().Select(o=>o.City).Distinct();

           
            return View(pagedList);
        }


//Show suppliers by categories
        public ActionResult ShowByCategory(int id, string supplierName, int? page, string cityFilter, string orderByName)
        {
            IList<Supplier> supplierList = new List<Supplier>();
            SupplierRepository repo = new SupplierRepository();
            CategoryRepository categoryRepo = new CategoryRepository();
            ViewBag.CategoryName = categoryRepo.GetById(id);
            supplierList = repo.GetByCategory(id);


            //Search function
            int pageNumber = page ?? 1;
            int totalCount;
            int pageSize = 10;
            int categoryID = id;
            supplierList = repo.Search2(categoryID, supplierName, cityFilter, pageNumber, pageSize, out totalCount, orderByName);
            var pagedList = new StaticPagedList<Supplier>(supplierList, pageNumber, pageSize, totalCount);

            ViewBag.CategoriesList = categoryRepo.GetAll();
            if (isAdminUser() || isBusinessUser())
            {
                ViewBag.DisplayCreate = "Yes";
            }



            ViewBag.Cities = repo.GetAll().Select(o => o.City).Distinct();

            IList<Order> orderDictionary = new List<Order>()
            {
                new Order() { Id = 1, Name ="A to Z",Meaning="ASC"},
                new Order() { Id = 2, Name ="Z to A",Meaning="DESC"}
            };


            ViewBag.Alphabet = orderDictionary;


            return View(pagedList);
        }
        


        // GET: Supplier/Details/5
        
        public ActionResult Details(int id)
        {
            SupplierRepository repo = new SupplierRepository();
            ImageRepository imageRepo = new ImageRepository();
            



            ViewBag.imgList = ShowImage(id);

            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            Supplier supplier = repo.GetById(id);
            if (supplier == null)
            {
                return HttpNotFound();
            }
            return View(supplier);
        }




        // GET: Supplier/Create
        [Authorize(Roles ="Admin,Business")]
        public ActionResult Create()
        {
            SupplierRepository sRepo=new SupplierRepository();
            CategoryRepository categoryRepository = new CategoryRepository();
            var categories = categoryRepository.GetAll();
            ViewBag.CategoriesList = categories;
            ViewBag.Cities=sRepo.GetAll().Select(o=>o.City).Distinct();
            ViewBag.OwnerID=User.Identity.GetUserId();
            return View();
        }
        
        



        // POST: Supplier/Create
        [Authorize(Roles = "Admin,Business")]
        [HttpPost]
        public ActionResult Create(Supplier supplier)
        {
            var repo = new SupplierRepository();


            supplier.OwnerID = User.Identity.GetUserId();
            repo.Create(supplier);



            CategoryRepository categoryRepo = new CategoryRepository();
            var categories = categoryRepo.GetAll();
            ViewBag.CategoryList = categories;
            return RedirectToAction("Index");
        }
        
        
        
        // GET: Supplier/Edit/5
        [Authorize(Roles = "Admin,Business")]
        public ActionResult Edit(int id)
        {
            var userId=User.Identity.GetUserId();
            var supplierRepo = new SupplierRepository();
            var supplier = supplierRepo.GetById(id);
            if (supplier.OwnerID == userId)
            {
                return View(supplier);
            }
            else
            {
                return RedirectToAction("Index", "Home", null);
            }
        }
        
        
        // POST: Supplier/Edit/5
        [Authorize(Roles = "Admin,Business")]
        [HttpPost]
        public ActionResult Edit(Supplier supplier)
        {
            var supplierRepo = new SupplierRepository();
            try
            {
                // TODO: Add update logic here
                supplierRepo.Update(supplier);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }
        }
        [Authorize(Roles = "Admin,Business")]
        
        
        // GET: Supplier/Delete/5
        public ActionResult Delete(int id)
        {
            var userId = User.Identity.GetUserId();
            var supplierRepo = new SupplierRepository();
            var sup=supplierRepo.GetById(id);
            if (sup.OwnerID == userId)
            {
                return View(sup);
            }
            else
            {
                return RedirectToAction("Index", "Home", null);
            }
           
        }
        [Authorize(Roles = "Admin,Business")]
        
        
        // POST: Supplier/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            SupplierRepository repo=new SupplierRepository();
            try
            {
                // TODO: Add delete logic here
                repo.Delete(id);
                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }
        }





        //Method for displaying background image of the business
        public FileResult ShowBackgroundImage(int id)
        {
            var repo = new SupplierRepository();
            var supplier = repo.GetById(id);
            var imgRepo = new ImageRepository();
            var imageBackground = imgRepo.GetById(supplier.BackgroundImageID);
            if (supplier != null && imageBackground != null)
            {
                return File(imageBackground.ImageContent, "image/jpeg", supplier.Name + ".jpg");

            }
            return null;

        }


        
        //Show only the suppliers that owns the user
        [Authorize]
        public ActionResult ShowByOwner(string ownerId)
        {
                ownerId=User.Identity.GetUserId();
                IList<Supplier> suppliers= new List<Supplier>();
                SupplierRepository repo=new SupplierRepository();
                CategoryRepository categoryRepository = new CategoryRepository();
                suppliers=repo.GetByOwnerID(ownerId);
                return View(suppliers);
          
            
        }
        
        //send enquiry to the supplier's email
        [Authorize]
        public ActionResult Inquiry(int id)
        {
            var repo=new SupplierRepository();
            var supplier=repo.GetById(id);
            ViewBag.SupplierName=supplier.Name;
            ViewBag.supplierEmail = supplier.SocialMedia;
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult Inquiry(int id,Inquiry inquiry)
        {
            var repo = new SupplierRepository();
            var supplier= repo.GetById(id);
            try
            {
                //for making email
                MailMessage mail = new MailMessage();
                mail.To.Add(supplier.Email);
                mail.From = new MailAddress("zafarbek2top@hotmail.com");
                mail.Subject=supplier.Name+ " services --- "+ inquiry.Subject;
                string text="This message is from Tantana.uz user "  + inquiry.UserName + " | I am interested in your services. Could you please give more details on my personal email at  "+ inquiry.UserEmail+ ". | (message from user): "+inquiry.Message +"|| Tantana.uz -- first wedding services company in Uzbekistan is pleased to help you!| Visit Tantana.uz";
                text=text.Replace("|",""+Environment.NewLine);
                mail.Body=text;
                //smtp client get mail and sends to the supplier's email using given credentials
                SmtpClient smtp = new SmtpClient("smtp.office365.com", 587);
                //smtp.Host = "smtp.gmail.com";
                //smtp.Port = 587;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new System.Net.NetworkCredential("zafarbek2top@hotmail.com", "iloveplov111");
                smtp.EnableSsl=true;

                smtp.Send(mail);
            } catch (Exception ex)
            {
                ModelState.AddModelError(ex.Message, "");
            }
            return RedirectToAction("Index","Home",null);
        }


        //Boolean for defining whether this is business user or not
        
        public Boolean isBusinessUser()
        {

            if (User.Identity.IsAuthenticated)
            {
                var user = User.Identity;
                ApplicationDbContext context = new ApplicationDbContext();
                var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                var s = UserManager.GetRoles(user.GetUserId());
                if (s[0].ToString() == "Business")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }
        
        
        
        public Boolean isAdminUser()
        {

            if (User.Identity.IsAuthenticated)
            {
                var user = User.Identity;
                ApplicationDbContext context = new ApplicationDbContext();
                var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                var s = UserManager.GetRoles(user.GetUserId());
                if (s[0].ToString() == "Admin")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }
        
        
        public FileResult ShowImage(int id)
        {

            var imgRepo = new ImageRepository();
            var image = imgRepo.GetBySupplierID(id);



            if (image != null && image.ImageContent != null)
            {
                return File(image.ImageContent, "image/jpeg", image.SupplierID + ".jpg");

            }
            return null;

        }

        public decimal ShowAverageRate(int id)
        {
            var repo = new FeedbackRepository();
            return repo.AverageRate(id);
        }
        
    }

    internal class Order
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string Meaning { get; set; }
    }
}