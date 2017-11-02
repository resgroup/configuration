using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RES.Configuration.Test
{
    [TestFixture]
    public class ConfigurationValidatorIntegerTest : ConfigurationTestBase
    {
        public int IntegerProperty { get; set; }

        [Test]
        public void CheckDoesNothingWhenSettingAvailable()
        {
            var settings = Setting("IntegerProperty", "14");

            using (var validator = new ConfigurationValidator(settings))
                validator.Check(() => IntegerProperty);
        }

        [Test]
        public void CheckWithDefaultDoesNothingWhenSettingMissing()
        {
            using (var validator = new ConfigurationValidator(NoSettings))
                validator.CheckWithDefault(() => IntegerProperty);
        }

        [Test]
        public void CheckDoesNothingWhenSettingWithPrefixAvailable()
        {
            var settings = Setting("UK-IntegerProperty", "983423");

            using (var validator = new ConfigurationValidator(settings))
                validator.Check("UK-", () => IntegerProperty);
        }

        [Test]
        public void CheckThrowsExceptionWhenSettingUnParseable()
        {
            const string UN_PARSEABLE = "this is not parseable to a bool";
            const string PROPERTY_NAME = "IntegerProperty";

            var settings = Setting(PROPERTY_NAME, UN_PARSEABLE);

            var exception = Assert.Throws<ConfigurationException>(()  =>
                {
                    using (var validator = new ConfigurationValidator(settings))
                        validator.Check(() => IntegerProperty);
                });

            Assert.AreEqual($"The {PROPERTY_NAME} setting ('{UN_PARSEABLE}') can not be converted to an int", exception.Message);
        }

        [Test]
        public void CheckThrowsExceptionWhenSettingWithPrefixUnParseable()
        {
            const string UN_PARSEABLE = "this is not parseable to an int";
            const string PROPERTY_NAME = "IntegerProperty";
            const string PREFIX = "UK-";

            var settings = Setting($"{PREFIX}{PROPERTY_NAME}", UN_PARSEABLE);

            var exception = Assert.Throws<ConfigurationException>(() =>
            {
                using (var validator = new ConfigurationValidator(settings))
                    validator.Check(PREFIX, () => IntegerProperty);
            });

            Assert.AreEqual($"The {PREFIX}{PROPERTY_NAME} setting ('{UN_PARSEABLE}') can not be converted to an int", exception.Message);
        }

        [Test]
        public void CheckThrowsExceptionWhenSettingMissing()
        {
            var exception = Assert.Throws<ConfigurationException>(() =>
            {
                using (var validator = new ConfigurationValidator(NoSettings))
                    validator.Check(() => IntegerProperty);
            });

            Assert.AreEqual("The IntegerProperty setting is missing", exception.Message);
        }

        [Test]
        public void CheckThrowsExceptionWhenSettingWithPrefixMissing()
        {
            var exception = Assert.Throws<ConfigurationException>(() =>
            {
                using (var validator = new ConfigurationValidator(NoSettings))
                    validator.Check("UK-", () => IntegerProperty);
            });

            Assert.AreEqual("The UK-IntegerProperty setting is missing", exception.Message);
        }
    }
}
