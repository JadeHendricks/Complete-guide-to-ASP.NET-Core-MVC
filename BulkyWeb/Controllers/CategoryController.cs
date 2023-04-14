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
                //redirect to index inside of category class
                return RedirectToAction("Index", "Category");
            }
            //stay on the page you're on
            return View();

        }
    }
}
