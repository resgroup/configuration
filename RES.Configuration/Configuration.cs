using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq.Expressions;
using static System.Diagnostics.Contracts.Contract;

namespace RES.Configuration
{
    // The optional prefix is designed so that several different configurations can be stored in the same config file easily. For example the settings for a UK deployment and a Sweden deployment would all be in the config file, prefixed with "UK-" and "SWE-" respectively. Then there can just be a single configuration item to indicate which settings to use (eg a "Deployment" setting that is either "UK" or "SWE"). This makes managing and deploying the config files a lot easier, especially when used with nested / included config files (eg. <appSettings file="shared.config">).
    // It is a bit annoying that we have to have the prefix code in this class, and that we can't just defer it to the caller. This happens because we get the name of the configuration by reflection of the property name, so if we pushed the prefix on to the caller (and asked the caller to just pass in a string of the configuration they wanted), the caller would also need to use reflection to get the name of the property. Or the caller wouldn't do this, and then the whole point of the strong connection between property name and configuration setting gets lost.
    // There is no support for nullable types. "Possibly null" values are annoying to work with. Use one of the WithDefault methods instead.
    // This class is getting quite long. Consider splitting it, probably by haveing one class for each thing being validated (eg BoolConfigurationValidator etc)
    public class Configuration : ConfigurationBase
    {
        public Configuration()
            : this(new ConfigurationGetter())
        { }

        public Configuration(IConfigurationGetter configurationGetter)
            : base(configurationGetter)
        {
            Requires(configurationGetter != null);
        }

        public ConfigurationValidator CreateValidator
        {
            get
            {
                Ensures(Result<ConfigurationValidator>() != null);

                return new ConfigurationValidator(configurationGetter);
            }
        }

        #region boolean settings
        public bool GetBool(MethodBase method)
        {
            Requires(method != null);

            return GetBool(NO_PREFIX, method);
        }

        public bool GetBool(string prefix, MethodBase method)
        {
            Requires(method != null);
            Requires(prefix != null);

            return bool.Parse(GetString(prefix, method));
        }

        public bool GetBoolWithDefault(MethodBase method, bool defaultValue)
        {
            Requires(method != null);

            return GetBoolWithDefault(NO_PREFIX, method, defaultValue);
        }

        public bool GetBoolWithDefault(string prefix, MethodBase method, bool defaultValue)
        {
            Requires(prefix != null);
            Requires(method != null);

            return
                IsMissing(prefix, method)
                ? defaultValue
                : GetBool(prefix, method);
        }
        #endregion

        #region int settings
        public int GetInt(MethodBase method)
        {
            Requires(method != null);

            return GetInt(NO_PREFIX, method);
        }

        public int GetInt(string prefix, MethodBase method)
        {
            Requires(prefix != null);
            Requires(method != null);

            return int.Parse(GetString(prefix, method));
        }

        public int GetIntWithDefault(MethodBase method, int defaultValue)
        {
            Requires(method != null);

            return GetIntWithDefault(NO_PREFIX, method, defaultValue);
        }

        public int GetIntWithDefault(string prefix, MethodBase method, int defaultValue)
        {
            Requires(prefix != null);
            Requires(method != null);

            return 
                IsMissing(prefix, method)
                ? defaultValue
                : GetInt(prefix, method);

        }
        #endregion

        #region double settings
        public double GetDouble(MethodBase method)
        {
            Requires(method != null);

            return GetDouble(NO_PREFIX, method);
        }

        public double GetDouble(string prefix, MethodBase method)
        {
            Requires(prefix != null);
            Requires(method != null);

            return double.Parse(GetString(prefix, method));
        }

        public double GetDoubleWithDefault(MethodBase method, double defaultValue)
        {
            Requires(method != null);

            return GetDoubleWithDefault(NO_PREFIX, method, defaultValue);
        }

