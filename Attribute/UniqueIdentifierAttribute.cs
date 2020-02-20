using System;

namespace Afonsoft.Data.Attribute
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    [Serializable]
    public sealed class UniqueIdentifierAttribute : System.Attribute
    {
    }
}