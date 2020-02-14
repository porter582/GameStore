using Game.DataAccess.Data.Repository.IRepository;
using Game.Models;
using GameStore.DataAccess;
using GameStore.DataAccess.Data.Repository;

namespace Game.DataAccess.Data.Repository
{
    public class ApplicationUserRepository : Repository<ApplicationUser>, IApplicationUserRepository
    {
        private readonly ApplicationDbContext _db;

        public ApplicationUserRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
    }
}
