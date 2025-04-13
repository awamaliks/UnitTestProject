using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitTestProjectHandsOn.Controllers;
using UnitTestProjectHandsOn.Data;
using UnitTestProjectHandsOn.Models;
using UnitTestProjectHandsOn.Services;

namespace UnitTestProjectHandsOn.Test
{
    public class ProductIntegrationTest
    {
        private AppDbContext _context;
        private ProductService _productService;
        private ProductController _controller;


        [SetUp]
        public void Setup()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            _context = new AppDbContext(configuration);

            // Reset and seed database
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

            SeedProducts();

            _productService = new ProductService(_context);
            _controller = new ProductController(_productService);
        }

        [Test]
        public void AddProduct_ShouldAddProductToDatabase()
        {
            // Arrange
            var newProduct = new Product
            {
                ProductName = "Smartwatch",
                ProductDescription = "This is smart Watch",
                ProductPrice = 20000
            };

            // Act
            var result = _controller.AddProduct(newProduct);

            // Assert
            //var product = _context.Products
            //    .OrderByDescending(p => p.ProductId == result.ProductId)
            //    .FirstOrDefault();

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ProductId, Is.EqualTo(newProduct.ProductId));
        }

        [Test]
        [TestCase("IPhone")]
        [TestCase("Laptop")]
        [TestCase("TV")]
       // [TestCase("TestProduct")]
        public void CheckProductExistOrNotByProductName_Product(string productName)
        {
            // Act
            var products = _controller.ProductList();
            var match = products.Any(p => p.ProductName == productName);

            // Assert
            Assert.That(match, Is.True, $"Product with name '{productName}' should exist.");
        }

        private void SeedProducts()
        {
            _context.Products.AddRange(new List<Product>
            {
                new Product
                {
                    ProductName = "IPhone",
                    ProductDescription = "IPhone 12",
                    ProductPrice = 55000,
                    ProductStock = 10
                },
                new Product
                {
                    ProductName = "Laptop",
                    ProductDescription = "HP Pavilion",
                    ProductPrice = 100000,
                    ProductStock = 20
                },
                new Product
                {
                    ProductName = "TV",
                    ProductDescription = "Samsung Smart TV",
                    ProductPrice = 35000,
                    ProductStock = 30
                }
            });

            _context.SaveChanges();
        }


    }
}
