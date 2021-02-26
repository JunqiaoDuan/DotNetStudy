using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using AzureProgrammerPractice.WebApi.Models;
using DAL.Model;
using Main.BLL;
using Main.WebApi.Controllers;
using NUnit.Framework;

namespace Main.WebApi.IntegrationTests
{
    [TestFixture]
    public class OrderControllerIntegrationTests
    {
        [Test]
        public void AddNewOrder_AddNew_Success()
        {
            // Arrange
            var controller = new OrderController();
            var binding = new OrderBindingModel()
            {
                BuyerName = "BuyerName1",
                PurchaseOrderNumber = "ExternalOrder" + Guid.NewGuid(),
                BillingZipCode = "BillingZipCode1",
                OrderAmount = 99.01m,
            };

            // Act
            var response = controller.Add(binding);

            // Assert
            Assert.NotNull(response);
            var bll = new OrderBLL();
            var order = bll.GetWhere(new Order()
            {
                PurchaseOrderNumber = binding.PurchaseOrderNumber,
            });
            Assert.NotNull(order);
        }

        [Test]
        public void AddNewOrder_ErrorWithDuplicating()
        {
            // Arrange
            var controller = new OrderController();
            var duplicateNumber = "ExternalOrder" + Guid.NewGuid();
            var binding = new OrderBindingModel()
            {
                BuyerName = "BuyerName1",
                PurchaseOrderNumber = duplicateNumber,
                BillingZipCode = "BillingZipCode1",
                OrderAmount = 99.01m,
            };

            // Act
            controller.Add(binding);

            // Assert
            Assert.Throws<HttpResponseException>(() => { controller.Add(binding); });
        }

        [Test]
        public void AddNewOrder_WithoutBillingZipCode()
        {
            // Arrange
            var controller = new OrderController();
            var duplicateNumber = "ExternalOrder" + Guid.NewGuid();
            var binding = new OrderBindingModel()
            {
                BuyerName = "BuyerName1",
                PurchaseOrderNumber = duplicateNumber,
                OrderAmount = 99.01m,
            };

            // Act
            // Assert
            Assert.Throws<HttpResponseException>(() => { controller.Add(binding); });
        }

        [Test]
        public void GetARecord_success()
        {
            // Arrange
            var controller = new OrderController();
            var duplicateNumber = "ExternalOrder" + Guid.NewGuid();
            var binding = new OrderBindingModel()
            {
                BuyerName = "BuyerName1",
                PurchaseOrderNumber = duplicateNumber,
                BillingZipCode = "BillingZipCode1",
                OrderAmount = 99.01m,
            };

            // Act
            controller.Add(binding);

            // Assert
            var bll = new OrderBLL();
            var order = bll.GetWhere(new Order()
            {
                PurchaseOrderNumber = binding.PurchaseOrderNumber,
            });
            Assert.NotNull(order);

        }
    }
}
