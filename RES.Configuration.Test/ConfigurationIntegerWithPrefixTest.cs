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

        class TestIntegerConfiguration
        {
            public int IntegerProperty => configuration.GetInt(UK_PREFIX, MethodBase.GetCurrentMethod());

            readonly Configuration configuration;

            public TestIntegerConfiguration(Configuration configuration)
            {
                this.configuration = configuration;
            }
        }

        [Test]
        public void Get()
        {
            var configuration = new TestIntegerConfiguration(ConfigurationWithSetting($"{UK_PREFIX}IntegerProperty", "234"));

            Assert.AreEqual(234, configuration.IntegerProperty);
        }

        [Test]
        public void GetMissing()
        {
            var configuration = new TestIntegerConfiguration(ConfigurationWithNoSettings);

            Assert.Throws<ConfigurationException>(
                () =>
                { var v = configuration.IntegerProperty; }
            );
        }

        [Test]
        public void GetUnParseable()
        {
            var configuration = new TestIntegerConfiguration(ConfigurationWithSetting($"{UK_PREFIX}IntegerProperty", "not parseable"));

            Assert.Throws<FormatException>(
                () =>
                { var v = configuration.IntegerProperty; }
            );
        }
    }
}
