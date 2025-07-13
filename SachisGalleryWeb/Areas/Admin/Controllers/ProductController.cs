using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SachisGallery.DataAccess.Data;
using SachisGallery.DataAccess.Repository.IRepository;
using SachisGallery.Models;
using SachisGallery.Models.Models;
using SachisGallery.Models.ViewModels;
using System;
using System.Collections.Generic;

namespace SachisGalleryWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfwork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(IUnitOfWork unitOfwork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfwork = unitOfwork;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            List<Product> objCategoryList = _unitOfwork.Product.GetAll(includeProperties: "Category").ToList();
            return View(objCategoryList);
        }

        public IActionResult Upsert(int? id)
        {

            ProductVM productVM = new()
            {
                CategoryList = _unitOfwork.Category.GetAll().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString(),
                }),
                Product = new Product()
            };
            if (id == null || id == 0)
            {
                return View(productVM);
            }
            else
            {
                productVM.Product = _unitOfwork.Product.Get(x => x.Id == id);
                return View(productVM);
            }
            
        }

        [HttpPost]
        public IActionResult Upsert(ProductVM productVM, IFormFile? file)
        {            
            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productPath = Path.Combine(wwwRootPath, @"images\product");

                    if (!string.IsNullOrEmpty(productVM.Product.ImageUrl))
                    {
                        var oldImage = Path.Combine(wwwRootPath, productVM.Product.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImage))
                        {
                            System.IO.File.Delete(oldImage);
                        }
                    }

                    using(var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    productVM.Product.ImageUrl = @"\images\product\" + fileName;
                }
                if (productVM.Product.Id == 0)
                {
                    _unitOfwork.Product.Add(productVM.Product);
                }
                else
                {
                    _unitOfwork.Product.Update(productVM.Product);
                }

                _unitOfwork.Save();
                TempData["success"] = "Product created successfully";
                return RedirectToAction("Index", "Product");
            }
            else
            {
                productVM.CategoryList = _unitOfwork.Category.GetAll().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString(),
                });

                return View(productVM);
            }
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            Product? product = _unitOfwork.Product.Get(x => x.Id == id);
            //Category? category1 = _db.Categories.FirstOrDefault(u => u.Id == id);

            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpPost]
        public IActionResult Edit(Product obj)
        {
            if (ModelState.IsValid)
            {
                _unitOfwork.Product.Update(obj);
                _unitOfwork.Save();
                TempData["success"] = "Product updated successfully";
                return RedirectToAction("Index", "Product");
            }

            return View();
        }

       
        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Product> objProductList = _unitOfwork.Product.GetAll(includeProperties: "Category").ToList();
            return Json(new { data = objProductList });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var productToBeDeleted = _unitOfwork.Product.Get(x => x.Id == id);
            if (productToBeDeleted == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }
            var oldImage = Path.Combine(_webHostEnvironment.WebRootPath, productToBeDeleted.ImageUrl.TrimStart('\\'));
            if (System.IO.File.Exists(oldImage))
            {
                System.IO.File.Delete(oldImage);
            }
            _unitOfwork.Product.Remove(productToBeDeleted);
            _unitOfwork.Save();
            
            return Json(new { success = true, message = "Delete successful" });
        }
        #endregion

    }
}
