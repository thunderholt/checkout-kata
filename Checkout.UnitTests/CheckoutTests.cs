using System;
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

            productRepository.AddProduct(new Product
            {
                Sku = "B",
                UnitPrice = 20
            });

            productRepository.AddProduct(new Product
            {
                Sku = "C",
                UnitPrice = 30
            });

            productRepository.AddProduct(new Product
            {
                Sku = "D",
                UnitPrice = 10,
                BundleQuantity = 2,
                BundleMultiplier = 1
            });

            productRepository.AddProduct(new Product
            {
                Sku = "E",
                UnitPrice = 20,
                BundleQuantity = 3,
                BundleMultiplier = 2
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

        [TestMethod]
        public void Checkout_CalculatesCorrectPriceFor_MultipleScans_MultipleProducts()
        {
            var checkout = new Checkout(this.productRepository);

            checkout.Scan("A");
            checkout.Scan("B");
            checkout.Scan("C");

            Assert.AreEqual(60, checkout.GetTotalPrice());
        }

        [TestMethod]
        public void Checkout_CalculatesCorrectPriceFor_SingleScan_SingleProduct_TwoForOneBundle()
        {
            var checkout = new Checkout(this.productRepository);

            checkout.Scan("D");

            Assert.AreEqual(10, checkout.GetTotalPrice());
        }

        [TestMethod]
        public void Checkout_CalculatesCorrectPriceFor_MultipleScans_SingleProduct_TwoForOneBundle()
        {
            var checkout = new Checkout(this.productRepository);

            checkout.Scan("D");
            checkout.Scan("D");
            checkout.Scan("D");
            checkout.Scan("D");
            checkout.Scan("D");

            Assert.AreEqual(30, checkout.GetTotalPrice());
        }

        [TestMethod]
        public void Checkout_CalculatesCorrectPriceFor_SingleScan_SingleProduct_ThreeForTwoBundle()
        {
            var checkout = new Checkout(this.productRepository);

            checkout.Scan("E");

            Assert.AreEqual(20, checkout.GetTotalPrice());
        }

        [TestMethod]
        public void Checkout_CalculatesCorrectPriceFor_MultipleScans_SingleProduct_ThreeForTwoBundle()
        {
            var checkout = new Checkout(this.productRepository);

            checkout.Scan("E");
            checkout.Scan("E");
            checkout.Scan("E");
            checkout.Scan("E");
            checkout.Scan("E");
            checkout.Scan("E");
            checkout.Scan("E");

            Assert.AreEqual(100, checkout.GetTotalPrice());
        }

        [TestMethod]
        public void Checkout_CalculatesCorrectPriceFor_MultipleScans_MultipleProducts_VariousBundles()
        {
            var checkout = new Checkout(this.productRepository);

            // 2 x £10 = £20
            checkout.Scan("A");
            checkout.Scan("A");

            // 3 x £20 = £60
            checkout.Scan("B");
            checkout.Scan("B");
            checkout.Scan("B");

            // 4 x £40 = £120
            checkout.Scan("C");
            checkout.Scan("C");
            checkout.Scan("C");
            checkout.Scan("C");

            // £10 + £10 + £10 = £30
            checkout.Scan("D");
            checkout.Scan("D");
            checkout.Scan("D");
            checkout.Scan("D");
            checkout.Scan("D");

            // 2 x £20 + 2 x £20 + £20 = £100
            checkout.Scan("E");
            checkout.Scan("E");
            checkout.Scan("E");
            checkout.Scan("E");
            checkout.Scan("E");
            checkout.Scan("E");
            checkout.Scan("E");

            Assert.AreEqual(330, checkout.GetTotalPrice());
        }

        [TestMethod]
        public void Checkout_CalculatesCorrectPriceFor_MultipleScans_MultipleProducts_VariousBundles_MixedUpScanOrder()
        {
            var checkout = new Checkout(this.productRepository);

            checkout.Scan("C");
            checkout.Scan("E");
            checkout.Scan("D");
            checkout.Scan("C");
            checkout.Scan("E");
            checkout.Scan("A");
            checkout.Scan("B");
            checkout.Scan("D");
            checkout.Scan("E");
            checkout.Scan("C");
            checkout.Scan("D");
            checkout.Scan("E");
            checkout.Scan("A");
            checkout.Scan("E");
            checkout.Scan("E");
            checkout.Scan("B");
            checkout.Scan("D");
            checkout.Scan("B");
            checkout.Scan("C");
            checkout.Scan("D");
            checkout.Scan("E");

            Assert.AreEqual(330, checkout.GetTotalPrice());
        }

        [TestMethod]
        [ExpectedException(typeof(ProductNotFoundException))]
        public void Checkout_ThrowsCorrectExceptionWhenUnrecognisedSkuIsScanned()
        {
            var checkout = new Checkout(this.productRepository);

            checkout.Scan("F");

            Assert.AreEqual(10, checkout.GetTotalPrice());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Checkout_ThrowsCorrectExceptionWhenNullSkuIsScanned()
        {
            var checkout = new Checkout(this.productRepository);

            checkout.Scan(null);

            Assert.AreEqual(10, checkout.GetTotalPrice());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Checkout_ThrowsCorrectExceptionWhenEmptySkuIsScanned()
        {
            var checkout = new Checkout(this.productRepository);

            checkout.Scan("");

            Assert.AreEqual(10, checkout.GetTotalPrice());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Checkout_ThrowsCorrectExceptionWhenWhitespaceSkuIsScanned()
        {
            var checkout = new Checkout(this.productRepository);

            checkout.Scan(" \r\n\t");

            Assert.AreEqual(10, checkout.GetTotalPrice());
        }

        [TestMethod]
        public void Checkout_ScansAreProperlyRegisteredWhenCorrectSkuIsSurroundedByWhiteSpace()
        {
            var checkout = new Checkout(this.productRepository);

            checkout.Scan(" A");
            checkout.Scan("B ");
            checkout.Scan(" \r\n\t C \r\n\t ");

            Assert.AreEqual(60, checkout.GetTotalPrice());
        }
    }
}
