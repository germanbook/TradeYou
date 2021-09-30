using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace TradeYou.Models
{
    public partial class Order
    {
        public int OId { get; set; }
        public int UId { get; set; }
        public int PId { get; set; }

        [Display(Name = "Payment Type")]
        public int? OPaymentype { get; set; }

        [Display(Name = "Shipping Type")]
        public int? OShippingtype { get; set; }

        [Display(Name = "Order Number")]
        public DateTime? OOrderumber { get; set; }

        [Display(Name = "Quantity")]
        public int OQuantity { get; set; }

        [Display(Name = "Product")]
        public virtual Product PIdNavigation { get; set; }

        [Display(Name = "User")]
        public virtual User UIdNavigation { get; set; }
    }
}
