using System;

namespace Afonsoft.Data.Args
{
    [Serializable]
    public class DataAccessLayerEventArgs<T> : EventArgs
    {
        private readonly T item;

        public T Item
        {
            get
            {
                return this.item;
            }
        }

        public DataAccessLayerEventArgs(T item)
        {
            this.item = item;
        }
    }
}
