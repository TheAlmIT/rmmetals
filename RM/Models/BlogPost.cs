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
    public class BlogPost
    {
        public int PostId { get; set; }
        public string Title { get; set; }
        // This attributes allows your HTML Content to be sent up  
        [AllowHtml]
        public string Body { get; set; }
        public string PostAuthor { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public Boolean IsActive { get; set; }
        public IPagedList<BlogPost> IPagedBlogPostList { get; set; }

        public void insert()
        {

            string cs = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;



            using (MySqlConnection con = new MySqlConnection(cs))
            {
                con.Open();

                MySqlCommand cmd = new MySqlCommand("Sp_AddBlogPost", con);

                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@_Title", Title);
                cmd.Parameters.AddWithValue("@_Body", Body);
                cmd.Parameters.AddWithValue("@_PostAuthor", PostAuthor);
                cmd.Parameters.AddWithValue("@_CreatedBy", CreatedBy);
                cmd.Parameters.AddWithValue("@_CreatedOn", DateTime.Now);
                cmd.Parameters.AddWithValue("@_IsActive", IsActive);
                cmd.ExecuteNonQuery();

            }


        }

        public List<BlogPost> GetBlogPostsList()
        {
            List<BlogPost> posts = new List<BlogPost>();

            string cs = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            using (MySqlConnection con = new MySqlConnection(cs))
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("Sp_GetBlogPosts", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    BlogPost post = new BlogPost();
                    post.PostId = Convert.ToInt32(rdr["PostId"]);
                    post.Title = Convert.ToString(rdr["Title"]);
                    post.Body = Convert.ToString(rdr["Body"]);
                    post.PostAuthor = Convert.ToString(rdr["PostAuthor"]);
                    post.IsActive = Convert.ToBoolean(rdr["IsActive"]);
                    post.CreatedBy = Convert.ToString(rdr["CreatedBy"]);
                    post.CreatedOn = Convert.ToDateTime(rdr["CreatedOn"]);
                    post.ModifiedBy = Convert.ToString(rdr["ModifiedBy"]);
                    if (rdr["ModifiedOn"] != null && rdr["ModifiedOn"] != DBNull.Value)
                        post.ModifiedOn = Convert.ToDateTime(rdr["ModifiedOn"]);

                    posts.Add(post);
                }
            }

            return posts;
        }

    }
}