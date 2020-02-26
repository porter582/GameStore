using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Game.Models.ViewModels;
using Game.Utility;
using GameStore.DataAccess;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GameStore.Pages.Customer.Cart
{
    public class IndexModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public IndexModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public OrderDetailsCart OrderDetailsCartVM { get; set; }
        public void OnGet()
        {
            OrderDetailsCartVM = new OrderDetailsCart()
            {
                OrderHeader = new Game.Models.OrderHeader(),
                listCart = new List<Game.Models.ShoppingCart>()
            };

            OrderDetailsCartVM.OrderHeader.OrderTotal = 0.0;

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            if (claim != null)
            {
                IEnumerable<Game.Models.ShoppingCart> cart =
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
            }
        }

        public IActionResult OnPostPlus(int cartId)
        {
            var cart = _unitOfWork.ShoppingCart.GetFirstOrDefault
                (c => c.Id == cartId);
            _unitOfWork.ShoppingCart.IncrementCount(cart, 1);
            _unitOfWork.Save();
            return RedirectToPage("/Customer/Cart/Index");
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
            return RedirectToPage("/Customer/Cart/Index");
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
            return RedirectToPage("/Customer/Cart/Index");
        }
    }
}