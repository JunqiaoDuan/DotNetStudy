using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AzureProgrammerPractice.WebApi.Models;
using NUnit.Framework;

namespace Utilities.IntegrationTests
{
    [TestFixture]
    public class RedisHelperIntegrationTests
    {
        [Test]
        public void SetRedisString_CRUD_Success()
        {
            // Arrange
            var key = "test_key1";
            var value1 = "test_value_1";
            var value2 = "test_value_2";

            // Act. Init
            var isExit = RedisHelper.IsKeyExist(key);
            if (isExit)
            {
                RedisHelper.KeyDelete(key);
            }

            // Act. Add
            RedisHelper.StringSet(key, value1);

            // Assert
            var actualValue1 = RedisHelper.StringGet(key);
            Assert.AreEqual(value1, actualValue1);

            // Act. Modify
            RedisHelper.StringSet(key, value2);

            // Assert
            var actualValue2 = RedisHelper.StringGet(key);
            Assert.AreEqual(value2, actualValue2);

            // Act. Delete
            RedisHelper.KeyDelete(key);

            // Assert
            Assert.IsFalse(RedisHelper.IsKeyExist(key));
            Assert.IsNull(RedisHelper.StringGet(key));

        }

        [Test]
        public void SetRedisHash_CRUD_Success()
        {
            // Arrange
            var key = "test_key2";
            var object1 = new TestOrderDTO()
            {
                ID = 1,
                OrderCode = "test_order_code1",
            };
            var object2 = new TestOrderDTO()
            {
                ID = 2,
                OrderCode = "test_order_code2",
            };

            // Act. Init
            var isExit = RedisHelper.IsKeyExist(key);
            if (isExit)
            {
                RedisHelper.KeyDelete(key);
            }

            // Act. Add
            RedisHelper.HashSet(key, "ID", object1.ID.ToString());
            RedisHelper.HashSet(key, "OrderCode", object1.OrderCode);

            // Assert
            var actualValue1 = RedisHelper.HashGet(key, "ID");
            var actualValue2 = RedisHelper.HashGet(key, "OrderCode");
            Assert.AreEqual(object1.ID.ToString(), actualValue1);
            Assert.AreEqual(object1.OrderCode, actualValue2);

            // Act. Modify
            RedisHelper.HashSet(key, "ID", object2.ID.ToString());
            RedisHelper.HashSet(key, "OrderCode", object2.OrderCode);

            // Assert
            var actualValue3 = RedisHelper.HashGet(key, "ID");
            var actualValue4 = RedisHelper.HashGet(key, "OrderCode");
            Assert.AreEqual(object2.ID.ToString(), actualValue3);
            Assert.AreEqual(object2.OrderCode, actualValue4);

            // Act. Delete
            RedisHelper.KeyDelete(key);

            // Assert
            Assert.IsFalse(RedisHelper.IsKeyExist(key));
            Assert.IsNull(RedisHelper.StringGet(key));
        }

    }

    public class TestOrderDTO
    {
        public int ID { get; set; }
        public string OrderCode { get; set; }
    }
}
