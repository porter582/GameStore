using System;
using System.Collections.Generic;
using System.Text;

namespace Game.Models.ViewModels
{
    public class OrderDetailsCart
    {
        public OrderHeader OrderHeader { get; set; }
        public List<ShoppingCart> listCart { get; set; }
    }
}
