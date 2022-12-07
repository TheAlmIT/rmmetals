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
using Newtonsoft.Json;

namespace RM.Controllers
{
    [Authorize]
    public class InventoryController : Controller
    {
        // GET: Product
        public ActionResult Index(int? page, int? Id, string sortOrder, string Pages, Inventory pro, int? Export)
        {
            ModelState.Clear();
            string submit = Request["submit"];
            ViewBag.Pages = Pages;
            ViewBag.LocSortParm = String.IsNullOrEmpty(sortOrder) ? "Loc_desc" : "";
            ViewBag.ItemSortParm = String.IsNullOrEmpty(sortOrder) ? "Item_desc" : "";
            ViewBag.TypeSortParm = sortOrder == "Type" ? "Type_desc" : "Type";
            ViewBag.FinishSortParm = sortOrder == "Finish" ? "Finish_desc" : "Finish";
            ViewBag.ThicknessSortParm = sortOrder == "Thickness" ? "Thickness_desc" : "Thickness";
            ViewBag.GaugeSortParm = sortOrder == "Gauge" ? "Gauge_desc" : "Gauge";
            ViewBag.WidthSortParm = sortOrder == "Width" ? "Width_desc" : "Width";
            ViewBag.WTNETSortParm = sortOrder == "WTNET" ? "WTNET_desc" : "WTNET";
            //ViewBag.NOOFPCSSortParm = sortOrder == "NOOFPCS" ? "NOOFPCS_desc" : "NOOFPCS";
            //if (string.IsNullOrEmpty(pro.Loc) && string.IsNullOrEmpty(pro.Type) && string.IsNullOrEmpty(pro.Finish) && string.IsNullOrEmpty(pro.Gauge) && string.IsNullOrEmpty(pro.Width) && string.IsNullOrEmpty(pro.WTNET))
            //{
            //    pro.ProductList = new List<Inventory>();

            //}
            if (pro.Locs == null && pro.Items == null && pro.Types == null && pro.Finishs == null && pro.Thicknesss == null && pro.Gauges == null && pro.Widths == null && pro.WTNETs == null)
            {
                pro.ProductList = new List<Inventory>();

            }
            else
            {
                Inventory objExisting = pro;
                pro.ProductList = pro.ProductsList().ToList();


                //List<string> objLocs = (List<string>) objExisting.Locs[0].Split(',');

                if (pro.Locs != null && pro.Locs[0] != "null")
                {
                    pro.ProductList = pro.ProductList.Where(a => objExisting.Locs[0].Replace("\"", "").Replace("[", "").Replace("]", "").Split(',').ToList().Contains(a.Loc)).ToList();
                    pro.Locs = objExisting.Locs[0].Replace("\"", "").Replace("[", "").Replace("]", "").Split(',').ToList();
                
                }
                if (pro.Items != null && pro.Items[0] != "null")
                {
                    pro.ProductList = pro.ProductList.Where(a => objExisting.Items[0].Replace("\"", "").Replace("[", "").Replace("]", "").Split(',').ToList().Contains(a.Item)).ToList();
                    pro.Items = objExisting.Items[0].Replace("\"", "").Replace("[", "").Replace("]", "").Split(',').ToList();
                }
                if (pro.Types != null && pro.Types[0] != "null")
                {
                    pro.ProductList = pro.ProductList.Where(a => objExisting.Types[0].Replace("\"", "").Replace("[", "").Replace("]", "").Split(',').ToList().Contains(a.Type)).ToList();
                    pro.Types = objExisting.Types[0].Replace("\"", "").Replace("[", "").Replace("]", "").Split(',').ToList();
                }
                if (pro.Finishs != null && pro.Finishs[0] != "null")
                {
                    pro.ProductList = pro.ProductList.Where(a => objExisting.Finishs[0].Replace("\"", "").Replace("[", "").Replace("]", "").Split(',').ToList().Contains(a.Finish)).ToList();
                    pro.Finishs = objExisting.Finishs[0].Replace("\"", "").Replace("[", "").Replace("]", "").Split(',').ToList();
                }
                if (pro.Thicknesss != null && pro.Thicknesss[0] != "null")
                {
                    pro.ProductList = pro.ProductList.Where(a => objExisting.Thicknesss[0].Replace("\"", "").Replace("[", "").Replace("]", "").Split(',').ToList().Contains(a.Thickness)).ToList();
                    pro.Thicknesss = objExisting.Thicknesss[0].Replace("\"", "").Replace("[", "").Replace("]", "").Split(',').ToList();
                }
                if (pro.Gauges != null && pro.Gauges[0] != "null")
                {
                    pro.ProductList = pro.ProductList.Where(a => objExisting.Gauges[0].Replace("\"", "").Replace("[", "").Replace("]", "").Split(',').ToList().Contains(a.Gauge)).ToList();
                    pro.Gauges = objExisting.Gauges[0].Replace("\"", "").Replace("[", "").Replace("]", "").Split(',').ToList();
                }
                if (pro.Widths != null && pro.Widths[0] != "null")
                {
                    pro.ProductList = pro.ProductList.Where(a => objExisting.Widths[0].Replace("\"", "").Replace("[", "").Replace("]", "").Split(',').ToList().Contains(a.Width)).ToList();
                    pro.Widths = objExisting.Widths[0].Replace("\"", "").Replace("[", "").Replace("]", "").Split(',').ToList();
                }
                if (pro.WTNETs != null && pro.WTNETs[0] != "null")
                {
                    pro.ProductList = pro.ProductList.Where(a => objExisting.WTNETs[0].Replace("\"", "").Replace("[", "").Replace("]", "").Split(',').ToList().Contains(a.WTNET)).ToList();
                    pro.WTNETs = objExisting.WTNETs[0].Replace("\"", "").Replace("[", "").Replace("]", "").Split(',').ToList();
                }

                if (pro.Locs[0] == "null") pro.Locs = null;
                if (pro.Items[0] == "null") pro.Items = null;
                if (pro.Types[0] == "null") pro.Types = null;
                if (pro.Finishs[0] == "null") pro.Finishs = null;
                if (pro.Thicknesss[0] == "null") pro.Thicknesss = null;
                if (pro.Gauges[0] == "null") pro.Gauges = null;
                if (pro.Widths[0] == "null") pro.Widths = null;
                if (pro.WTNETs[0] == "null") pro.WTNETs = null;
                



                //if (pro.Locs != null)
                //    pro.ProductList = pro.ProductList.Where(a => objExisting.Locs.Contains(a.Loc)).ToList();
                //if (pro.Items != null)
                //  pro.ProductList = pro.ProductList.Where(a => objExisting.Items.Contains(a.Item)).ToList();
                //if (pro.Types != null)
                //    pro.ProductList = pro.ProductList.Where(a => objExisting.Types.Contains(a.Type)).ToList();
                //if (pro.Finishs != null)
                //    pro.ProductList = pro.ProductList.Where(a => objExisting.Finishs.Contains(a.Finish)).ToList();
                //if (pro.Thicknesss != null)
                //    pro.ProductList = pro.ProductList.Where(a => objExisting.Thicknesss.Contains(a.Thickness)).ToList();
                //if (pro.Gauges != null)
                //    pro.ProductList = pro.ProductList.Where(a => objExisting.Gauges.Contains(a.Gauge)).ToList();
                //if (pro.Widths != null)
                //    pro.ProductList = pro.ProductList.Where(a => objExisting.Widths.Contains(a.Width)).ToList();
                //if (pro.WTNETs != null)
                //    pro.ProductList = pro.ProductList.Where(a => objExisting.WTNETs.Contains(a.WTNET)).ToList();




                //pro.ProductList = pro.ProductList.Where(a => a.Loc.ToUpper().Contains(string.IsNullOrEmpty(pro.Loc) ? a.Loc.ToUpper() : pro.Loc.ToUpper())).
                //    Where(a => a.Item.ToUpper().Contains(string.IsNullOrEmpty(pro.Item) ? a.Item.ToUpper() : pro.Item.ToUpper())).
                //    Where(a => a.Type.ToUpper().Contains(string.IsNullOrEmpty(pro.Type) ? a.Type.ToUpper() : pro.Type.ToUpper())).
                //    Where(a => a.Finish.ToUpper().Contains(string.IsNullOrEmpty(pro.Finish) ? a.Finish.ToUpper() : pro.Finish.ToUpper())).
                //    Where(a => a.Thickness.ToUpper().Contains(string.IsNullOrEmpty(pro.Thickness) ? a.Thickness.ToUpper() : pro.Thickness.ToUpper())).
                //    Where(a => a.Gauge.ToUpper().Contains(string.IsNullOrEmpty(pro.Gauge) ? a.Gauge.ToUpper() : pro.Gauge.ToUpper())).
                //    //Where(a => a.Width.ToUpper().Contains(string.IsNullOrEmpty(pro.Width) ? a.Width.ToUpper() : (pro.Width.ToUpper()))).
                //    Where(a => a.WTNET.ToUpper().Contains(string.IsNullOrEmpty(pro.WTNET) ? a.WTNET.ToUpper() : (pro.WTNET.ToUpper()))).ToList();


                //if (pro.Width!=null && !string.IsNullOrEmpty(pro.Width.Trim()) && pro.Width.ToLower() != "all")
                //{
                //    decimal outDecimal = 0;
                //    string[] widths = pro.Width.Split('-');
                //    decimal minWidth = Convert.ToDecimal(widths[0].Trim());
                //    decimal maxWidth = 9999999;
                //    if (widths[1].Trim().ToLower() != "high")
                //    {
                //        maxWidth = Convert.ToDecimal(widths[1].Trim());
                //    }
                //    pro.ProductList = (from item in pro.ProductList
                //                       where decimal.TryParse(item.Width, out outDecimal) && Convert.ToDecimal(item.Width) >= minWidth && Convert.ToDecimal(item.Width) <= maxWidth
                //                       select item).ToList();
                //}
            }

            //List Displayed only after searched in filters

            //Commented By Vaidula 24May2017 for bulkRequest Quote 
            //This code moved to seperate Method
            //if (Id.HasValue)
            //{
            //    RequestedQuote quote = new RequestedQuote();
            //    quote = quote.GetData(User.Identity.GetUserId());
            //    quote.User_Id = User.Identity.GetUserId();
            //    quote.product = new Inventory();
            //    quote.product = quote.product.productDetails(Id.Value);
            //    quote.insert(quote);

            //    var smtpClient = new SmtpClient();
            //    string fromEmailId = System.Configuration.ConfigurationManager.AppSettings["SystemEmailId"];
            //    string toEmailId = System.Configuration.ConfigurationManager.AppSettings["RequestQuoteToEmailId"];

            //    var message = new MailMessage(fromEmailId, toEmailId)
            //    {
            //        Subject = "Requested Quote",
            //        Body = "User Name : " + User.Identity.GetUserName() + Environment.NewLine + "Phone Number : " + quote.PhoneNumber + Environment.NewLine + "IP Address : " + Request.ServerVariables["REMOTE_ADDR"] + Environment.NewLine + "Location : " + quote.product.Loc + Environment.NewLine + "Type : " + quote.product.Type + Environment.NewLine + "Finish : " + quote.product.Finish + Environment.NewLine + "Gauge : " + quote.product.Gauge + Environment.NewLine + "Width : " + quote.product.Width + Environment.NewLine + "Net Wt : " + quote.product.WTNET + Environment.NewLine + "Pieces : " + quote.product.NOOFPCS,

            //    };
            //    smtpClient.Send(message);

            //}

            if (Export.HasValue)
            {
                var grid = new GridView();
                grid.DataSource = from data in pro.ProductList
                                  select new
                                  {

                                      Location = data.Loc,
                                      Item = data.Item,
                                      Type = data.Type,
                                      Finish = data.Finish,
                                      Thickness = data.Thickness,
                                      Gauge = data.Gauge,
                                      Width = data.Width,
                                      WTNET = data.WTNET//,
                                      //NOOFPCS = 0
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
                case "Item_desc":
                    pro.ProductList = pro.ProductList.OrderByDescending(s => s.Item).ToList();
                    break;
                case "Finish":
                    pro.ProductList = pro.ProductList.OrderBy(s => s.Finish).ToList();
                    break;
                case "Finish_desc":
                    pro.ProductList = pro.ProductList.OrderByDescending(s => s.Finish).ToList();
                    break;
                case "Thickness_desc":
                    pro.ProductList = pro.ProductList.OrderByDescending(s => s.Thickness).ToList();
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
                //case "NOOFPCS_desc":
                //    pro.ProductList = pro.ProductList.OrderByDescending(s => s.NOOFPCS).ToList();
                //    break;
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

                if (pro.Locs != null && pro.Locs.Count > 0)
                {
                    string strLocs = string.Join(",", pro.Locs.Select(p => "'" + p.ToString() + "'"));
                    searchData.Add(1, strLocs);
                }
                if (!(string.IsNullOrEmpty(pro.Item)))
                {
                    searchData.Add(1, pro.Item);
                }
                if (!(string.IsNullOrEmpty(pro.Type)))
                {
                    searchData.Add(2, pro.Type);
                }
                if (!(string.IsNullOrEmpty(pro.Finish)))
                {
                    searchData.Add(3, pro.Finish);
                }
                if (!(string.IsNullOrEmpty(pro.Thickness)))
                {
                    searchData.Add(3, pro.Thickness);
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

                //if (string.IsNullOrEmpty(pro.Item) && string.IsNullOrEmpty(pro.Item) && string.IsNullOrEmpty(pro.Type) && string.IsNullOrEmpty(pro.Finish) && string.IsNullOrEmpty(pro.Thickness) && string.IsNullOrEmpty(pro.Gauge) && string.IsNullOrEmpty(pro.Width) && string.IsNullOrEmpty(pro.WTNET))
                //{
                //    pro.ProductList = new List<Inventory>();

                //}
                if (pro.Locs == null && pro.Items == null && pro.Types == null && pro.Finishs == null && pro.Thicknesss == null && pro.Gauges == null && pro.Widths == null && pro.WTNETs == null)
                {
                    pro.ProductList = new List<Inventory>();

                }
                else
                {
                    Inventory objExisting = pro;
                    pro.ProductList = pro.ProductsList().ToList();

                    if (pro.Locs != null)
                        pro.ProductList = pro.ProductList.Where(a => objExisting.Locs.Contains(a.Loc)).ToList();
                    if (pro.Items != null)
                        pro.ProductList = pro.ProductList.Where(a => objExisting.Items.Contains(a.Item)).ToList();
                    if (pro.Types != null)
                        pro.ProductList = pro.ProductList.Where(a => objExisting.Types.Contains(a.Type)).ToList();
                    if (pro.Finishs != null)
                        pro.ProductList = pro.ProductList.Where(a => objExisting.Finishs.Contains(a.Finish)).ToList();
                    if (pro.Thicknesss != null)
                        pro.ProductList = pro.ProductList.Where(a => objExisting.Thicknesss.Contains(a.Thickness)).ToList();
                    if (pro.Gauges != null)
                        pro.ProductList = pro.ProductList.Where(a => objExisting.Gauges.Contains(a.Gauge)).ToList();
                    if (pro.Widths != null)
                        pro.ProductList = pro.ProductList.Where(a => objExisting.Widths.Contains(a.Width)).ToList();
                    if (pro.WTNETs != null)
                        pro.ProductList = pro.ProductList.Where(a => objExisting.WTNETs.Contains(a.WTNET)).ToList();

                    //pro.ProductList = pro.ProductList.Where(a => pro.Locs != null && pro.Locs.Contains(a.Loc)).

                    //    Where(a => a.Type.ToUpper().Contains(string.IsNullOrEmpty(pro.Type) ? a.Type.ToUpper() : pro.Type.ToUpper())).
                    //    Where(a => a.Finish.ToUpper().Contains(string.IsNullOrEmpty(pro.Finish) ? a.Finish.ToUpper() : pro.Finish.ToUpper())).
                    //    //Where(a => a.Thickness.ToUpper().Contains(string.IsNullOrEmpty(pro.Thickness) ? a.Thickness.ToUpper() : pro.Thickness.ToUpper())).
                    //    Where(a => a.Gauge.ToUpper().Contains(string.IsNullOrEmpty(pro.Gauge) ? a.Gauge.ToUpper() : pro.Gauge.ToUpper())).

                    //    //vaidula 23May2017
                    //    Where(a => a.Width.ToUpper().Contains(string.IsNullOrEmpty(pro.Width) ? a.Width.ToUpper() : (pro.Width.ToUpper()))).
                    //    Where(a => a.WTNET.ToUpper().Contains(string.IsNullOrEmpty(pro.WTNET) ? a.WTNET.ToUpper() : (pro.WTNET.ToUpper()))).ToList();




                    //pro.ProductList = pro.ProductList.Where(a => a.Loc.ToUpper().Contains(string.IsNullOrEmpty(pro.Loc) ? a.Loc.ToUpper() : pro.Loc.ToUpper())).
                    // Where(a => a.Item.ToUpper().Contains(string.IsNullOrEmpty(pro.Item) ? a.Type.ToUpper() : pro.Item.ToUpper())).
                    //Where(a => a.Type.ToUpper().Contains(string.IsNullOrEmpty(pro.Type) ? a.Type.ToUpper() : pro.Type.ToUpper())).
                    //Where(a => a.Finish.ToUpper().Contains(string.IsNullOrEmpty(pro.Finish) ? a.Finish.ToUpper() : pro.Finish.ToUpper())).
                    ////Where(a => a.Thickness.ToUpper().Contains(string.IsNullOrEmpty(pro.Thickness) ? a.Thickness.ToUpper() : pro.Thickness.ToUpper())).
                    //Where(a => a.Gauge.ToUpper().Contains(string.IsNullOrEmpty(pro.Gauge) ? a.Gauge.ToUpper() : pro.Gauge.ToUpper())).

                    ////vaidula 23May2017
                    //Where(a => a.Width.ToUpper().Contains(string.IsNullOrEmpty(pro.Width) ? a.Width.ToUpper() : (pro.Width.ToUpper()))).
                    //Where(a => a.WTNET.ToUpper().Contains(string.IsNullOrEmpty(pro.WTNET) ? a.WTNET.ToUpper() : (pro.WTNET.ToUpper()))).ToList();

                    //commented by vaidula 23may2017
                    //if (pro.Width != null && !string.IsNullOrEmpty(pro.Width.Trim()) && pro.Width.ToLower() != "all")
                    //{
                    //    decimal outDecimal = 0;
                    //    string[] widths = pro.Width.Split('-');
                    //    decimal minWidth = Convert.ToDecimal(widths[0].Trim());
                    //    decimal maxWidth = 9999999;
                    //    if (widths[1].Trim().ToLower() != "high")
                    //    {
                    //      maxWidth = Convert.ToDecimal(widths[1].Trim());
                    //    }
                    //    pro.ProductList = (from item in pro.ProductList
                    //                       where decimal.TryParse(item.Width,out outDecimal) && Convert.ToDecimal(item.Width) >= minWidth && Convert.ToDecimal(item.Width) <= maxWidth
                    //        select item).ToList();
                    //}



                    pro.Locs = objExisting.Locs;
                    pro.Items = objExisting.Items;
                    pro.Types = objExisting.Types;
                    pro.Finishs = objExisting.Finishs;
                    pro.Gauges = objExisting.Gauges;
                    pro.Thicknesss = objExisting.Thicknesss;
                    pro.Widths = objExisting.Widths;
                    pro.WTNETs = objExisting.WTNETs;


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

        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Product/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Product/Create
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Create(HttpPostedFileBase file)
        {

            if (ModelState.IsValid)
            {
                if (file != null && file.ContentLength > 0)
                {
                    if (file.FileName.EndsWith(".csv"))
                    {
                        Stream stream = file.InputStream;
                        DataTable csvTable = new DataTable();
                        using (CsvReader csvReader =
                            new CsvReader(new StreamReader(stream), true))
                        {
                            csvTable.Load(csvReader);
                        }
                        if (!csvTable.Columns.Contains("LOC") || !csvTable.Columns.Contains("ITEM") || !csvTable.Columns.Contains("TYPE") ||
                            !csvTable.Columns.Contains("FINISH") || !csvTable.Columns.Contains("THCKNS") || !csvTable.Columns.Contains("GAUGE") ||
                            !csvTable.Columns.Contains("WIDTH") || !csvTable.Columns.Contains("WT NET"))
                        {
                            ModelState.AddModelError("File", "This file columns are not matched with web site columns");
                            return View();
                        }

                        string conn1 = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                        MySqlConnection con1 = new MySqlConnection(conn1);
                        string query1 = "truncate table products";
                        MySqlCommand cmd1 = new MySqlCommand(query1, con1);
                        con1.Open();
                        cmd1.ExecuteNonQuery();

                        foreach (DataRow row in csvTable.Rows)
                        {


                            string query = string.Format("Insert into products(Loc,Item,Type,Finish,Thickness,Gauge,Width,WTNET) Values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}')",
                                row[csvTable.Columns["LOC"]].ToString().Replace("'", "''"), row[csvTable.Columns["ITEM"]].ToString().Replace("'", "''"),
                                row[csvTable.Columns["TYPE"]].ToString().Replace("'", "''"), row[csvTable.Columns["FINISH"]].ToString().Replace("'", "''"),
                                row[csvTable.Columns["THCKNS"]].ToString().Replace("'", "''"), row[csvTable.Columns["GAUGE"]].ToString().Replace("'", "''"),
                                row[csvTable.Columns["WIDTH"]].ToString().Replace("'", "''"), row[csvTable.Columns["WT NET"]].ToString().Replace("'", "''"));


                            MySqlCommand cmd = new MySqlCommand(query, con1);
                            cmd.ExecuteNonQuery();


                        }
                        con1.Close();
                        ViewBag.ShowData = csvTable.Rows.Count;

                    }


                    else
                    {
                        ModelState.AddModelError("File", "This file format is not supported");
                        return View();
                    }

                }
                else
                {
                    ModelState.AddModelError("File", "Please Upload Your file");
                    return View();
                }
            }
            return View();
        }

        public JsonResult GetLocation(string term)
        {

            Inventory pro = new Inventory();
            List<string> Location;

            pro.ProductList = pro.ProductsList();

            Location = pro.ProductList.Where(a => a.Loc.StartsWith(term.ToUpper())).Select(b => b.Loc).Distinct().ToList();


            return Json(Location, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetItem(string term)
        {

            Inventory pro = new Inventory();
            List<string> Item;

            pro.ProductList = pro.ProductsList();

            Item = pro.ProductList.Where(a => a.Item.StartsWith(term.ToUpper())).Select(b => b.Item).Distinct().ToList();


            return Json(Item, JsonRequestBehavior.AllowGet);
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

        public JsonResult GetThickness(string term)
        {

            Inventory pro = new Inventory();
            List<string> Thickness;

            pro.ProductList = pro.ProductsList();

            Thickness = pro.ProductList.Where(a => a.Thickness.StartsWith(term.ToUpper())).Select(b => b.Thickness).Distinct().ToList();


            return Json(Thickness, JsonRequestBehavior.AllowGet);
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

        public ActionResult Approval(FormCollection formCollection,string submit)
        {
            if (formCollection["remove"] != null)
            {
                return DeleteUsers(formCollection);
            }
            else
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

        // [Authorize(Roles = "Admin")]
        //public ActionResult DeleteUsers()
        //{
        //    RegisterViewModel user = new RegisterViewModel();
        //    user.Users = user.GetUsers();

        //    return View(user.Users);
        //}

        //[HttpPost]
        // [Authorize(Roles = "Admin")]
        public ActionResult DeleteUsers(FormCollection formCollection)
        {
            RegisterViewModel user = new RegisterViewModel();
            var Users = formCollection.AllKeys.Where(c => c.StartsWith("chk")).ToList();
            for (var i = 0; i < Users.Count(); i++)
            {
                var val = Users[i].Split('=');
                string UserId = val[1];
                user.DeleteUser(UserId);
            }
            user.Users = user.GetUsers();
            return View(user.Users.Where(a => a.Approved == false));
        }

        public void Update(Inventory inventory)
        {
            inventory.UpdateproductDetails();
        }

        public void Delete(Inventory inventory)
        {
            //inventory.DeleteproductDetails();
        }


        #region Rquest Quote
        [HttpPost]
        public void BulkRequestQuote(List<int> rqids = null)
        {
            foreach (int Id in rqids)
            {
                RequestedQuote quote = new RequestedQuote();
                quote = quote.GetData(User.Identity.GetUserId());
                quote.User_Id = User.Identity.GetUserId();
                quote.product = new Inventory();
                quote.product = quote.product.productDetails(Id);
                quote.insert(quote);

                var smtpClient = new SmtpClient();
                string fromEmailId = System.Configuration.ConfigurationManager.AppSettings["SystemEmailId"];
                string toEmailId = System.Configuration.ConfigurationManager.AppSettings["RequestQuoteToEmailId"];

                var message = new MailMessage(fromEmailId, toEmailId)
                {
                    Subject = "Requested Quote",
                    Body = "User Name : " + User.Identity.GetUserName() + Environment.NewLine + "Phone Number : " + quote.PhoneNumber + Environment.NewLine + "IP Address : " + Request.ServerVariables["REMOTE_ADDR"] + Environment.NewLine + "Location : " + quote.product.Loc + Environment.NewLine + "Item : " + quote.product.Item + Environment.NewLine + "Type : " + quote.product.Type + Environment.NewLine + "Finish : " + quote.product.Finish + Environment.NewLine + "Thickness : " + quote.product.Thickness + Environment.NewLine + "Gauge : " + quote.product.Gauge + Environment.NewLine + "Width : " + quote.product.Width + Environment.NewLine + "Net Wt : " + quote.product.WTNET + Environment.NewLine, // + "Pieces : " + quote.product.NOOFPCS,

                };

                message.CC.Add(new MailAddress(User.Identity.Name));

                smtpClient.Send(message);
            }

        }



        public void ExporttoExcel(int? Export, string Locs, string Items, string Types, string Finishs,
            string Thicknesss, string Gauges, string Widths, string WTNETs)
        {
            Inventory pro = new Inventory();
            pro.Locs = JsonConvert.DeserializeObject<List<string>>(Locs);
            pro.Items = JsonConvert.DeserializeObject<List<string>>(Items);
            pro.Types = JsonConvert.DeserializeObject<List<string>>(Types);
            pro.Finishs = JsonConvert.DeserializeObject<List<string>>(Finishs);
            pro.Thicknesss = JsonConvert.DeserializeObject<List<string>>(Thicknesss);
            pro.Gauges = JsonConvert.DeserializeObject<List<string>>(Gauges);
            pro.Widths = JsonConvert.DeserializeObject<List<string>>(Widths);
            pro.WTNETs = JsonConvert.DeserializeObject<List<string>>(WTNETs);
            if (pro.Locs == null && pro.Items == null && pro.Types == null && pro.Finishs == null && pro.Thicknesss == null && pro.Gauges == null && pro.Widths == null && pro.WTNETs == null)
            {
                pro.ProductList = new List<Inventory>();

            }
            else
            {
                Inventory objExisting = pro;
                pro.ProductList = pro.ProductsList().ToList();

                if (pro.Locs != null)
                    pro.ProductList = pro.ProductList.Where(a => objExisting.Locs.Contains(a.Loc)).ToList();
                if (pro.Items != null)
                    pro.ProductList = pro.ProductList.Where(a => objExisting.Items.Contains(a.Item)).ToList();
                if (pro.Types != null)
                    pro.ProductList = pro.ProductList.Where(a => objExisting.Types.Contains(a.Type)).ToList();
                if (pro.Finishs != null)
                    pro.ProductList = pro.ProductList.Where(a => objExisting.Finishs.Contains(a.Finish)).ToList();
                if (pro.Thicknesss != null)
                    pro.ProductList = pro.ProductList.Where(a => objExisting.Thicknesss.Contains(a.Thickness)).ToList();
                if (pro.Gauges != null)
                    pro.ProductList = pro.ProductList.Where(a => objExisting.Gauges.Contains(a.Gauge)).ToList();
                if (pro.Widths != null)
                    pro.ProductList = pro.ProductList.Where(a => objExisting.Widths.Contains(a.Width)).ToList();
                if (pro.WTNETs != null)
                    pro.ProductList = pro.ProductList.Where(a => objExisting.WTNETs.Contains(a.WTNET)).ToList();

            }
            // if (Export.HasValue)
            //{
            var grid = new GridView();
            grid.DataSource = from data in pro.ProductList
                              select new
                              {

                                  Location = data.Loc,
                                  Item = data.Item,
                                  Type = data.Type,
                                  Finish = data.Finish,
                                  Thickness = data.Thickness,
                                  Gauge = data.Gauge,
                                  Width = data.Width,
                                  WTNET = data.WTNET//,
                                  //NOOFPCS = 0
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

            //}
        }
        #endregion Rquest Quote
    }
}
