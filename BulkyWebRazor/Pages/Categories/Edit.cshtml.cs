using BulkyWebRazor.Data;
using BulkyWebRazor.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BulkyWebRazor.Pages.Categories
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        //when we are working with razor pages, if we want this property to be available when we are psoting
        //we need to use [bind] otherwise the property will be empty
        [BindProperty]
        public Category Category { get; set; }

        //constructor
        public EditModel(ApplicationDbContext db)
        {
            _db = db;
        }

        //get handler
        public void OnGet(int? id)
        {
            if(id != null && id != 0)
            {
                Category = _db.Categories.Find(id);
            }
        }

        //we use IActionResult here to return to a different page at the end of the post
        //post handler
        //nothing needs to be passed here because of [BindProperty] above
        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                _db.Categories.Update(Category);
                _db.SaveChanges();
                TempData["Success"] = "Category edited successfully";
                return RedirectToPage("Index");
            }
            return Page();
        }
    }
}
