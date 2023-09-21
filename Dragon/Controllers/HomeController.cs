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

namespace Dragon.Controllers
{
    public class HomeController : Controller
    {
        public Boolean isAdminUser()
        {

            if (User.Identity.IsAuthenticated)
            {
                ViewBag.displayMenu = "No";
                var user = User.Identity;
                ApplicationDbContext context = new ApplicationDbContext();
                var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                var s = UserManager.GetRoles(user.GetUserId());
                if (s[0].ToString() == "Admin")
                {
                    ViewBag.displayMenu = "Yes";

                    return true;
                }
                else
                {
                    return false;
                    
                }
            }
            return false;

        }

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



        public ActionResult Index()
        {
            IList<Category> categories = new List<Category>();
            CategoryRepository categoryRepo = new CategoryRepository();
            categories = categoryRepo.GetAll();
            if (User.Identity.IsAuthenticated)
            {
                var user = User.Identity;
                ViewBag.Name = user.Name;
                if (isAdminUser())
                {
                    return View(categories);
                }
                if (isBusinessUser())
                {
                   
                    return RedirectToAction("ShowByOwner", "Supplier");
                }
                
            }
            
                return View(categories);


        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {

            return View();
        }

        

        public FileResult ShowCategoryImage(int id)
        {
            var repo = new ImageRepository();
            CategoryImage image = repo.GetCategoryImage(id);
            
                return File(image.ImageContent, "image/jpeg", image.CategoryId + ".jpg");

            
        
        }
    }
}