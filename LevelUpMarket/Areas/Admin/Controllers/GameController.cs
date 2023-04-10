using LevelUpMarket.Data;
using LevelUpMarket.DataAccess.Repository;
using LevelUpMarket.DataAccess.Repository.IRepository;
using LevelUpMarket.Models;
using LevelUpMarket.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Hosting;
using System.Linq;
using System.Text.Json.Serialization;
using System.Text.Json;
using Newtonsoft.Json;

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
                PlateformeList = _unitOfWork.Plateforme.GetAll(),

                SubtitleList = _unitOfWork.Subtitle.GetAll(),
                VoiceLanguagesList = _unitOfWork.VoiceLanguage.GetAll(),
                GenderList = _unitOfWork.Gender.GetAll()

            };

            if (id == null || id == 0)
            {

                return View(gameVM);
            }
            else
            {
                // update Game
                gameVM.Game = _unitOfWork.Game.GetFirstOrDefault(u => u.Id == id, includeProperties: "Images,Plateformes,Subtitles,VoiceLanguages,Genders");
                gameVM.SelectedGenders = (IEnumerable<string>)gameVM.Game.Genders.Select(g => g.Id.ToString());
                gameVM.SelectedSubtitle = (IEnumerable<string>)gameVM.Game.Subtitles.Select(g => g.Id.ToString());
                gameVM.SelectedVoice = (IEnumerable<string>)gameVM.Game.VoiceLanguages.Select(g => g.Id.ToString());
                gameVM.SelectedPlateformes = (IEnumerable<string>)gameVM.Game.Plateformes.Select(g => g.Id.ToString());

                return View(gameVM);
            }

            return View(gameVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(GameVM gameVm, List<IFormFile> files)
        {
         

            if (ModelState.IsValid)
            {
               

                if (gameVm.Game.Plateformes == null)
                {
                    gameVm.Game.Plateformes = new List<Plateforme>();
                }
                if(gameVm.Game.Subtitles == null)
                {
                    gameVm.Game.Subtitles = new List<Subtitle>();

                }
                if(gameVm.Game.VoiceLanguages == null)
                {
                    gameVm.Game.VoiceLanguages = new List<VoiceLanguage>();

                }
                if(gameVm.Game.Genders == null)
                {
                    gameVm.Game.Genders = new List<Gender>();

                }

                // upload files
                string wwwRootPath = _hostEnvironment.WebRootPath;
                if (files != null && files.Count > 0)
                {
                    gameVm.Game.Images = new List<Image>();
                    foreach (var file in files)
                    {
                        if (file != null)
                        {
                            // set image type
                            ImageType type;
                            if (file.FileName.ToLower().Contains("init"))
                            {
                                type = ImageType.INIT;
                            }
                            else if (file.FileName.ToLower().Contains("store"))
                            {
                                type = ImageType.BG_STORE;
                            }
                            else if (file.FileName.ToLower().Contains("story"))
                            {
                                type = ImageType.BG_STORY;
                            }
                            else if (file.FileName.ToLower().Contains("poster"))
                            {
                                type = ImageType.POSTER;
                            }
                            else
                            {
                                type = ImageType.NAVGATION;
                            }

                            string fileName = Guid.NewGuid().ToString();
                            var uploads = Path.Combine(wwwRootPath, @"images/Games");
                            var extension = Path.GetExtension(file.FileName);
                            if (gameVm.Game.Images != null)
                            {
                                foreach (var img in gameVm.Game.Images)
                                {
                                    var oldImagePath = Path.Combine(wwwRootPath, img.ImageUrl);
                                    if (System.IO.File.Exists(oldImagePath))
                                    {
                                        System.IO.File.Delete(oldImagePath);
                                    }
                                }
                                using (var stream = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                                {
                                    file.CopyTo(stream);
                                }

                                gameVm.Game.Images.Add(new Image { Name = fileName, ImageUrl = @"images/Games/" + fileName + extension, ImageType = type });
                            }

                        }
                    }

                }


                // retrieve selected Plateformes
                foreach (var id in gameVm.SelectedPlateformes)
                {
                    var plateforme = _unitOfWork.Plateforme.GetFirstOrDefault(p => p.Id.ToString().Equals(id));

                    if (plateforme != null)
                    {
                       gameVm.Game.Plateformes.Add(plateforme);
                    }
                }


                // retrieve selected Genders
                foreach (var id in gameVm.SelectedGenders)
                {

                    var gender = _unitOfWork.Gender.GetFirstOrDefault(p => p.Id.ToString().Equals(id));
                    if (gender != null)
                    {
                        gameVm.Game.Genders.Add(gender);
                    }


                }
                // retrieve selected Subtitles
                foreach (var id in gameVm.SelectedSubtitle)
                {
                    var subtitle = _unitOfWork.Subtitle.GetFirstOrDefault(p => p.Id.ToString().Equals(id));
                    if (subtitle != null)
                    {
                        gameVm.Game.Subtitles.Add(subtitle);
                    }

                }
                // retrieve selected Voice Languages
          
                foreach (var id in gameVm.SelectedVoice)
                {

                    var voice = _unitOfWork.VoiceLanguage.GetFirstOrDefault(p => p.Id.ToString().Equals(id));
                    if (voice != null)
                    {
                        gameVm.Game.VoiceLanguages.Add(voice);

                    }


                }

                if (gameVm.Game.Id == 0)
                {
                    _unitOfWork.Game.Add(gameVm.Game);
                    TempData["success"] = "Game has been created successfully";

                }
                else
                {
                    _unitOfWork.deleteFroGamePlateforme(gameVm.Game.Id);
                    _unitOfWork.deleteFroGameGender(gameVm.Game.Id);
                    _unitOfWork.deleteFroGameSubtitle(gameVm.Game.Id);
                    _unitOfWork.deleteFroGameVoiceLanguage(gameVm.Game.Id);
                    _unitOfWork.Game.Update(gameVm.Game);
                    TempData["success"] = gameVm.Game.Name + " has been updated successfully";


                }
                _unitOfWork.Save();

                return RedirectToAction("UpsertMoreDetails", gameVm.Game);

            }
            return View(gameVm);

        }
        // Get
        public IActionResult UpsertMoreDetails(Game game)
        {

            return View(game);
        }



        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()

        {
            var gameList = _unitOfWork.Game.GetAll(includeProperties: "Images,Developer");

            return Json(new { data = gameList });
        }
        [HttpDelete]
        public IActionResult DeletePost(int? id)
        {
            var game = _unitOfWork.Game.GetFirstOrDefault(x => x.Id == id,includeProperties: "Images");
            if (game == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }
            foreach (var img in game.Images)
            {
                var oldImagePath = Path.Combine(_hostEnvironment.WebRootPath, img.ImageUrl);
                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }
            }
            _unitOfWork.Game.Remove(game);
            _unitOfWork.Save();
            return Json(new { success = true, message = game.Name + " Delete Successful" });

        }

        #endregion




    }

}
