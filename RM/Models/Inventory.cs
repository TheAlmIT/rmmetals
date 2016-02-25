﻿using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList.Mvc;
using PagedList;
using System.ComponentModel.DataAnnotations;

namespace RM.Models
{
    public class Inventory
    {
        public int Id { get; set; }
        [RegularExpression("^[0-9A-Za-z ]+$",ErrorMessage ="Enter Valid Location")]
        public string Loc { get; set; }
        [RegularExpression("^[0-9A-Za-z ]+$",ErrorMessage="Enter Valid Type")]
        public string Type { get; set; }
        [RegularExpression("^[0-9A-Za-z ]+$", ErrorMessage ="Enter Valid Finish")]
        public string Finish { get; set; }
        [RegularExpression("^[0-9A-Za-z ]+$", ErrorMessage ="Enter Valid Gauge")]
        public string Gauge { get; set; }
        [RegularExpression("^[0-9A-Za-z ]+$", ErrorMessage ="Enter Valid Width")]
        public string Width { get; set; }
        [RegularExpression("^[0-9A-Za-z ]+$", ErrorMessage ="Enter Valid WTNET")]
        public string WTNET { get; set; }
        public string NOOFPCS { get; set; }      
        public string SelectedPage { get; set; }
        public List<Inventory>  ProductList { get; set; }
        public IPagedList<Inventory> IPagedProductsList { get; set; }

        public IEnumerable<SelectListItem> GetLocation()
        {
            Inventory pro = new Inventory();

            pro.ProductList = pro.ProductsList();


            var Location = pro.ProductList.Select(a => a.Loc).Distinct();
           

            return new SelectList(Location);
        }
        public IEnumerable<SelectListItem> GetType()
        {
            Inventory pro = new Inventory();

            pro.ProductList = pro.ProductsList();


            var Type = pro.ProductList.Select(a => a.Type).Distinct();

            return new SelectList(Type);
        }
        public IEnumerable<SelectListItem> GetFinish()
        {
            Inventory pro = new Inventory();

            pro.ProductList = pro.ProductsList();


            var Finish = pro.ProductList.Select(a => a.Finish).Distinct();

            return new SelectList(Finish);
        }
        public IEnumerable<SelectListItem> GetGauge()
        {
            Inventory pro = new Inventory();

            pro.ProductList = pro.ProductsList();


            var Gauge = pro.ProductList.Select(a =>a.Gauge).Distinct();

            return new SelectList(Gauge);
        }
        public IEnumerable<SelectListItem> GetWidth()
        {
            Inventory pro = new Inventory();

            pro.ProductList = pro.ProductsList();


            var Width = pro.ProductList.Select(a => a.Width).Distinct();

            return new SelectList(Width);
        }
        public IEnumerable<SelectListItem> GetWTNET()
        {
            Inventory pro = new Inventory();

            pro.ProductList = pro.ProductsList();


            var WTNET = pro.ProductList.Select(a =>a.WTNET).Distinct();

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
                Text= a.Text , Value = a.Value, Selected = (a.Value== SelectedPage)
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
                    products.Type = rdr["Type"].ToString();
                    products.Finish = rdr["Finish"].ToString();
                    products.Gauge = rdr["Gauge"].ToString();
                    products.Width = rdr["Width"].ToString();
                    products.WTNET = rdr["WTNET"].ToString();
                    products.NOOFPCS = rdr["NOOFPCS"].ToString();
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
                    products.Type = rdr["Type"].ToString();
                    products.Finish = rdr["Finish"].ToString();
                    products.Gauge = rdr["Gauge"].ToString();
                    products.Width = rdr["Width"].ToString();
                    products.WTNET = rdr["WTNET"].ToString();
                    products.NOOFPCS = rdr["NOOFPCS"].ToString();
                }

                return products;
            }
        }
    }
}