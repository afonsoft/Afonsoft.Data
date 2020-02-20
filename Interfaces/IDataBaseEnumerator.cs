using System.Collections.Generic;

namespace Afonsoft.Data.Interfaces
{
    public interface IDataBaseEnumerator<T> : IEnumerator<T>
    {
        IDataConnector DataConnector { get; }

        void CreateDataReader();

    }
}