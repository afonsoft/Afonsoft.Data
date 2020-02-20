using Afonsoft.Data.Interfaces;
using System;


namespace Afonsoft.Data.Args
{
    [Serializable]
    public class CompositeEventArgs<T> : EventArgs
    {
        public IDataConnector DataConnector { get; }

        public T Composite { get; }

        public CompositeEventArgs(T composite)
        {
            this.Composite = composite;
        }

        public CompositeEventArgs(IDataConnector dataConnector, T composite)
          : this(composite)
        {
            this.DataConnector = dataConnector;
        }
    }
}
