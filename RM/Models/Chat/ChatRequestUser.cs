using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RM.Models
{
    public class ChatRequestUser
    {

        [Required]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Contact Number")]
        public bool ContactNumber { get; set; }
        [Required]
        [Display(Name = "Message")]
        public string Notes { get; set; }
    }
}