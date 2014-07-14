﻿using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SportsStore.WebUI.Controllers
{
    public class AdminController : Controller
    {
        private IProductRepository repository;

        public AdminController(IProductRepository repository)
        {
            this.repository = repository;
        }

        public ViewResult Index()
        {
            return View(repository.Products);
        }

        public ViewResult Edit(int productID)
        {
            var product = repository.Products.FirstOrDefault(p => p.ProductID == productID);
            return View(product);
        }

        [HttpPost]
        public ActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                repository.SaveProduct(product);
                TempData["message"] = string.Format("{0} has been saved", product.Name);
                return RedirectToAction("Index");
            }

            return View(product);
        }

        public ViewResult Create()
        {
            return View("Edit", new Product());
        }

        public ActionResult Delete(int productID)
        {
            var prod = repository.Products.FirstOrDefault(p => p.ProductID == productID);
            if (prod != null)
            {
                repository.DeleteProduct(prod);
                TempData["message"] = string.Format("{0} was deleted", prod.Name);
            }

            return RedirectToAction("Index");
        }
    }
}