using LevelUpMarket.Data;
using LevelUpMarket.DataAccess.Repository;
using LevelUpMarket.DataAccess.Repository.IRepository;
using LevelUpMarket.Models;
using Microsoft.AspNetCore.Mvc;

namespace LevelUpMarketWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PlateformeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public PlateformeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
      
        public IActionResult Index()
        {
            IEnumerable<Plateforme> plateformeList = _unitOfWork.Plateforme.GetAll();

            return View(plateformeList);
        }
        //Get
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Plateforme plateforme)
        {
           bool existe =  _unitOfWork.Plateforme.Any(p => p.Name.Equals(plateforme.Name));
            if (existe)
            {
                ModelState.AddModelError("name", plateforme.Name+" already exists");
            }
            if (ModelState.IsValid)
            {
                _unitOfWork.Plateforme.Add(plateforme);
                _unitOfWork.Save();
                TempData["success"] = plateforme.Name + " created successfuly";
                return RedirectToAction("Index");
            }
            return View(plateforme);

        }

        //Get
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var plateforme = _unitOfWork.Plateforme.GetFirstOrDefault(x => x.Id == id);
            if (plateforme == null)
            {
                return NotFound();
            }

            return View(plateforme);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Plateforme plateforme)
        {
            bool existe = _unitOfWork.Plateforme.Any(p => p.Name.Equals(plateforme.Name));
            
            if (ModelState.IsValid)
            {
                _unitOfWork.Plateforme.Update(plateforme);
                _unitOfWork.Save();
                TempData["success"] = plateforme.Name + " updated successfully";
                return RedirectToAction("Index");
            }
            return View(plateforme);

        }
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var plateforme = _unitOfWork.Plateforme.GetFirstOrDefault(x => x.Id == id);
            if (plateforme == null)
            {
                return NotFound();
            }

            return View(plateforme);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var plateforme = _unitOfWork.Plateforme.GetFirstOrDefault(x => x.Id == id);
            if (plateforme == null)
            {
                return NotFound();
            }
            _unitOfWork.Plateforme.Remove(plateforme);
            _unitOfWork.Save();
            TempData["success"] = plateforme.Name + " deleted successfuly";
            return RedirectToAction("Index");

        }
    }
}
