using Game.DataAccess.Data.Repository.IRepository;
using Game.Models;
using GameStore.DataAccess;
using GameStore.DataAccess.Data.Repository;
using System.Linq;

namespace Game.DataAccess.Data.Repository
{
    public class OrderHeaderRepository : Repository<OrderHeader>, IOrderHeaderRepository
    {
        private readonly ApplicationDbContext _db;

        public OrderHeaderRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(OrderHeader objectToBeUpdated)
        {
            var orderHeaderFromDb = _db.OrderHeader.FirstOrDefault(s => s.Id == objectToBeUpdated.Id);
            _db.OrderHeader.Update(objectToBeUpdated);
            _db.SaveChanges();
        }

    }
}
