using Moq;
using NUnit.Framework;

namespace RES.Configuration.Test
{
    [TestFixture]
    public class ConfigurationTestBase
    {
        protected IConfigurationGetter Setting(string configurationKey, string configurationValue)
        {
            var configurationGetter = new Mock<RES.Configuration.IConfigurationGetter>();
            configurationGetter.Setup(m => m.Get(configurationKey)).Returns(configurationValue);

            return configurationGetter.Object;
        }

        protected IConfigurationGetter NoSettings =>
            Mock.Of<IConfigurationGetter>();
    }
}
