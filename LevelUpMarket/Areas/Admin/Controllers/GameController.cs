using LevelUpMarket.Data;
using LevelUpMarket.DataAccess.Repository;
using LevelUpMarket.DataAccess.Repository.IRepository;
using LevelUpMarket.Models;
using LevelUpMarket.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Hosting;
using System.Linq;

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
              }),
                SubtitleList = _unitOfWork.Subtitle.GetAll().Select(
              s => new SelectListItem
              {
                  Text = s.Name,
                  Value = s.Id.ToString()
              }),
                VoiceLanguagesList = _unitOfWork.VoiceLanguage.GetAll().Select(
              v => new SelectListItem
              {
                  Text = v.Name,
                  Value = v.Id.ToString()
              }),
                GenderList = _unitOfWork.Gender.GetAll().Select(
              g => new SelectListItem
              {
                  Text = g.Name,
                  Value = g.Id.ToString()
              })

            };
          
            if (id == null || id == 0)
            {
               
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
        public  IActionResult Upsert(GameVM gameVm,List<IFormFile> files)
        {
            
            if (ModelState.IsValid)
            {
                string wwwRootPath = _hostEnvironment.WebRootPath;
                if(files != null && files.Count>0)
                {
                    gameVm.Game.Images = new List<Image>();
                    foreach (var file in files)
                    {
                        if(file != null)
                        {
                            ImageType type;
                            if (file.FileName.ToLower().Contains(ImageType.INIT.ToString().ToLower()))
                            {
                                type = ImageType.INIT;
                            }
                            else if (file.FileName.ToLower().Contains(ImageType.BG_STORE.ToString().ToLower()))
                            {
                                type = ImageType.BG_STORE;
                            }
                            else if (file.FileName.ToLower().Contains(ImageType.BG_STORY.ToString().ToLower()))
                            {
                                type = ImageType.BG_STORE;
                            }
                            else
                            {
                                type = ImageType.NAVGATION;
                            }
                            string fileName = Guid.NewGuid().ToString();
                            var uploads = Path.Combine(wwwRootPath, @"images/Games");
                            var extension = Path.GetExtension(file.FileName);
                            using (var stream = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create)) 
                            {
                                file.CopyTo(stream);
                            }

                            gameVm.Game.Images.Add(new Image { Name = fileName, ImageUrl= @"images/game"+fileName+extension, ImageType= type});
                        }
                 
              
                    }

                }
                var items = gameVm.Game.Plateformes;
                _unitOfWork.Game.Add(gameVm.Game);
             
                _unitOfWork.Save();
                TempData["success"] = "Game has created successfuly";
                return RedirectToAction("UpsertMoreDetails");

            }
            return View(gameVm);

        }
        public IActionResult UpsertMoreDetails(int? id)
        {
            GameVM gameVM = new()
            {
                Game = new(),
              
                DeveloperList = _unitOfWork.Developer.GetAll().Select(
                 d => new SelectListItem
                 {
                     Text = d.Name,
                     Value = d.Id.ToString()
                 }),
                PlateformeList = _unitOfWork.Plateforme.GetAll().Select(
              p => new SelectListItem
              {
                  Text = p.Name,
                  Value = p.Id.ToString()
              }),
                SubtitleList = _unitOfWork.Subtitle.GetAll().Select(
              s => new SelectListItem
              {
                  Text = s.Name,
                  Value = s.Id.ToString()
              }),
                VoiceLanguagesList = _unitOfWork.VoiceLanguage.GetAll().Select(
              v => new SelectListItem
              {
                  Text = v.Name,
                  Value = v.Id.ToString()
              }),
                GenderList = _unitOfWork.Gender.GetAll().Select(
              g => new SelectListItem
              {
                  Text = g.Name,
                  Value = g.Id.ToString()
              })

            };

            if (id == null || id == 0)
            {
                
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
        public async Task<IActionResult> UpsertMoreDeatils(GameVM gameVm)
        {

            if (ModelState.IsValid)
            {
               
               
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
