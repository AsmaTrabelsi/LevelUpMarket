using LevelUpMarket.DataAccess.Repository.IRepository;
using LevelUpMarket.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;

namespace LevelUpMarketWeb.Areas.Admin.Controllers
{
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
        public IActionResult DeletePost(int? id)
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
                URL= url,
                GameId= gameId
            };
            _unitOfWork.Video.Add(video);
            _unitOfWork.Save();
            return Json(new { success = true, message = "added Successful" });
        }
        #endregion
    }
}
