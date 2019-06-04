using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BlogSystem.Models;

namespace BlogSystem.Controllers
{
    public class ArticlesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Articles
        public ActionResult Index()
        {
            return View(db.Articles.ToList());
        }

        // GET: Articles/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {

                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Article article = db.Articles.Find(id);
            string image = Convert.ToBase64String(article.Photo);
             ViewBag.imgSrc = string.Format("data:0; base64, {1}", MimeMapping.GetMimeMapping(image), image);
            List<Comment> comments = db.Comments.Where(m => m.ArticleId == id).Select(m=>m).ToList<Comment>();
            ArticleCommentsModelView articleCommentsModelView = new ArticleCommentsModelView()
            {
                Article = article,
                Comments = comments,
            };
            if (article == null)
            {
                return HttpNotFound();
            }
            return View(articleCommentsModelView);
        }
        // Add Comments
        [HttpPost]
        public PartialViewResult AddComments(int id, string CommentContent)
        {
            db.Comments.Add(new Comment() { Content = CommentContent, ArticleId = id });
            db.SaveChanges();
            return PartialView("_Comments", db.Comments.Where(co => co.ArticleId == id));
        }


        // GET: Articles/Create
        public ActionResult Create()
        {
            ViewBag.Cat = new SelectList(db.Categoris.ToList(), "CategoryId", "Name");
           
            return View();
        }

        // POST: Articles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Exclude = "Photo")] Article article, HttpPostedFileBase Photo)
        {
            
            if (ModelState.IsValid)
            {

                if (Photo != null)
                {
                    article.Photo = new byte[Photo.ContentLength];
                    Photo.InputStream.Read(article.Photo, 0, Photo.ContentLength);
                }
                else
                {
                    // Get image path  
                    string imgPath = Server.MapPath("~/Images/default.png");
                    // Convert image to byte array  
                    article.Photo = System.IO.File.ReadAllBytes(imgPath);
                }

                db.Articles.Add(article);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Cat = new SelectList(db.Categoris.ToList(), "CategoryId", "Name");

            return View(article);
        }

        // GET: Articles/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Article article = db.Articles.Find(id);
            
            if (article == null)
            {
                return HttpNotFound();
            }
            string image = Convert.ToBase64String(article.Photo);
            ViewBag.imgSrc = string.Format("data:0; base64, {1}", MimeMapping.GetMimeMapping(image), image);
            ViewBag.Cat = new SelectList(db.Categoris.ToList(), "CategoryId", "Name", article.CategoryId.ToString());


            return View(article);
        }

        // POST: Articles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Exclude = "Photo")]Article article, HttpPostedFileBase Photo)
        {
            if (ModelState.IsValid)
            {
                
                //db.Entry(article).State = EntityState.Modified;
                Article articleNew= db.Articles.SingleOrDefault(a=>a.ArticleId == article.ArticleId);
                articleNew.CategoryId = article.CategoryId;
                articleNew.Content = article.Content;
                articleNew.Date = DateTime.Now;
                articleNew.Title = article.Title;
                if (Photo != null)
                {
                    articleNew.Photo = new byte[Photo.ContentLength];
                    Photo.InputStream.Read(articleNew.Photo, 0, Photo.ContentLength);
                }
             
                db.SaveChanges();
                return RedirectToAction("Details", new { id = article.ArticleId});
            }
            return View(article);
        }

        // GET: Articles/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Article article = db.Articles.Find(id);
            if (article == null)
            {
                return HttpNotFound();
            }
            return View(article);
        }

        // POST: Articles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Article article = db.Articles.Find(id);
            db.Articles.Remove(article);
            db.SaveChanges();
            return RedirectToAction("Index", "Home");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
