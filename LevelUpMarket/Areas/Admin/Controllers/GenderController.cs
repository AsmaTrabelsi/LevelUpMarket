using LevelUpMarket.Data;
using LevelUpMarket.DataAccess.Repository;
using LevelUpMarket.DataAccess.Repository.IRepository;
using LevelUpMarket.Models;
using LevelUpMarket.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LevelUpMarketWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class GenderController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public GenderController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
      
        public IActionResult Index()
        {
            IEnumerable<Gender> genderList = _unitOfWork.Gender.GetAll();

            return View(genderList);
        }
        //Get
        [Authorize(Roles = SD.Role_Admin)]
        public IActionResult Create()
        {
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_Create");
            }
            return View();
        }
        [HttpPost]
        [Authorize(Roles = SD.Role_Admin)]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Gender gender)
        {
           bool existe =  _unitOfWork.Gender.Any(p => p.Name.Equals(gender.Name));
            if (existe)
            {
                ModelState.AddModelError("name", gender.Name+" already exists");
            }
            if (ModelState.IsValid)
            {
                _unitOfWork.Gender.Add(gender);
                _unitOfWork.Save();
                TempData["success"] = gender.Name + " created successfuly";
                return RedirectToAction("Index");
            }
            return View(gender);

        }

        //Get
        [Authorize(Roles = SD.Role_Admin)]
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var gender = _unitOfWork.Gender.GetFirstOrDefault(x => x.Id == id);
            if (gender == null)
            {
                return NotFound();
            }

            return View(gender);
        }
        [HttpPost]
        [Authorize(Roles = SD.Role_Admin)]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Gender gender)
        {
            bool existe = _unitOfWork.Gender.Any(p => p.Name.Equals(gender.Name));
            
            if (ModelState.IsValid)
            {
                _unitOfWork.Gender.Update(gender);
                _unitOfWork.Save();
                TempData["success"] = gender.Name + " updated successfully";
                return RedirectToAction("Index");
            }
            return View(gender);

        }
        [Authorize(Roles = SD.Role_Admin)]
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var gender = _unitOfWork.Gender.GetFirstOrDefault(x => x.Id == id);
            if (gender == null)
            {
                return NotFound();
            }

            return View(gender);
        }
        [HttpPost]
        [Authorize(Roles = SD.Role_Admin)]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var gender = _unitOfWork.Gender.GetFirstOrDefault(x => x.Id == id);
            if (gender == null)
            {
                return NotFound();
            }
            _unitOfWork.Gender.Remove(gender);
            _unitOfWork.Save();
            TempData["success"] = gender.Name + " deleted successfuly";
            return RedirectToAction("Index");

        }
    }
}
