using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Lakshmi.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;

namespace Lakshmi.Controllers
{

    [Authorize]
    public class HomeController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index() //Лента фотографий
        {
            var photos = db.Photos.Include(p => p.ApplicationUser);
            return View(photos.ToList());
        }

        public ActionResult FindUser(string SearchName, string SearchEmail)
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
            ViewBag.Name = SearchName;
            ViewBag.Email = SearchEmail;
            return View(users);
        }
        public ActionResult FindPhoto(string SearchTitle)
        {
            var photos = from m in db.Photos select m;
            if (!String.IsNullOrEmpty(SearchTitle))
            {
                photos = photos.Where(s => s.Name.Contains(SearchTitle));
            }
            ViewBag.Name = SearchTitle;
            return View(photos);
        }
        public ActionResult FindComment(string SearchRecord)
        {
            var comments = from m in db.Comments select m;
            if (!String.IsNullOrEmpty(SearchRecord))
            {
                comments = comments.Where(s => s.Record.Contains(SearchRecord));
            }
            comments = comments.Include(p => p.Photo).Include(p => p.ApplicationUser);
            ViewBag.Record = SearchRecord;
            return View(comments);
        }

        //!!!!-------------INFO ABOUT PHOTO | ПОДРОБНЫЙ ПРОСМОТР ФОТО----------!!!!
        // GET: /Editor/PhotoInfo, А вообще это частичное представление
        [HttpGet]
        public ActionResult PhotoInfo(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            ViewBag.comments = db.Comments.Include(p => p.ApplicationUser).Where(m => m.PhotoId == id);
            var b = db.Photos.Include(p => p.ApplicationUser).Where(m => m.Id == id);
            if (b != null)
            {
                return PartialView(b.ToList());
            }
            return HttpNotFound();
        }
        // GET: /Editor/PhotoInfo, А вообще это частичное представление
        //!!!!-------------INFO ABOUT PHOTO | ПОДРОБНЫЙ ПРОСМОТР ФОТО----------!!!!
    }
}