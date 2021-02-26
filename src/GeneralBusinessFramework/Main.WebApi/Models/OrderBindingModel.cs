using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AzureProgrammerPractice.WebApi.Models
{
    public class OrderBindingModel
    {
        public int? ID { get; set; }
        public string BuyerName { get; set; }
        public string PurchaseOrderNumber { get; set; }
        public string BillingZipCode { get; set; }
        public decimal? OrderAmount { get; set; }
    }
}