        public double GetDoubleWithDefault(string prefix, MethodBase method, double defaultValue)
        {
            Requires(prefix != null);
            Requires(method != null);

            return 
                IsMissing(prefix, method)
                ? defaultValue
                : GetDouble(prefix, method);
        }
        #endregion

        #region Enum settings
        public T GetEnum<T>(MethodBase method)
            where T : struct, IConvertible
        {
            Requires(typeof(T).IsEnum, "T must be an enumerated type");
            Requires(method != null);

            return GetEnum<T>(NO_PREFIX, method);
        }

        public T GetEnum<T>(string prefix, MethodBase method)
            where T : struct, IConvertible
        {
            Requires(typeof(T).IsEnum, "T must be an enumerated type");
            Requires(prefix != null);
            Requires(method != null);

            return (T)Enum.Parse(typeof(T), GetString(prefix, method));
        }

        public T GetEnumWithDefault<T>(MethodBase method, T defaultValue)
            where T : struct, IConvertible
        {
            Requires(typeof(T).IsEnum, "T must be an enumerated type");
            Requires(method != null);

            return GetEnumWithDefault(NO_PREFIX, method, defaultValue);
        }

        public T GetEnumWithDefault<T>(string prefix, MethodBase method, T defaultValue)
            where T : struct, IConvertible
        {
            Requires(typeof(T).IsEnum, "T must be an enumerated type");
            Requires(prefix != null);
            Requires(method != null);

            return 
                IsMissing(prefix, method)
                ? defaultValue
                : GetEnum<T>(prefix, method);
        }
        #endregion

        #region IntegerList settings
        public IEnumerable<int> GetIntegerList(MethodBase method)
        {
            Requires(method != null);
            Ensures(Result<IEnumerable<int>>() != null);

            return GetIntegerList(NO_PREFIX, method);
        }

        public IEnumerable<int> GetIntegerList(string prefix, MethodBase method)
        {
            Requires(prefix != null);
            Requires(method != null);
            Ensures(Result<IEnumerable<int>>() != null);

            return ParseIntegerListFromCsv.Parse(GetString(prefix, method));
        }

        public IEnumerable<int> GetIntegerListWithDefault(MethodBase method, IEnumerable<int> defaultValue)
        {
            Requires(method != null);
            Ensures(Result<IEnumerable<int>>() != null);

            return GetIntegerListWithDefault(NO_PREFIX, method, defaultValue);
        }

        public IEnumerable<int> GetIntegerListWithDefault(string prefix, MethodBase method, IEnumerable<int> defaultValue)
        {
            Requires(prefix != null);
            Requires(method != null);
            Ensures(Result<IEnumerable<int>>() != null);

            return 
                IsMissing(prefix, method)
                ? defaultValue
                : GetIntegerList(prefix, method);
        }
        #endregion

        #region string settings
        public string GetString(MethodBase method)
        {
            Requires(method != null);
            Ensures(Result<string>() != null);

            return GetString(NO_PREFIX, method);
        }

        public string GetString(string prefix, MethodBase method)
        {
            Requires(prefix != null);
            Requires(method != null);
            Ensures(Result<string>() != null);

            return GetString(prefix, PropertyName(method));
        }

        public string GetStringWithDefault(MethodBase method, string defaultValue)
        {
            Requires(method != null);
            Ensures(Result<string>() != null);

            return GetStringWithDefault(NO_PREFIX, method, defaultValue);
        }

        public string GetStringWithDefault(string prefix, MethodBase method, string defaultValue)
        {
            Requires(prefix != null);
            Requires(method != null);
            Ensures(Result<string>() != null);

            return
                IsMissing(prefix, method)
                ? defaultValue
                : GetString(prefix, method);
        }
        #endregion

        #region utilities

        static string PropertyName(MethodBase method)
        {
            Requires(method != null);
            Ensures(Result<string>() != null);

            return method.Name.Replace("get_", "");
        }

        bool IsMissing(string prefix, MethodBase method) =>
            IsAvailable(prefix, PropertyName(method));

        #endregion
    }
}
