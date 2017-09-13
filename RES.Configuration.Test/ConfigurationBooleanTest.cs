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
        private class TestBooleanConfiguration : Configuration
        {
            public bool BooleanProperty => GetBool(MethodBase.GetCurrentMethod());

            public TestBooleanConfiguration(IConfigurationGetter configurationGetter)
                : base(configurationGetter) { }
        }

        [Test]
        public void Get()
        {
            var configuration = new TestBooleanConfiguration(Setting("BooleanProperty", "true"));

            Assert.AreEqual(true, configuration.BooleanProperty);
        }

        [Test]
        public void GetMissing()
        {
            var configuration = new TestBooleanConfiguration(NoSettings);

            Assert.Throws<ConfigurationException>(
                () => 
                { var v = configuration.BooleanProperty; }
            );
        }

        [Test]
        public void GetUnParseable()
        {
            var configuration = new TestBooleanConfiguration(Setting("BooleanProperty", "not parseable"));

            Assert.Throws<FormatException>(
                () =>
                { var v = configuration.BooleanProperty; }
            );
        }
    }
}
