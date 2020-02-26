using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GameStore.Pages.Customer.Cart
{
    public class OrderConfirmationModel : PageModel
    {
        [BindProperty]
        public int orderId { get; set; }
        public void OnGet(int id)
        {
            orderId = id;
        }
    }
}