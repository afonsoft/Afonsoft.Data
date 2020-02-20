using Afonsoft.Data.Args;
using Afonsoft.Data.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Afonsoft.Data
{
    [Serializable]
    public abstract class BusinessCollectionList<T> : IBusinessCollection<T>, IBusinessCollectionList<T> 
    {
        protected bool isDirty = true;
        protected bool isRefreshed;
        protected bool isDisposed;
        private readonly List<T> container;

        protected event EventHandler<BusinessCollectionEventArgs> Populate;

        protected event EventHandler<CompositeEventArgs<T>> DoComposition;

        public event EventHandler<ViewEventArgs<T>> OnCreate;

        public event EventHandler<EventArgs> OnRefresh;

        public event EventHandler<ViewEventArgs<T>> OnRemove;

        public event EventHandler<ViewEventArgs<T>> OnUpdate;

        public event EventHandler<ViewErrorEventArgs<T>> OnError;

        protected BusinessCollectionList(IDataAccessLayer<T> dal)
        {
            this.container = new List<T>(10);
            this.DataAccessLayer = dal;
        }

        protected BusinessCollectionList()
        {
            this.container = new List<T>(10);
        }

        private void PopulateContainer()
        {
            try
            {
                this.Clear();
                this.OnRefresh?.Invoke((object)this, new EventArgs());
                this.Populate((object)this, this.DataAccessLayer != null ? new BusinessCollectionEventArgs(this.DataAccessLayer.DataConnector) : new BusinessCollectionEventArgs());
                this.isDirty = true;
            }
            catch (Exception ex)
            {
                this.OnError?.Invoke((object)this, new ViewErrorEventArgs<T>(default(T), $"Populate : {ex.Message}", ex));
            }
        }

        public virtual T Find(T item)
        {
            if (!this.isRefreshed)
                this.Refresh();
            if (this.isDirty)
                this.Sort();
            T obj = default(T);
            int index = this.container.BinarySearch(item);
            if (index >= 0)
                obj = this.container[index];
            return obj;
        }

        public virtual void Sort()
        {
            if (!this.isDirty)
                return;
            this.container.Sort();
            this.isDirty = false;
        }

        public void Refresh()
        {
            this.isDirty = true;
            this.isRefreshed = true;
            this.PopulateContainer();
        }

        public IDataAccessLayer<T> DataAccessLayer { get; }
       

      
        public void ForEach(Action<T> action)
        {
            foreach (T obj in this.container)
                action(obj);
        }

        public T this[int index]
        {
            get
            {
                if (this.Count == 0)
                    this.Refresh();
                return this.container[index];
            }
        }

        protected void AddToContainer(T item)
        {
            CompositeEventArgs<T> e = this.DataAccessLayer != null ? new CompositeEventArgs<T>(this.DataAccessLayer.DataConnector, item) : new CompositeEventArgs<T>(item);
            if (this.DoComposition != null && (object)item is IComposable)
                this.DoComposition((object)this, e);
            this.container.Add(item);
            this.OnCreate?.Invoke((object)this, new ViewEventArgs<T>(item));
            this.isDirty = true;
        }

        public virtual void Add(T item)
        {
            try
            {
                this.DataAccessLayer?.Create(item);
                this.AddToContainer(item);
                this.isDirty = true;
            }
            catch (Exception ex)
            {
                this.OnError?.Invoke((object)this, new ViewErrorEventArgs<T>(item, $"Add : {ex.Message}", ex));
            }
        }

        public virtual bool Remove(T item)
        {
            try
            {
                this.DataAccessLayer?.Remove(item);
                this.container.Remove(item);
                this.OnRemove?.Invoke((object)this, new ViewEventArgs<T>(item));
                return true;
            }
            catch (Exception ex)
            {
                this.OnError?.Invoke((object)this, new ViewErrorEventArgs<T>(item, $"Add : {ex.Message}", ex));
                return false;
            }
        }

        public virtual void Update(T item)
        {
            try
            {
                this.DataAccessLayer?.Update(item);
                int index = this.container.BinarySearch(item);
                if (index >= 0)
                    this.container[index] = item;
                if (this.OnUpdate == null)
                    return;
                this.OnUpdate((object)this, new ViewEventArgs<T>(item));
            }
            catch (Exception ex)
            {
                this.OnError?.Invoke((object)this, new ViewErrorEventArgs<T>(item, $"Add : {ex.Message}", ex));
            }
        }

        public void Clear()
        {
            this.container.Clear();
            this.OnRefresh?.Invoke((object)this, new EventArgs());
        }

        public bool Contains(T item)
        {
            return this.container.Contains(item);
        }

        public int Count
        {
            get
            {
                if (!this.isRefreshed)
                    this.Refresh();
                return this.container.Count;
            }
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            this.container.CopyTo(array, arrayIndex);
        }

        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

       
        IEnumerator IEnumerable.GetEnumerator()
        {
            if (this.Count == 0)
                this.Refresh();
            return ((IEnumerable)this.container).GetEnumerator();
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            if (this.Count == 0 && !this.isRefreshed)
                this.Refresh();
            return ((IEnumerable<T>)this.container).GetEnumerator();
        }

        public void Dispose()
        {
            GC.SuppressFinalize((object)this);
            this.Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposing || this.isDisposed)
                return;
            foreach (T obj in this.container)
            {
                if ((object)obj is IDisposable)
                    ((IDisposable)(object)obj).Dispose();
            }
            this.isDisposed = true;
        }
    }
}
