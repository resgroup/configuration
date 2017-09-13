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
        private class TestIntegerConfiguration : Configuration
        {
            public int IntegerProperty => GetInt(MethodBase.GetCurrentMethod());

            public TestIntegerConfiguration(IConfigurationGetter configurationGetter)
                : base(configurationGetter) { }
        }

        [Test]
        public void Get()
        {
            var configuration = new TestIntegerConfiguration(Setting("IntegerProperty", "2"));

            Assert.AreEqual(2, configuration.IntegerProperty);
        }

        [Test]
        public void GetMissing()
        {
            var configuration = new TestIntegerConfiguration(NoSettings);

            Assert.Throws<ConfigurationException>(
                () => 
                { var v = configuration.IntegerProperty; }
            );
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
