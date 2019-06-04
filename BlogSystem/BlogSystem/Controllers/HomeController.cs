using BlogSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace BlogSystem.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        static int page = 1; static int? categorId;

        int pageSize = 5;
        public ActionResult Index()
        {
            page = 1;
            ViewBag.Cat = new SelectList(db.Categoris.ToList(), "CategoryId", "Name");

            return View(db.Articles.Include(m => m.Category).OrderByDescending(p => p.Date).Take(pageSize).ToList());
            //return RedirectToAction("ShowArticles");

        }

        [HttpPost]
        public PartialViewResult FilterArticles(int? CategoryId)
        {
            page = 1;
            categorId = CategoryId;
            if (CategoryId == null)
            {
                return PartialView("_Articles", db.Articles.Include(m => m.Category).OrderByDescending(p => p.Date).Take(pageSize).ToList());

            }
            return PartialView("_Articles", db.Articles.Include(m => m.Category).Where(a=>a.CategoryId == CategoryId).OrderByDescending(p => p.Date).Take(pageSize));
        }





        public ActionResult ShowArticles()
        {
            int total = 0;
            if (categorId == null)
            {
                 total = db.Articles.Count();
            }
            else
            {
                total = db.Articles.Where(m=>m.CategoryId == categorId).Count();

            }

            page += 1; // set current page number, must be >= 1 (ideally this value will be passed to this logic/function from outside)

            var skip = pageSize * (page - 1);

            var canPage = skip < total;

            if (!canPage) // do what you wish if you can page no further
                return PartialView("_Articles", new Article());

            List<Article> articles = new List<Article>();


            if (categorId == null)
            {
                articles = db.Articles.Include(x => x.Category).Select(p => p).OrderByDescending(p => p.Date)
                         .Skip(skip)
                         .Take(pageSize)
                         .ToList();

            }
            else
            {
                articles = db.Articles.Include(x => x.Category).Where(a => a.CategoryId == categorId).Select(p => p).OrderByDescending(p => p.Date)
                         .Skip(skip)
                         .Take(pageSize)
                         .ToList();
            }
         

            return PartialView("_Articles", articles);
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}