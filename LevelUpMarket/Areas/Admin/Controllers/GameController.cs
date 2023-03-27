using LevelUpMarket.Data;
using LevelUpMarket.DataAccess.Repository;
using LevelUpMarket.DataAccess.Repository.IRepository;
using LevelUpMarket.Models;
using LevelUpMarket.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LevelUpMarketWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class GameController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostEnvironment;
        public GameController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _hostEnvironment = hostEnvironment;
        }

        public IActionResult Index()
        {

            return View();
        }
        

        //Get
        public IActionResult Upsert(int? id)
        {
            GameVM gameVM = new()
            {
                Game = new(),
               /* CategoryList = _unitOfWork.Category.GetAll().Select(
                d => new SelectListItem
                {
                    Text = d.Name,
                    Value = d.Id.ToString()
                }),*/
                DeveloperList = _unitOfWork.Developer.GetAll().Select(
                 d => new SelectListItem
                 {
                     Text = d.Name,
                     Value = d.Id.ToString()
                 }),
                PlateformeList = _unitOfWork.Plateforme.GetAll().Select(
              d => new SelectListItem
              {
                  Text = d.Name,
                  Value = d.Id.ToString()
              })

            };
          
            if (id == null || id == 0)
            {
                // cretae Game
               // ViewBag.DeveloperList = DeveloperList;
                //ViewBag.PlateformeList = PlateformeList;
                //ViewBag.CategoryList = CategoryList;
                return View(gameVM);
            }
            else
            {
                // update Game
            }
           
            return View(gameVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(GameVM gameVm,List<IFormFile> files)
        {
            
            if (ModelState.IsValid)
            {
                if(files != null && files.Count>0)
                {
                    gameVm.Game.Images = new List<Image>();
                    foreach (var file in files)
                    {
                        if (file.Length > 0 && file.ContentType.StartsWith("image/"))
                        {
                            var image = new Image { Name = file.FileName };

                            using (var stream = new MemoryStream())
                            {
                                await file.CopyToAsync(stream);
                                image.Bytes = stream.ToArray();
                            }
                            gameVm.Game.Images.Add(image);

                        }
             
                    }

                }
                _unitOfWork.Game.Add(gameVm.Game);
                _unitOfWork.Save();
                TempData["success"] = "Game has created successfuly";
                return RedirectToAction("Index");
            }
            return View(gameVm);

        }
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var game = _unitOfWork.Game.GetFirstOrDefault(x => x.Id == id);
            if (game == null)
            {
                return NotFound();
            }

            return View(game);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var game = _unitOfWork.Game.GetFirstOrDefault(x => x.Id == id);
            if (game == null)
            {
                return NotFound();
            }
            _unitOfWork.Game.Remove(game);
            _unitOfWork.Save();
            TempData["success"] = "category has deleted successfuly";
            return RedirectToAction("Index");

        }
        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            var gameList = _unitOfWork.Game.GetAll();
            return Json(new {data = gameList});
        }
        #endregion
    }
}
