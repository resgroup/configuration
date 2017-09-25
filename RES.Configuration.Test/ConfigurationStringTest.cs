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
    public class ConfigurationStringTest : ConfigurationTestBase
    {
        class TestStringConfiguration
        {
            public string StringProperty => configuration.GetString(MethodBase.GetCurrentMethod());

            readonly Configuration configuration;

            public TestStringConfiguration(Configuration configuration)
            {
                this.configuration = configuration;
            }
        }

        [Test]
        public void Get()
        {
            const string CONFIG = "cedd";

            var configuration = new TestStringConfiguration(ConfigurationWithSetting("StringProperty", CONFIG));

            Assert.AreEqual(CONFIG, configuration.StringProperty);
        }

        [Test]
        public void GetMissing()
        {
            var configuration = new TestStringConfiguration(ConfigurationWithNoSettings);

            Assert.Throws<ConfigurationException>(
                () =>
                { var v = configuration.StringProperty; }
            );
        }

        // Can always parse a string, so no need for the GetUnParseable test
    }
}
