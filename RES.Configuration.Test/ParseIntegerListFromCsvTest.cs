using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RES.Configuration;

namespace RES.Configuration.Test
{
    [TestFixture]
    public class ParseIntegerListFromCsvTest
    {
        [Test]
        public void EmptyStringReturnsEmptyList()
        {
            string emptyList = "";

            Assert.True(ParseIntegerListFromCsv.CanParse(emptyList));
            Assert.False(ParseIntegerListFromCsv.Parse(emptyList).Any());
        }

        [Test]
        public void InvalidItemInvalidatesEntireList()
        {
            string oneIntegerUnparseable = "1,2,4,f,2,4";

            Assert.False(ParseIntegerListFromCsv.CanParse(oneIntegerUnparseable));
        }

        [Test]
        public void DecimalPointsAreInvalid()
        {
            string decimalPoints = "2.3";

            Assert.False(ParseIntegerListFromCsv.CanParse(decimalPoints));
        }

        [Test]
        public void CsvList()
        {
            string csvList = "1,2,8,6";

            var intList = new List<int> { 1, 2, 8, 6 };

            CollectionAssert.AreEquivalent(intList, ParseIntegerListFromCsv.Parse(csvList));
        }
    }
}
