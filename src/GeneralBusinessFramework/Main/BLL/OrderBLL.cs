using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DAL.Model;

namespace Main.BLL
{
    public class OrderBLL
    {
        public Order AddOrder(Order order)
        {
            // TODO: simple code later
            using (var context = new AzureDbContext())
            {
                context.Orders.Add(order);
                context.SaveChanges();

                return order;
            }
        }

        public bool IsPurchaseOrderNumberUnique(string inputCode)
        {
            using (var context = new AzureDbContext())
            {
                return context.Orders.Any(o => o.PurchaseOrderNumber == inputCode);
            }
        }

        public IList<Order> GetWhere(Order order)
        {
            using (var context = new AzureDbContext())
            {
                List<Expression<Func<Order, bool>>> whereList = new List<Expression<Func<Order, bool>>>();
                if (order.ID > 0) 
                    whereList.Add(o => o.ID == order.ID);
                if (!string.IsNullOrEmpty(order.PurchaseOrderNumber)) 
                    whereList.Add(o => o.PurchaseOrderNumber == order.PurchaseOrderNumber);
                if (!string.IsNullOrEmpty(order.BuyerName)) 
                    whereList.Add(o => o.BuyerName == order.BuyerName);
                if (!string.IsNullOrEmpty(order.BillingZipCode)) 
                    whereList.Add(o => o.BillingZipCode == order.BillingZipCode);

                var query = context.Orders.AsQueryable();
                foreach (var where in whereList)
                {
                    query = query.Where(where);
                }

                return query.ToList();
            }
        }

    }
}
