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
        const string UK_PREFIX = "Uk-";
        const int DEFAULT = 4;

        private class TestIntegerConfiguration : Configuration
        {
            public int IntegerProperty => GetIntWithDefault(MethodBase.GetCurrentMethod(), DEFAULT);

            public TestIntegerConfiguration(IConfigurationGetter configurationGetter)
                : base(configurationGetter) { }
        }

        [Test]
        public void Get()
        {
            // This test makes sure that if there is a config setting then it is used in place of the default. Make sure that the default value and the config value are different.
            const int CONFIG = DEFAULT + 1;
            var configuration = new TestIntegerConfiguration(Setting("IntegerProperty", $"{CONFIG}"));

            Assert.AreEqual(CONFIG, configuration.IntegerProperty);
        }

        [Test]
        public void GetMissing()
        {
            var configuration = new TestIntegerConfiguration(NoSettings);

            Assert.AreEqual(DEFAULT, configuration.IntegerProperty);
        }

        [Test]
        public void GetUnParseable()
        {
            var configuration = new TestIntegerConfiguration(Setting("IntegerProperty", "not parseable"));

            Assert.Throws<FormatException>(
                () =>
                { var v = configuration.IntegerProperty; }
            );
        }
    }
}
