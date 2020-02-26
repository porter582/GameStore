using System.Linq;
using System.Security.Claims;
using Game.Models;
using Game.Utility;
using GameStore.DataAccess;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GameStore.Pages.Customer.Home
{
    public class DetailsModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public DetailsModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [BindProperty]
        public ShoppingCart ShoppingCartObj { get; set; }

        public void OnGet(int id)
        {
            ShoppingCartObj = new ShoppingCart()
            {
                GameItem = _unitOfWork.GameObj.GetFirstOrDefault
                (includeProperties: "Genre,Rating", filter: c => c.Id == id),
                GameItemId = id
            };
        }

        public IActionResult OnPost()
        {
            if (ModelState.IsValid && User.IsInRole(SD.CustomerRole)) //change later
            {
                var claimsIdentity = (ClaimsIdentity)this.User.Identity;
                var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

                ShoppingCartObj.ApplicationUserId = claim.Value;
                ShoppingCart cartFromDb =
                    _unitOfWork.ShoppingCart.GetFirstOrDefault(c =>
                    c.ApplicationUserId == ShoppingCartObj.ApplicationUserId &&
                    c.GameItemId == ShoppingCartObj.GameItemId);

                //does a shopping cart (item list) exist in the db
                if (cartFromDb == null)
                {
                    _unitOfWork.ShoppingCart.Add(ShoppingCartObj);
                }

                //already exists for this item/user combo just update count
                else
                {
                    _unitOfWork.ShoppingCart.IncrementCount(cartFromDb, ShoppingCartObj.Count);
                }

                _unitOfWork.Save();
                var count = _unitOfWork.ShoppingCart.GetAll(c =>
                c.ApplicationUserId == ShoppingCartObj.ApplicationUserId).ToList().Count;
                HttpContext.Session.SetInt32(SD.ShoppingCart, count);
                return RedirectToPage("Index");
            }
            else //Adding new item
            {
                ShoppingCartObj.GameItem =
                    _unitOfWork.GameObj.GetFirstOrDefault(includeProperties:
                    "Genre,Rating", filter: c => c.Id == ShoppingCartObj.GameItemId);
                return Page();
            }
        }
    }
}