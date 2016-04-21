using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Models.ViewModel
{
    public class OrderViewModel
    {
        public Models.Order Order { get; set; }
        public Models.OrderDetails [] OrderDetails { get; set; }
    }
}