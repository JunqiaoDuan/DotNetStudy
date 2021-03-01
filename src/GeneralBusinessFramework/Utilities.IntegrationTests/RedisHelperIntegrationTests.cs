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
        public void SetRedisString_success()
        {
            var stores = RedisHelperBase.Get("test1");
            
            
        }

    }
}
