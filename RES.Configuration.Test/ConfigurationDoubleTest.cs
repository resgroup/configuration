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
    public class ConfigurationDoubleTest : ConfigurationTestBase
    {
        class TestDoubleConfiguration
        {
            public double DoubleProperty => configuration.GetDouble(MethodBase.GetCurrentMethod());

            readonly Configuration configuration;

            public TestDoubleConfiguration(Configuration configuration)
            {
                this.configuration = configuration;
            }
        }

        [Test]
        public void Get()
        {
            var configuration = new TestDoubleConfiguration(ConfigurationWithSetting("DoubleProperty", "2"));

            Assert.AreEqual(2, configuration.DoubleProperty);
        }

        [Test]
        public void GetMissing()
        {
            var configuration = new TestDoubleConfiguration(ConfigurationWithNoSettings);

            Assert.Throws<ConfigurationException>(
                () => 
                { var v = configuration.DoubleProperty; }
            );
        }

        [Test]
        public void GetUnParseable()
        {
            var configuration = new TestDoubleConfiguration(ConfigurationWithSetting("DoubleProperty", "not parseable"));

            Assert.Throws<FormatException>(
                () =>
                { var v = configuration.DoubleProperty; }
            );
        }
    }
}
