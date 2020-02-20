using Afonsoft.Data.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;

namespace Afonsoft.Data
{
    [Serializable]
    public abstract class DataBaseEnumerator<T> : InLineDataManipulationCommand,  IDataBaseEnumerator<T>
    {
        protected IDataConnector dataConnector;
        protected bool isDisposed;
        protected DbCommand cmd;
        protected DbDataReader rdr;
        protected SortedList<string, string> argList;

        public IDataConnector DataConnector
        {
            get
            {
                return this.dataConnector;
            }
        }

        protected DataBaseEnumerator(IDataConnector dataConnector)
        {
            this.dataConnector = dataConnector;
        }

        public virtual void CreateDataReader()
        {
            if (this.rdr != null)
                this.rdr.Close();
            this.rdr = this.cmd.ExecuteReader();
        }

        public abstract T Current { get; }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize((object)this);
        }

        public virtual void Dispose(bool disposing)
        {
            if (this.isDisposed)
                return;
            if (disposing)
            {
                if (this.cmd != null)
                    this.cmd.Dispose();
                if (this.rdr != null)
                    this.rdr.Dispose();
            }
            this.isDisposed = true;
        }

        object IEnumerator.Current
        {
            get
            {
                return (object)this.Current;
            }
        }

        public virtual bool MoveNext()
        {
            int num = this.rdr.Read() ? 1 : 0;
            if (num != 0)
                return num != 0;
            this.rdr.Close();
            this.rdr.Dispose();
            return num != 0;
        }

        public virtual void Reset()
        {
            this.CreateDataReader();
        }
    }
}
