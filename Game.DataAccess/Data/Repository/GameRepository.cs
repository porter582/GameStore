using Game.Models;
using GameStore.DataAccess;
using GameStore.DataAccess.Data.Repository;
using GameStore.DataAccess.Data.Repository.IRepository;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game.DataAccess.Data.Repository
{
    class GameRepository : Repository<GameModel>, IGameRepository
    {
        private readonly ApplicationDbContext _db;

        public GameRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        //public IEnumerable<SelectListItem> GetGameListForDropDown()
        //{
        //    return _db.GameModel.Select(i => new SelectListItem()
        //    {
        //        Text = i.Name,
        //        Value = i.Id.ToString()
        //    });
        //}

        public void Update(GameModel game)
        {
            var objFromDb = _db.GameModel.FirstOrDefault(s => s.Id == game.Id);

            objFromDb.Name = game.Name;
            objFromDb.Description = game.Description;
            objFromDb.RatingId = game.RatingId;
            objFromDb.GenreId = game.GenreId;
            if (game.Image != null)
            {
                objFromDb.Image = objFromDb.Image;
            }
            //objFromDb.ReleaseDate = genre.ReleaseDate;

            _db.SaveChanges();
        }
    }
}
