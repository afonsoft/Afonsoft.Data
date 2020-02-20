using System;
using System.Data.Common;
using System.Globalization;
using System.Threading;

namespace Afonsoft.Data
{
    public class InLineDataManipulationCommand
    {
        private readonly CultureInfo cultureInfo;

        public InLineDataManipulationCommand()
        {
            this.cultureInfo = Thread.CurrentThread.CurrentCulture;
        }

        public InLineDataManipulationCommand(CultureInfo cultureInfo)
        {
            this.cultureInfo = cultureInfo;
        }

        public void BuildInlineCommand(DbCommand cmd, string parameterPrefix)
        {
            string str = cmd.CommandText;
            foreach (DbParameter parameter in cmd.Parameters)
            {
                string formattedValue = this.GetFormattedValue(parameter.Value.GetType(), parameter.Value);
                string oldValue = parameter.ParameterName.StartsWith(parameterPrefix) ? parameter.ParameterName : parameterPrefix + parameter.ParameterName;
                str = str.Replace(oldValue, formattedValue);
            }
            cmd.CommandText = str;
            cmd.Parameters.Clear();
        }

        protected string GetFormattedValue(Type dataType, object value)
        {
            if (dataType.IsEnum)
                return Convert.ToInt32(value).ToString();
            if (this.IsNull(dataType, value) || dataType.FullName.ToLower() == "system.dbnull")
                return "NULL";
            string lower = dataType.FullName.ToLower();
            if (lower == "system.string")
                return "'" + ((string)value).Replace("'", "''") + "'";
            if (lower == "system.boolean")
                return (bool)value ? "TRUE" : "FALSE";
            if (lower == "system.datetime")
                return "'" + ((DateTime)value).ToString("yyyy-MM-dd HH:mm:ss.fff") + "'";
            return Convert.ToString(value, (IFormatProvider)this.cultureInfo);
        }

        protected bool IsInteger(object value)
        {
            if (!(value is sbyte) && !(value is short) && (!(value is int) && !(value is long)) && (!(value is byte) && !(value is ushort) && (!(value is uint) && !(value is ulong))))
            {
#if NET35
                return false;
#else
                return value is System.Numerics.BigInteger;
#endif
            }
            return true;
        }

        protected bool IsFloat(object value)
        {
            return value is float || value is double || value is Decimal;
        }

        protected bool IsNumeric(object value)
        {
#if NET35
            return value is byte || value is short || (value is int || value is long) || (value is sbyte || value is ushort || (value is uint || value is ulong)) || (value is Decimal || (value is double || value is float));
#else
            return value is byte || value is short || (value is int || value is long) || (value is sbyte || value is ushort || (value is uint || value is ulong)) || (value is System.Numerics.BigInteger || value is Decimal || (value is double || value is float));
#endif
        }

        protected bool IsNull(Type propertyType, object value)
        {
            return value is string && string.IsNullOrEmpty((string)value) || (this.IsNumeric(value) || value is DateTime) && value.ToString() == propertyType.GetField("MinValue").GetValue((object)null).ToString();
        }
    }
}

