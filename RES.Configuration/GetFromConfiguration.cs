using System.Configuration;
using static System.Diagnostics.Contracts.Contract;
using Microsoft.Extensions.Configuration;

namespace RES.Configuration
{
    public class GetFromConfiguration : IConfigurationGetter
    {
        readonly IConfiguration configuration;

        public GetFromConfiguration(IConfiguration configuration)
        {
            Requires(configuration != null);

            this.configuration = configuration;
        }
        public string Get(string setting)
        {
            Requires(setting != null);

            return configuration[setting];
        }
    }
}