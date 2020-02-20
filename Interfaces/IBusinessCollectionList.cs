using System;
using System.Collections.Generic;

namespace Afonsoft.Data.Interfaces
{
    public interface IBusinessCollectionList<T> : ICollection<T>, IDisposable
    {
        T this[int index] { get; }
    }
}
