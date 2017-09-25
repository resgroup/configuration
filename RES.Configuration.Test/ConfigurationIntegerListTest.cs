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
        class TestIntegerListConfiguration
        {
            public IEnumerable<int> IntegerListProperty => configuration.GetIntegerList(MethodBase.GetCurrentMethod());

            readonly Configuration configuration;

            public TestIntegerListConfiguration(Configuration configuration)
            {
                this.configuration = configuration;
            }
        }

        [Test]
        public void Get()
        {
            var configuration = new TestIntegerListConfiguration(ConfigurationWithSetting("IntegerListProperty", "1,2"));

            CollectionAssert.AreEqual(new List<int> { 1,2 }, configuration.IntegerListProperty);
        }

        [Test]
        public void GetMissing()
        {
            var configuration = new TestIntegerListConfiguration(ConfigurationWithNoSettings);

            Assert.Throws<ConfigurationException>(
                () => 
                { var v = configuration.IntegerListProperty; }
            );
        }

        [Test]
        public void GetUnParseable()
        {
            var configuration = new TestIntegerListConfiguration(ConfigurationWithSetting("IntegerListProperty", "not parseable"));

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
