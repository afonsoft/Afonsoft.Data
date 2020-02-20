using Afonsoft.Data.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Afonsoft.Data
{
    [Serializable]
    public abstract class DataAccessLayerCollection<T> : DataBaseEnumerator<T>, IDataAccessLayerCollection<T>
    {
        protected IEnumerator<T> enumerator;

        protected DataAccessLayerCollection(IDataConnector dataConnector) : base(dataConnector)
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

        public override void Dispose(bool disposing)
        {
            if (this.isDisposed)
                return;
            if (disposing && this.enumerator != null)
                this.enumerator.Dispose();
            this.isDisposed = true;
            base.Dispose(disposing);
        }
    }
}