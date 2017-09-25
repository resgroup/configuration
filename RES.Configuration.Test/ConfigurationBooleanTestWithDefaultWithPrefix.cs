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
    public class ConfigurationBooleanTestWithDefaultWithPrefix : ConfigurationTestBase
    {
        const string UK_PREFIX = "Uk-";
        const bool DEFAULT = true;

        private class TestBooleanConfiguration
        {
            public bool BooleanProperty => configuration.GetBoolWithDefault(UK_PREFIX, MethodBase.GetCurrentMethod(), DEFAULT);

            readonly Configuration configuration;

            public TestBooleanConfiguration(Configuration configuration)
            {
                this.configuration = configuration;
            }
        }

        [Test]
        public void GetBool()
        {
            // This test makes sure that if there is a config setting then it is used in place of the default. Make sure that the default value and the config value are different.
            const bool CONFIG = !DEFAULT;
            var configuration = new TestBooleanConfiguration(ConfigurationWithSetting($"{UK_PREFIX}BooleanProperty", $"{CONFIG}"));

            Assert.AreEqual(CONFIG, configuration.BooleanProperty);
        }

        [Test]
        public void GetMissing()
        {
            var configuration = new TestBooleanConfiguration(ConfigurationWithNoSettings);

            Assert.AreEqual(DEFAULT, configuration.BooleanProperty);
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
