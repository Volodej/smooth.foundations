using System;
using NUnit.Framework;
using Smooth.Compare.Comparers;
using Smooth.Tests.Utils;

namespace Smooth.Tests
{
    [TestFixture]
    public class ComparerTests
    {
        [Test]
        public void TestEnumComparer_Byte()
        {
            var comparer = Enum32EqualityComparer.Create<ByteEnum>();

            Assert.IsFalse(comparer.Equals(ByteEnum.Value1, ByteEnum.Value2));
            Assert.IsTrue(comparer.Equals(ByteEnum.Value3, ByteEnum.Value3));
            Assert.AreEqual((int) ByteEnum.Value2, comparer.GetHashCode(ByteEnum.Value2));
        }

        [Test]
        public void TestEnumComparer_Short()
        {
            var comparer = Enum32EqualityComparer.Create<ShortEnum>();

            Assert.IsFalse(comparer.Equals(ShortEnum.Value1, ShortEnum.Value2));
            Assert.IsTrue(comparer.Equals(ShortEnum.Value3, ShortEnum.Value3));
            Assert.AreEqual((int) ShortEnum.Value2, comparer.GetHashCode(ShortEnum.Value2));
        }

        [Test]
        public void TestEnumComparer_Int()
        {
            var comparer = Enum32EqualityComparer.Create<IntEnum>();

            Assert.IsFalse(comparer.Equals(IntEnum.Value1, IntEnum.Value2));
            Assert.IsTrue(comparer.Equals(IntEnum.Value3, IntEnum.Value3));
            Assert.AreEqual((int) IntEnum.Value2, comparer.GetHashCode(IntEnum.Value2));
        }

        [Test]
        public void TestEnumComparer_Long()
        {
            var comparer = Enum64EqualityComparer.Create<LongEnum>();

            Assert.IsFalse(comparer.Equals(LongEnum.Value1, LongEnum.Value2));
            Assert.IsTrue(comparer.Equals(LongEnum.Value3, LongEnum.Value3));
            Assert.AreEqual((long) LongEnum.Value2.GetHashCode(), comparer.GetHashCode(LongEnum.Value2));
        }

        [Test]
        public void TestEnumComparer_WrongType()
        {
            Assert.Throws<InvalidOperationException>(() => Enum32EqualityComparer.Create<int>());
        }
    }
}