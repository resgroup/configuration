using System.Reflection;
namespace RES.Configuration
{
    public class ConfigurationBase
    {
        protected const string NO_PREFIX = "";
        protected readonly IConfigurationGetter configurationGetter;

        internal ConfigurationBase(IConfigurationGetter configuration)
        {
            this.configurationGetter = configuration;
        }

        protected bool IsMissing(string prefix, string setting) =>
            IsMissing(prefix + setting);

        protected bool IsMissing(string setting) =>
            GetStringOrNull(setting) == null;

        protected string GetString(string prefix, string setting) =>
            GetString(prefix + setting);

        protected string GetString(string setting)
        {
            if (GetStringOrNull(setting) == null)
                ThrowMissingSetting(setting);

            return GetStringOrNull(setting);
        }

        static void ThrowMissingSetting(string setting)
        {
            throw new ConfigurationException($"Configuration Setting '{setting}' not found. It is better to use the Validator to check settings at startup, rather than waiting until they are first used.");
        }

        string GetStringOrNull(string setting) =>
            configurationGetter.Get(setting);

    }
}
