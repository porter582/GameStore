﻿using Game.Models;
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
    class RatingRepository : Repository<Rating>, IRatingRepository
    {
        private readonly ApplicationDbContext _db;

        public RatingRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public IEnumerable<SelectListItem> GetRatingListForDropDown()
        {
            return _db.Rating.Select(i => new SelectListItem()
            {
                Text = i.Name,
                Value = i.Id.ToString()
            });
        }

        public void Update(Rating genre)
        {
            var objFromDb = _db.Rating.FirstOrDefault(s => s.Id == genre.Id);

            objFromDb.Name = genre.Name;
            objFromDb.Description = genre.Description;
            //objFromDb.ReleaseDate = genre.ReleaseDate;

            _db.SaveChanges();
        }
    }
}
