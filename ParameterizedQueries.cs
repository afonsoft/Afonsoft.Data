using System;
using System.Data;
using System.Data.Common;

namespace Afonsoft.Data
{
    [Serializable]
    public abstract class ParameterizedQueries
    {
        protected DbProviderFactory providerFactory;

        protected ParameterizedQueries()
        {
        }

        protected ParameterizedQueries(DbProviderFactory providerFactory)
          : this()
        {
            this.providerFactory = providerFactory;
        }

        public DbParameter CreateParameter(string parameterName)
        {
            DbParameter parameter = this.providerFactory.CreateParameter();
            parameter.ParameterName = parameterName;
            return parameter;
        }

        public DbParameter CreateParameter(string parameterName, DbType dataType)
        {
            DbParameter parameter = this.CreateParameter(parameterName);
            parameter.DbType = dataType;
            return parameter;
        }

        public DbParameter CreateParameter(
          string parameterName,
          DbType dataType,
          object value)
        {
            DbParameter parameter = this.CreateParameter(parameterName, dataType);
            parameter.Value = value;
            return parameter;
        }

        public DbParameter CreateParameter(
          string parameterName,
          DbType dataType,
          int size,
          object value)
        {
            DbParameter parameter = this.CreateParameter(parameterName, dataType, value);
            parameter.Size = size;
            return parameter;
        }

        public DbParameter CreateParameter<TValue>(string parameterName, TValue value) where TValue : struct
        {
            DbParameter parameter = this.CreateParameter(parameterName);
            parameter.Value = (object)value;
            return parameter;
        }

        public DbParameter CreateParameter<TValue>(
          string parameterName,
          DbType dataType,
          TValue value) where TValue : struct
        {
            DbParameter parameter = this.CreateParameter<TValue>(parameterName, value);
            parameter.DbType = dataType;
            return parameter;
        }

        public DbParameter CreateParameter<TValue>(
          string parameterName,
          DbType dataType,
          int size,
          TValue value) where TValue : struct
        {
            DbParameter parameter = this.CreateParameter<TValue>(parameterName, dataType, value);
            parameter.Size = size;
            return parameter;
        }
    }
}