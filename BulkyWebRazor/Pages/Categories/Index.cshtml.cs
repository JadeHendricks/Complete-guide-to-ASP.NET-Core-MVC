using BulkyWebRazor.Data;
using BulkyWebRazor.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BulkyWebRazor.Pages.Categories
{
    public class IndexModel : PageModel
    {

        private readonly ApplicationDbContext _db;
        public List<Category> CategoryList { get; set; }

        //constructor
        public IndexModel(ApplicationDbContext db)
        {
            _db = db;
        }

        //when we use "OnGet", "OnPost" we do not have to write return view();
        //it will automatically bind everything gor us in the model
        //On is required
        public void OnGet()
        {
            //getting all the categories and storing it into CategoryList
            CategoryList = _db.Categories.ToList();
        }
    }
}
