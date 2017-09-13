using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RES.Configuration.Test
{
    [TestFixture]
    public class ConfigurationValidatorBooleanTest : ConfigurationTestBase
    {
        public bool BooleanProperty { get; set; }

        [Test]
        public void CheckDoesNothingWhenSettingAvailable()
        {
            var setting = Setting("BooleanProperty", "false");

            using (var validator = new ConfigurationValidator(setting))
                validator.Check(() => BooleanProperty);
        }

        [Test]
        public void CheckDoesNothingWhenSettingWithPrefixAvailable()
        {
            var setting = Setting("UK-BooleanProperty", "false");

            using (var validator = new ConfigurationValidator(setting))
                validator.Check("UK-", () => BooleanProperty);
        }

        [Test]
        public void CheckThrowsExceptionWhenSettingUnParseable()
        {
            const string UN_PARSEABLE = "this is not parseable to a bool";
            const string PROPERTY_NAME = "BooleanProperty";

            var setting = Setting(PROPERTY_NAME, UN_PARSEABLE);

            var exception = Assert.Throws<ConfigurationException>(()  =>
                {
                    using (var validator = new ConfigurationValidator(setting))
                        validator.Check(() => BooleanProperty);
                });

            Assert.AreEqual($"The {PROPERTY_NAME} setting ('{UN_PARSEABLE}') can not be converted to a boolean", exception.Message);
        }

        [Test]
        public void CheckThrowsExceptionWhenSettingWithPrefixUnParseable()
        {
            const string UN_PARSEABLE = "this is not parseable to a bool";
            const string PROPERTY_NAME = "BooleanProperty";
            const string PREFIX = "UK-";

            var setting = Setting($"{PREFIX}{PROPERTY_NAME}", UN_PARSEABLE);

            var exception = Assert.Throws<ConfigurationException>(() =>
            {
                using (var validator = new ConfigurationValidator(setting))
                    validator.Check(PREFIX, () => BooleanProperty);
            });

            Assert.AreEqual($"The {PREFIX}{PROPERTY_NAME} setting ('{UN_PARSEABLE}') can not be converted to a boolean", exception.Message);
        }

        [Test]
        public void CheckThrowsExceptionWhenSettingMissing()
        {
            var exception = Assert.Throws<ConfigurationException>(() =>
            {
                using (var validator = new ConfigurationValidator(NoSettings))
                    validator.Check(() => BooleanProperty);
            });

            Assert.AreEqual("The BooleanProperty setting is missing", exception.Message);
        }

        [Test]
        public void CheckThrowsExceptionWhenSettingWithPrefixMissing()
        {
            var exception = Assert.Throws<ConfigurationException>(() =>
            {
                using (var validator = new ConfigurationValidator(NoSettings))
                    validator.Check("UK-", () => BooleanProperty);
            });

            Assert.AreEqual("The UK-BooleanProperty setting is missing", exception.Message);
        }
    }
}
