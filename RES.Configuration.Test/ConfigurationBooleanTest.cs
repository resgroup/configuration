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
    public class ConfigurationBooleanTest : ConfigurationTestBase
    {
        class TestBooleanConfiguration
        {
            public bool BooleanProperty => configuration.GetBool(MethodBase.GetCurrentMethod());

            readonly Configuration configuration;

            public TestBooleanConfiguration(Configuration configuration)
            {
                this.configuration = configuration;
            }
        }

        [Test]
        public void Get()
        {
            var configuration = new TestBooleanConfiguration(ConfigurationWithSetting("BooleanProperty", "true"));

            Assert.AreEqual(true, configuration.BooleanProperty);
        }

        [Test]
        public void GetMissing()
        {
            var configuration = new TestBooleanConfiguration(ConfigurationWithNoSettings);

            Assert.Throws<ConfigurationException>(
                () => 
                { var v = configuration.BooleanProperty; }
            );
        }

        [Test]
        public void GetUnParseable()
        {
            var configuration = new TestBooleanConfiguration(ConfigurationWithSetting("BooleanProperty", "not parseable"));

            Assert.Throws<FormatException>(
                () =>
                { var v = configuration.BooleanProperty; }
            );
        }
    }
}
