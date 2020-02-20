using Afonsoft.Data.Interfaces;
using System;

namespace Afonsoft.Data.Args
{
    [Serializable]
    public class BusinessCollectionEventArgs : EventArgs
    {
        public IDataConnector DataConnector { get; }

        public BusinessCollectionEventArgs()
        {
        }

        public BusinessCollectionEventArgs(IDataConnector dataConnector)
          : this()
        {
            this.DataConnector = dataConnector;
        }
    }
}
