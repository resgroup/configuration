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

        class TestDoubleConfiguration
        {
            public double DoubleProperty => configuration.GetDouble(UK_PREFIX, MethodBase.GetCurrentMethod());

            readonly Configuration configuration;

            public TestDoubleConfiguration(Configuration configuration)
            {
                this.configuration = configuration;
            }
        }

        [Test]
        public void Get()
        {
            var configuration = new TestDoubleConfiguration(ConfigurationWithSetting($"{UK_PREFIX}DoubleProperty", "234"));

            Assert.AreEqual(234, configuration.DoubleProperty);
        }

        [Test]
        public void GetMissing()
        {
            var configuration = new TestDoubleConfiguration(ConfigurationWithNoSettings);

            Assert.Throws<ConfigurationException>(
                () =>
                { var v = configuration.DoubleProperty; }
            );
        }

        [Test]
        public void GetUnParseable()
        {
            var configuration = new TestDoubleConfiguration(ConfigurationWithSetting($"{UK_PREFIX}DoubleProperty", "not parseable"));

            Assert.Throws<FormatException>(
                () =>
                { var v = configuration.DoubleProperty; }
            );
        }
    }
}
