using LevelUpMarket.DataAccess.Repository.IRepository;
using LevelUpMarket.Models;
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
        [ValidateAntiForgeryToken]
        public IActionResult AddCharacter(Character character, IFormFile file, int idGame)
        {
            if (ModelState.IsValid)
            {
                character.GameId = idGame;
                string wwwRootPath = _hostEnvironment.WebRootPath;
                string imgUrl = "";
                if (file != null)
                {

                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(wwwRootPath, @"images/Characters");
                    var extension = Path.GetExtension(file.FileName);

                    using (var stream = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                    imgUrl = @"images/Characters/" + fileName + extension;

                }
                Enum.TryParse<CharacterType>(character.CharacterType.ToString(), out CharacterType type);
                character.ImageUrl = imgUrl;
                character.CharacterType = type;
                _unitOfWork.Character.Add(character);
                _unitOfWork.Save();
                var game = _unitOfWork.Game.GetFirstOrDefault(g => g.Id == character.GameId,includeProperties: "Videos,Characters");

                TempData["success"] = "Character created successfuly";

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
        public IActionResult DeleteCharacterPost(int? id)
        {
            var character = _unitOfWork.Character.GetFirstOrDefault(c => c.Id == id);
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
