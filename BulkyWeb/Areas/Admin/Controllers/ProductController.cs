 using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Bulky.Models.ViewModels;
using Bulky.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;

namespace BulkyWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    //this will apply to all methods, you can also do them individually.
    //[Authorize(Roles = SD.Role_Admin)]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        //we need this in our wwwroot folder
        private readonly IWebHostEnvironment _webHostEnvironment;

        //because dbContext is registered in our services, we have access to it here via dependency injection
        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            //getting all the categories from the Product table
            //includeProperteies allows us to fill in the category via a foreign key
            List<Product> objProductList = _unitOfWork.Product.GetAll(includeProperteies: "Category").ToList();
            return View(objProductList);
        }

        //creating a new action method for the "Create new Product button"
        //update and insert
        //if you are creating a product you won't have and id, else if you are editing you will have an id.
        public IActionResult Upsert(int? id)
        {
            //allows us to send tempdata to the view
            //ViewBag.CategoryList = CategoryList;

            //create new object that matches ProductVM to send to the view
            ProductVM productVM = new()
            {
                //each category object here will be converted into a SelectListItem that has a text and a value - this is projection (like a map)
                CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                Product = new Product()
            };

            //this will mean we want to create a product
            //else we are updating a product
            if (id == null || id == 0)
            {
                //create
                return View(productVM);
            } else
            {
                //update
                productVM.Product = _unitOfWork.Product.Get(u => u.Id == id);
                return View(productVM);
            }
        }

        //whenever something is being posted, this endpoint will be invoked
        //when then get the Product object from the form as @model Product gives the form this object model
        //IFormFile aka if there is a file being uploaded
        [HttpPost]
        public IActionResult Upsert(ProductVM productVM, IFormFile? file)
        {
            //check that all the parts follow the requirements set in the model
            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if (file != null)
                {
                    //this will give us a random name for our image
                    //the get extension method is built in, it allows us to save the file as the same extension it was uploaded in
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    //this will give us the path to the product folder
                    string productPath = Path.Combine(wwwRootPath, @"images\product");

                    //if there is an image and we upload a new image
                    if(!string.IsNullOrEmpty(productVM.Product.ImageUrl))
                    {
                        //delete old image
                        //we need to remove the slash here because that's how it's stored in the DB
                        var oldImagePath = Path.Combine(wwwRootPath, productVM.Product.ImageUrl.TrimStart('\\'));

                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    //we use filemode.create to say we are creating a new file here
                    //we use file stream here to give us the fullpath and what we are saving in that path
                    using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    //we are saving the file image url + path into the model that will then go to the DB
                    productVM.Product.ImageUrl = @"\images\product\" + fileName;
                }

                if (productVM.Product.Id == 0)
                {
                    //adding the Product to the table queue
                    _unitOfWork.Product.Add(productVM.Product);
                } else
                {
                    //update the Product to the table queue
                    _unitOfWork.Product.Update(productVM.Product);
                }

                //saving the Product to the table (whatever is in the queue)
                _unitOfWork.Save();
                //allows you to show a notification on the next page
                TempData["success"] = "Product created successfully!";
                //redirect to index inside of Product class
                return RedirectToAction("Index", "Product");
            } else
            {
                productVM.CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                });
                //stay on the page you're on
                return View(productVM);
            }
        }

        [HttpPost]
        public IActionResult Edit(Product obj)
        {
            //check that all the parts follow the requirements set in the model
            if (ModelState.IsValid)
            {
                //updating the Product to the table queue
                _unitOfWork.Product.Update(obj);
                //saving the Product to the table (whatever is in the queue)
                //it will automatically update all the fields present inside of obj inside of the table
                _unitOfWork.Save();
                //allows you to show a notification on the next page
                TempData["success"] = "Product updated successfully!";
                //redirect to index inside of Product class
                return RedirectToAction("Index", "Product");
            }
            //stay on the page you're on
            return View();
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePost(int? id)
        {
            Product? obj = _unitOfWork.Product.Get(u => u.Id == id);
            if (obj == null)
            {
                return NotFound();
            }

            _unitOfWork.Product.Remove(obj);
            _unitOfWork.Save();
            //allows you to show a notification on the next page
            TempData["success"] = "Product deleted successfully!";
            return RedirectToAction("Index", "Product");
        }

        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            List<Product> objProductList = _unitOfWork.Product.GetAll(includeProperteies: "Category").ToList();
            return Json(new { data = objProductList });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var productToDeleted = _unitOfWork.Product.Get(u => u.Id == id);

            if (productToDeleted == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            //delete old image
            //we need to remove the slash here because that's how it's stored in the DB
            var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, productToDeleted.ImageUrl.TrimStart('\\'));

            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }

            _unitOfWork.Product.Remove(productToDeleted);
            _unitOfWork.Save();

            return Json(new { success = true, message = "Delete Successful" });
        }

        #endregion
    }
}
