using Game.DataAccess.Data.Repository.IRepository;
using GameStore.DataAccess.Data.Repository.IRepository;
using System;

namespace GameStore.DataAccess
{
    public interface IUnitOfWork : IDisposable
    {
        IGenreRepository Genre { get; }
        IRatingRepository Rating { get; }
        IGameRepository GameObj { get; }
        IApplicationUserRepository ApplicationUser { get; }


        void Save();
    }
}
