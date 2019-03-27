using System;

namespace Checkout
{
    public class Checkout : ICheckout
    {
        public Checkout(IProductRepository productRepository)
        {
            throw new NotImplementedException();
        }

        public decimal GetTotalPrice()
        {
            throw new NotImplementedException();
        }

        public void Scan(string sku)
        {
            throw new NotImplementedException();
        }
    }
}
