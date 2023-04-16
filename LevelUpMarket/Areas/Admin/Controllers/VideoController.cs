using LevelUpMarket.DataAccess.Repository.IRepository;
using LevelUpMarket.Models;
using LevelUpMarket.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LevelUpMarketWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class VideoController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public VideoController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        #region API CALLS
        [HttpGet]
        public IActionResult GetVideoByGame(int gameId)

        {
            var videoList = _unitOfWork.Video.GetAll().Where(c => c.GameId == gameId);

            return Json(new { data = videoList });
        }
        [HttpDelete]
        [Authorize(Roles = SD.Role_Admin)]
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

        // test this method with Video parametre
        [HttpPost]
        [Authorize(Roles = SD.Role_Admin)]
        public IActionResult AddVideo(Video v)

        {
            Enum.TryParse<VideoType>(v.Type.ToString(), out VideoType type);
                //(v.Type, out VideoType type);
            Video video = new Video
            {
                Type = type,
                URL = v.URL,
                GameId = v.GameId
            };
            _unitOfWork.Video.Add(video);
            _unitOfWork.Save();
            return Json(new { success = true, message = "added Successful" });
        }
        #endregion
    }
}
