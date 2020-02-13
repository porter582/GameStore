using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game.Models.ViewModels
{
    public class GameVM
    {
        public GameModel Game { get; set; }
        public IEnumerable<SelectListItem> RatingList { get; set; }
        public IEnumerable<SelectListItem> GenreList { get; set; }
    }
}
