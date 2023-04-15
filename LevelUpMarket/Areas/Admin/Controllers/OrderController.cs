using LevelUpMarket.DataAccess.Repository.IRepository;
using LevelUpMarket.Models;
using LevelUpMarket.Models.ViewModel;
using LevelUpMarket.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace LevelUpMarketWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        [BindProperty]
        public OrderVM orderVM { get; set; }    
        public OrderController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Details(int orderId)
        {
            OrderVM orderVM = new OrderVM()
            {
                Orderheader = _unitOfWork.OrderHeader.GetFirstOrDefault(u => u.Id == orderId,includeProperties:"ApplicationUser"),
                OrderDetail = _unitOfWork.OrderDetail.GetAll(u=> u.OrderId == orderId,includeProperties:"Game")
            };
            return View(orderVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateOrderDetail()
        {

            var orderHEFromDb = _unitOfWork.OrderHeader.GetFirstOrDefault(u => u.Id == orderVM.Orderheader.Id,tracked:false);
            orderHEFromDb.Name = orderVM.Orderheader.Name;
            orderHEFromDb.PhoneNumber = orderVM.Orderheader.PhoneNumber;
            orderHEFromDb.StreetAddress = orderVM.Orderheader.StreetAddress;
            orderHEFromDb.City = orderVM.Orderheader.City;
            orderHEFromDb.State = orderVM.Orderheader.State;
            orderHEFromDb.PostalCode = orderVM.Orderheader.PostalCode;
            if(orderVM.Orderheader.Carrier != null)
            {
                orderHEFromDb.Carrier = orderVM.Orderheader.Carrier;
            }
            if (orderVM.Orderheader.TrackingNumber != null)
            {
                orderHEFromDb.TrackingNumber = orderVM.Orderheader.TrackingNumber;
            }
            _unitOfWork.OrderHeader.Update(orderHEFromDb);
            _unitOfWork.Save();
            TempData["Success"] = "Order Details Updated Successfully.";
            return RedirectToAction("Details","Order", new {orderId=orderHEFromDb.Id});
        }

        #region API CALLS
        [HttpGet]
        public IActionResult GetAll(string status)

        {
            IEnumerable<Orderheader> orderheaders;
            if (User.IsInRole(SD.Role_Admin))
            {
                orderheaders = _unitOfWork.OrderHeader.GetAll(includeProperties: "ApplicationUser");

            }
            else
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                orderheaders = _unitOfWork.OrderHeader.GetAll(u=> u.ApplicationUser.Id ==claim.Value ,includeProperties: "ApplicationUser");

            }
            switch (status)
            {
                case "pending":
                    orderheaders= orderheaders.Where(u => u.OrderStatus == SD.StatusPending);
                    break;
                case "inprocess":
                    orderheaders = orderheaders.Where(u => u.OrderStatus == SD.StatusInProcess);
                    break;
                case "completed":
                    orderheaders = orderheaders.Where(u => u.OrderStatus == SD.StatusShipped);break;
                case "approved":
                    orderheaders = orderheaders.Where(u => u.OrderStatus == SD.StatusApproved);
                    break;
               

            }
            return Json(new { data = orderheaders });
        }


        #endregion
    }
}
