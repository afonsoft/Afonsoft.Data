using System;
using System.Collections.Generic;

namespace Afonsoft.Data.Interfaces
{
    public interface IBusinessCollectionDictionary<TKey, T> : IDictionary<TKey, T>, IDisposable
    {
        void Update(TKey key);
    }
}
