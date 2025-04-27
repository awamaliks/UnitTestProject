using Moq;
using ProductApi.Controllers;
using ProductApi.Models;
using ProductApi.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace UnitTestProject.Test
{
    using NUnit.Framework;
    using Moq;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.AspNetCore.Routing;

    namespace YourNamespace.Tests
    {
        public class ProductControllerTest
        {
            private Mock<IProductService> _productServiceMock;
            private ProductController _productController;

            [SetUp]
            public void Setup()
            {
                _productServiceMock = new Mock<IProductService>();
                _productController = new ProductController(_productServiceMock.Object);
            }

            [Test]
            public void GetProductList_ProductList()
            {
                // Arrange
                var expectedProducts = GetProductsData();
                _productServiceMock.Setup(x => x.GetProductList())
                    .Returns(expectedProducts);

                // Act
                var result = _productController.ProductList();

                // Assert
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Count(), Is.EqualTo(expectedProducts.Count));
                Assert.That(result, Is.EquivalentTo(expectedProducts));
            }

            [Test]
            public void GetProductByID_Product()
            {
                // Arrange
                var productList = GetProductsData();
                _productServiceMock.Setup(x => x.GetProductById(2))
                    .Returns(productList[1]);

                // Act
                var productResult = _productController.GetProductById(2);

                // Assert
                Assert.That(productResult, Is.Not.Null);
                Assert.That(productResult.ProductId, Is.EqualTo(productList[1].ProductId));
            }

            [TestCase("IPhone")]
            public void CheckProductExistOrNotByProductName_Product(string productName)
            {
                // Arrange
                var productList = GetProductsData();
                _productServiceMock.Setup(x => x.GetProductList())
                    .Returns(productList);

                // Act
                var productResult = _productController.ProductList();
                var expectedProductName = productResult.ToList()[0].ProductName;

                // Assert
                Assert.That(expectedProductName, Is.EqualTo(productName));
            }

            [Test]
            public void AddProduct_Product()
            {
                // Arrange
                var productList = GetProductsData();
                _productServiceMock.Setup(x => x.AddProduct(productList[2]))
                    .Returns(productList[2]);

                // Act
                var productResult = _productController.AddProduct(productList[2]);

                // Assert
                Assert.That(productResult, Is.Not.Null);
                Assert.That(productResult.ProductId, Is.EqualTo(productList[2].ProductId));
            }


            private List<Product> GetProductsData()
            {
                return new List<Product>
            {
                new Product
                {
                    ProductId = 1,
                    ProductName = "IPhone",
                    ProductDescription = "IPhone 12",
                    ProductPrice = 55000,
                    ProductStock = 10
                },
                new Product
                {
                    ProductId = 2,
                    ProductName = "Laptop",
                    ProductDescription = "HP Pavilion",
                    ProductPrice = 100000,
                    ProductStock = 20
                },
                new Product
                {
                    ProductId = 3,
                    ProductName = "TV",
                    ProductDescription = "Samsung Smart TV",
                    ProductPrice = 35000,
                    ProductStock = 30
                },
            };
            }
        }
    }

}

