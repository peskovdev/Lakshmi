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
    public class EditorController : Controller //Отвечает за редактирование контента страницы
    {
        ApplicationDbContext db = new ApplicationDbContext();
        //!!!!-------------MY PAGE | Моя страница----------!!!!
        // GET: /Editor/Delete
        public ActionResult MyPage(string id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            ApplicationUser applicationUser = db.Users.Include(t => t.Photos).FirstOrDefault(t => t.Id == id);
            if (applicationUser == null)
            {
                return HttpNotFound();
            }
            return View(applicationUser);
        }
        // GET: /Editor/Delete
        //!!!!-------------MY PAGE | Моя страница----------!!!!

        
        //!!!!-------------ADD PHOTOS | Добавление фотографий----------!!!!
        // GET: /Editor/AddPhoto

        [HttpGet]
        public ActionResult AddPhoto()//айди для вторичного ключа(айди должен быть юзера)
        {
            return PartialView();
        }
        [HttpPost]
        public ActionResult AddPhoto(Photo photo, HttpPostedFileBase uploadImage)
        {
            if (uploadImage != null && uploadImage.ContentType.Contains("image"))
            {
                byte[] imageData = null;
                //считываем следующий переданный файл в массив данных
                // считываем переданный файл в массив байтов
                using (var binaryReader = new BinaryReader(uploadImage.InputStream))
                {
                    imageData = binaryReader.ReadBytes(uploadImage.ContentLength);
                }
                // установка массива байтов
                photo.Image = imageData;
                db.Photos.Add(photo);
                db.SaveChanges();
                return RedirectToAction("MyPage", "Editor", new { id = photo.ApplicationUserId });
            }
            return View();
        }
        // GET: /Editor/AddPhoto
        //!!!!-------------ADD PHOTOS | Добавление фотографий----------!!!!
       

        //!!!!-------------DELETE PHOTOS | Удаление фотографий----------!!!!
        // GET: /Editor/AddPhoto

        [HttpGet]
        public ActionResult Delete(int id) //Удалить фото
        {
            Photo b = db.Photos.Find(id);
            if (b == null)
            {
                return HttpNotFound();
            }
            if (b.ApplicationUserId == User.Identity.GetUserId() || User.IsInRole("Admin") || User.IsInRole("Moder"))
            {
                    return PartialView(b);
            }
            return Content("Вы не можете удалить не свою фотографию, для данной операции вы должны обладать правами модератора");
        }
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Photo b = db.Photos.Find(id);
            if (b == null)
            {
                return HttpNotFound();
            }
            if (!(b.ApplicationUserId == User.Identity.GetUserId() || User.IsInRole("Admin") || User.IsInRole("Moder")))
            {
                return Content("Вы не можете удалить не свою фотографию, для данной операции вы должны обладать правами модератора");
            }
            string idback = b.ApplicationUserId;
            db.Photos.Remove(b);
            db.SaveChanges();
            return RedirectToAction("MyPage", "Editor", new { id = idback });
        }

        // GET: /Editor/Delete
        //!!!!-------------DELETE PHOTOS | Удаление фотографий----------!!!!

        //!!!!-------------СHANGE PHOTOS | Изменение фотографий----------!!!!
        // GET: /Editor/AddPhoto

        [HttpGet]
        public ActionResult Change(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            Photo b = db.Photos.Find(id);
            if (b == null)
            {
                return HttpNotFound();
            }
            if (!(b.ApplicationUserId == User.Identity.GetUserId() || User.IsInRole("Admin") || User.IsInRole("Moder")))
            {
                return Content("Вы не можете изменить не свою фотографию, для данной операции вы должны обладать правами модератора");
            }
            return PartialView(b);
        }
        [HttpPost]
        public ActionResult Change(Photo photo)
        {
            if (!(photo.ApplicationUserId == User.Identity.GetUserId() || User.IsInRole("Admin") || User.IsInRole("Moder")))
            {
                return Content("Вы не можете изменить не свою фотографию, для данной операции вы должны обладать правами модератора");
            }
            db.Entry(photo).State = EntityState.Modified;
            db.SaveChanges();   
            return RedirectToAction("MyPage", "Editor", new { id = photo.ApplicationUserId });
        }

        //!!!!-------------ADD Comments | Добавление Коментариев к фоткам----------!!!!
        // GET: /Editor/AddComment
        [HttpGet]
        public ActionResult AddComment(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            var comments = db.Comments.Include(p => p.ApplicationUser).Include(p => p.Photo).Where(m => m.PhotoId == id);
            ViewBag.hozyain = db.Photos.Include(p => p.ApplicationUser).Where(m => m.Id == id);
            return View(comments);
        }

        [HttpPost]
        public ActionResult AddComment(Comment comment)
        {
            //Коментарий в бд
            if (comment.Record == null)
            {
                return HttpNotFound();
            }
            db.Comments.Add(comment);
            db.SaveChanges();
            // перенаправляем на главную страницу
            return RedirectToAction("Addcomment", "Editor", new { id = comment.PhotoId });
        }
        // GET: /Editor/AddComment
        //!!!!-------------ADD Comments | Добавление Коментариев к фоткам----------!!!!

        //!!!!-------------DELETE PHOTOS | Удаление фотографий----------!!!!
        // GET: /Editor/AddPhoto

        [HttpGet]
        public ActionResult DeleteComment(int? id) //Удалить фото
        {
            Comment b = db.Comments.Find(id);
            if (id == null || b == null)
            {
                return HttpNotFound();
            }
            if (b.ApplicationUserId == User.Identity.GetUserId() || User.IsInRole("Admin") || User.IsInRole("Moder"))
            {
                return PartialView(b);
            }
            return Content("Вы не можете удалить не свою фотографию, для данной операции вы должны обладать правами модератора");
        }
        [HttpPost, ActionName("DeleteComment")]
        public ActionResult DeleteCommentConfirmed(int? id)
        {
            Comment b = db.Comments.Find(id);
            if (id == null || b == null)
            {
                return HttpNotFound();
            }
            if (!(b.ApplicationUserId == User.Identity.GetUserId() || User.IsInRole("Admin") || User.IsInRole("Moder")))
            {
                return Content("Вы не можете удалить не свою фотографию, для данной операции вы должны обладать правами модератора");
            }
            int idback = b.PhotoId;
            db.Comments.Remove(b);
            db.SaveChanges();
            return RedirectToAction("AddComment", "Editor", new { id = idback });
        }

        // GET: /Editor/Delete
        //!!!!-------------DELETE PHOTOS | Удаление фотографий----------!!!!
        [HttpGet]
        public ActionResult ChangeComment(int? id)
        {
            Comment b = db.Comments.Find(id);
            if (id == null)
            {
                return HttpNotFound();
            }
            if (b == null)
            {
                return HttpNotFound();
            }
            if (!(b.ApplicationUserId == User.Identity.GetUserId() || User.IsInRole("Admin") || User.IsInRole("Moder")))
            {
                return Content("Вы не можете изменить не свою фотографию, для данной операции вы должны обладать правами модератора");
            }
            return PartialView(b);
        }
        [HttpPost]
        public ActionResult ChangeComment(Comment comment)
        {
            if (!(comment.ApplicationUserId == User.Identity.GetUserId() || User.IsInRole("Admin") || User.IsInRole("Moder")))
            {
                return Content("Вы не можете изменить не свою фотографию, для данной операции вы должны обладать правами модератора");
            }
            db.Entry(comment).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("AddComment", "Editor", new { id = comment.PhotoId });
        }
    }
}