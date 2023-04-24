using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using BulkyWeb.DataAccess.Data;
using System.Linq.Expressions;

namespace Bulky.DataAccess.Repository
{
    //doing this allows us to only implement the methods we want extra and not everything from Repository + OrderHeaderRepository - only OrderHeaderRepository
    public class OrderHeaderRepository : Repository<OrderHeader>, IOrderHeaderRepository
    {
        private ApplicationDbContext _db;

        public OrderHeaderRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(OrderHeader obj)
        {
            _db.OrderHeader.Update(obj);
        }

		public void UpdateStatus(int id, string orderStatus, string? paymentStatus = null)
		{
            var orderFromDb = _db.OrderHeader.FirstOrDefault(u => u.Id == id);
            if (orderFromDb != null)
            {
				orderFromDb.OrderStatus = orderStatus;
                if (!string.IsNullOrEmpty(paymentStatus))
                {
					orderFromDb.PaymentStatus = paymentStatus;

				}
			}
		}

        public void UpdateStripePaymentId(int id, string sessionId, string paymentIntentId)
        {
			var orderFromDb = _db.OrderHeader.FirstOrDefault(u => u.Id == id);
            //sessionId gets generated when a user makes a payment
            if (!string.IsNullOrEmpty(sessionId))
            {
				orderFromDb.SessionId = sessionId;

			}

			//if paymentIntentId != null, that means the payment was successful
			if (!string.IsNullOrEmpty(paymentIntentId))
			{
				orderFromDb.PaymentIntentId = paymentIntentId;
				orderFromDb.PaymentDate = DateTime.Now;

			}
		}
	}
}
