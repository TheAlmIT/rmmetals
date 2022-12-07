using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList.Mvc;
using PagedList;
namespace RM.Models
{
    public class Inventory
    {
        public int Id { get; set; }
        public string Loc { get; set; }
        public List<string> Locs { get; set; }

        //Added on May262017
        public string Item { get; set; }
        public List<string> Items { get; set; }


        public string Type { get; set; }
        public List<string> Types { get; set; }

        public string Finish { get; set; }
        public List<string> Finishs { get; set; }

        //Added on May262017 
        public string Thickness { get; set; }
        public List<string> Thicknesss { get; set; }


        public string Gauge { get; set; }
        public List<string> Gauges { get; set; }

        public string Width { get; set; }
        public List<string> Widths { get; set; }

        public string WTNET { get; set; }
        public List<string> WTNETs { get; set; }

        
        //Removed on May262017 by vaidula
        public string NOOFPCS { get; set; }
        public string SelectedPage { get; set; }

        public List<Inventory> ProductList { get; set; }
        public IPagedList<Inventory> IPagedProductsList { get; set; }

        public IEnumerable<SelectListItem> GetLocation()
        {
            Inventory pro = new Inventory();

            pro.ProductList = pro.ProductsList();


            var Location = pro.ProductList.Select(a => a.Loc).Distinct().OrderBy(m => m.ToString()).Where(x => x != "");


            return new SelectList(Location);
        }
        public IEnumerable<SelectListItem> GetItem()
        {
            Inventory pro = new Inventory();

            pro.ProductList = pro.ProductsList();


            var Item = pro.ProductList.Select(a => a.Item).Distinct().OrderBy(m => m.ToString()).Where(x => x != "");


            return new SelectList(Item);
        }

        public new IEnumerable<SelectListItem> GetType()
        {
            Inventory pro = new Inventory();

            pro.ProductList = pro.ProductsList();


            var Type = pro.ProductList.Select(a => a.Type).Distinct().OrderBy(m => m.ToString()).Where(x => x != ""); 

            return new SelectList(Type);
        }
        public IEnumerable<SelectListItem> GetFinish()
        {
            Inventory pro = new Inventory();

            pro.ProductList = pro.ProductsList();


            var Finish = pro.ProductList.Select(a => a.Finish).Distinct();
            Finish = Finish.Where(x=>x!="");
            //List<string> Finish = new List<string>();
            //Finish.Add("2B");
            //Finish.Add("2D");
            //Finish.Add("HRAP");
            //Finish.Add("BA");
            //Finish.Add("POL");

            return new SelectList(Finish);
        }
        public IEnumerable<SelectListItem> GetThickness()
        {
            Inventory pro = new Inventory();

            pro.ProductList = pro.ProductsList();


            var Thickness = pro.ProductList.Select(a => a.Thickness).Distinct().OrderBy(m => m.ToString()).Where(x => x != ""); 

            return new SelectList(Thickness);
        }

        public IEnumerable<SelectListItem> GetGauge()
        {
            Inventory pro = new Inventory();

            pro.ProductList = pro.ProductsList();


            var Gauge = pro.ProductList.Select(a => a.Gauge).Distinct().OrderBy(m => m.ToString()).Where(x=>x!="");
            return new SelectList(Gauge);
        }
        public IEnumerable<SelectListItem> GetWidth()
        {
            //Uncommented Below and Commented Static one by Vaidula 23may2017

            Inventory pro = new Inventory();

            pro.ProductList = pro.ProductsList();

            var Width = pro.ProductList.Select(a => a.Width).Distinct().Where(x => x != "").OrderBy(m => m.ToString()); 
            //List<string> Width = new List<string>();
            //Width.Add("0 - 24");
            //Width.Add("24.1 - 48");
            //Width.Add("48.1 - High");
            //Width.Add("All");

            return new SelectList(Width);
        }
        public IEnumerable<SelectListItem> GetWTNET()
        {
            Inventory pro = new Inventory();

            pro.ProductList = pro.ProductsList();


            var WTNET = pro.ProductList.Select(a => a.WTNET).Distinct().OrderBy(m => m.ToString()).Where(x => x != "");

            return new SelectList(WTNET);
        }

