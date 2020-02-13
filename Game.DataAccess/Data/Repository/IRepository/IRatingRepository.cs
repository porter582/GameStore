using Game.Models;
using GameStore.DataAccess;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace GameStore.DataAccess.Data.Repository.IRepository
{
    public interface IRatingRepository : IRepository<Rating>
    {
        IEnumerable<SelectListItem> GetRatingListForDropDown();

        void Update(Rating rating);
    }
}
