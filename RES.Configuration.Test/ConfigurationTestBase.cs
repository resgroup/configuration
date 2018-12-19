using Moq;
using NUnit.Framework;

namespace RES.Configuration.Test
{
    [TestFixture]
    public class ConfigurationTestBase
    {
        protected Configuration ConfigurationWithSetting(string configurationKey, string configurationValue) =>
            new Configuration(Setting(configurationKey, configurationValue));

        protected IConfigurationGetter Setting(string configurationKey, string configurationValue)
        {
            var configurationGetter = new Mock<IConfigurationGetter>();
            configurationGetter.Setup(m => m.Get(configurationKey)).Returns(configurationValue);

            return configurationGetter.Object;
        }

        protected Configuration ConfigurationWithNoSettings =>
            new Configuration(NoSettings);

        protected IConfigurationGetter NoSettings =>
            Mock.Of<IConfigurationGetter>();
    }
}
