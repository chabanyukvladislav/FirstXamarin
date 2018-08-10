using FirstXamarin.Models;
using FirstXamarin.Services;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace FirstXamarin.Collections
{
    public class PhonesCollection : INotifyCollectionChanged
    {
        private static readonly object Locker = new object();
        private readonly IDataStore<PhonesBook> _dataStore;
        private static PhonesCollection _phonesCollection;

        public static PhonesCollection GetPhonesCollection
        {
            get
            {
                if (_phonesCollection == null)
                {
                    lock (Locker)
                    {
                        if (_phonesCollection == null)
                        {
                            _phonesCollection = new PhonesCollection();
                        }
                    }
                }

                return _phonesCollection;
            }
        }
        public List<PhonesBook> Collection { get; private set; }

        private PhonesCollection()
        {
            _dataStore = DataStore.GetDataStore;
            UpdateCollection();
        }

        private async void UpdateCollection()
        {
            Collection = await _dataStore.GetItemsAsync();
            OnCollectionChanged(NotifyCollectionChangedAction.Reset, null);
        }

        public async void AddPhone(PhonesBook item)
        {
            if (!await _dataStore.AddItemAsync(item)) return;
            Collection.Add(item);
            OnCollectionChanged(NotifyCollectionChangedAction.Add, item);
        }
        public async void EditPhone(PhonesBook item)
        {
            if (!await _dataStore.UpdateItemAsync(item)) return;
            PhonesBook element = Collection.Find(el => el.Id == item.Id);
            Collection.Remove(element);
            OnCollectionChanged(NotifyCollectionChangedAction.Remove, element);
            Collection.Add(item);
            OnCollectionChanged(NotifyCollectionChangedAction.Add, item);
        }
        public async void RemovePhone(PhonesBook item)
        {
            if (!await _dataStore.DeleteItemAsync(item)) return;
            Collection.Remove(item);
            OnCollectionChanged(NotifyCollectionChangedAction.Remove, item);
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged;
        protected void OnCollectionChanged(NotifyCollectionChangedAction action, PhonesBook item)
        {
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(action, item));
        }
    }
}