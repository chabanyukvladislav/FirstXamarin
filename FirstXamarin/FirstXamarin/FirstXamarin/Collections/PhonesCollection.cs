using FirstXamarin.Models;
using FirstXamarin.Services;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace FirstXamarin.Collections
{
    public class PhonesCollection : INotifyCollectionChanged
    {
        private static object locker = new object();
        private IDataStore<PhonesBook> dataStore;
        private static PhonesCollection phonesCollection;
        private List<PhonesBook> collection;

        public static PhonesCollection GetPhonesCollection
        {
            get
            {
                if (phonesCollection == null)
                {
                    lock (locker)
                    {
                        if (phonesCollection == null)
                        {
                            phonesCollection = new PhonesCollection();
                        }
                    }
                }
                return phonesCollection;
            }
        }
        public List<PhonesBook> GetCollection
        {
            get
            {
                return collection;
            }
        }

        private PhonesCollection()
        {
            dataStore = DataStore.GetDataStore;
            UpdateCollection();
        }

        private async void UpdateCollection()
        {
            collection = await dataStore.GetItemsAsync();
            OnCollectionChanged(NotifyCollectionChangedAction.Reset, null);
        }

        public async void AddPhone(PhonesBook item)
        {
            if (await dataStore.AddItemAsync(item))
            {
                collection.Add(item);
                OnCollectionChanged(NotifyCollectionChangedAction.Add, item);
            }
        }
        public async void EditPhone(PhonesBook item)
        {
            if (await dataStore.UpdateItemAsync(item))
            {
                PhonesBook element = collection.Find(el => el.Id == item.Id);
                collection.Remove(element);
                OnCollectionChanged(NotifyCollectionChangedAction.Remove, element);
                collection.Add(item);
                OnCollectionChanged(NotifyCollectionChangedAction.Add, item);
            }
        }
        public async void RemovePhone(PhonesBook item)
        {
            if (await dataStore.DeleteItemAsync(item))
            {
                collection.Remove(item);
                OnCollectionChanged(NotifyCollectionChangedAction.Remove, item);
            }
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged;
        protected void OnCollectionChanged(NotifyCollectionChangedAction action, PhonesBook item)
        {
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(action, item));
        }
    }
}