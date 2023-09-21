using Dragon.Models;
using Dragon.Repositories;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Dragon.Controllers
{
    public class UsersController : Controller
    {

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
        // GET: Users
        [Authorize]
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = User.Identity;
                ViewBag.Name = user.Name;
                ViewBag.displayMenu = "No";
                if (isAdminUser())
                {
                    ViewBag.displayMenu = "Yes";
                }
                IList<Users> users = new List<Users>();
                var repo = new UsersRepository();
                users=repo.GetAllUsers();
                return View(users);
            }
            else
            {
                ViewBag.Name = "Not Logged IN";
            }
            return View();
        }
      
        public ActionResult ChangeRole(string id)
        {
            ApplicationDbContext context = new ApplicationDbContext();
            var repo=new UsersRepository();
            var usr=repo.GetById(id);
            ViewBag.RolesList=context.Roles.ToList();
            return View(usr);
        }

        [Authorize(Roles="Admin")]
        [HttpPost]
        public ActionResult ChangeRole(Users usr)
        {
            
            var repo=new UsersRepository();
            try
            {
                repo.Edit(usr);
                
                return RedirectToAction("Index");
            } catch (Exception ex)
            {
                ModelState.AddModelError("",ex.Message);
            }
            return RedirectToAction("Index");
        }

        [Authorize(Roles ="Admin")]
        [HttpPost]
        public ActionResult Delete(string id)
        {
            ApplicationDbContext context = new ApplicationDbContext();
            ;
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var usr= UserManager.FindById(id);

            UserManager.Delete(usr);

            
          
            return RedirectToAction("Index","Users");
        }
    }
}