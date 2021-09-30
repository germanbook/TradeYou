using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace TradeYou.Models
{
    public partial class User
    {
        public User()
        {
            Orders = new HashSet<Order>();
        }

        public int UId { get; set; }

        [Required]
        [Display(Name = "User Name")]
        public string UName { get; set; }

        [Required]
        [Display(Name = "User Password")]
        public string UPassword { get; set; }

        [Required]
        [Display(Name = "Date of Birth")]
        public DateTime UDob { get; set; }

        [Required]
        [Display(Name = "E-mail")]
        [DataType(DataType.EmailAddress)]
        public string UEmail { get; set; }

        [Required]
        [Display(Name = "Account Type")]
        public int UIsAdmin { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
