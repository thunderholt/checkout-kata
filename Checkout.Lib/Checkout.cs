using System.Collections.Generic;

namespace Checkout
{
    public class Checkout : ICheckout
    {
        private IProductRepository productRepository = null;
        private List<BasketItem> basketItems = new List<BasketItem>();

        public Checkout(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }

        public void Scan(string sku)
        {
            this.basketItems.Add(new BasketItem
            {
                Sku = sku
            });
        }

        public decimal GetTotalPrice()
        {
            decimal totalPrice = 0;

            foreach (var basketItem in this.basketItems)
            {
                totalPrice += this.CalculateBasketItemPrice(basketItem);
            }

            return totalPrice;
        }

        private decimal CalculateBasketItemPrice(BasketItem basketItem)
        {
            var product = this.productRepository.GetProduct(basketItem.Sku);

            return product.UnitPrice;
        }

        private class BasketItem
        {
            public string Sku { get; set; }
        }
    }
}
