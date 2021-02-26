using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Model
{
    public class OrderBase : ModelBase
    {
        public string BuyerName { get; set; }
        public string PurchaseOrderNumber { get; set; }
        public string BillingZipCode { get; set; }
        public decimal OrderAmount { get; set; }
    }

    public class Order : OrderBase
    {

    }
}
