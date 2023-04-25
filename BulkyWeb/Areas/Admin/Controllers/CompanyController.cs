 using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Bulky.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BulkyWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    //this will apply to all methods, you can also do them individually.
    [Authorize(Roles = SD.Role_Admin)]
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        //because dbContext is registered in our services, we have access to it here via dependency injection
        public CompanyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            //getting all the categories from the Company table
            List<Company> objCompanyList = _unitOfWork.Company.GetAll().ToList();
            return View(objCompanyList);
        }

        //creating a new action method for the "Create new Company button"
        //update and insert
        //if you are creating a company you won't have and id, else if you are editing you will have an id.
        public IActionResult Upsert(int? id)
        {
            if (id == null || id == 0)
            {
                //create
                return View(new Company());
            } else
            {
                //update
                Company CompanyObj = _unitOfWork.Company.Get(u => u.Id == id);
                return View(CompanyObj);
            }
        }

        //whenever something is being posted, this endpoint will be invoked
        //when then get the Company object from the form as @model Company gives the form this object model
        [HttpPost]
        public IActionResult Upsert(Company companyObj)
        {
            //check that all the parts follow the requirements set in the model
            if (ModelState.IsValid)
            {
                if (companyObj.Id == 0)
                {
                    //adding the Company to the table queue
                    _unitOfWork.Company.Add(companyObj);
                } else
                {
                    //update the Company to the table queue
                    _unitOfWork.Company.Update(companyObj);
                }

                //saving the Company to the table (whatever is in the queue)
                _unitOfWork.Save();
                //allows you to show a notification on the next page
                TempData["success"] = "Company created successfully!";
                //redirect to index inside of Company class
                return RedirectToAction("Index", "Company");
            } else
            {
                return View(companyObj);
            }
        }

        [HttpPost]
        public IActionResult Edit(Company obj)
        {
            //check that all the parts follow the requirements set in the model
            if (ModelState.IsValid)
            {
                //updating the Company to the table queue
                _unitOfWork.Company.Update(obj);
                //saving the Company to the table (whatever is in the queue)
                //it will automatically update all the fields present inside of obj inside of the table
                _unitOfWork.Save();
                //allows you to show a notification on the next page
                TempData["success"] = "Company updated successfully!";
                //redirect to index inside of Company class
                return RedirectToAction("Index", "Company");
            }
            //stay on the page you're on
            return View();
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePost(int? id)
        {
            Company? obj = _unitOfWork.Company.Get(u => u.Id == id);
            if (obj == null)
            {
                return NotFound();
            }

            _unitOfWork.Company.Remove(obj);
            _unitOfWork.Save();
            //allows you to show a notification on the next page
            TempData["success"] = "Company deleted successfully!";
            return RedirectToAction("Index", "Company");
        }

        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            List<Company> objCompanyList = _unitOfWork.Company.GetAll().ToList();
            return Json(new { data = objCompanyList });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var companyToDeleted = _unitOfWork.Company.Get(u => u.Id == id);

            if (companyToDeleted == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            _unitOfWork.Company.Remove(companyToDeleted);
            _unitOfWork.Save();

            return Json(new { success = true, message = "Delete Successful" });
        }

        #endregion
    }
}
