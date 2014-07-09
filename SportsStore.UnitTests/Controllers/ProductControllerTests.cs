﻿using System;
using NUnit.Framework;
using Moq;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;
using System.Linq;
using SportsStore.WebUI.Controllers;
using System.Collections;
using System.Collections.Generic;
using SportsStore.WebUI.Models;

namespace SportsStore.UnitTests.Controllers
{
    [TestFixture]
    public class ProductControllerTests
    {
        private IQueryable<Product> products()
        {
            return Enumerable.Range(1, 5)
                .Select(i => new Product
                { 
                    ProductID = i,
                    Name = "P" + i,
                    Category = "Cat" + ((i % 3) + 1)
                }).AsQueryable();
        }

        [Test]
        public void CanPaginate()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(products());
            ProductController controller = new ProductController(mock.Object);
            controller.PageSize = 3;

            var result = (ProductsListViewModel)controller.List(null, 2).Model;

            var prodArray = result.Products.ToArray();
            Assert.That(prodArray.Length, Is.EqualTo(2));
            Assert.That(prodArray[0].Name, Is.EqualTo("P4"));
            Assert.That(prodArray[1].Name, Is.EqualTo("P5"));

            var pagingInfo = result.PagingInfo;
            Assert.That(pagingInfo.CurrentPage, Is.EqualTo(2));
            Assert.That(pagingInfo.ItemsPerPage, Is.EqualTo(3));
            Assert.That(pagingInfo.TotalItems, Is.EqualTo(5));
            Assert.That(pagingInfo.TotalPages, Is.EqualTo(2));
        }

        [Test]
        public void CanFilterProducts()
        {
            var mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(this.products());

            var controller = new ProductController(mock.Object);
            controller.PageSize = 3;

            var result = ((ProductsListViewModel)controller.List("Cat2", 1).Model).Products.ToArray();

            Assert.That(result.Length, Is.EqualTo(2));
            Assert.That(result[0].Name, Is.EqualTo("P1"));
            Assert.That(result[0].Category, Is.EqualTo("Cat2"));
            Assert.That(result[1].Name, Is.EqualTo("P4"));
            Assert.That(result[1].Category, Is.EqualTo("Cat2"));
        }
    }
}