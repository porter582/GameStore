using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameStore.DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GameStore.Pages.Admin.Rating
{
    public class UpsertModel : PageModel
    {
        private readonly IUnitOfWork _unitofWork;

        public UpsertModel(IUnitOfWork unitofWork)
        {
            _unitofWork = unitofWork;
        }

        [BindProperty]
        public Game.Models.Rating RatingObj { get; set; }
        public IActionResult OnGet(int? id) ///IActionResult return type is page, obj
        {
            RatingObj = new Game.Models.Rating();
            if (id != null) //edit
            {
                RatingObj = _unitofWork.Rating.GetFirstOrDefault(u => u.Id == id);
                if (RatingObj == null)
                {
                    return NotFound();
                }
            }

            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (RatingObj.Id == 0) //new genre
            {
                _unitofWork.Rating.Add(RatingObj);
            }
            else //else we edit
            {
                _unitofWork.Rating.Update(RatingObj);
            }
            _unitofWork.Save();
            return RedirectToPage("./Index");
        }
    }
}