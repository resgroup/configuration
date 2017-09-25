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
    public class ConfigurationStringWithDefaultTest : ConfigurationTestBase
    {
        const string DEFAULT = "default";

        class TestStringConfiguration
        {
            public string StringProperty => configuration.GetStringWithDefault(MethodBase.GetCurrentMethod(), DEFAULT);

            readonly Configuration configuration;

            public TestStringConfiguration(Configuration configuration)
            {
                this.configuration = configuration;
            }
        }

        [Test]
        public void Get()
        {
            // This test makes sure that if there is a config setting then it is used in place of the default. Make sure that the default value and the config value are different.
            const string CONFIG = "not " + DEFAULT;
            var configuration = new TestStringConfiguration(ConfigurationWithSetting("StringProperty", $"{CONFIG}"));

            Assert.AreEqual(CONFIG, configuration.StringProperty);
        }

        [Test]
        public void GetMissing()
        {
            var configuration = new TestStringConfiguration(ConfigurationWithNoSettings);

            Assert.AreEqual(DEFAULT, configuration.StringProperty);
        }

        // Can always parse a string, so no need for the GetUnParseable test
    }
}
