using LevelUpMarket.Data;
using LevelUpMarket.DataAccess.Repository.IRepository;
using LevelUpMarket.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LevelUpMarket.DataAccess.Repository
{
    public class OrderHeaderRepository : Repository<Orderheader>, IOrderHeaderRepository
    {
        private ApplicationDbContext _db;
        public OrderHeaderRepository(ApplicationDbContext db):base(db)
        {
            _db = db;
        }
       

        void IOrderHeaderRepository.Update(Orderheader obj)
        {
            _db.Orderheaders.Update(obj);
        }

        void IOrderHeaderRepository.UpdateStatus(int id, string orderStatus, string? paymentStatus=null)
        {
            var orderFromDb = _db.Orderheaders.FirstOrDefault(o => o.Id == id);
            if(orderFromDb != null)
            {
                orderFromDb.OrderStatus = orderStatus;
                if(paymentStatus != null)
                {
                    orderFromDb.PaymentStatus = paymentStatus;
                }
            }
        }
        void IOrderHeaderRepository.UpdateStripePaymentId(int id, string sessionId, string paymentItentId)
        {
            var orderFromDb = _db.Orderheaders.FirstOrDefault(o => o.Id == id);
            orderFromDb.PaymentDate = DateTime.Now;
            orderFromDb.SessionId = sessionId;
            orderFromDb.PaymentIntentId = paymentItentId;

        }
    }
}
