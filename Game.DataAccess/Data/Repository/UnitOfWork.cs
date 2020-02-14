using Game.DataAccess.Data.Repository;
using Game.DataAccess.Data.Repository.IRepository;
using GameStore.DataAccess.Data.Repository.IRepository;

namespace GameStore.DataAccess.Data.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;
        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            Genre = new GenreRepository(_db);
            Rating = new RatingRepository(_db);
            GameObj = new GameRepository(_db);
            ApplicationUserObj = new ApplicationUserRepository(_db);
        }

        public IGenreRepository Genre { get; private set; }
        public IRatingRepository Rating { get; private set; }
        public IGameRepository GameObj { get; private set; }
        public IApplicationUserRepository ApplicationUserObj { get; private set; }

        public void Dispose()
        {
            //throw new NotImplementedException();
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
