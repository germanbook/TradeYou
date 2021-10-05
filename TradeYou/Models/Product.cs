using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace TradeYou.Models
{
    public partial class Product
    {
        public Product()
        {
            Orders = new HashSet<Order>();
        }

        public int PId { get; set; }

        [Required]
        [Display(Name = "Product Name")]
        public string PProductname { get; set; }

        [Required]
        [Display(Name = "Price")]
        [Range(0, double.MaxValue, ErrorMessage = "Price cannot be negative!")]
        public double PPrice { get; set; }

        [Required]
        [Display(Name = "Quantity")]
        [Range(0, int.MaxValue, ErrorMessage = "Quantity cannot be negative!")]
        public int PQuantity { get; set; }

        [Required]
        [Display(Name = "Made")]
        public string PMade { get; set; }

        [Required]
        [Display(Name = "Condition")]
        public int PNewUsed { get; set; }

        [Display(Name = "Image")]
        public string PImagePath { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
