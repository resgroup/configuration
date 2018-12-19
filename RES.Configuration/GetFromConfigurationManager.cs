using System.Configuration;
using static System.Diagnostics.Contracts.Contract;

namespace RES.Configuration
{
    public class GetFromConfigurationManager : IConfigurationGetter
    {
        public string Get(string setting)
        {
            Requires(setting != null);

            return ConfigurationManager.AppSettings[setting];
        }
    }
}