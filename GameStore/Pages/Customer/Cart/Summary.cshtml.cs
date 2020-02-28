using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Game.Utility;
using Game.Models;
using Game.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Stripe;
using GameStore.DataAccess;

namespace GameStore.Pages.Customer.Cart
{
    public class SummaryModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public SummaryModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [BindProperty] //Gets hidden Id from html
        public OrderDetailsCart OrderDetailsCartVM { get; set; }
        public void OnGet()
        {
            OrderDetailsCartVM = new OrderDetailsCart()
            {
                OrderHeader = new OrderHeader(),
                listCart = new List<ShoppingCart>()
            };

            OrderDetailsCartVM.OrderHeader.OrderTotal = 0.0;

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            if (claim != null)
            {
                IEnumerable<ShoppingCart> cart =
                    _unitOfWork.ShoppingCart.GetAll(c => c.ApplicationUserId == claim.Value);

                if (cart != null)
                {
                    OrderDetailsCartVM.listCart = cart.ToList();
                }

                foreach (var cartList in OrderDetailsCartVM.listCart)
                {
                    cartList.GameItem = _unitOfWork.GameObj.GetFirstOrDefault
                        (m => m.Id == cartList.GameItemId);
                    OrderDetailsCartVM.OrderHeader.OrderTotal +=
                        (cartList.GameItem.Price * cartList.Count);
                }

                OrderDetailsCartVM.OrderHeader.OrderTotal +=
                    OrderDetailsCartVM.OrderHeader.OrderTotal * SD.SalesTaxPercent; //added sales tax to total

                ApplicationUser applicationUser = _unitOfWork.ApplicationUser.GetFirstOrDefault(c => c.Id == claim.Value);
                OrderDetailsCartVM.OrderHeader.PhoneNumber = applicationUser.PhoneNumber;
                OrderDetailsCartVM.OrderHeader.ApplicationUser = applicationUser;
            }
        }

        public IActionResult OnPost(string stripeToken)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            OrderDetailsCartVM.listCart = _unitOfWork.ShoppingCart.GetAll(c => c.ApplicationUserId == claim.Value).ToList();

            OrderDetailsCartVM.OrderHeader.PaymentStatus = SD.PaymentStatusPending;
            OrderDetailsCartVM.OrderHeader.OrderDate = DateTime.Now;
            OrderDetailsCartVM.OrderHeader.UserId = claim.Value;
            List<OrderDetails> orderDetailsList = new List<OrderDetails>();
            _unitOfWork.OrderHeader.Add(OrderDetailsCartVM.OrderHeader);
            _unitOfWork.Save();

            foreach (var item in OrderDetailsCartVM.listCart)
            {
                item.GameItem = _unitOfWork.GameObj.GetFirstOrDefault(m => m.Id == item.GameItemId);
                OrderDetails orderDetails = new OrderDetails
                {
                    GameItemId = item.GameItemId,
                    OrderId = OrderDetailsCartVM.OrderHeader.Id,
                    Name = item.GameItem.Name,
                    Price = item.GameItem.Price,
                    Count = item.Count
                };

                OrderDetailsCartVM.OrderHeader.OrderTotal += (orderDetails.Count * orderDetails.Price) * (1 + SD.SalesTaxPercent);
                _unitOfWork.OrderDetails.Add(orderDetails);
            }
            OrderDetailsCartVM.OrderHeader.OrderTotal = Convert.ToDouble(String.Format("{0:.##}", OrderDetailsCartVM.OrderHeader.OrderTotal));
            _unitOfWork.ShoppingCart.RemoveRange(OrderDetailsCartVM.listCart);
            HttpContext.Session.SetInt32(SD.ShoppingCart, 0);
            _unitOfWork.Save();

            if (stripeToken != null)
            {
                var options = new ChargeCreateOptions
                {
                    Amount = Convert.ToInt32(OrderDetailsCartVM.OrderHeader.OrderTotal * 100 * SD.SalesTaxPercent), //could be salestaxrate
                    Currency = "usd",
                    Description = "Order ID: " + OrderDetailsCartVM.OrderHeader.Id,
                    Source = stripeToken
                };
                var service = new ChargeService();
                Charge charge = service.Create(options);
                OrderDetailsCartVM.OrderHeader.TransactionId = charge.Id;

                if (charge.Status.ToLower() == "succeeded")
                {
                    //Send Confirmation email here
                    OrderDetailsCartVM.OrderHeader.PaymentStatus = SD.PaymentStatusApproved;
                }
                else
                {
                    OrderDetailsCartVM.OrderHeader.PaymentStatus = SD.PaymentStatusRejected;
                }
            }
            _unitOfWork.Save();
            return RedirectToPage("/Customer/Cart/ORderConfirmation", new { id = OrderDetailsCartVM.OrderHeader.Id }); //pass order id to new page
        }

        public IActionResult OnPostPlus(int cartId)
        {
            var cart = _unitOfWork.ShoppingCart.GetFirstOrDefault
                (c => c.Id == cartId);
            _unitOfWork.ShoppingCart.IncrementCount(cart, 1);
            _unitOfWork.Save();
            return RedirectToPage("/Customer/Cart/Summary");
        }

        public IActionResult OnPostMinus(int cartId)
        {
            var cart = _unitOfWork.ShoppingCart.GetFirstOrDefault
                (c => c.Id == cartId);
            if (cart.Count == 1)
            {
                _unitOfWork.ShoppingCart.Remove(cart);
            }
            else
            {
                _unitOfWork.ShoppingCart.DecrementCount(cart, 1);
            }
            _unitOfWork.Save();
            var cnt = _unitOfWork.ShoppingCart.GetAll
                (u => u.ApplicationUserId == cart.ApplicationUserId).ToList().Count;

            HttpContext.Session.SetInt32(SD.ShoppingCart, cnt);
            return RedirectToPage("/Customer/Cart/Summary");
        }

        public IActionResult OnPostRemove(int cartId)
        {
            var cart = _unitOfWork.ShoppingCart.GetFirstOrDefault
                (c => c.Id == cartId);

            _unitOfWork.ShoppingCart.Remove(cart);
            _unitOfWork.Save();
            var cnt = _unitOfWork.ShoppingCart.GetAll
                (u => u.ApplicationUserId == cart.ApplicationUserId).ToList().Count;

            HttpContext.Session.SetInt32(SD.ShoppingCart, cnt);
            return RedirectToPage("/Customer/Cart/Summary");
        }
    }
}