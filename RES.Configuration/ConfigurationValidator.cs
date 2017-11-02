using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Linq.Expressions;
using static System.Diagnostics.Contracts.Contract;
using System.Linq;

namespace RES.Configuration
{
    // This class is getting quite long. Consider splitting it, probably by having one class for each thing being validated (eg BoolConfigurationValidator etc)
    // Could have a base / helper class that handles keeping a record of the errors (eg list of errors, adderror, anyerrors, errormessages)
    public sealed class ConfigurationValidator : ConfigurationBase, IDisposable
    {
        readonly List<string> errors;

        public ConfigurationValidator()
            : this(new GetFromConfigurationManager()) { }

        public ConfigurationValidator(IConfigurationGetter configurationGetter)
            : base(configurationGetter)
        {
            Requires(configurationGetter != null);

            errors = new List<string>();
        }

        public void Dispose()
        {
            RaiseFailedChecks();
        }

        void RaiseFailedChecks()
        {
            if (errors.Any())
                throw new ConfigurationException(string.Join("\r\n", errors));
        }

        #region string settings
        public void Check(Expression<Func<string>> setting)
        {
            Requires(setting != null);

            Check("", setting);
        }

        public void Check(string prefix, Expression<Func<string>> setting)
        {
            Requires(prefix != null);
            Requires(setting != null);

            CheckMissing(prefix, setting);
        }

        public void CheckWithDefault(Expression<Func<string>> setting)
        {
            Requires(setting != null);

            CheckWithDefault("", setting);
        }

        public void CheckWithDefault(string prefix, Expression<Func<string>> setting)
        {
            Requires(prefix != null);
            Requires(setting != null);

            if (IsAvailable(prefix, GetProperty(setting)))
                Check(prefix, setting);
        }

        #endregion

        #region boolean settings
        public void Check(Expression<Func<bool>> setting)
        {
            Requires(setting != null);

            Check("", setting);
        }

        public void Check(string prefix, Expression<Func<bool>> setting)
        {
            Requires(prefix != null);

            string propertyName = GetProperty(setting).Name;

            CheckMissing(prefix, setting);

            if (BoolSettingAvailableButNotParseable(prefix, propertyName))
                errors.Add($"The {prefix}{propertyName} setting ('{GetString(prefix, propertyName)}') can not be converted to a boolean");
        }

        bool BoolSettingAvailableButNotParseable(string prefix, string propertyName)
        {
            Requires(prefix != null);
            Requires(propertyName != null);

            return IsAvailable(prefix, propertyName) &&
            CanParseBool(prefix, propertyName) == false;
        }

        public void CheckWithDefault(Expression<Func<bool>> setting)
        {
            Requires(setting != null);

            CheckWithDefault("", setting);
        }

        public void CheckWithDefault(string prefix, Expression<Func<bool>> setting)
        {
            Requires(prefix != null);
            Requires(setting != null);

            if (IsAvailable(prefix, GetProperty(setting)))
                Check(prefix, setting);
        }

        bool CanParseBool(string prefix, string propertyName)
        {
            Requires(prefix != null);
            Requires(propertyName != null);

            bool irrelevant;

            return bool.TryParse(
                GetString(prefix + propertyName),
                out irrelevant);
        }
        #endregion

        #region int settings
        public void Check(Expression<Func<int>> setting)
        {
            Requires(setting != null);

            Check("", setting);
        }

        public void Check(string prefix, Expression<Func<int>> setting)
        {
            Requires(prefix != null);
            Requires(setting != null);

            string propertyName = GetProperty(setting).Name;

            CheckMissing(prefix, setting);

            if (IntSettingAvailableButNotParseable(prefix, propertyName))
                errors.Add($"The {prefix}{propertyName} setting ('{GetString(prefix, propertyName)}') can not be converted to an int");
        }

        bool IntSettingAvailableButNotParseable(string prefix, string propertyName)
        {
            Requires(prefix != null);
            Requires(propertyName != null);

            return IsAvailable(prefix, propertyName) &&
            CanParseInt(prefix, propertyName) == false;
        }

        public void CheckWithDefault(Expression<Func<int>> setting)
        {
            Requires(setting != null);

            CheckWithDefault("", setting);
        }

        public void CheckWithDefault(string prefix, Expression<Func<int>> setting)
        {
            Requires(prefix != null);
            Requires(setting != null);

            if (IsAvailable(prefix, GetProperty(setting)))
                Check(prefix, setting);
        }

        bool CanParseInt(string prefix, string propertyName)
        {
            Requires(prefix != null);
            Requires(propertyName != null);

            int irrelevant;

            return int.TryParse(
                GetString(prefix + propertyName),
                out irrelevant);
        }

        #endregion

        #region double settings
        public void Check(Expression<Func<double>> setting)
        {
            Requires(setting != null);

            Check("", setting);
        }

        public void Check(string prefix, Expression<Func<double>> setting)
        {
            Requires(prefix != null);
            Requires(setting != null);

            string propertyName = GetProperty(setting).Name;

            CheckMissing(prefix, setting);

            if (DoubleSettingAvailableButNotParseable(prefix, propertyName))
                errors.Add($"The {prefix}{propertyName} setting ('{GetString(prefix, propertyName)}') can not be converted to a double");
        }

        public void CheckWithDefault(Expression<Func<double>> setting)
        {
            Requires(setting != null);

            CheckWithDefault("", setting);
        }

        public void CheckWithDefault(string prefix, Expression<Func<double>> setting)
        {
            Requires(prefix != null);
            Requires(setting != null);

            if (IsAvailable(prefix, GetProperty(setting)))
                Check(prefix, setting);
        }

