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
    public class ConfigurationIntegerTest : ConfigurationTestBase
    {
        class TestIntegerConfiguration
        {
            public int IntegerProperty => configuration.GetInt(MethodBase.GetCurrentMethod());

            readonly Configuration configuration;

            public TestIntegerConfiguration(Configuration configuration)
            {
                this.configuration = configuration;
            }
        }

        [Test]
        public void Get()
        {
            var configuration = new TestIntegerConfiguration(ConfigurationWithSetting("IntegerProperty", "2"));

            Assert.AreEqual(2, configuration.IntegerProperty);
        }

        [Test]
        public void GetMissing()
        {
            var configuration = new TestIntegerConfiguration(ConfigurationWithNoSettings);

            Assert.Throws<ConfigurationException>(
                () => 
                { var v = configuration.IntegerProperty; }
            );
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
