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
    public class DeveloperController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public DeveloperController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            IEnumerable<Developer> developerList = _unitOfWork.Developer.GetAll();

            return View(developerList);
        }
        //Get
        [Authorize(Roles = SD.Role_Admin)]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = SD.Role_Admin)]
        public IActionResult Create(Developer developer)
        {
           
            if (ModelState.IsValid)
            {
                _unitOfWork.Developer.Add(developer);
                _unitOfWork.Save();
                TempData["success"] = "Developer has created successfuly";
                return RedirectToAction("Index");
            }
            return View(developer);

        }

        //Get
        [Authorize(Roles = SD.Role_Admin)]
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var developer = _unitOfWork.Developer.GetFirstOrDefault(x => x.Id == id);
            if (developer == null)
            {
                return NotFound();
            }

            return View(developer);
        }
        [HttpPost]
        [Authorize(Roles = SD.Role_Admin)]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Developer developer)
        {
            
            if (ModelState.IsValid)
            {
                _unitOfWork.Developer.Update(developer);
                _unitOfWork.Save();
                TempData["success"] = "category has updated successfuly";
                return RedirectToAction("Index");
            }
            return View(developer);

        }
        [Authorize(Roles = SD.Role_Admin)]
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var developer = _unitOfWork.Developer.GetFirstOrDefault(x => x.Id == id);
            if (developer == null)
            {
                return NotFound();
            }

            return View(developer);
        }
        [HttpPost]
        [Authorize(Roles = SD.Role_Admin)]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var developer = _unitOfWork.Developer.GetFirstOrDefault(x => x.Id == id);
            if (developer == null)
            {
                return NotFound();
            }
            _unitOfWork.Developer.Remove(developer);
            _unitOfWork.Save();
            TempData["success"] = "category has deleted successfuly";
            return RedirectToAction("Index");

        }
    }
}
