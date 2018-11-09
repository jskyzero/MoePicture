using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace MoePicture.ViewModels
{
    /// <summary>
    /// 数据储存虚类，实现IList接口，ISupportIncrementalLoading 接口, INotifyCollectionChanged接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class IncrementalLoadingBase<T> : IList, ISupportIncrementalLoading, INotifyCollectionChanged, INotifyPropertyChanged
    {
        #region IList

        public int Add(object value)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            _storage.Clear();
        }

        public bool Contains(object value)
        {
            return _storage.Contains((T)value);
        }

        public int IndexOf(object value)
        {
            return _storage.IndexOf((T)value);
        }

        public void Insert(int index, object value)
        {
            throw new NotImplementedException();
        }

        public bool IsFixedSize
        {
            get { return false; }
        }

        public bool IsReadOnly
        {
            get { return true; }
        }

        public void Remove(object value)
        {
            throw new NotImplementedException();
        }

        public void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        public object this[int index]
        {
            get
            {
                return _storage[index];
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public void CopyTo(Array array, int index)
        {
            ((IList)_storage).CopyTo(array, index);
        }

        public int Count
        {
            get { return _storage.Count; }
        }

        public bool IsSynchronized
        {
            get { return false; }
        }

        public object SyncRoot
        {
            get { throw new NotImplementedException(); }
        }

        public IEnumerator GetEnumerator()
        {
            return _storage.GetEnumerator();
        }

        #endregion IList

        #region ISupportIncrementalLoading

        public bool HasMoreItems
        {
            get { return HasMoreItemsOverride(); }
        }


        public Windows.Foundation.IAsyncOperation<LoadMoreItemsResult> LoadMoreItemsAsync(uint count)
        {
            if (Busy)
            {
                throw new InvalidOperationException("Only one operation in flight at a time");
            }

            Busy = true;

            return AsyncInfo.Run((c) => LoadMoreItemsAsync(c, count));
        }

        #endregion ISupportIncrementalLoading

        #region INotifyCollectionChanged

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        #endregion INotifyCollectionChanged

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        //[NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion INotifyPropertyChanged

        #region Private methods

        // 扩充现有数组，并通知前台进行对应的响应
        private async Task<LoadMoreItemsResult> LoadMoreItemsAsync(CancellationToken c, uint count)
        {
            try
            {
                // 调用虚方法，得到新增对象链表
                var items = await LoadMoreItemsOverrideAsync(c, (int)count);
                var baseIndex = _storage.Count;

                _storage.AddRange(items);

                // Now notify of the new items
                NotifyOfInsertedItems(baseIndex, items.Count);

                return new LoadMoreItemsResult { Count = (uint)items.Count };
            }
            //catch
            //{
            //    return new LoadMoreItemsResult { Count = 0 };
            //}
            finally
            {
                Busy = false;
            }
        }

        // 对链表里面，下标为baseIndex到baseIndex+count的对象发送通知请求更新
        private void NotifyOfInsertedItems(int baseIndex, int count)
        {
            // OnPropertyChanged("count");

            if (CollectionChanged == null)
            {
                return;
            }

            // 对每一项，向前台发送通知
            for (int i = 0; i < count; i++)
            {
                var args = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, _storage[i + baseIndex], i + baseIndex);
                CollectionChanged(this, args);
            }
        }

        #endregion Private methods

        #region Overridable methods

        protected abstract Task<IList<T>> LoadMoreItemsOverrideAsync(CancellationToken c, int count);

        protected abstract bool HasMoreItemsOverride();

        #endregion Overridable methods

        #region State

        // 用于储存数据的链表
        protected List<T> _storage = new List<T>();

        // 表示后台线程是否正在进行，一次只能进行一个线程
        private bool busy = false;
        protected bool Busy { get => busy; set { busy = value; OnPropertyChanged("Busy"); } }


        #endregion State
    }
}
