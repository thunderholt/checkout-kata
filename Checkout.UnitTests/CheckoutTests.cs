﻿using Microsoft.VisualStudio.TestTools.UnitTesting;

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
    }
}
