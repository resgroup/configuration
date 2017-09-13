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
        private class TestDoubleConfiguration : Configuration
        {
            public double DoubleProperty => GetDouble(MethodBase.GetCurrentMethod());

            public TestDoubleConfiguration(IConfigurationGetter configurationGetter)
                : base(configurationGetter) { }
        }

        [Test]
        public void Get()
        {
            var configuration = new TestDoubleConfiguration(Setting("DoubleProperty", "2"));

            Assert.AreEqual(2, configuration.DoubleProperty);
        }

        [Test]
        public void GetMissing()
        {
            var configuration = new TestDoubleConfiguration(NoSettings);

            Assert.Throws<ConfigurationException>(
                () => 
                { var v = configuration.DoubleProperty; }
            );
        }

        [Test]
        public void GetUnParseable()
        {
            var configuration = new TestDoubleConfiguration(Setting("DoubleProperty", "not parseable"));

            Assert.Throws<FormatException>(
                () =>
                { var v = configuration.DoubleProperty; }
            );
        }
    }
}
