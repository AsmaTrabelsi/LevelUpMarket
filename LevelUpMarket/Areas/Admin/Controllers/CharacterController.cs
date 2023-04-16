using LevelUpMarket.DataAccess.Repository.IRepository;
using LevelUpMarket.Models;
using LevelUpMarket.Models.ViewModel;
using LevelUpMarket.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NuGet.Packaging.Signing;

namespace LevelUpMarketWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CharacterController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostEnvironment;

      

        public CharacterController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _hostEnvironment = hostEnvironment;
        }

          // get
        public IActionResult index()
        {

            return View();
        }
        [HttpPost]
        [Authorize(Roles = SD.Role_Admin)]
        [ValidateAntiForgeryToken]
        public IActionResult AddCharacter(Character character, IFormFile file, int idGame, int charcterId)
        {
            if (ModelState.IsValid)
            {
                Console.WriteLine(character.CharacterId);
                character.GameId = idGame;
                character.CharacterId = charcterId;
                string wwwRootPath = _hostEnvironment.WebRootPath;
                string imgUrl = "";
                if (file != null)
                {

                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(wwwRootPath, @"images/Characters");
                    var extension = Path.GetExtension(file.FileName);
                    if (character.ImageUrl != null)
                    {

                        var oldImagePath = Path.Combine(wwwRootPath, character.ImageUrl);
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }

                }
                using (var stream = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                    imgUrl = @"images/Characters/" + fileName + extension;

                }
                Enum.TryParse<CharacterType>(character.CharacterType.ToString(), out CharacterType type);
                character.ImageUrl = imgUrl;
                character.CharacterType = type;
                if(character.CharacterId != 0)
                {
                    _unitOfWork.Character.Update(character);
                    _unitOfWork.Save();
                    TempData["success"] = character.CharacterName+ " updated successfully";

                }
                else
                {
                    _unitOfWork.Character.Add(character);
                    _unitOfWork.Save();
                    TempData["success"] = character.CharacterName + " created successfuly";

                }
                var game = _unitOfWork.Game.GetFirstOrDefault(g => g.Id == character.GameId,includeProperties: "Videos,Characters");

                return RedirectToAction("UpsertMoreDetails", "Game", game);
            }
            return View(character);
         
        }
        #region API CALLS

        [HttpGet]
        public IActionResult GetCharacterByGame(int gameId)

        {
            var characterList = _unitOfWork.Character.GetAll().Where(c => c.GameId == gameId);

            return Json(new { data = characterList });
        }

        [HttpDelete]
        [Authorize(Roles = SD.Role_Admin)]
        public IActionResult DeleteCharacterPost(int? id)
        {
            var character = _unitOfWork.Character.GetFirstOrDefault(c => c.CharacterId == id);
            if (character == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            _unitOfWork.Character.Remove(character);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete Successful" });

        }
       
        #endregion
    }
}
