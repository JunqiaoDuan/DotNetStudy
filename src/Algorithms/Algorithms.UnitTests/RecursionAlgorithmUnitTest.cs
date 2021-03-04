using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Algorithms.UnitTests
{
    [TestFixture]
    public class RecursionAlgorithmUnitTest
    {
        [Test]
        public void SumRecursion_NormalList_Success()
        {
            // Arrange
            var datum = new List<int>()
            {
                1, 2, 3, 4, 10
            };
            var excepted = datum.Sum();

            // Act
            var actual = RecursionAlgorithm.SumByRecursion(datum);

            // Assert
            Assert.AreEqual(excepted, actual);
        }

        [Test]
        public void SumRecursion_EmptyList_Success()
        {
            // Arrange
            var datum = new List<int>();
            var excepted = 0;

            // Act
            var actual = RecursionAlgorithm.SumByRecursion(datum);

            // Assert
            Assert.AreEqual(excepted, actual);
        }

        [Test]
        public void SumRecursion_NullList_Throw()
        {
            // Arrange
            List<int> datum = null;

            // Act. Assert
            Assert.Throws<NullReferenceException>(
                () => { RecursionAlgorithm.SumByRecursion(datum); });

        }

    }
}
