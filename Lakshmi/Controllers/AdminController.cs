using Lakshmi.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace Lakshmi.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        
        public ActionResult Index(string SearchName, string SearchEmail, string SearchId)
        {
            var users = from m in db.Users select m;

            if (!String.IsNullOrEmpty(SearchName))
            {
                users = users.Where(s => s.NickName.Contains(SearchName));
            }
            if (!String.IsNullOrEmpty(SearchEmail))
            {
                users = users.Where(s => s.Email.Contains(SearchEmail));
            }
            if (!String.IsNullOrEmpty(SearchId))
            {
                users = users.Where(s => s.Id.Contains(SearchId));
            }
            ViewBag.Name = SearchName;
            ViewBag.Email = SearchEmail;
            ViewBag.Id = SearchId;
            return View(users);
        }

        public ActionResult GetInfo(string id) //управление ролями
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            ApplicationUser b = db.Users.Find(id);
            var userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(db));
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
            ViewBag.HavingRoles = userManager.GetRoles(id);
            ViewBag.AllRoles = roleManager.Roles;
            if (b != null)
            { 
                return View(b);
            }
            return HttpNotFound();
        }

        public ActionResult CreateRole(string userid, string roleid) //Добавление роли
        {
            var userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(db));
            userManager.AddToRole(userid, roleid);

            return RedirectToAction("GetInfo", "Admin", new { id = userid });
        }
        public ActionResult DeleteRole(string userid, string roleid) //Удаление роли
        {
            var userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(db));
            userManager.RemoveFromRole(userid, roleid);

            return RedirectToAction("GetInfo", "Admin", new { id = userid });
        }

    }
}