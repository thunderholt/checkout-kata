﻿namespace Checkout
{
    public class Product
    {
        public string Sku { get; set; }
        public decimal UnitPrice { get; set; }
        public int BundleQuantity { get; set; }
        public decimal BundlePrice { get; set; }
    }
}
