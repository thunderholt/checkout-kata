using System;
using System.Collections.Generic;
using System.Linq;

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
            if (sku == null)
            {
                throw new ArgumentNullException(nameof(sku));
            }

            var product = this.productRepository.GetProduct(sku);
            if (product == null)
            {
                throw new ProductNotFoundException();
            }

            var basketItem = this.CoalesceBasketItem(sku);
            basketItem.Quantity++;
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

        private BasketItem CoalesceBasketItem(string sku)
        {
            var basketItem = this.basketItems.FirstOrDefault(bi => bi.Sku.Equals(sku, StringComparison.InvariantCultureIgnoreCase));
            if (basketItem == null)
            {
                basketItem = new BasketItem
                {
                    Sku = sku
                };

                this.basketItems.Add(basketItem);
            }

            return basketItem;
        }

        private decimal CalculateBasketItemPrice(BasketItem basketItem)
        {
            decimal basketItemPrice = 0;

            var product = this.productRepository.GetProduct(basketItem.Sku);

            int remainderQuantity = basketItem.Quantity;
            int bundleQuantity = 0;

            if (product.BundleQuantity > 0 && product.BundleMultiplier > 0)
            {
                bundleQuantity = Math.DivRem(basketItem.Quantity, product.BundleQuantity, out remainderQuantity);
            }

            basketItemPrice =
                bundleQuantity * product.BundleMultiplier * product.UnitPrice +
                remainderQuantity * product.UnitPrice;

            return basketItemPrice;
        }

        private class BasketItem
        {
            public string Sku { get; set; }
            public int Quantity { get; set; }
        }
    }
}
