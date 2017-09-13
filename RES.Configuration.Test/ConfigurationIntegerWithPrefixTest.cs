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
    public class ConfigurationIntegerWithPrefixTest : ConfigurationTestBase
    {
        const string UK_PREFIX = "Uk-";

        private class TestIntegerConfiguration : Configuration
        {
            public int IntegerProperty => GetInt(UK_PREFIX, MethodBase.GetCurrentMethod());

            public TestIntegerConfiguration(IConfigurationGetter configurationGetter)
                : base(configurationGetter) { }
        }

        [Test]
        public void Get()
        {
            var configuration = new TestIntegerConfiguration(Setting($"{UK_PREFIX}IntegerProperty", "234"));

            Assert.AreEqual(234, configuration.IntegerProperty);
        }

        [Test]
        public void GetMissing()
        {
            var configuration = new TestIntegerConfiguration(NoSettings);

            Assert.Throws<ConfigurationException>(
                () =>
                { var v = configuration.IntegerProperty; }
            );
        }

        [Test]
        public void GetUnParseable()
        {
            var configuration = new TestIntegerConfiguration(Setting($"{UK_PREFIX}IntegerProperty", "not parseable"));

            Assert.Throws<FormatException>(
                () =>
                { var v = configuration.IntegerProperty; }
            );
        }
    }
}
