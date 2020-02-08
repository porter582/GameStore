﻿using GameStore.DataAccess.Data.Repository.IRepository;
using System;

namespace GameStore.DataAccess
{
    public interface IUnitOfWork : IDisposable
    {
        IGenreRepository Genre { get; }


        void Save();
    }
}