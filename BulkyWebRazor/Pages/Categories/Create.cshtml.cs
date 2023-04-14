using BulkyWebRazor.Data;
using BulkyWebRazor.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BulkyWebRazor.Pages.Categories
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        //when we are working with razor pages, if we want this property to be available when we are psoting
        //we need to use [bind] otherwise the property will be empty
        [BindProperty]
        public Category Category { get; set; }

        //constructor
        public CreateModel(ApplicationDbContext db)
        {
            _db = db;
        }

        //get handler
        public void OnGet()
        {
        }

        //we use IActionResult here to return to a different page at the end of the post
        //post handler
        //nothing needs to be passed here because of [BindProperty] above
        public IActionResult OnPost()
        {
            _db.Categories.Add(Category);
            _db.SaveChanges();
            TempData["Success"] = "Category created successfully";
            return RedirectToPage("Index");
        }
    }
}
