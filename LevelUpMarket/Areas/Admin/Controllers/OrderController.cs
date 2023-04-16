using LevelUpMarket.DataAccess.Repository.IRepository;
using LevelUpMarket.Models;
using LevelUpMarket.Models.ViewModel;
using LevelUpMarket.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.Checkout;
using Stripe.TestHelpers;
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
        public IActionResult Details_PayNow(int orderId)
        {

            orderVM.Orderheader = _unitOfWork.OrderHeader.GetFirstOrDefault(u => u.Id == orderId, includeProperties: "ApplicationUser");
            orderVM.OrderDetail = _unitOfWork.OrderDetail.GetAll(u => u.OrderId == orderId, includeProperties: "Game");
            //stripe settings
            var domain = "https://localhost:7051/";
            var options = new SessionCreateOptions
            {
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
                SuccessUrl = domain + $"Admin/Order/PaymentConfirmation?orderHeaderId={orderVM.Orderheader.Id}",
                CancelUrl = domain + "Admin/Order/Details?orderId={orderVM.Orderheader.Id}",
            };
            foreach (var item in orderVM.OrderDetail)
            {
                var sessionLineItem = new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)(item.Game.Price * 100),
                        Currency = "usd",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.Game.Name,

                        },
                    },
                    Quantity = item.Count,
                };
                options.LineItems.Add(sessionLineItem);
            }
            var service = new SessionService();
            Session session = service.Create(options);
            orderVM.Orderheader.SessionId = session.Id;
            orderVM.Orderheader.PaymentIntentId = session.PaymentIntentId;
            _unitOfWork.OrderHeader.UpdateStripePaymentId(orderVM.Orderheader.Id, session.Id, session.PaymentIntentId);
            _unitOfWork.Save();
            Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303);
        }
        public IActionResult PaymentConfirmation(int orderHeaderId)
        {
            Orderheader orderheader = _unitOfWork.OrderHeader.GetFirstOrDefault(u => u.Id == orderHeaderId);

            var service = new SessionService();
            Session session = service.Get(orderheader.SessionId);
            // check the stripe status
            if (session.PaymentStatus.ToLower() == "paid")
            {
                _unitOfWork.OrderHeader.UpdateStripePaymentId(orderHeaderId, orderheader.SessionId, session.PaymentIntentId);

                _unitOfWork.OrderHeader.UpdateStatus(orderHeaderId, SD.StatusApproved, SD.PaymentStatusApproved);
                _unitOfWork.Save();
            }
            List<ShoppingCart> shoppingCarts = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == orderheader.ApplicationUserId).ToList();
            _unitOfWork.ShoppingCart.RemoveRange(shoppingCarts);
            _unitOfWork.Save();

            return View(orderHeaderId);

        }
        [HttpPost]
        [Authorize(Roles =SD.Role_Admin)]
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

        [HttpPost]
        [Authorize(Roles = SD.Role_Admin)]
        [ValidateAntiForgeryToken]
        public IActionResult StartProcessing()
        {
            _unitOfWork.OrderHeader.UpdateStatus(orderVM.Orderheader.Id, SD.StatusInProcess);
            _unitOfWork.Save();
            TempData["Success"] = "Order Status Updated Successfully.";
            return RedirectToAction("Details", "Order", new { orderId = orderVM.Orderheader.Id });
        }

        [HttpPost]
        [Authorize(Roles = SD.Role_Admin)]
        [ValidateAntiForgeryToken]
        public IActionResult ShipOrder()
        {

            var orderHeader = _unitOfWork.OrderHeader.GetFirstOrDefault(u => u.Id == orderVM.Orderheader.Id, tracked: false);
            orderHeader.TrackingNumber = orderVM.Orderheader.TrackingNumber;
            orderHeader.Carrier = orderVM.Orderheader.Carrier;
            orderHeader.OrderStatus = SD.StatusShipped;
            orderHeader.ShippingDate = DateTime.Now;
           
            _unitOfWork.OrderHeader.Update(orderHeader);
            _unitOfWork.Save();
            TempData["Success"] = "Order Shipped Successfully.";
            return RedirectToAction("Details", "Order", new { orderId = orderVM.Orderheader.Id });
        }


        [HttpPost]
        [Authorize(Roles = SD.Role_Admin)]
        [ValidateAntiForgeryToken]
        public IActionResult CancelOrder()
        {

            var orderHeader = _unitOfWork.OrderHeader.GetFirstOrDefault(u => u.Id == orderVM.Orderheader.Id, tracked: false);
            if(orderHeader.PaymentStatus == SD.PaymentStatusApproved)
            {
                var options = new RefundCreateOptions
                {
                    Reason = RefundReasons.RequestedByCustomer,
                    PaymentIntent = orderHeader.PaymentIntentId,
                    
                };
                var service = new Stripe.RefundService();
                Refund refund = service.Create(options);
                _unitOfWork.OrderHeader.UpdateStatus(orderHeader.Id, SD.StatusCancelled,SD.StatusRefunded);
            }
            else
            {
                _unitOfWork.OrderHeader.UpdateStatus(orderHeader.Id, SD.StatusCancelled, SD.StatusCancelled);

            }
            _unitOfWork.Save();
            TempData["Success"] = "Order Cancelled Successfully.";
            return RedirectToAction("Details", "Order", new { orderId = orderVM.Orderheader.Id });
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
