using Dragon.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Dragon.Controllers
{
    public class RoleController : Controller
    {
       public Boolean IsAdminUser()
        {
            if (User.Identity.IsAuthenticated)
            {
                var usr =User.Identity;
                ApplicationDbContext context = new ApplicationDbContext();
                var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                var s=UserManager.GetRoles(usr.GetUserId());
                if(s[0].ToString() == "Admin")
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
        ApplicationDbContext context=new ApplicationDbContext();
        //GET: Role

        public ActionResult Index()
        {
            var Roles = context.Roles.ToList();

            if (User.Identity.IsAuthenticated)
            {
                if (IsAdminUser())
                {
                    return View(Roles);
                }
            }
            else
            {
                return RedirectToAction("Index","Home");
            }
            return RedirectToAction("Index", "Home");

        }
        //Give Role
        [Authorize(Roles ="Admin")]
        public ActionResult GiveRole(string usrId)
        {
            ViewBag.Roles=context.Roles.ToList();
            ViewBag.user=User.Identity.GetUserName();
            return View(usrId);
        }
        //POST:GiveRole
        [Authorize(Roles ="Admin")]
        [HttpPost]
        public ActionResult GiveRole(string usrID, int roleID)
        {
            try
            {
                using(var conn=new SqlConnection())
                {
                    using(var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"INSERT INTO [dbo].[AspNetUserRoles]
                                                  ([UserId]
                                                  ,[RoleId])
                                            VALUES
                                                  (@UserId
                                                  , @RoleId)";
                        cmd.Parameters.AddWithValue("@UserId",usrID);
                        cmd.Parameters.AddWithValue("@RoleId",roleID);
                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("",ex.Message);
                return View();
            }

            return RedirectToAction("Index", "Role");
        }

    }
}