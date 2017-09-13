using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RES.Configuration.Test
{
    [TestFixture]
    public class ConfigurationValidatorEnumTest : ConfigurationTestBase
    {
        enum Test { A , B }

        Test EnumProperty { get; set; }

        [Test]
        public void CheckDoesNothingWhenSettingAvailable()
        {
            var settings =Setting("EnumProperty", "A");

            using (var validator = new ConfigurationValidator(settings))
                validator.Check(() => EnumProperty);
        }

        [Test]
        public void CheckDoesNothingWhenSettingWithPrefixAvailable()
        {
            var settings = Setting("UK-EnumProperty", "B");

            using (var validator = new ConfigurationValidator(settings))
                validator.Check("UK-", () => EnumProperty);
        }

        [Test]
        public void CheckThrowsExceptionWhenSettingUnParseable()
        {
            const string UN_PARSEABLE = "this is not parseable to a TestEnum";
            const string PROPERTY_NAME = "EnumProperty";

            var settings = Setting(PROPERTY_NAME, UN_PARSEABLE);

            var exception = Assert.Throws<ConfigurationException>(() =>
                {
                    using (var validator = new ConfigurationValidator(settings))
                        validator.Check(() => EnumProperty);
                });

            Assert.AreEqual($"The {PROPERTY_NAME} setting ('{UN_PARSEABLE}') can not be converted to a {nameof(Test)}", exception.Message);
        }

        [Test]
        public void CheckThrowsExceptionWhenSettingWithPrefixUnParseable()
        {
            const string UN_PARSEABLE = "this is not parseable to an Enum";
            const string PROPERTY_NAME = "EnumProperty";
            const string PREFIX = "UK-";

            var settings = Setting($"{PREFIX}{PROPERTY_NAME}", UN_PARSEABLE);

            var exception = Assert.Throws<ConfigurationException>(() =>
            {
                using (var validator = new ConfigurationValidator(settings))
                    validator.Check(PREFIX, () => EnumProperty);
            });

            Assert.AreEqual($"The {PREFIX}{PROPERTY_NAME} setting ('{UN_PARSEABLE}') can not be converted to a {nameof(Test)}", exception.Message);
        }

        [Test]
        public void CheckThrowsExceptionWhenSettingMissing()
        {
            var exception = Assert.Throws<ConfigurationException>(() =>
            {
                using (var validator = new ConfigurationValidator(NoSettings))
                    validator.Check(() => EnumProperty);
            });

            Assert.AreEqual("The EnumProperty setting is missing", exception.Message);
        }

        [Test]
        public void CheckThrowsExceptionWhenSettingWithPrefixMissing()
        {
            var exception = Assert.Throws<ConfigurationException>(() =>
            {
                using (var validator = new ConfigurationValidator(NoSettings))
                    validator.Check("UK-", () => EnumProperty);
            });

            Assert.AreEqual("The UK-EnumProperty setting is missing", exception.Message);
        }
    }
}
