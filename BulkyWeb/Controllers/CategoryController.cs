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
    }
}
