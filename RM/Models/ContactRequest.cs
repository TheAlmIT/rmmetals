using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;

namespace RM.Models
{
    public class ContactRequest
    {
        public int ContactId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }       
        public string ContactNumber { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string Notes { get; set; }

        public void insert(ContactRequest user)
        {

            string cs = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;



            using (MySqlConnection con = new MySqlConnection(cs))
            {
                con.Open();

                MySqlCommand cmd = new MySqlCommand("Sp_AddContactRequest", con);

                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@_UserName", UserName);
                cmd.Parameters.AddWithValue("@_Email", Email);
                cmd.Parameters.AddWithValue("@_ContactNumber", ContactNumber);
                cmd.Parameters.AddWithValue("@_CreatedBy", CreatedBy);
                cmd.Parameters.AddWithValue("@_CreatedOn", DateTime.Now);
                cmd.Parameters.AddWithValue("@_Notes", Notes);
                cmd.ExecuteNonQuery();

            }


        }

    }
}