using Game.Models;
using GameStore.DataAccess;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace GameStore.DataAccess.Data.Repository.IRepository
{
    public interface IGenreRepository : IRepository<Genre>
    {
        IEnumerable<SelectListItem> GetGenreListForDropDown();

        void Update(Genre genre);
    }
}
