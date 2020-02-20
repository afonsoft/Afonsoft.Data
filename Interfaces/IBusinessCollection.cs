using Afonsoft.Data.Args;
using System;

namespace Afonsoft.Data.Interfaces
{
    public interface IBusinessCollection<T>
    {
        IDataAccessLayer<T> DataAccessLayer { get; }

        void Update(T item);

        void ForEach(Action<T> action);

        event EventHandler<ViewEventArgs<T>> OnCreate;

        event EventHandler<EventArgs> OnRefresh;

        event EventHandler<ViewEventArgs<T>> OnRemove;

        event EventHandler<ViewEventArgs<T>> OnUpdate;

    }
}
