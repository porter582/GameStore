using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameStore.DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GameStore.Pages.Admin.Genre
{
    public class UpsertModel : PageModel
    {
        private readonly IUnitOfWork _unitofWork;

        public UpsertModel(IUnitOfWork unitofWork)
        {
            _unitofWork = unitofWork;
        }

        [BindProperty]
        public Game.Models.Genre GenreObj { get; set; }
        public IActionResult OnGet(int? id) ///IActionResult return type is page, obj
        {
            GenreObj = new Game.Models.Genre();
            if (id != null) //edit
            {
                GenreObj = _unitofWork.Genre.GetFirstOrDefault(u => u.Id == id);
                if (GenreObj == null)
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

            if (GenreObj.Id == 0) //new genre
            {
                _unitofWork.Genre.Add(GenreObj);
            }
            else //else we edit
            {
                _unitofWork.Genre.Update(GenreObj);
            }
            _unitofWork.Save();
            return RedirectToPage("./Index");
        }
    }
}