﻿using ecommerce.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ecommerce.Models
{
    public class CartModel
    {
        public Product Product { get; set; }
        public int Quantity { get; set; }
        
    }
}