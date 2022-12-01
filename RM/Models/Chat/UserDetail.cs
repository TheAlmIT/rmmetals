using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RM.Models
{
    public class UserDetail
    {
        public string ConnectionId { get; set; }
        public string UserName { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsFreeFlag { get; set; }
    }
}