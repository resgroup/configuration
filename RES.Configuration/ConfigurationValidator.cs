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
    public class ConfigurationValidator : ConfigurationBase, IDisposable
    {
        readonly List<string> errors;

        public ConfigurationValidator()
            : this(new ConfigurationGetter()) { }

        public ConfigurationValidator(IConfigurationGetter configurationGetter) 
            : base(configurationGetter)
        {
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
        public void Check(Expression<Func<string>> setting) =>
            Check("", setting);

        public void Check(string prefix, Expression<Func<string>> setting) =>
            CheckMissing(prefix, setting);

        public void CheckWithDefault(string prefix, Expression<Func<string>> setting)
        {
            if (IsMissing(prefix, GetProperty(setting)) == false)
                Check(prefix, setting);
        }

        #endregion

        #region boolean settings
        public void Check(Expression<Func<bool>> setting) =>
            Check("", setting);

        public void Check(string prefix, Expression<Func<bool>> setting)
        {
            string propertyName = GetProperty(setting).Name;

            CheckMissing(prefix, setting);

            if (BoolSettingAvailableButNotParseable(prefix, propertyName))
                errors.Add($"The {prefix}{propertyName} setting ('{GetString(prefix + propertyName)}') can not be converted to a boolean");
        }

        bool BoolSettingAvailableButNotParseable(string prefix, string propertyName) =>
            IsMissing(prefix, propertyName) == false && 
            CanParseBool(prefix, propertyName) == false;

        public void CheckWithDefault(string prefix, Expression<Func<bool>> setting)
        {
            if (IsMissing(prefix, GetProperty(setting)) == false)
                Check(prefix, setting);
        }

        bool CanParseBool(string prefix, string propertyName)
        {
            bool irrelevant;

            return bool.TryParse(
                GetString(prefix + propertyName),
                out irrelevant);
        }
        #endregion

        #region int settings
        public void Check(Expression<Func<int>> setting) =>
            Check("", setting);

        public void Check(string prefix, Expression<Func<int>> setting)
        {
            string propertyName = GetProperty(setting).Name;

            CheckMissing(prefix, setting);

            if (IntSettingAvailableButNotParseable(prefix, propertyName))
                errors.Add($"The {prefix}{propertyName} setting ('{GetString(prefix + propertyName)}') can not be converted to an int");
        }

        bool IntSettingAvailableButNotParseable(string prefix, string propertyName) =>
            IsMissing(prefix, propertyName) == false &&
            CanParseInt(prefix, propertyName) == false;

        public void CheckWithDefault(string prefix, Expression<Func<int>> setting)
        {
            if (IsMissing(prefix, GetProperty(setting)) == false) 
                Check(prefix, setting);
        }

        bool CanParseInt(string prefix, string propertyName)
        {
            int irrelevant;

            return int.TryParse(
                GetString(prefix + propertyName),
                out irrelevant);
        }

        #endregion

        #region double settings
        public void Check(Expression<Func<double>> setting) =>
            Check("", setting);

        public void Check(string prefix, Expression<Func<double>> setting)
        {
            string propertyName = GetProperty(setting).Name;

            CheckMissing(prefix, setting);

            if (DoubleSettingAvailableButNotParseable(prefix, propertyName))
                errors.Add($"The {prefix}{propertyName} setting ('{GetString(prefix + propertyName)}') can not be converted to a double");
        }

        public void CheckWithDefault(string prefix, Expression<Func<double>> setting)
        {
            if (IsMissing(prefix, GetProperty(setting)) == false)
                Check(prefix, setting);
        }

        bool DoubleSettingAvailableButNotParseable(string prefix, string propertyName) =>
            IsMissing(prefix, propertyName) == false &&
            CanParseDouble(prefix, propertyName) == false;

        bool CanParseDouble(string prefix, string propertyName)
        {
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

            Check(NO_PREFIX, setting);
        }

        public void Check<T>(string prefix, Expression<Func<T>> setting)
            where T : struct, IConvertible
        {
            Requires(typeof(T).IsEnum, "T must be an enumerated type");

            CheckMissing(prefix, setting);

            var property = GetProperty(setting);

            string propertyName = GetProperty(setting).Name;

            if (EnumSettingAvailableButNotParseable<T>(prefix, propertyName))
                errors.Add($"The {prefix}{propertyName} setting ('{GetString(prefix + property.Name)}') can not be converted to a {typeof(T).Name}");
        }

        bool EnumSettingAvailableButNotParseable<T>(string prefix, string propertyName) 
            where T : struct, IConvertible =>
                IsMissing(prefix, propertyName) == false &&
                CanParseEnum<T>(prefix, propertyName) == false;

        bool CanParseEnum<T>(string prefix, string propertyName)
            where T : struct, IConvertible
        {
            T irrelevant;

            return
                Enum.TryParse(
                    GetString(prefix + propertyName),
                    out irrelevant);
        }

        #endregion

        #region IntegerList settings
        public void Check(Expression<Func<IEnumerable<int>>> setting) =>
            Check("", setting);


        public void Check(string prefix, Expression<Func<IEnumerable<int>>> setting)
        {
            string propertyName = GetProperty(setting).Name;

            CheckMissing(prefix, setting);

            if (IntegerListSettingAvailableButNotParseable(prefix, propertyName))
                errors.Add($"The {prefix}{propertyName} setting ('{GetString(prefix + propertyName)}') can not be converted to an IntegerList");
        }

        public void CheckWithDefault(string prefix, Expression<Func<IEnumerable<int>>> setting)
        {
            if (IsMissing(prefix, GetProperty(setting)) == false)
                Check(prefix, setting);
        }

        bool IntegerListSettingAvailableButNotParseable(string prefix, string propertyName) =>
            IsMissing(prefix, propertyName) == false &&
            CanParseIntegerList(prefix, propertyName) == false;

        bool CanParseIntegerList(string prefix, string propertyName) =>
            ParseIntegerListFromCsv.CanParse(GetString(prefix + propertyName));

        #endregion

        #region missing settings
        void CheckMissing<T>(string prefix, Expression<Func<T>> setting) =>
            CheckMissing(prefix + GetProperty<T>(setting).Name);

        void CheckMissing(string setting)
        {
            if (IsMissing(setting))
                errors.Add(string.Format("The {0} setting is missing", setting));
        }

        bool IsMissing(string prefix, PropertyInfo property) =>
            string.IsNullOrEmpty(GetString(prefix + property.Name));
        #endregion

        # region utilities
        static PropertyInfo GetProperty<TValue>(Expression<Func<TValue>> selector)
        {
            switch (NodeType(selector))
            {
                case ExpressionType.MemberAccess:
                    return MemberExpressionPropertyInfo(selector);
                default:
                    throw new InvalidOperationException();
            }
        }

        static ExpressionType NodeType<TValue>(Expression<Func<TValue>> selector) =>
            GetBody(selector).NodeType;

        static PropertyInfo MemberExpressionPropertyInfo<TValue>(Expression<Func<TValue>> selector) =>
            (PropertyInfo)((MemberExpression)GetBody(selector)).Member;

        static Expression GetBody<TValue>(Expression<Func<TValue>> selector)
        {
            Expression body = selector;

            return (body is LambdaExpression)
                ? ((LambdaExpression)body).Body
                : body;
        }

        #endregion
    }
}
