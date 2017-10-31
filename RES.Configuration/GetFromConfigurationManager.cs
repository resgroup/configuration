using System.Configuration;
using static System.Diagnostics.Contracts.Contract;

namespace RES.Configuration
{
    internal class ConfigurationManagerConfigurationGetter : IConfigurationGetter
    {
        public string Get(string setting)
        {
            Requires(setting != null);

            return ConfigurationManager.AppSettings[setting];
        }
    }
}