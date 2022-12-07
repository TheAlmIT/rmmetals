using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Configuration;

namespace RM.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "Remember this browser?")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        //[StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        [RegularExpression(@"^(?=.{6,})(?=.*[a-z])(?=.*[0-9])(?=.*[A-Z])(?=.*[@#$%^&+=]).*$", ErrorMessage = "Invalid password format, verify above instructions")]        
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        [Display(Name = "Full Name")]
        public string Name { get; set; }

        [DisplayName("Phone Number2")]
        public string PhoneNumber2 { get; set; }

        public string CompanyName { get; set; }

        [Display(Name = "Address line 1")]
        public string Address { get; set; }
        
        [Display(Name = "Address line 2")]
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string CompanyEmail { get; set; }
        public string Fax { get; set; }

        public string UserId { get; set; }

        public bool Approved { get; set; }

        public List<RegisterViewModel> Users { get; set; }

        public List<RegisterViewModel> GetUsers()
        {
           
            List<RegisterViewModel> RVMList = new List<RegisterViewModel>();

            string cs = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (MySqlConnection con = new MySqlConnection(cs))
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("Sp_AllUsers", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
               
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    RegisterViewModel RVM = new RegisterViewModel();
                    RVM.UserId = rdr["UserId"].ToString();
                    RVM.Name= rdr["Name"].ToString();
                    RVM.Email = rdr["Email"].ToString();
                    RVM.PhoneNumber = rdr["PhoneNumber"].ToString(); 
                    RVM.CompanyName = rdr["CompanyName"].ToString();
                    RVM.Address  = rdr["Address"].ToString();
                    RVM.Address2 = rdr["Address2"].ToString(); 
                    RVM.City = rdr["City"].ToString();
                    RVM.State  = rdr["State"].ToString();
                    RVM.Approved = (bool)rdr["Approved"];
                    RVMList.Add(RVM);

                }
            }
            return RVMList;
        }


        public void ApproveUsers(string UserId)
        {
            string cs = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (MySqlConnection con = new MySqlConnection(cs))
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("Sp_ApproveUsers", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@_User_Id", UserId);
                cmd.ExecuteNonQuery();
            }
           
        }


        public void DisApproveUsers(string UserId)
        {
            string cs = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (MySqlConnection con = new MySqlConnection(cs))
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("Sp_DisApproveUsers", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@_User_Id", UserId);
                cmd.ExecuteNonQuery();
            }

        }

        public void DeleteUser(string UserId)
        {
            string cs = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (MySqlConnection con = new MySqlConnection(cs))
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("Sp_DeleteUser", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@_User_Id", UserId);
                cmd.ExecuteNonQuery();
            }

        }

    }

    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}
