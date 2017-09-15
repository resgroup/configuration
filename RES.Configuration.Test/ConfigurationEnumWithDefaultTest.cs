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
    public class ConfigurationEnumWithDefaultTest : ConfigurationTestBase
    {
        const Test DEFAULT = Test.A;
        enum Test { A, B }

        class TestEnumConfiguration
        {
            public Test EnumProperty => configuration.GetEnumWithDefault<Test>(MethodBase.GetCurrentMethod(), DEFAULT);

            readonly Configuration configuration;

            public TestEnumConfiguration(Configuration configuration)
            {
                this.configuration = configuration;
            }
        }
        [Test]
        public void Get()
        {
            // This test makes sure that if there is a config setting then it is used in place of the default. Make sure that the default value and the config value are different.
            const Test CONFIG = Test.B;
            var configuration = new TestEnumConfiguration(ConfigurationWithSetting("EnumProperty", $"B"));

            Assert.AreEqual(CONFIG, configuration.EnumProperty);
        }

        [Test]
        public void GetMissing()
        {
            var configuration = new TestEnumConfiguration(ConfigurationWithNoSettings);

            Assert.AreEqual(DEFAULT, configuration.EnumProperty);
        }

        [Test]
        public void GetUnParseable()
        {
            var configuration = new TestEnumConfiguration(ConfigurationWithSetting("EnumProperty", "not parseable"));

            Assert.Throws<ArgumentException>(
                () =>
                { var v = configuration.EnumProperty; }
            );
        }
    }
}
