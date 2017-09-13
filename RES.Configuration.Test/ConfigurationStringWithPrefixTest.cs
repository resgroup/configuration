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
    public class ConfigurationStringWithPrefixTest : ConfigurationTestBase
    {
        const string UK_PREFIX = "Uk-";

        private class TestStringConfiguration : Configuration
        {
            public string StringProperty => GetString(UK_PREFIX, MethodBase.GetCurrentMethod());

            public TestStringConfiguration(IConfigurationGetter configurationGetter)
                : base(configurationGetter) { }
        }

        [Test]
        public void Get()
        {
            const string CONFIG = "config";

            var configuration = new TestStringConfiguration(Setting($"{UK_PREFIX}StringProperty", CONFIG));

            Assert.AreEqual(CONFIG, configuration.StringProperty);
        }

        [Test]
        public void GetMissing()
        {
            var configuration = new TestStringConfiguration(NoSettings);

            Assert.Throws<ConfigurationException>(
                () =>
                { var v = configuration.StringProperty; }
            );
        }

        // Can always parse a string, so no need for the GetUnParseable test
    }
}
