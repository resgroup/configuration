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
    public class ConfigurationDoubleWithPrefixTest : ConfigurationTestBase
    {
        const string UK_PREFIX = "Uk-";

        private class TestDoubleConfiguration : Configuration
        {
            public double DoubleProperty => GetDouble(UK_PREFIX, MethodBase.GetCurrentMethod());

            public TestDoubleConfiguration(IConfigurationGetter configurationGetter)
                : base(configurationGetter) { }
        }

        [Test]
        public void Get()
        {
            var configuration = new TestDoubleConfiguration(Setting($"{UK_PREFIX}DoubleProperty", "234"));

            Assert.AreEqual(234, configuration.DoubleProperty);
        }

        [Test]
        public void GetMissing()
        {
            var configuration = new TestDoubleConfiguration(NoSettings);

            Assert.Throws<ConfigurationException>(
                () =>
                { var v = configuration.DoubleProperty; }
            );
        }

        [Test]
        public void GetUnParseable()
        {
            var configuration = new TestDoubleConfiguration(Setting($"{UK_PREFIX}DoubleProperty", "not parseable"));

            Assert.Throws<FormatException>(
                () =>
                { var v = configuration.DoubleProperty; }
            );
        }
    }
}
