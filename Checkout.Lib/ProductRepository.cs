using System.Collections.Generic;
using System.Linq;

namespace Checkout
{
    public class ProductRepository : IProductRepository
    {
        private List<Product> products = new List<Product>();

        public void AddProduct(Product product)
        {
            this.products.Add(product);
        }

        public Product GetProduct(string sku)
        {
            return this.products.FirstOrDefault(p => p.Sku == sku);
        }
    }
}
