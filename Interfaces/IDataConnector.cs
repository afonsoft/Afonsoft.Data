using System;
using System.Data;
using System.Data.Common;

namespace Afonsoft.Data.Interfaces
{
    public interface IDataConnector : IDisposable
    {
       DbConnection Connection { get; }
       DbProviderFactory ProviderFactory { get; }
       DbTransaction Transaction { get; }
       void BeginTransaction();
       void BeginTransaction(IsolationLevel isolationLevel);
       void CloseConnection();
       void Commit();
       void Rollback();
    }
}
