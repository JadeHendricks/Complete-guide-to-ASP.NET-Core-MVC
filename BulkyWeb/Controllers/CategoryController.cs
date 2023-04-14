using BulkyWeb.Data;
using BulkyWeb.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyWeb.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _db;

        //because dbContext is registered in our services, we have access to it here via dependency injection
        public CategoryController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            //getting all the categories from the category table
            List<Category> objCategoryList = _db.Categories.ToList();
            return View(objCategoryList);
        }

        //creating a new action method for the "Create new category button"
        public IActionResult Create()
        {
            return View();
        }

        //whenever something is being posted, this endpoint will be invoked
        //when then get the category object from the form as @model Category gives the form this object model
        [HttpPost]
        public IActionResult Create(Category obj)
        {
            //custom validation
            if(obj.Name == obj.DisplayOrder.ToString())
            {
                //adding custom validation to the current modelState
                ModelState.AddModelError("name", "The DisplayOrder cannot exactly match the name");
            }

            //check that all the parts follow the requirements set in the model
            if(ModelState.IsValid)
            {
                //adding the category to the table queue
                _db.Categories.Add(obj);
                //saving the category to the table (whatever is in the queue)
                _db.SaveChanges();
                //allows you to show a notification on the next page
                TempData["success"] = "Category created successfully!";
                //redirect to index inside of category class
                return RedirectToAction("Index", "Category");
            }
            //stay on the page you're on
            return View();
        }

        //Edit Category action
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            //retrieve one category from the DB
            //find only works on the primary key
            //if you want to find other things use FirstOrDefault();
            Category? categoryFromDb = _db.Categories.Find(id);

            if (categoryFromDb == null)
            {
                return NotFound();
            }

            return View(categoryFromDb);
        }

        [HttpPost]
        public IActionResult Edit(Category obj)
        {
            //check that all the parts follow the requirements set in the model
            if (ModelState.IsValid)
            {
                //updating the category to the table queue
                _db.Categories.Update(obj);
                //saving the category to the table (whatever is in the queue)
                //it will automatically update all the fields present inside of obj inside of the table
                _db.SaveChanges();
                //allows you to show a notification on the next page
                TempData["success"] = "Category updated successfully!";
                //redirect to index inside of category class
                return RedirectToAction("Index", "Category");
            }
            //stay on the page you're on
            return View();
        }

        //Delete Category action
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            //retrieve one category from the DB
            //find only works on the primary key
            //if you want to find other things use FirstOrDefault();
            Category? categoryFromDb = _db.Categories.Find(id);

            if (categoryFromDb == null)
            {
                return NotFound();
            }

            return View(categoryFromDb);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePost(int? id)
        {
            Category? obj = _db.Categories.Find(id);
            if (obj == null)
            {
                return NotFound();                
            }

            _db.Categories.Remove(obj);
            _db.SaveChanges();
            //allows you to show a notification on the next page
            TempData["success"] = "Category deleted successfully!";
            return RedirectToAction("Index", "Category");
        }
    }
}
