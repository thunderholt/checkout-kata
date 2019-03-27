using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Checkout.UnitTests
{
    [TestClass]
    public class CheckoutTests
    {
        private IProductRepository productRepository = null;

        [TestInitialize]
        public void Init()
        {
            this.productRepository = new ProductRepository();

            productRepository.AddProduct(new Product
            {
                Sku = "A",
                UnitPrice = 10
            });
        }

        [TestMethod]
        public void Checkout_CalculatesCorrectPriceFor_SingleScan_SingleProduct()
        {
            var checkout = new Checkout(this.productRepository);

            checkout.Scan("A");

            Assert.AreEqual(10, checkout.GetTotalPrice());
        }

        [TestMethod]
        public void Checkout_CalculatesCorrectPriceFor_MultipleScans_SingleProduct()
        {
            var checkout = new Checkout(this.productRepository);

            checkout.Scan("A");
            checkout.Scan("A");
            checkout.Scan("A");

            Assert.AreEqual(30, checkout.GetTotalPrice());
        }
    }
}
