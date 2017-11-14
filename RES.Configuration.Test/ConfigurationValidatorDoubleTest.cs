using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RES.Configuration.Test
{
    [TestFixture]
    public class ConfigurationValidatorDoubleTest : ConfigurationTestBase
    {
        public double DoubleProperty { get; set; }

        [Test]
        public void CheckDoesNothingWhenSettingAvailable()
        {
            var settings = Setting("DoubleProperty", "14.45");

            using (var validator = new ConfigurationValidator(settings))
                validator.Check(() => DoubleProperty);
        }

        [Test]
        public void CheckWithDefaultDoesNothingWhenSettingMissing()
        {
            using (var validator = new ConfigurationValidator(NoSettings))
                validator.CheckWithDefault(() => DoubleProperty);
        }

        [Test]
        public void CheckWithDefaultThrowsExceptionWhenSettingUnParseable()
        {
            const string UN_PARSEABLE = "this is not parseable to a double";
            const string PROPERTY_NAME = "DoubleProperty";

            var settings = Setting(PROPERTY_NAME, UN_PARSEABLE);

            var exception = Assert.Throws<ConfigurationException>(() =>
            {
                using (var validator = new ConfigurationValidator(settings))
                    validator.CheckWithDefault(setting: () => DoubleProperty);
            });

            Assert.AreEqual($"The {PROPERTY_NAME} setting ('{UN_PARSEABLE}') can not be converted to a double", exception.Message);
        }

        [Test]
        public void CheckDoesNothingWhenSettingWithPrefixAvailable()
        {
            var settings = Setting("UK-DoubleProperty", "-0.123");

            using (var validator = new ConfigurationValidator(settings))
                validator.Check("UK-", () => DoubleProperty);
        }

        [Test]
        public void CheckThrowsExceptionWhenSettingUnParseable()
        {
            const string UN_PARSEABLE = "this is not parseable to a double";
            const string PROPERTY_NAME = "DoubleProperty";

            var settings = Setting(PROPERTY_NAME, UN_PARSEABLE);

            var exception = Assert.Throws<ConfigurationException>(()  =>
                {
                    using (var validator = new ConfigurationValidator(settings))
                        validator.Check(setting: () => DoubleProperty);
                });

            Assert.AreEqual($"The {PROPERTY_NAME} setting ('{UN_PARSEABLE}') can not be converted to a double", exception.Message);
        }

        [Test]
        public void CheckThrowsExceptionWhenSettingWithPrefixUnParseable()
        {
            const string UN_PARSEABLE = "this is not parseable to an double";
            const string PROPERTY_NAME = "DoubleProperty";
            const string PREFIX = "UK-";

            var settings = Setting($"{PREFIX}{PROPERTY_NAME}", UN_PARSEABLE);

            var exception = Assert.Throws<ConfigurationException>(() =>
            {
                using (var validator = new ConfigurationValidator(settings))
                    validator.Check(PREFIX, () => DoubleProperty);
            });

            Assert.AreEqual($"The {PREFIX}{PROPERTY_NAME} setting ('{UN_PARSEABLE}') can not be converted to a double", exception.Message);
        }

        [Test]
        public void CheckThrowsExceptionWhenSettingMissing()
        {
            var exception = Assert.Throws<ConfigurationException>(() =>
            {
                using (var validator = new ConfigurationValidator(NoSettings))
                    validator.Check(setting: () => DoubleProperty);
            });

            Assert.AreEqual("The DoubleProperty setting is missing", exception.Message);
        }

        [Test]
        public void CheckThrowsExceptionWhenSettingWithPrefixMissing()
        {
            var exception = Assert.Throws<ConfigurationException>(() =>
            {
                using (var validator = new ConfigurationValidator(NoSettings))
                    validator.Check("UK-", () => DoubleProperty);
            });

            Assert.AreEqual("The UK-DoubleProperty setting is missing", exception.Message);
        }
    }
}
