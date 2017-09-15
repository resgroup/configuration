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
    public class ConfigurationEnumTestWithDefaultWithPrefix : ConfigurationTestBase
    {
        const string UK_PREFIX = "Uk-";
        const Test DEFAULT = Test.A;
        enum Test { A, B }

        class TestEnumWithDefaultWithPrefixConfiguration
        {
            public Test EnumProperty => configuration.GetEnumWithDefault(UK_PREFIX, MethodBase.GetCurrentMethod(), DEFAULT);

            readonly Configuration configuration;

            public TestEnumWithDefaultWithPrefixConfiguration(Configuration configuration)
            {
                this.configuration = configuration;
            }
        }

        [Test]
        public void Get()
        {
            // This test makes sure that if there is a config setting then it is used in place of the default. Make sure that the default value and the config value are different.
            const Test CONFIG = Test.B;
            var configuration = new TestEnumWithDefaultWithPrefixConfiguration(ConfigurationWithSetting($"{UK_PREFIX}EnumProperty", $"B"));

            Assert.AreEqual(CONFIG, configuration.EnumProperty);
        }

        [Test]
        public void GetMissing()
        {
            var configuration = new TestEnumWithDefaultWithPrefixConfiguration(ConfigurationWithNoSettings);

            Assert.AreEqual(DEFAULT, configuration.EnumProperty);
        }

        [Test]
        public void GetUnParseable()
        {
            var configuration = new TestEnumWithDefaultWithPrefixConfiguration(ConfigurationWithSetting($"{UK_PREFIX}EnumProperty", "not parseable"));

            Assert.Throws<ArgumentException>(
                () =>
                { var v = configuration.EnumProperty; }
            );
        }
    }
}
