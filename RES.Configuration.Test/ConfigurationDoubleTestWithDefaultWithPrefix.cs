﻿using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace RES.Configuration.Test
{
    [TestFixture]
    public class ConfigurationDoubleTestWithDefaultWithPrefix : ConfigurationTestBase
    {
        const string UK_PREFIX = "Uk-";
        const double DEFAULT = -1234098.2435;

        class TestDoubleConfiguration
        {
            public double DoubleProperty => configuration.GetDoubleWithDefault(UK_PREFIX, MethodBase.GetCurrentMethod(), DEFAULT);

            readonly Configuration configuration;

            public TestDoubleConfiguration(Configuration configuration)
            {
                this.configuration = configuration;
            }
        }

        [Test]
        public void GetBool()
        {
            // This test makes sure that if there is a config setting then it is used in place of the default. Make sure that the default value and the config value are different.
            const double CONFIG = DEFAULT + 1;
            var configuration = new TestDoubleConfiguration(ConfigurationWithSetting($"{UK_PREFIX}DoubleProperty", $"{CONFIG}"));

            Assert.AreEqual(CONFIG, configuration.DoubleProperty);
        }

        [Test]
        public void GetMissing()
        {
            var configuration = new TestDoubleConfiguration(ConfigurationWithNoSettings);

            Assert.AreEqual(DEFAULT, configuration.DoubleProperty);
        }

        [Test]
        public void GetUnParseable()
        {
            var configuration = new TestDoubleConfiguration(ConfigurationWithSetting($"{UK_PREFIX}DoubleProperty", "not parseable"));

            Assert.Throws<FormatException>(
                () =>
                { var v = configuration.DoubleProperty; }
            );
        }
    }
}
