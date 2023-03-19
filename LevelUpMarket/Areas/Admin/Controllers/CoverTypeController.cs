using LevelUpMarket.DataAccess.Repository.IRepository;
using LevelUpMarket.Models;
using Microsoft.AspNetCore.Mvc;

namespace LevelUpMarketWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CoverTypeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CoverTypeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            IEnumerable<CoverType> coverTypes = _unitOfWork.CoverType.GetAll();
            return View(coverTypes);
        }

        //Get
        public IActionResult Create()
        {
            return View();
        }
        //Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CoverType coverType)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.CoverType.Add(coverType);
                _unitOfWork.Save();
                TempData["success"] = "cover type has created successfuly";
                return RedirectToAction("Index");
            }
            return View(coverType);
        }
        //Get
        public IActionResult Edit(int? id)
        {
            if(id == null || id == 0)
            {
                return NotFound();
            }
            var coverType = _unitOfWork.CoverType.GetFirstOrDefault(x=> x.Id == id);
            if(coverType == null)
            {
                return NotFound();
            }
            return View(coverType);
        }

        //Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(CoverType coverType)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.CoverType.Update(coverType);
                _unitOfWork.Save();
                TempData["success"] = "cover type has updated successfuly";
                return RedirectToAction("Index");

            }
            return View(coverType);
        }
        //Get
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var coverType = _unitOfWork.CoverType.GetFirstOrDefault(x => x.Id == id);
            if (coverType == null)
            {
                return NotFound();
            }
            return View(coverType);
        }

        //Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var coverType = _unitOfWork.CoverType.GetFirstOrDefault(x => x.Id == id);
            if (coverType == null)
            {
                return NotFound();
            }
            _unitOfWork.CoverType.Remove(coverType);
                _unitOfWork.Save();
                TempData["success"] = "cover type has removed successfuly";
                return RedirectToAction("Index");
        }

    }
}
