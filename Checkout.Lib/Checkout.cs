using System;

namespace Checkout
{
    public class Checkout : ICheckout
    {
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
