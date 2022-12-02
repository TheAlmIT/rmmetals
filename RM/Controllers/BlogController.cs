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
using RM.Common;

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

        //[ValidateAntiForgeryToken]
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
                request.PostAuthor = userId ?? "Admin";
                request.Title = model.Title;
                request.Body = model.Body;
                request.insert();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.errorMessage = ex.Message;
                return View(model);
            }
        }
        private void fbpost(BlogPost blogPost)
        {
            try
            {
                var postValues = new Dictionary<string, string>();

                // list of available parameters available @ http://developers.facebook.com/docs/reference/api/post
                postValues.Add("access_token", Session["accessToken"].ToString());
                postValues.Add("message", blogPost.Body);

                string facebookWallMsgId = string.Empty;
                string response;
                MethodResult header = Helper.SubmitPost(string.Format("https://graph.facebook.com/{0}/feed", Session["uid"].ToString()),
                                                            Helper.BuildPostString(postValues),
                                                            out response);

                if (header.returnCode == MethodResult.ReturnCode.Success)
                {
                    var deserialised =
                        Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(response);
                    facebookWallMsgId = deserialised["id"];
                    //return RedirectToAction("CreatedSuccessfully");
                }

            }
            catch
            {

            }
        }
    }
}