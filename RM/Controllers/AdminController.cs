using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RM.Models;
using System.IO;
using System.Data;
using System.Xml;
using System.Configuration;
using System.Data.SqlClient;
using System.Data.OleDb;
using MySql.Data.MySqlClient;
using PagedList.Mvc;
using PagedList;
using Microsoft.AspNet.Identity;
using System.Net.Mail;
using System.Globalization;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Web.Configuration;
using LumenWorks.Framework.IO.Csv;
using System.Web.Security;

namespace RM.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        public ActionResult Index(int? page, int? Id, string sortOrder, string Pages, Inventory pro, int? Export)
        {
            ModelState.Clear();
            string submit = Request["submit"];
            ViewBag.Pages = Pages;
            ViewBag.LocSortParm = String.IsNullOrEmpty(sortOrder) ? "Loc_desc" : "";
            ViewBag.TypeSortParm = sortOrder == "Type" ? "Type_desc" : "Type";
            ViewBag.FinishSortParm = sortOrder == "Finish" ? "Finish_desc" : "Finish";
            ViewBag.GaugeSortParm = sortOrder == "Gauge" ? "Gauge_desc" : "Gauge";
            ViewBag.WidthSortParm = sortOrder == "Width" ? "Width_desc" : "Width";
            ViewBag.WTNETSortParm = sortOrder == "WTNET" ? "WTNET_desc" : "WTNET";
            ViewBag.NOOFPCSSortParm = sortOrder == "NOOFPCS" ? "NOOFPCS_desc" : "NOOFPCS";
            if (string.IsNullOrEmpty(pro.Loc) && string.IsNullOrEmpty(pro.Type) && string.IsNullOrEmpty(pro.Finish) && string.IsNullOrEmpty(pro.Gauge) && string.IsNullOrEmpty(pro.Width) && string.IsNullOrEmpty(pro.WTNET))
            {
                pro.ProductList = new List<Inventory>();

            }
            else
            {
                pro.ProductList = pro.ProductsList().ToList();
                pro.ProductList = pro.ProductList.Where(a => a.Loc.ToUpper().Contains(string.IsNullOrEmpty(pro.Loc) ? a.Loc.ToUpper() : pro.Loc.ToUpper())).
                    Where(a => a.Type.ToUpper().Contains(string.IsNullOrEmpty(pro.Type) ? a.Type.ToUpper() : pro.Type.ToUpper())).
                    Where(a => a.Finish.ToUpper().Contains(string.IsNullOrEmpty(pro.Finish) ? a.Finish.ToUpper() : pro.Finish.ToUpper())).
                    Where(a => a.Gauge.ToUpper().Contains(string.IsNullOrEmpty(pro.Gauge) ? a.Gauge.ToUpper() : pro.Gauge.ToUpper())).
                    Where(a => a.Width.ToUpper().Contains(string.IsNullOrEmpty(pro.Width) ? a.Width.ToUpper() : (pro.Width.ToUpper()))).
                    Where(a => a.WTNET.ToUpper().Contains(string.IsNullOrEmpty(pro.WTNET) ? a.WTNET.ToUpper() : (pro.WTNET.ToUpper()))).ToList();
            }

            //List Displayed only after searched in filters

            if (Id.HasValue)
            {
                RequestedQuote quote = new RequestedQuote();
                quote = quote.GetData(User.Identity.GetUserId());
                quote.User_Id = User.Identity.GetUserId();
                quote.product = new Inventory();
                quote.product = quote.product.productDetails(Id.Value);
                quote.insert(quote);

                var smtpClient = new SmtpClient();
                string fromEmailId = System.Configuration.ConfigurationManager.AppSettings["SystemEmailId"];
                string toEmailId = System.Configuration.ConfigurationManager.AppSettings["RequestQuoteToEmailId"];

                var message = new MailMessage(fromEmailId, toEmailId)
                {
                    Subject = "Requested Quote",
                    Body = "User Name : " + User.Identity.GetUserName() + Environment.NewLine + "Phone Number : " + quote.PhoneNumber + Environment.NewLine + "IP Address : " + Request.ServerVariables["REMOTE_ADDR"] + Environment.NewLine + "Location : " + quote.product.Loc + Environment.NewLine + "Type : " + quote.product.Type + Environment.NewLine + "Finish : " + quote.product.Finish + Environment.NewLine + "Gauge : " + quote.product.Gauge + Environment.NewLine + "Width : " + quote.product.Width + Environment.NewLine + "Net Wt : " + quote.product.WTNET + Environment.NewLine + "Pieces : " + quote.product.NOOFPCS,

                };
                smtpClient.Send(message);

            }

            if (Export.HasValue)
            {
                var grid = new GridView();
                grid.DataSource = from data in pro.ProductList
                                  select new
                                  {

                                      Location = data.Loc,
                                      Type = data.Type,
                                      Finish = data.Finish,
                                      Gauge = data.Gauge,
                                      Width = data.Width,
                                      WTNET = data.WTNET,
                                      NOOFPCS = 0
                                  };
                grid.DataBind();
                Response.AddHeader("content-disposition", "attachment; filename=MyExcelFile.xls");
                Response.ContentType = "application/ms-excel";
                Response.Charset = "";
                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);

                grid.RenderControl(htw);

                Response.Output.Write(sw.ToString());

                Response.End();

            }

            switch (sortOrder)
            {
                case "Loc_desc":
                    pro.ProductList = pro.ProductList.OrderByDescending(s => s.Loc).ToList();
                    break;
                case "Finish":
                    pro.ProductList = pro.ProductList.OrderBy(s => s.Finish).ToList();
                    break;
                case "Finish_desc":
                    pro.ProductList = pro.ProductList.OrderByDescending(s => s.Finish).ToList();
                    break;
                case "Gauge":
                    pro.ProductList = pro.ProductList.OrderBy(s => s.Gauge).ToList();
                    break;
                case "Gauge_desc":
                    pro.ProductList = pro.ProductList.OrderByDescending(s => s.Gauge).ToList();
                    break;
                case "Type":
                    pro.ProductList = pro.ProductList.OrderBy(s => s.Type).ToList();
                    break;
                case "Type_desc":
                    pro.ProductList = pro.ProductList.OrderByDescending(s => s.Type).ToList();
                    break;
                case "Width":
                    pro.ProductList = pro.ProductList.OrderBy(s => s.Width).ToList();
                    break;
                case "Width_desc":
                    pro.ProductList = pro.ProductList.OrderByDescending(s => s.Width).ToList();
                    break;

                case "WTNET":
                    pro.ProductList = pro.ProductList.OrderBy(s => s.WTNET).ToList();
                    break;
                case "WTNET_desc":
                    pro.ProductList = pro.ProductList.OrderByDescending(s => s.WTNET).ToList();
                    break;

                case "NOOFPCS":
                    pro.ProductList = pro.ProductList.OrderBy(s => s.NOOFPCS).ToList();
                    break;
                case "NOOFPCS_desc":
                    pro.ProductList = pro.ProductList.OrderByDescending(s => s.NOOFPCS).ToList();
                    break;
                default:
                    pro.ProductList = pro.ProductList.OrderBy(s => s.Loc).ToList();
                    break;
            }

            List<SelectListItem> listitems = new List<SelectListItem>();

            listitems.Add(new SelectListItem { Text = "50 items ", Value = "50" });
            listitems.Add(new SelectListItem { Text = "100 items ", Value = "100" });
            listitems.Add(new SelectListItem { Text = "150 items ", Value = "150" });
            ViewBag.ListItems = listitems;



            if (string.IsNullOrEmpty(Pages))
            {
                pro.IPagedProductsList = pro.ProductList.ToPagedList(page ?? 1, 50);     //Default Paging is 50
                return View(pro);
            }
            else
            {
                pro.IPagedProductsList = pro.ProductList.ToPagedList(page ?? 1, Convert.ToInt32(Pages));
                return View(pro);
            }

        }
        [HttpPost]
        public ActionResult Index(int? page, string Pages, Inventory pro)
        {
            List<SelectListItem> listitems = new List<SelectListItem>();
            listitems.Add(new SelectListItem { Text = "50 items ", Value = "50" });
            listitems.Add(new SelectListItem { Text = "100 items ", Value = "100" });
            listitems.Add(new SelectListItem { Text = "150 items ", Value = "150" });
            ViewBag.ListItems = listitems;


            if (ModelState.IsValid)
            {
                string submit = Request["submit"];
                Dictionary<int, string> searchData = new Dictionary<int, string>();

                if (!(string.IsNullOrEmpty(pro.Loc)))
                {
                    searchData.Add(1, pro.Loc);
                }
                if (!(string.IsNullOrEmpty(pro.Type)))
                {
                    searchData.Add(2, pro.Type);
                }
                if (!(string.IsNullOrEmpty(pro.Finish)))
                {
                    searchData.Add(3, pro.Finish);
                }
                if (!(string.IsNullOrEmpty(pro.Gauge)))
                {
                    searchData.Add(4, pro.Gauge);
                }
                if (!(string.IsNullOrEmpty(pro.Width)))
                {
                    searchData.Add(5, pro.Width);
                }
                if (!(string.IsNullOrEmpty(pro.WTNET)))
                {
                    searchData.Add(6, pro.WTNET);
                }
                if (!(string.IsNullOrEmpty(submit)))
                {
                    SearchedData search = new SearchedData();
                    search.Filerdata = new FilterData();
                    search.Filerdata = search.Filerdata.GetData(User.Identity.GetUserId());
                    search.Filerdata.IPAddress = Request.ServerVariables["REMOTE_ADDR"];
                    search.Details = searchData;
                    search.Filerdata.user_Id = User.Identity.GetUserId();
                    search.insert(search);
                }

                if (string.IsNullOrEmpty(pro.Loc) && string.IsNullOrEmpty(pro.Type) && string.IsNullOrEmpty(pro.Finish) && string.IsNullOrEmpty(pro.Gauge) && string.IsNullOrEmpty(pro.Width) && string.IsNullOrEmpty(pro.WTNET))
                {
                    pro.ProductList = new List<Inventory>();

                }
                else
                {
                    pro.ProductList = pro.ProductsList().ToList();
                    pro.ProductList = pro.ProductList.Where(a => a.Loc.ToUpper().Contains(string.IsNullOrEmpty(pro.Loc) ? a.Loc.ToUpper() : pro.Loc.ToUpper())).
                        Where(a => a.Type.ToUpper().Contains(string.IsNullOrEmpty(pro.Type) ? a.Type.ToUpper() : pro.Type.ToUpper())).
                        Where(a => a.Finish.ToUpper().Contains(string.IsNullOrEmpty(pro.Finish) ? a.Finish.ToUpper() : pro.Finish.ToUpper())).
                        Where(a => a.Gauge.ToUpper().Contains(string.IsNullOrEmpty(pro.Gauge) ? a.Gauge.ToUpper() : pro.Gauge.ToUpper())).
                        Where(a => a.Width.ToUpper().Contains(string.IsNullOrEmpty(pro.Width) ? a.Width.ToUpper() : (pro.Width.ToUpper()))).
                        Where(a => a.WTNET.ToUpper().Contains(string.IsNullOrEmpty(pro.WTNET) ? a.WTNET.ToUpper() : (pro.WTNET.ToUpper()))).ToList();
                }

                if (string.IsNullOrEmpty(Pages))
                {

                    pro.IPagedProductsList = pro.ProductList.ToPagedList(page ?? 1, 50);   //Default Paging is 50
                    return View(pro);
                }
                else
                {
                    pro.IPagedProductsList = pro.ProductList.ToPagedList(page ?? 1, Convert.ToInt32(Pages));
                    return View(pro);
                }
            }
            else
            {
                return View(pro);
            }
        }

        //// GET: Admin/Details/5
        //public ActionResult Details(int id)
        //{
        //    return View();
        //}

        //// GET: Admin/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        //// POST: Admin/Create
        //[HttpPost]
        //public ActionResult Create(FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add insert logic here

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //// GET: Admin/Edit/5
        //public ActionResult Edit(int id)
        //{
        //    return View();
        //}

        //// POST: Admin/Edit/5
        //[HttpPost]
        //public ActionResult Edit(int id, FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add update logic here

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //// GET: Admin/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        //// POST: Admin/Delete/5
        //[HttpPost]
        //public ActionResult Delete(int id, FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add delete logic here

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        public JsonResult GetLocation(string term)
        {

            Inventory pro = new Inventory();
            List<string> Location;

            pro.ProductList = pro.ProductsList();

            Location = pro.ProductList.Where(a => a.Loc.StartsWith(term.ToUpper())).Select(b => b.Loc).Distinct().ToList();


            return Json(Location, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetType(string term)
        {

            Inventory pro = new Inventory();
            List<string> Type;

            pro.ProductList = pro.ProductsList();

            Type = pro.ProductList.Where(a => a.Type.StartsWith(term.ToUpper())).Select(b => b.Type).Distinct().ToList();


            return Json(Type, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetFinish(string term)
        {

            Inventory pro = new Inventory();
            List<string> Finish;

            pro.ProductList = pro.ProductsList();

            Finish = pro.ProductList.Where(a => a.Finish.StartsWith(term.ToUpper())).Select(b => b.Finish).Distinct().ToList();


            return Json(Finish, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetGauge(string term)
        {

            Inventory pro = new Inventory();
            List<string> Gauge;

            pro.ProductList = pro.ProductsList();

            Gauge = pro.ProductList.Where(a => a.Gauge.StartsWith(term.ToUpper())).Select(b => b.Gauge).Distinct().ToList();


            return Json(Gauge, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetWidth(string term)
        {

            Inventory pro = new Inventory();
            List<string> Width;

            pro.ProductList = pro.ProductsList();

            Width = pro.ProductList.Where(a => a.Width.StartsWith(term.ToUpper())).Select(b => b.Width).Distinct().ToList();


            return Json(Width, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetWTNET(string term)
        {

            Inventory pro = new Inventory();
            List<string> WTNET;

            pro.ProductList = pro.ProductsList();

            WTNET = pro.ProductList.Where(a => a.WTNET.StartsWith(term.ToUpper())).Select(b => b.WTNET).Distinct().ToList();


            return Json(WTNET, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Approval()
        {
            RegisterViewModel user = new RegisterViewModel();
            user.Users = user.GetUsers();
            return View(user.Users.Where(a => a.Approved == false));
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Approval(FormCollection formCollection)
        {
            RegisterViewModel Allusers = new RegisterViewModel();
            Allusers.Users = Allusers.GetUsers();


            var Users = formCollection.AllKeys.Where(c => c.StartsWith("chk")).ToList();
            for (var i = 0; i < Users.Count(); i++)
            {
                var val = Users[i].Split('=');
                string UserId = val[1];
                Allusers.ApproveUsers(UserId);

                var user = Allusers.Users.Where(a => a.UserId == UserId).Single();

                var smtpClient = new SmtpClient();
                string fromEmailId = System.Configuration.ConfigurationManager.AppSettings["SystemEmailId"];

                var message = new MailMessage(fromEmailId, user.Email)
                {
                    Subject = "Registration Approved",
                    Body = "Hello " + user.Name + Environment.NewLine + "You registration is approved by Admin, now you can login to the system. Please click <a href='http://www.rm-metals.com/rm-metals/Account/Login'>here</a>."
                };
                message.IsBodyHtml = true;
                smtpClient.Send(message);
            }
            Allusers.Users = Allusers.GetUsers();
            return View(Allusers.Users.Where(a => a.Approved == false));
        }

        [Authorize(Roles = "Admin")]
        public ActionResult InActive()
        {
            RegisterViewModel user = new RegisterViewModel();
            user.Users = user.GetUsers();

            return View(user.Users.Where(a => a.Approved == true));
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult InActive(FormCollection formCollection)
        {
            RegisterViewModel user = new RegisterViewModel();
            var Users = formCollection.AllKeys.Where(c => c.StartsWith("chk")).ToList();
            for (var i = 0; i < Users.Count(); i++)
            {
                var val = Users[i].Split('=');
                string UserId = val[1];
                user.DisApproveUsers(UserId);

            }
            user.Users = user.GetUsers();
            return View(user.Users.Where(a => a.Approved == true));
        }

        public void Update(Inventory inventory)
        {
            inventory.UpdateproductDetails();
        }

        public void Delete(Inventory inventory)
        {
            inventory.DeleteproductDetails();
        }
    }
}
