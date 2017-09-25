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
    public class ConfigurationBooleanWithPrefixTest : ConfigurationTestBase
    {
        const string UK_PREFIX = "Uk-";

        private class TestBooleanConfiguration
        {
            public bool BooleanProperty => configuration.GetBool(UK_PREFIX, MethodBase.GetCurrentMethod());

            readonly Configuration configuration;

            public TestBooleanConfiguration(Configuration configuration)
            {
                this.configuration = configuration;
            }
        }

        [Test]
        public void Get()
        {
            var configuration = new TestBooleanConfiguration(ConfigurationWithSetting($"{UK_PREFIX}BooleanProperty", "false"));

            Assert.AreEqual(false, configuration.BooleanProperty);
        }

        [Test]
        public void GetMissing()
        {
            var configuration = new TestBooleanConfiguration(ConfigurationWithNoSettings);

            Assert.Throws<ConfigurationException>(
                () =>
                { var v = configuration.BooleanProperty; }
            );
        }

        [Test]
        public void GetUnParseable()
        {
            var configuration = new TestBooleanConfiguration(ConfigurationWithSetting($"{UK_PREFIX}BooleanProperty", "not parseable"));

            Assert.Throws<FormatException>(
                () =>
                { var v = configuration.BooleanProperty; }
            );
        }
    }
}
