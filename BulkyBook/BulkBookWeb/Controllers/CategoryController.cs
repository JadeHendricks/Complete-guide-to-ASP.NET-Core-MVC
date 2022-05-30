using BulkBookWeb.Data;
using BulkBookWeb.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkBookWeb.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _db;
        public CategoryController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            IEnumerable<Category> objCategoryList = _db.Categories; 
            return View(objCategoryList);
        }

        //Get Action Method
        public IActionResult Create()
        {

            return View();
        }

        //Post Action Method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category obj)
        {
            _db.Categories.Add(obj);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
