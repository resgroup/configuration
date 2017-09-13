using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace RES.Configuration.Test
{
    [TestFixture]
    public class ConfigurationIntegerListTest : ConfigurationTestBase
    {
        private class TestIntegerListConfiguration : Configuration
        {
            public IEnumerable<int> IntegerListProperty => GetIntegerList(MethodBase.GetCurrentMethod());

            public TestIntegerListConfiguration(IConfigurationGetter configurationGetter)
                : base(configurationGetter) { }
        }

        [Test]
        public void Get()
        {
            var configuration = new TestIntegerListConfiguration(Setting("IntegerListProperty", "1,2"));

            CollectionAssert.AreEqual(new List<int> { 1,2 }, configuration.IntegerListProperty);
        }

        [Test]
        public void GetMissing()
        {
            var configuration = new TestIntegerListConfiguration(NoSettings);

            Assert.Throws<ConfigurationException>(
                () => 
                { var v = configuration.IntegerListProperty; }
            );
        }

        [Test]
        public void GetUnParseable()
        {
            var configuration = new TestIntegerListConfiguration(Setting("IntegerListProperty", "not parseable"));

            Assert.Throws<FormatException>(
                () =>
                {
                    // FirstOrDefault is required to execute the list, otherwise lazy evaluation kicks in and nothing happens
                    var v = configuration.IntegerListProperty.FirstOrDefault();
                }
            );
        }
    }
}
