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
    public class ConfigurationIntegerListWithDefaultTest : ConfigurationTestBase
    {
        const string UK_PREFIX = "Uk-";
        static IEnumerable<int> DEFAULT = new List<int> { 1, 2 };

        private class TestIntegerListConfiguration : Configuration
        {
            public IEnumerable<int> IntegerListProperty => GetIntegerListWithDefault(MethodBase.GetCurrentMethod(), DEFAULT);

            public TestIntegerListConfiguration(IConfigurationGetter configurationGetter)
                : base(configurationGetter) { }
        }

        [Test]
        public void Get()
        {
            // This test makes sure that if there is a config setting then it is used in place of the default. Make sure that the default value and the config value are different.
            const string CONFIG = "3,4";

            var configuration = new TestIntegerListConfiguration(Setting("IntegerListProperty", CONFIG));

            var expected = new List<int> { 3, 4 };

            CollectionAssert.AreEqual(expected, configuration.IntegerListProperty);
        }

        [Test]
        public void GetMissing()
        {
            var configuration = new TestIntegerListConfiguration(NoSettings);

            Assert.AreEqual(DEFAULT, configuration.IntegerListProperty);
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
