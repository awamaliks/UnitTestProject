using Castle.Core.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ProductApi.Controllers;
using ProductApi.Data;
using ProductApi.Models;
using ProductApi.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestProject.Test
{
    public class ProductIntegrationTest
    {
        private readonly AppDbContext _context;
        private readonly ProductService _productService;
        private readonly ProductController _controller;

        public ProductIntegrationTest()
        {
             var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            _context = new AppDbContext(configuration);

            // OPTIONAL: Reset database state for clean test
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

            // Seed data
            SeedProducts();

            //bool canConnect = _context.Database.CanConnect();           
            _productService = new ProductService(_context);
            _controller = new ProductController(_productService);
        }

        [Fact]
        public void AddProduct_ShouldAddProductToDatabase()
        {
            // Arrange
            var newProduct = new Product { ProductName = "Smartwatch", ProductDescription="This is smart Watch", ProductPrice = 20000 };

            // Act
            var result = _controller.AddProduct(newProduct);

            // Assert           
                var product = _context.Products.OrderByDescending(p => p.ProductId == result.ProductId).FirstOrDefault();
                Assert.NotNull(product);
                Assert.Equal("Smartwatch", product.ProductName);
            
        }

        [Fact]
        public void GetProductList_ProductList()
        {
            // act
            var result = _controller.ProductList();

            // assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count()); // Match with seeded data
        }


        [Fact]
        public void GetProductByID_Product()
        {
            // Act
            var result = _controller.GetProductById(2);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.ProductId);
            Assert.Equal("Laptop", result.ProductName);
        }

        [Theory]
        [InlineData("IPhone")]
        [InlineData("Laptop")]
        [InlineData("TV")]
        public void CheckProductExistOrNotByProductName_Product(string productName)
        {
            // Act
            var products = _controller.ProductList();
            var match = products.Any(p => p.ProductName == productName);

            // Assert
            Assert.True(match, $"Product with name '{productName}' should exist.");
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
