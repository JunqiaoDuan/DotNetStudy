using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AzureProgrammerPractice.WebApi.Models;
using DAL.Model;
using Main.BLL;

namespace Main.WebApi.Controllers
{
    [RoutePrefix("api")]
    public class OrderController : ApiController
    {
        [Route("post-order")]
        [HttpPost]
        public IHttpActionResult Add([FromBody]OrderBindingModel binding)
        {
            // check para
            decimal? orderAmount = binding.OrderAmount;
            if (orderAmount == null
                || string.IsNullOrEmpty(binding.PurchaseOrderNumber)
                || string.IsNullOrEmpty(binding.BillingZipCode)
                || string.IsNullOrEmpty(binding.BuyerName))
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
            var bll = new OrderBLL();
            if (bll.IsPurchaseOrderNumberUnique(binding.PurchaseOrderNumber))
                throw new HttpResponseException(HttpStatusCode.NoContent);


            var order = new Order
            {
                BuyerName = binding.BuyerName,
                PurchaseOrderNumber = binding.PurchaseOrderNumber,
                BillingZipCode = binding.BillingZipCode,
                OrderAmount = (decimal) orderAmount,
            };


            var createdOrder = bll.AddOrder(order);

            // todo: make it simple
            var view = new OrderViewModel()
            {
                ID = createdOrder.ID,
                BuyerName = createdOrder.BuyerName,
                PurchaseOrderNumber = createdOrder.PurchaseOrderNumber,
                BillingZipCode = createdOrder.BillingZipCode,
                OrderAmount = createdOrder.OrderAmount,
            };

            return Created("Order", view);
        }

        [Route("get")]
        [HttpGet]
        public IHttpActionResult Get([FromUri]string code = "", string purchaseOrderCode = "", string buyerName = "")
        {
            var bll = new OrderBLL();

            var inputOrder = new Order()
            {
                PurchaseOrderNumber = purchaseOrderCode,
                BuyerName = buyerName,
                BillingZipCode = code,
            };

            var orders = bll.GetWhere(inputOrder);

            // todo: mapper later.
            return Ok(orders);
        }
    }
}
