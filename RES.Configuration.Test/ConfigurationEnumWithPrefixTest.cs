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
    public class ConfigurationEnumWithPrefixTest : ConfigurationTestBase
    {
        const string UK_PREFIX = "Uk-";
        enum Test { A, B }

        class TestEnumConfiguration
        {
            public Test EnumProperty => configuration.GetEnum<Test>(UK_PREFIX, MethodBase.GetCurrentMethod());

            readonly Configuration configuration;

            public TestEnumConfiguration(Configuration configuration)
            {
                this.configuration = configuration;
            }
        }

        [Test]
        public void Get()
        {
            var configuration = new TestEnumConfiguration(ConfigurationWithSetting($"{UK_PREFIX}EnumProperty", "B"));

            Assert.AreEqual(Test.B, configuration.EnumProperty);
        }

        [Test]
        public void GetMissing()
        {
            var configuration = new TestEnumConfiguration(ConfigurationWithNoSettings);

            Assert.Throws<ConfigurationException>(
                () =>
                { var v = configuration.EnumProperty; }
            );
        }

        [Test]
        public void GetUnParseable()
        {
            var configuration = new TestEnumConfiguration(ConfigurationWithSetting($"{UK_PREFIX}EnumProperty", "not parseable"));

            Assert.Throws<ArgumentException>(
                () =>
                { var v = configuration.EnumProperty; }
            );
        }
    }
}
