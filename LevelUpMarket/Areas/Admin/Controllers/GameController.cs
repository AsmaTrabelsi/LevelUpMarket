﻿using LevelUpMarket.Data;
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
        public  IActionResult Upsert(GameVM gameVm,List<IFormFile> files)
        {
            
            if (ModelState.IsValid)
            {
               if(gameVm.Game.Plateformes == null)
                {
                    gameVm.Game.Plateformes = new List<Plateforme>();

                    gameVm.Game.Subtitles = new List<Subtitle>();

                    gameVm.Game.VoiceLanguages = new List<VoiceLanguage>();
                    gameVm.Game.Genders = new List<Gender>();
                }
                
                // upload files
                string wwwRootPath = _hostEnvironment.WebRootPath;
                if (files != null && files.Count>0)
                {
                    gameVm.Game.Images = new List<Image>();
                    foreach (var file in files)
                    {
                        if(file != null)
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
                            if(gameVm.Game.Images != null)
                            {
                                foreach(var img in gameVm.Game.Images)
                                {
                                    var oldImagePath = Path.Combine(wwwRootPath,img.ImageUrl);
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

                gameVm.Game.Plateformes = new List<Plateforme>();

                // retrieve selected Plateformes
                foreach (var id in gameVm.SelectedPlateformes)
                {
                    var plateforme = gameVm.PlateformeList.FirstOrDefault(p => p.Id.ToString().Equals(id));
                    // var plateforme = _unitOfWork.Plateforme.GetFirstOrDefault(p => p.Id.ToString().Equals(id));

                    if (plateforme != null)
                    {
                        gameVm.Game.Plateformes.Add(plateforme);
                    }
                }


                // retrieve selected Genders
                foreach (var id in gameVm.SelectedGenders)
                {
                    var gender = gameVm.GenderList.FirstOrDefault(p => p.Id.ToString().Equals(id));
                    if (gender != null)
                    {
                        gameVm.Game.Genders.Add(gender);
                    }
                    

                }
                // retrieve selected Subtitles
                foreach (var id in gameVm.SelectedSubtitle)
                {
                    var subtitle = gameVm.SubtitleList.FirstOrDefault(p => p.Id.ToString().Equals(id));
                    if (subtitle != null)
                    {
                        gameVm.Game.Subtitles.Add(subtitle);
                    }

                }
                // retrieve selected Voice Languages
                foreach (var id in gameVm.SelectedVoice)
                {
                    
                    var voice = gameVm.VoiceLanguagesList.FirstOrDefault(p => p.Id.ToString().Equals(id));
                    if(voice != null)
                    {
                        gameVm.Game.VoiceLanguages.Add(voice);

                    }


                }

                if(gameVm.Game.Id == 0)
                {
                    _unitOfWork.Game.Add(gameVm.Game);
                }
                else
                {
                    _unitOfWork.Game.Update(gameVm.Game);

                }
                _unitOfWork.Save();
                TempData["success"] = "Game has created successfuly";
                return RedirectToAction("UpsertMoreDetails",gameVm.Game);

            }
            return View(gameVm);

        }
        
        public IActionResult UpsertMoreDetails(Game game)
        {
            
            return View(game);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpsertMoreDeatils(IFormFile files)
        {

            return View();

        }
     
        
        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()

        {
            var gameList = _unitOfWork.Game.GetAll(includeProperties: "Images,Developer");
            
            return Json(new {data = gameList});
        }
        [HttpDelete]
        public IActionResult DeletePost(int? id)
        {
            var game = _unitOfWork.Game.GetFirstOrDefault(x => x.Id == id);
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
            return Json(new { success = true, message = "Delete Successful" });

        }
        [HttpGet]
        public IActionResult GetVideoByGame(int gameId)

        {
            var videoList = _unitOfWork.Video.GetAll().Where(c => c.GameId == gameId);
            
            return Json(new { data = videoList });
        }
        [HttpDelete]
        public IActionResult DeleteVideoPost(int? id)
        {
            var video = _unitOfWork.Video.GetFirstOrDefault(c => c.Id == id);
            if (video == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            _unitOfWork.Video.Remove(video);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete Successful" });

        }
        [HttpPost]
        public IActionResult AddVideo(int gameId, string videoType, string url)

        {
            Enum.TryParse<VideoType>(videoType, out VideoType type);
            Video video = new Video
            {
                Type = type,
                URL = url,
                GameId = gameId
            };
            _unitOfWork.Video.Add(video);
            _unitOfWork.Save();
            return Json(new { success = true, message = "added Successful" });
        }
        #endregion
    }

   
   
}