        bool DoubleSettingAvailableButNotParseable(string prefix, string propertyName)
        {
            Requires(prefix != null);
            Requires(propertyName != null);

            return IsAvailable(prefix, propertyName) &&
            CanParseDouble(prefix, propertyName) == false;
        }

        bool CanParseDouble(string prefix, string propertyName)
        {
            Requires(prefix != null);
            Requires(propertyName != null);

            double irrelevant;

            return double.TryParse(
                GetString(prefix + propertyName),
                out irrelevant);
        }

        #endregion

        #region Enum settings
        public void Check<T>(Expression<Func<T>> setting)
            where T : struct, IConvertible
        {
            Requires(typeof(T).IsEnum, "T must be an enumerated type");
            Requires(setting != null);

            Check(NO_PREFIX, setting);
        }

        public void Check<T>(string prefix, Expression<Func<T>> setting)
            where T : struct, IConvertible
        {
            Requires(typeof(T).IsEnum, "T must be an enumerated type");
            Requires(prefix != null);
            Requires(setting != null);

            CheckMissing(prefix, setting);

            var property = GetProperty(setting);

            string propertyName = GetProperty(setting).Name;

            if (EnumSettingAvailableButNotParseable<T>(prefix, propertyName))
                errors.Add($"The {prefix}{propertyName} setting ('{GetString(prefix, property.Name)}') can not be converted to a {typeof(T).Name}");
        }

        public void CheckWithDefault<T>(Expression<Func<T>> setting)
            where T : struct, IConvertible
        {
            Requires(setting != null);

            CheckWithDefault("", setting);
        }

        public void CheckWithDefault<T>(string prefix, Expression<Func<T>> setting)
            where T : struct, IConvertible
        {
            Requires(prefix != null);
            Requires(setting != null);

            if (IsAvailable(prefix, GetProperty(setting)))
                Check(prefix, setting);
        }

        bool EnumSettingAvailableButNotParseable<T>(string prefix, string propertyName)
            where T : struct, IConvertible
        {
            Requires(prefix != null);
            Requires(propertyName != null);

            return IsAvailable(prefix, propertyName) &&
                CanParseEnum<T>(prefix, propertyName) == false;
        }

        bool CanParseEnum<T>(string prefix, string propertyName)
            where T : struct, IConvertible
        {
            Requires(prefix != null);
            Requires(propertyName != null);

            T irrelevant;

            return
                Enum.TryParse(
                    GetString(prefix + propertyName),
                    out irrelevant);
        }

        #endregion

        #region IntegerList settings
        public void Check(Expression<Func<IEnumerable<int>>> setting)
        {
            Requires(setting != null);

            Check("", setting);
        }

        public void Check(string prefix, Expression<Func<IEnumerable<int>>> setting)
        {
            Requires(prefix != null);
            Requires(setting != null);

            string propertyName = GetProperty(setting).Name;

            CheckMissing(prefix, setting);

            if (IntegerListSettingAvailableButNotParseable(prefix, propertyName))
                errors.Add($"The {prefix}{propertyName} setting ('{GetString(prefix, propertyName)}') can not be converted to an IntegerList");
        }

        public void CheckWithDefault(Expression<Func<IEnumerable<int>>> setting)
        {
            Requires(setting != null);

            CheckWithDefault("", setting);
        }

        public void CheckWithDefault(string prefix, Expression<Func<IEnumerable<int>>> setting)
        {
            Requires(prefix != null);
            Requires(setting != null);

            if (IsAvailable(prefix, GetProperty(setting)))
                Check(prefix, setting);
        }

        bool IntegerListSettingAvailableButNotParseable(string prefix, string propertyName)
        {
            Requires(prefix != null);
            Requires(propertyName != null);

            return IsAvailable(prefix, propertyName) &&
            CanParseIntegerList(prefix, propertyName) == false;
        }

        bool CanParseIntegerList(string prefix, string propertyName)
        {
            Requires(prefix != null);
            Requires(propertyName != null);

            return ParseIntegerListFromCsv.CanParse(GetString(prefix + propertyName));
        }

        #endregion

        #region missing settings
        void CheckMissing<T>(string prefix, Expression<Func<T>> setting)
        {
            Requires(prefix != null);
            Requires(setting != null);

            CheckMissing(prefix + GetProperty<T>(setting).Name);
        }

        void CheckMissing(string setting)
        {
            Requires(setting != null);

            if (IsAvailable(setting) == false)
                errors.Add(string.Format("The {0} setting is missing", setting));
        }

        bool IsAvailable(string prefix, PropertyInfo property)
        {
            Requires(prefix != null);
            Requires(property != null);

            return string.IsNullOrEmpty(GetString(prefix + property.Name)) == false;
        }
        #endregion

        # region utilities
        static PropertyInfo GetProperty<TValue>(Expression<Func<TValue>> selector)
        {
            Requires(selector != null);

            switch (NodeType(selector))
            {
                case ExpressionType.MemberAccess:
                    return MemberExpressionPropertyInfo(selector);
                default:
                    throw new InvalidOperationException();
            }
        }

        static ExpressionType NodeType<TValue>(Expression<Func<TValue>> selector)
        {
            Requires(selector != null);

            return GetBody(selector).NodeType;
        }

        static PropertyInfo MemberExpressionPropertyInfo<TValue>(Expression<Func<TValue>> selector)
        {
            Requires(selector != null);

            return (PropertyInfo)((MemberExpression)GetBody(selector)).Member;
        }

        static Expression GetBody<TValue>(Expression<Func<TValue>> selector)
        {
            Requires(selector != null);

            Expression body = selector;

            return (body is LambdaExpression)
                ? ((LambdaExpression)body).Body
                : body;
        }

        #endregion
    }
}
