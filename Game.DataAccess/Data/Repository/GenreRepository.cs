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
    class GenreRepository : Repository<Genre>, IGenreRepository
    {
        private readonly ApplicationDbContext _db;

        public GenreRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public IEnumerable<SelectListItem> GetGenreListForDropDown()
        {
            return _db.Genre.Select(i => new SelectListItem()
            {
                Text = i.Name,
                Value = i.Id.ToString()
            });
        }

        public void Update(Genre genre)
        {
            var objFromDb = _db.Genre.FirstOrDefault(s => s.Id == genre.Id);

            objFromDb.Name = genre.Name;
            objFromDb.ReleaseDate = genre.ReleaseDate;

            _db.SaveChanges();
        }
    }
}
