using System.Collections.Generic;
using System.Linq;
using Game.Models;
using GameStore.DataAccess;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GameStore.Pages.Customer.Home
{
    public class IndexModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public IEnumerable<GameModel> GameList { get; set; }
        public IEnumerable<Game.Models.Genre> GenreList { get; set; }
        public IndexModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void OnGet()
        {
            GameList = _unitOfWork.GameObj.GetAll(null, null, "Genre,Rating");
            GenreList = _unitOfWork.Genre.GetAll(null, null, null);
        }
    }
}