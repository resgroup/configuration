using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using System.Text;

namespace RES.Configuration.Test
{
    [TestFixture]
    public class ConfigurationEnumTest : ConfigurationTestBase
    {
        enum Test { A, B }

        class TestEnumConfiguration
        {
            public Test EnumProperty => configuration.GetEnum<Test>(MethodBase.GetCurrentMethod());

            readonly Configuration configuration;

            public TestEnumConfiguration(Configuration configuration)
            {
                this.configuration = configuration;
            }
        }

        [Test]
        public void Get()
        {
            var configuration = new TestEnumConfiguration(ConfigurationWithSetting("EnumProperty", "A"));

            Assert.AreEqual(Test.A, configuration.EnumProperty);
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
            var configuration = new TestEnumConfiguration(ConfigurationWithSetting("EnumProperty", "not parseable"));

            Assert.Throws<ArgumentException>(
                () =>
                { var v = configuration.EnumProperty; }
            );
        }
    }
}
