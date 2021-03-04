using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Algorithms.UnitTests
{
    [TestFixture]
    public class BinarySearchUnitTest
    {
        [Test]
        public void BinarySearch_OddCount_ContainedValue_Success()
        {
            // Arrange. Prepare datum
            var datum = new List<int>()
            {
                1, 2, 3, 4, 5, 6, 7, 8, 10
            };

            // Test 10 times
            for (int i = 0; i < 10; i++)
            {
                AssertBinarySearch_RandomContainedValue(datum);
            }
        }

        [Test]
        public void BinarySearch_EvenCount_ContainedValue_Success()
        {
            // Arrange. Prepare datum
            var datum = new List<int>()
            {
                1, 2, 3, 4, 5, 6, 7, 10
            };

            // Test 10 times
            for (int i = 0; i < 10; i++)
            {
                AssertBinarySearch_RandomContainedValue(datum);
            }
        }

        private void AssertBinarySearch_RandomContainedValue(List<int> datum)
        {
            // Arrange
            var index = new Random().Next(datum.Count);
            var data = datum[index];

            // Act
            var actualIndex = BinarySearch.GetSameValue(datum, data);

            // Assert
            Assert.AreEqual(index, actualIndex);
        }

        [Test]
        public void BinarySearch_OddCount_NotContainedValue_Success()
        {
            // Arrange. Prepare datum
            var datum = new List<int>()
            {
                1, 2, 3, 4, 5, 6, 7, 8, 10
            };

            // Test 10 times
            for (int i = 0; i < 10; i++)
            {
                AssertBinarySearch_RandomContainedValue(datum);
            }
        }

        [Test]
        public void BinarySearch_EvenCount_NotContainedValue_Success()
        {
            // Arrange. Prepare datum
            var datum = new List<int>()
            {
                1, 2, 3, 4, 5, 6, 7, 10
            };

            // Test 10 times
            for (int i = 0; i < 10; i++)
            {
                AssertBinarySearch_RandomContainedValue(datum);
            }
        }

        private void AssertBinarySearch_RandomNotContainedValue(List<int> datum)
        {
            // Arrange
            int data;
            while (true)
            {
                data = new Random().Next(10000);
                if (!datum.Contains(data))
                {
                    break;
                }
            }

            // Act
            var actualIndex = BinarySearch.GetSameValue(datum, data);

            // Assert
            Assert.AreEqual(-1, actualIndex);
        }
    }
}
