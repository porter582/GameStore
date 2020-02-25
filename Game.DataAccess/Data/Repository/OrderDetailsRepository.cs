using Game.DataAccess.Data.Repository.IRepository;
using Game.Models;
using GameStore.DataAccess;
using GameStore.DataAccess.Data.Repository;
using System.Linq;

namespace Game.DataAccess.Data.Repository
{
    public class OrderDetailsRepository : Repository<OrderDetails>, IOrderDetailsRepository
    {
        private readonly ApplicationDbContext _db;

        public OrderDetailsRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(OrderDetails objectToBeUpdated)
        {
            var orderDetailsFromDb = _db.OrderDetails.FirstOrDefault(s => s.Id == objectToBeUpdated.Id);
            _db.OrderDetails.Update(objectToBeUpdated);
            _db.SaveChanges();
        }

    }
}
