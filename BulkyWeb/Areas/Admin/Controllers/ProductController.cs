using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        //because dbContext is registered in our services, we have access to it here via dependency injection
        public ProductController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            //getting all the categories from the Product table
            List<Product> objProductList = _unitOfWork.Product.GetAll().ToList();
            return View(objProductList);
        }

        //creating a new action method for the "Create new Product button"
        public IActionResult Create()
        {
            return View();
        }

        //whenever something is being posted, this endpoint will be invoked
        //when then get the Product object from the form as @model Product gives the form this object model
        [HttpPost]
        public IActionResult Create(Product obj)
        {
            //check that all the parts follow the requirements set in the model
            if (ModelState.IsValid)
            {
                //adding the Product to the table queue
                _unitOfWork.Product.Add(obj);
                //saving the Product to the table (whatever is in the queue)
                _unitOfWork.Save();
                //allows you to show a notification on the next page
                TempData["success"] = "Product created successfully!";
                //redirect to index inside of Product class
                return RedirectToAction("Index", "Product");
            }
            //stay on the page you're on
            return View();
        }

        //Edit Product action
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            //retrieve one Product from the DB
            //find only works on the primary key
            //if you want to find other things use FirstOrDefault();
            Product? productFromDb = _unitOfWork.Product.Get(u => u.Id == id);

            if (productFromDb == null)
            {
                return NotFound();
            }

            return View(productFromDb);
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

        //Delete Product action
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            //retrieve one Product from the DB
            //find only works on the primary key
            //if you want to find other things use FirstOrDefault();
            Product? productFromDb = _unitOfWork.Product.Get(u => u.Id == id);

            if (productFromDb == null)
            {
                return NotFound();
            }

            return View(productFromDb);
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
    }
}
