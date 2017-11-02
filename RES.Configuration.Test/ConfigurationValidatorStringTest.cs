using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RES.Configuration.Test
{
    [TestFixture]
    public class ConfigurationValidatorStringTest : ConfigurationTestBase
    {
        public string StringProperty { get; set; }

        [Test]
        public void CheckDoesNothingWhenSettingAvailable()
        {
            var settings = Setting("StringProperty", "blah");

            using (var validator = new ConfigurationValidator(settings))
                validator.Check(() => StringProperty);
        }

        [Test]
        public void CheckWithDefaultDoesNothingWhenSettingMissing()
        {
            using (var validator = new ConfigurationValidator(NoSettings))
                validator.CheckWithDefault(() => StringProperty);
        }

        [Test]
        public void CheckDoesNothingWhenSettingWithPrefixAvailable()
        {
            var settings = Setting("UK-StringProperty", "blah");

            using (var validator = new ConfigurationValidator(settings))
                validator.Check("UK-", () => StringProperty);
        }

        [Test]
        public void CheckThrowsExceptionWhenSettingMissing()
        {
            var exception = Assert.Throws<ConfigurationException>(() =>
            {
                using (var validator = new ConfigurationValidator(NoSettings))
                    validator.Check(() => StringProperty);
            });

            Assert.AreEqual("The StringProperty setting is missing", exception.Message);
        }

        [Test]
        public void CheckThrowsExceptionWhenSettingWithPrefixMissing()
        {
            var exception = Assert.Throws<ConfigurationException>(() =>
            {
                using (var validator = new ConfigurationValidator(NoSettings))
                    validator.Check("UK-", () => StringProperty);
            });

            Assert.AreEqual("The UK-StringProperty setting is missing", exception.Message);
        }
    }
}
