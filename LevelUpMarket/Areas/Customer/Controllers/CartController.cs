using LevelUpMarket.DataAccess.Repository.IRepository;
using LevelUpMarket.Models;
using LevelUpMarket.Models.ViewModel;
using LevelUpMarket.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Stripe.Checkout;
using System.Security.Claims;

namespace LevelUpMarketWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class CartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailSender _emailSender; 
        [BindProperty]
        public ShoppingCartVM ShoppingCartVM { get; set; }
        
        public CartController(IUnitOfWork unitOfWork, IEmailSender emailSender)
        {
            _unitOfWork = unitOfWork;
            _emailSender = emailSender;
        }

        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            ShoppingCartVM = new ShoppingCartVM
            {
                ListCart = _unitOfWork.ShoppingCart.GetAll(s => s.ApplicationUserId == claim.Value, includeProperties: "Game"),
                Orderheader = new()

            };

            foreach(var cart in ShoppingCartVM.ListCart)
            {
                ShoppingCartVM.Orderheader.OrderTotal += cart.Count * cart.Game.Price;
            }
            return View(ShoppingCartVM);
        }

        public IActionResult Plus(int cartId)
        {
            var cart = _unitOfWork.ShoppingCart.GetFirstOrDefault(u=> u.Id == cartId);
            _unitOfWork.ShoppingCart.IncrementCount(cart, 1);
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Minus(int cartId)
        {
            var cart = _unitOfWork.ShoppingCart.GetFirstOrDefault(u => u.Id == cartId);
           if(cart.Count <= 1)
            {
                _unitOfWork.ShoppingCart.Remove(cart);
                var count = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == cart.ApplicationUserId).ToList().Count-1;
                HttpContext.Session.SetInt32(SD.SessionCart, count);

            }
            else
            {
                _unitOfWork.ShoppingCart.DecrementCount(cart, 1);

            }
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Remove(int cartId)
        {
            var cart = _unitOfWork.ShoppingCart.GetFirstOrDefault(u => u.Id == cartId);
            _unitOfWork.ShoppingCart.Remove(cart);
            _unitOfWork.Save();
            var count = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == cart.ApplicationUserId).ToList().Count;
            HttpContext.Session.SetInt32(SD.SessionCart, count);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Summary()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            ShoppingCartVM = new ShoppingCartVM
            {
                ListCart = _unitOfWork.ShoppingCart.GetAll(s => s.ApplicationUserId == claim.Value, includeProperties: "Game"),
                Orderheader = new()

            };

            ShoppingCartVM.Orderheader.ApplicationUser = _unitOfWork.ApplicationUser.GetFirstOrDefault(u => u.Id == claim.Value);

            ShoppingCartVM.Orderheader.Name = ShoppingCartVM.Orderheader.ApplicationUser.Name;
            ShoppingCartVM.Orderheader.PhoneNumber = ShoppingCartVM.Orderheader.ApplicationUser.PhoneNumber;
            ShoppingCartVM.Orderheader.StreetAddress = ShoppingCartVM.Orderheader.ApplicationUser.StreetAdress;
            ShoppingCartVM.Orderheader.City = ShoppingCartVM.Orderheader.ApplicationUser.City;
            ShoppingCartVM.Orderheader.State = ShoppingCartVM.Orderheader.ApplicationUser.State;
            ShoppingCartVM.Orderheader.PostalCode = ShoppingCartVM.Orderheader.ApplicationUser.PostalCode;





            foreach (var cart in ShoppingCartVM.ListCart)
            {
                ShoppingCartVM.Orderheader.OrderTotal += cart.Count * cart.Game.Price;
            }
            return View(ShoppingCartVM);
           
        }

        [HttpPost]
        [ActionName("Summary")]
        [ValidateAntiForgeryToken]
        public IActionResult SummaryPost()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            ShoppingCartVM.ListCart = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUser.Id == claim.Value,includeProperties: "Game");

            ShoppingCartVM.Orderheader.PaymentStatus = SD.PaymentStatusPending;
            ShoppingCartVM.Orderheader.OrderStatus = SD.StatusPending;
            ShoppingCartVM.Orderheader.OrderDate = System.DateTime.Now;
            ShoppingCartVM.Orderheader.ApplicationUserId = claim.Value;
            foreach (var cart in ShoppingCartVM.ListCart)
            {
                ShoppingCartVM.Orderheader.OrderTotal += cart.Count * cart.Game.Price;
            }

            _unitOfWork.OrderHeader.Add(ShoppingCartVM.Orderheader);
            _unitOfWork.Save();
            foreach(var cart in ShoppingCartVM.ListCart)
            {
                OrderDetail orderDetail = new()
                {
                    GameId = cart.GameId,
                    OrderId = ShoppingCartVM.Orderheader.Id,
                    Price = cart.Game.Price,
                    Count = cart.Count
                };
                _unitOfWork.OrderDetail.Add(orderDetail);
                _unitOfWork.Save();
            }

            // stripe settings
            var domain = "https://localhost:7051/";
            var options = new SessionCreateOptions
            {
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
                SuccessUrl = domain+$"customer/cart/OrderConfirmation?id={ShoppingCartVM.Orderheader.Id}",
                CancelUrl = domain+"customer/cart/index",
            };
            foreach (var item in ShoppingCartVM.ListCart)
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
            ShoppingCartVM.Orderheader.SessionId = session.Id;
            ShoppingCartVM.Orderheader.PaymentIntentId = session.PaymentIntentId;
            _unitOfWork.OrderHeader.UpdateStripePaymentId(ShoppingCartVM.Orderheader.Id, session.Id, session.PaymentIntentId);
            _unitOfWork.Save();
            Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303);
        
      

        }

        public IActionResult OrderConfirmation(int id)
        {
            Orderheader orderheader = _unitOfWork.OrderHeader.GetFirstOrDefault(u => u.Id == id,includeProperties:"ApplicationUser");
          

            var service = new SessionService();
            Session session = service.Get(orderheader.SessionId);
            // check the stripe status
            if(session.PaymentStatus.ToLower() == "paid")
            {
                _unitOfWork.OrderHeader.UpdateStripePaymentId(id, orderheader.SessionId, session.PaymentIntentId);

                _unitOfWork.OrderHeader.UpdateStatus(id, SD.StatusApproved,SD.PaymentStatusApproved);
                _unitOfWork.Save();
            }
            _emailSender.SendEmailAsync(orderheader.ApplicationUser.Email, "New order - LevelUp Market","<p> new OrderDetail Created </p> ");
            List<ShoppingCart> shoppingCarts = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == orderheader.ApplicationUserId).ToList();
            HttpContext.Session.Clear(); 
            _unitOfWork.ShoppingCart.RemoveRange(shoppingCarts);
               _unitOfWork.Save();
             return View(id);

        }
    }
}