        public List<SelectListItem> PageList
        {
            get
            {
                return new List<SelectListItem>()
                {
                   new SelectListItem() {Text = "50", Value="50" },
                   new SelectListItem() {Text = "100", Value="100" },
                   new SelectListItem() {Text = "150", Value="150" }
                };
            }
        }


        public IEnumerable<SelectListItem> GetPages()
        {
            return PageList.Select(a => new SelectListItem()

            {
                Text = a.Text,
                Value = a.Value,
                Selected = (a.Value == SelectedPage)
            }
            );

        }


        public List<Inventory> ProductsList()
        {
            List<Inventory> productsListItems = new List<Inventory>();

            string cs = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            using (MySqlConnection con = new MySqlConnection(cs))
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("Sp_GetAllProducts", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    Inventory products = new Inventory();
                    products.Id = Convert.ToInt32(rdr["Id"]);
                    products.Loc = rdr["Loc"].ToString();
                    products.Item = rdr["Item"].ToString();
                    products.Type = rdr["Type"].ToString();
                    products.Finish = rdr["Finish"].ToString();
                    products.Thickness = rdr["Thickness"].ToString();
                    products.Gauge = rdr["Gauge"].ToString();
                    products.Width = rdr["Width"].ToString();
                    products.WTNET = rdr["WTNET"].ToString();
                    //products.NOOFPCS = rdr["NOOFPCS"].ToString();
                    productsListItems.Add(products);
                }
            }

            return productsListItems;
        }

        public Inventory productDetails(int id)
        {

            string cs = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            using (MySqlConnection con = new MySqlConnection(cs))
            {
                con.Open();

                MySqlCommand cmd = new MySqlCommand("Sp_GetProductDetails", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@_id", id);
                MySqlDataReader rdr = cmd.ExecuteReader();
                Inventory products = new Inventory();
                while (rdr.Read())
                {
                    products.Loc = rdr["Loc"].ToString();
                    products.Item = rdr["Item"].ToString();
                    products.Type = rdr["Type"].ToString();
                    products.Finish = rdr["Finish"].ToString();
                    products.Thickness = rdr["Thickness"].ToString();
                    products.Gauge = rdr["Gauge"].ToString();
                    products.Width = rdr["Width"].ToString();
                    products.WTNET = rdr["WTNET"].ToString();
                    //products.NOOFPCS = rdr["NOOFPCS"].ToString();
                }

                return products;
            }
        }

        public void UpdateproductDetails()
        {

            string cs = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            using (MySqlConnection con = new MySqlConnection(cs))
            {
                con.Open();

                MySqlCommand cmd = new MySqlCommand("sp_Update_Product", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@p_id", Id);
                cmd.Parameters.AddWithValue("@p_Loc", Loc);
                cmd.Parameters.AddWithValue("@p_Item", Item);
                cmd.Parameters.AddWithValue("@p_Type", Type);
                cmd.Parameters.AddWithValue("@p_Finish", Finish);
                cmd.Parameters.AddWithValue("@p_Thickness", Thickness);
                cmd.Parameters.AddWithValue("@p_Gauge", Gauge);
                cmd.Parameters.AddWithValue("@p_Width", Width);
                cmd.Parameters.AddWithValue("@p_Wtnet", WTNET);
                //cmd.Parameters.AddWithValue("@p_NoofPcs", NOOFPCS);
                cmd.ExecuteNonQuery();
            }
        }

        public void DeleteproductDetails()
        {

            string cs = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            using (MySqlConnection con = new MySqlConnection(cs))
            {
                con.Open();

                MySqlCommand cmd = new MySqlCommand("sp_Delete_Product", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@p_id", Id);                
                cmd.ExecuteNonQuery();
            }
        }
    }
}