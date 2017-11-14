using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RES.Configuration.Test
{
    [TestFixture]
    public class ConfigurationValidatorIntegerListTest : ConfigurationTestBase
    {
        public IEnumerable<int> IntegerListProperty { get; set; }

        [Test]
        public void CheckDoesNothingWhenSettingAvailable()
        {
            var settings = Setting("IntegerListProperty", "1,2");

            using (var validator = new ConfigurationValidator(settings))
                validator.Check(() => IntegerListProperty);
        }

        [Test]
        public void CheckWithDefaultDoesNothingWhenSettingMissing()
        {
            using (var validator = new ConfigurationValidator(NoSettings))
                validator.CheckWithDefault(() => IntegerListProperty);
        }

        [Test]
        public void CheckWithDefaultThrowsExceptionWhenSettingUnParseable()
        {
            const string UN_PARSEABLE = "this is not parseable to an integer list";
            const string PROPERTY_NAME = "IntegerListProperty";

            var setting = Setting(PROPERTY_NAME, UN_PARSEABLE);

            var exception = Assert.Throws<ConfigurationException>(() =>
            {
                using (var validator = new ConfigurationValidator(setting))
                    validator.CheckWithDefault(() => IntegerListProperty);
            });

            Assert.AreEqual($"The {PROPERTY_NAME} setting ('{UN_PARSEABLE}') can not be converted to an IntegerList", exception.Message);
        }

        [Test]
        public void CheckDoesNothingWhenSettingWithPrefixAvailable()
        {
            var settings = Setting("UK-IntegerListProperty", "3,4,5");

            using (var validator = new ConfigurationValidator(settings))
                validator.Check("UK-", () => IntegerListProperty);
        }

        [Test]
        public void CheckThrowsExceptionWhenSettingUnParseable()
        {
            const string UN_PARSEABLE = "this is not parseable to a IntegerList";
            const string PROPERTY_NAME = "IntegerListProperty";

            var settings = Setting(PROPERTY_NAME, UN_PARSEABLE);

            var exception = Assert.Throws<ConfigurationException>(()  =>
                {
                    using (var validator = new ConfigurationValidator(settings))
                        validator.Check(() => IntegerListProperty);
                });

            Assert.AreEqual($"The {PROPERTY_NAME} setting ('{UN_PARSEABLE}') can not be converted to an IntegerList", exception.Message);
        }

        [Test]
        public void CheckThrowsExceptionWhenSettingWithPrefixUnParseable()
        {
            const string UN_PARSEABLE = "this is not parseable to an IntegerList";
            const string PROPERTY_NAME = "IntegerListProperty";
            const string PREFIX = "UK-";

            var settings = Setting($"{PREFIX}{PROPERTY_NAME}", UN_PARSEABLE);

            var exception = Assert.Throws<ConfigurationException>(() =>
            {
                using (var validator = new ConfigurationValidator(settings))
                    validator.Check(PREFIX, () => IntegerListProperty);
            });

            Assert.AreEqual($"The {PREFIX}{PROPERTY_NAME} setting ('{UN_PARSEABLE}') can not be converted to an IntegerList", exception.Message);
        }

        [Test]
        public void CheckThrowsExceptionWhenSettingMissing()
        {
            var exception = Assert.Throws<ConfigurationException>(() =>
            {
                using (var validator = new ConfigurationValidator(NoSettings))
                    validator.Check(() => IntegerListProperty);
            });

            Assert.AreEqual("The IntegerListProperty setting is missing", exception.Message);
        }

        [Test]
        public void CheckThrowsExceptionWhenSettingWithPrefixMissing()
        {
            var exception = Assert.Throws<ConfigurationException>(() =>
            {
                using (var validator = new ConfigurationValidator(NoSettings))
                    validator.Check("UK-", () => IntegerListProperty);
            });

            Assert.AreEqual("The UK-IntegerListProperty setting is missing", exception.Message);
        }
    }
}
