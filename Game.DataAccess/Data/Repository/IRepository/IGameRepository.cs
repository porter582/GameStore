using Game.Models;
using GameStore.DataAccess;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace GameStore.DataAccess.Data.Repository.IRepository
{
    public interface IGameRepository : IRepository<GameModel>
    {
        //IEnumerable<SelectListItem> GetGameListForDropDown();

        void Update(GameModel game);
    }
}
