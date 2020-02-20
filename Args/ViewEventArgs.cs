using System;

namespace Afonsoft.Data.Args
{
    [Serializable]
    public class ViewEventArgs<T> : EventArgs
    {
        public ViewEventArgs(T entity)
        {
            this.Entity = entity;
        }

        public T Entity { get; set; }
    }

    [Serializable]
    public class ViewErrorEventArgs<T> : EventArgs
    {
        public ViewErrorEventArgs(T entity, string message, Exception ex)
        {
            this.Entity = entity;
            this.Message = message;
            this.Failure = ex;
        }

        public T Entity { get; set; }

        public Exception Failure { get; set; }
        public string Message { get; set; }
    }
}

