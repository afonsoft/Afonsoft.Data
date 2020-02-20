using System;

namespace Afonsoft.Data.Interfaces
{
    public interface IComposable
    {
        void Compose(object composite, Type compositeType);
    }
}
