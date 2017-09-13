using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RES.Configuration.Test
{
    [TestFixture]
    public class ConfigurationValidatorMultipleFailuresTest : ConfigurationTestBase
    {
        public int Property1 { get; set; }
        public double Property2 { get; set; }

        [Test]
        public void CheckTwoFailuresAreReported()
        {
            var exception = Assert.Throws<ConfigurationException>(() =>
            {
                using (var validator = new ConfigurationValidator(NoSettings))
                {
                    validator.Check(() => Property1);
                    validator.Check(() => Property2);
                }
            });

            Assert.That(exception.Message.Contains("Property1"));
            Assert.That(exception.Message.Contains("Property2"));
        }

    }
}
