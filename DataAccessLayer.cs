using Afonsoft.Data.Interfaces;
using System;

namespace Afonsoft.Data
{
    [Serializable]
    public abstract class DataAccessLayer<T, TConnector> : InLineDataManipulationCommand, IDataAccessLayer<T>
     where TConnector : IDataConnector
    {
        protected TConnector dataConnector;

        protected DataAccessLayer(TConnector dataConnector)
        {
            this.dataConnector = dataConnector;
        }

        public abstract int Create(T entity);

        public abstract int Update(T entity);

        public abstract int Remove(T entity);

        public IDataConnector DataConnector
        {
            get
            {
                return (IDataConnector)this.dataConnector;
            }
        }
    }
}
