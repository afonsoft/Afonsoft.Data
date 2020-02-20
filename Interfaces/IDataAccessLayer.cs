using Afonsoft.Data.Args;
using System;

namespace Afonsoft.Data.Interfaces
{
    public interface IDataAccessLayer<T>
    {
        int Create(T entity);

        int Update(T entity);

        int Remove(T entity);

        IDataConnector DataConnector { get; }

    }
}
