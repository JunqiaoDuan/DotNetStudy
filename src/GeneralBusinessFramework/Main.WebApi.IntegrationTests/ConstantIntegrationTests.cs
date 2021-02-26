using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Main.WebApi.IntegrationTests
{
    [TestFixture]
    public class ConstantIntegrationTests
    {
        [Test]
        public void ConstResult_Return_True()
        {
            Assert.IsTrue(true);
        }
    }
}
