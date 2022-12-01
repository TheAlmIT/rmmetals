using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.Net;
using RM.Models;
using System.Globalization;
using PagedList.Mvc;
using PagedList;
using System.Threading.Tasks;

namespace RM.Controllers
{
    public class BlogController : Controller
    {
        // GET: Blog
        public ActionResult Index(int? page)
        {
            BlogPost posts = new BlogPost();
            posts.IPagedBlogPostList = posts.GetBlogPostsList().ToPagedList(page ?? 1, 50); 
            return View(posts);
        }
        public ActionResult Create()
        {
           
            return View();
        }
        // POST: Default/Create
        [HttpPost]

        [ValidateAntiForgeryToken]
        public ActionResult Create(BlogPost model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }
                BlogPost request = new BlogPost();
                var userId = User.Identity.GetUserId();
                request.IsActive = true;
                request.CreatedBy = "System";
                request.PostAuthor = userId;
                request.insert(model);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}