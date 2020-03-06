using Afonsoft.Data.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Afonsoft.Data
{
    [Serializable]
    public abstract class DataAccessLayerCollection<T> : IDataAccessLayerCollection<T>
    {
        protected IDataConnector dataConnector;
        protected IEnumerator<T> enumerator;
        protected bool isDisposed;

        public IDataConnector DataConnector => throw new NotImplementedException();

        protected DataAccessLayerCollection(IDataConnector dataConnector) 
        {
            this.dataConnector = dataConnector;
        }

        public IEnumerator<T> GetEnumerator()
        {
            this.enumerator.Reset();
            return this.enumerator;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            this.enumerator.Reset();
            return (IEnumerator)this.enumerator;
        }

        public void Dispose()
        {
            Dispose(true);
        }

        public void Dispose(bool disposing)
        {
            if (this.isDisposed)
                return;
            if (disposing && this.enumerator != null)
                this.enumerator.Dispose();
            this.isDisposed = true;
        }

        ~DataAccessLayerCollection()
        {
            Dispose(false);
        }
    }
}