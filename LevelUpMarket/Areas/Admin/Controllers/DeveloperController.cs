using LevelUpMarket.Data;
using LevelUpMarket.DataAccess.Repository;
using LevelUpMarket.DataAccess.Repository.IRepository;
using LevelUpMarket.Models;
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
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
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
