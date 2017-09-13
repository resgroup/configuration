using System.Configuration;

namespace RES.Configuration
{
    internal class ConfigurationGetter : IConfigurationGetter
    {
        public string Get(string setting) =>
            ConfigurationManager.AppSettings[setting];

    }
}