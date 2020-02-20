using Afonsoft.Data.Exceptions;
using Afonsoft.Data.Interfaces;
using System;
using System.Data;
using System.Data.Common;

namespace Afonsoft.Data
{
    [Serializable]
    public sealed class DataBaseConnector : ParameterizedQueries, IDataConnector
    {
        private readonly DbConnection cn;
        private bool disposed;


        public DataBaseConnector(
          string connectionString,
          string invariantProviderName)
        {
            this.providerFactory = DbProviderFactories.GetFactory(invariantProviderName);
            this.cn = this.providerFactory.CreateConnection();
            this.cn.ConnectionString = connectionString;
        }



        public DataBaseConnector(DbConnection cn, DbProviderFactory providerFactory)
          : base(providerFactory)
        {
            this.cn = cn;
            this.providerFactory = providerFactory;
        }

        public DbConnection Connection
        {
            get
            {
                if (this.cn.State != ConnectionState.Open)
                {
                    this.cn.Open();
                }
                return this.cn;
            }
        }

        public DbProviderFactory ProviderFactory
        {
            get
            {
                return this.providerFactory;
            }
        }

        public void CloseConnection()
        {
            if (this.cn == null || this.cn.State != ConnectionState.Open)
                return;
            this.cn.Close();
        }

        public DbTransaction Transaction { get; private set; }

        public void BeginTransaction()
        {
            if (this.Transaction == null)
            {
                this.Transaction = this.Connection.BeginTransaction();
            }
            else
            {
                this.Rollback();
                throw new TransactionException("A ultima transação iniciada não foi terminada com o método Commit ou Rollback, a transação foi desfeita.");
            }
        }

        public void BeginTransaction(System.Data.IsolationLevel isolationLevel)
        {
            if (this.Transaction == null)
            {
                this.Transaction = this.Connection.BeginTransaction(isolationLevel);
            }
            else
            {
                this.Rollback();
                throw new TransactionException("A ultima transação iniciada não foi terminada com o método Commit ou Rollback, a transação foi desfeita.");
            }
        }

        public void Rollback()
        {
            this.Transaction.Rollback();
            this.Transaction = (DbTransaction)null;
        }

        public void Commit()
        {
            this.Transaction.Commit();
            this.Transaction = (DbTransaction)null;
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize((object)this);
        }

        private void Dispose(bool disposing)
        {
            if (!disposing || this.disposed || this.cn == null)
                return;
            this.cn.Dispose();
            this.disposed = true;
        }
    }
}
