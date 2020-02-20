using Afonsoft.Data.Args;
using Afonsoft.Data.Attribute;
using Afonsoft.Data.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace Afonsoft.Data
{
    [Serializable]
    public abstract class BusinessCollectionDictionary<TKey, T> : IBusinessCollection<T>, IBusinessCollectionDictionary<TKey, T>
    {
        private SortedDictionary<TKey, T> container;
        protected bool isDisposed;

        protected BusinessCollectionDictionary()
        {
            this.container = new SortedDictionary<TKey, T>();
        }

        protected BusinessCollectionDictionary(IDataAccessLayer<T> dal)
        {
            this.container = new SortedDictionary<TKey, T>();
            this.DataAccessLayer = dal;
        }


        protected event EventHandler<BusinessCollectionEventArgs> Populate;


        protected event EventHandler<CompositeEventArgs<T>> DoComposition;

        public event EventHandler<ViewEventArgs<T>> OnCreate;

        public event EventHandler<EventArgs> OnRefresh;

        public event EventHandler<ViewEventArgs<T>> OnRemove;

        public event EventHandler<ViewEventArgs<T>> OnUpdate;

        public event EventHandler<ViewErrorEventArgs<T>> OnError;

        protected void AddToContainer(TKey key, T item)
        {
            CompositeEventArgs<T> e = this.DataAccessLayer != null ? new CompositeEventArgs<T>(this.DataAccessLayer.DataConnector, item) : new CompositeEventArgs<T>(item);
            if (this.DoComposition != null && (object)item is IComposable)
                this.DoComposition((object)this, e);
            this.container.Add(key, item);
            this.OnCreate?.Invoke((object)this, new ViewEventArgs<T>(item));
        }

        private void PopulateContainer()
        {
            try
            {
                this.Clear();
                this.OnRefresh?.Invoke((object)this, new EventArgs());
                this.Populate((object)this, this.DataAccessLayer != null ? new BusinessCollectionEventArgs(this.DataAccessLayer.DataConnector) : new BusinessCollectionEventArgs());
            }
            catch (Exception ex)
            {
                this.OnError?.Invoke((object)this, new ViewErrorEventArgs<T>(default(T), $"Populate : {ex.Message}", ex));
            }
        }

        public void Refresh()
        {
            this.PopulateContainer();
        }

        public IDataAccessLayer<T> DataAccessLayer { get; }

        public void ForEach(Action<T> action)
        {
            foreach (T obj in (IEnumerable<T>)this.Values)
                action(obj);
        }

        private TKey LookForUniqueIdentifier(T item)
        {
            foreach (PropertyInfo property in item.GetType().GetProperties())
            {
                if (((System.Attribute[])property.GetCustomAttributes(typeof(UniqueIdentifierAttribute), false)).Length != 0)
                {
                    MethodInfo getMethod = property.GetGetMethod();
                    object[] parameters = new object[0];
                    return (TKey)getMethod.Invoke((object)item, parameters);
                }
            }
            throw new NotSupportedException("Entidades utilizadas em BusinessCollectionDictionary devem possuir uma propriedade/campo identificada com o atributo [UniqueIdentifier].");
        }

        public virtual void Update(TKey key)
        {

            this.Update(this.container[key]);
        }

        public virtual void Update(T item)
        {
            try
            {
                this.DataAccessLayer?.Update(item);
                this[this.LookForUniqueIdentifier(item)] = item;
                this.OnUpdate?.Invoke((object)this, new ViewEventArgs<T>(item));
            }
            catch (Exception ex)
            {
                this.OnError?.Invoke(this, new ViewErrorEventArgs<T>(item, $"Update : {ex.Message}", ex));
            }
        }

        public bool ContainsKey(TKey key)
        {

            return this.container.ContainsKey(key);
        }

        public ICollection<TKey> Keys
        {
            get
            {
                return (ICollection<TKey>)this.container.Keys;
            }
        }



        public bool TryGetValue(TKey key, out T value)
        {
            return this.container.TryGetValue(key, out value);
        }

        public ICollection<T> Values
        {
            get
            {
                return (ICollection<T>)this.container.Values;
            }
        }

        public T this[TKey key]
        {
            get
            {
                return this.container[key];
            }
            set
            {
                if (this.ContainsKey(key))
                    this.container[key] = value;
                else
                    this.Add(key, value);
            }
        }

        public virtual void Add(KeyValuePair<TKey, T> item)
        {
            this.Add(item.Key, item.Value);
        }

        public virtual void Add(TKey key, T value)
        {
            try
            {
                this.DataAccessLayer?.Create(value);
                this.container.Add(key, value);
                this.OnCreate?.Invoke((object)this, new ViewEventArgs<T>(value));
            }
            catch (Exception ex)
            {
                this.OnError?.Invoke(this, new ViewErrorEventArgs<T>(value, $"Add : {ex.Message}", ex));
            }
        }

        public void Clear()
        {
            this.container.Clear();
            this.OnRefresh?.Invoke((object)this, new EventArgs());
        }

        public bool Contains(KeyValuePair<TKey, T> item)
        {
            return ((ICollection<KeyValuePair<TKey, T>>)this.container).Contains(item);
        }

        public void CopyTo(KeyValuePair<TKey, T>[] array, int arrayIndex)
        {
            this.container.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get
            {
                return this.container.Count;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return ((ICollection<KeyValuePair<TKey, T>>)this.container).IsReadOnly;
            }
        }

        public virtual bool Remove(KeyValuePair<TKey, T> item)
        {
            return this.Remove(item.Key);
        }

        public virtual bool Remove(TKey key)
        {
            T entity = this.container[key];
            try
            {
                this.DataAccessLayer?.Remove(entity);
                int num = this.container.Remove(key) ? 1 : 0;
                this.OnRemove?.Invoke((object)this, new ViewEventArgs<T>(entity));
                return num != 0;
            }
            catch (Exception ex)
            {
                this.OnError?.Invoke(this, new ViewErrorEventArgs<T>(entity, $"Add : {ex.Message}", ex));
                return false;
            }
        }

        public IEnumerator<KeyValuePair<TKey, T>> GetEnumerator()
        {
            return ((IEnumerable<KeyValuePair<TKey, T>>)this.container).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)this.container).GetEnumerator();
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize((object)this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposing || this.isDisposed)
                return;
            foreach (T obj in this.container.Values)
            {
                if ((object)obj is IDisposable)
                    ((IDisposable)(object)obj).Dispose();
            }
            this.isDisposed = true;
        }
    }
}