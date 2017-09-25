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
    public class ConfigurationIntegerWithDefaultTest : ConfigurationTestBase
    {
        const int DEFAULT = 4;

        class TestIntegerConfiguration
        {
            public int IntegerProperty => configuration.GetIntWithDefault(MethodBase.GetCurrentMethod(), DEFAULT);

            readonly Configuration configuration;

            public TestIntegerConfiguration(Configuration configuration)
            {
                this.configuration = configuration;
            }
        }

        [Test]
        public void Get()
        {
            // This test makes sure that if there is a config setting then it is used in place of the default. Make sure that the default value and the config value are different.
            const int CONFIG = DEFAULT + 1;
            var configuration = new TestIntegerConfiguration(ConfigurationWithSetting("IntegerProperty", $"{CONFIG}"));

            Assert.AreEqual(CONFIG, configuration.IntegerProperty);
        }

        [Test]
        public void GetMissing()
        {
            var configuration = new TestIntegerConfiguration(ConfigurationWithNoSettings);

            Assert.AreEqual(DEFAULT, configuration.IntegerProperty);
        }

        [Test]
        public void GetUnParseable()
        {
            var configuration = new TestIntegerConfiguration(ConfigurationWithSetting("IntegerProperty", "not parseable"));

            Assert.Throws<FormatException>(
                () =>
                { var v = configuration.IntegerProperty; }
            );
        }
    }
}
