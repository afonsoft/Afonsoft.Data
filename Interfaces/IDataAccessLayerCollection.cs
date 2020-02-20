using System;
using System.Collections.Generic;


namespace Afonsoft.Data.Interfaces
{
    public interface IDataAccessLayerCollection<T> : IEnumerable<T>, IDisposable
    {
        IDataConnector DataConnector { get; }
    }
}
