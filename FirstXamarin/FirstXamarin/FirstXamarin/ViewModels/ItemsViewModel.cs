using System.Collections.ObjectModel;
using Xamarin.Forms;
using FirstXamarin.Models;
using FirstXamarin.Views;
using System.ComponentModel;
using System.Windows.Input;
using FirstXamarin.Collections;
using System.Collections.Specialized;

namespace FirstXamarin.ViewModels
{
    public class ItemsViewModel : INotifyPropertyChanged
    {
        private readonly PhonesCollection _collection;
        private ObservableCollection<PhonesBook> _items;
        
        public ObservableCollection<PhonesBook> Items
        {
            get => _items;
            private set
            {
                _items = value;
                OnPropertyChanged(nameof(Items));
            }
        }

        public PhonesBook ItemSelected { get; set; }
        public ICommand AddItem { get; }
        public ICommand RemoveItem { get; }
        public ICommand UpdateItem { get; }

        public ItemsViewModel()
        {
            _collection = PhonesCollection.GetPhonesCollection;
            AddItem = new Command(ExecuteAddItem);
            RemoveItem = new Command(ExecuteRemoveItem);
            UpdateItem = new Command(ExecuteUpdateItem);
            _collection.CollectionChanged += ExecuteRefresh;
        }

        private void ExecuteRefresh(object sender, NotifyCollectionChangedEventArgs e)
        {
            Items = new ObservableCollection<PhonesBook>(_collection.Collection);
        }
        private async void ExecuteAddItem()
        {
            ItemSelected = null;
            await Application.Current.MainPage.Navigation.PushAsync(new NewItemPage());
        }
        private async void ExecuteRemoveItem()
        {
            if (ItemSelected == null)
                return;
            if (!await Application.Current.MainPage.DisplayAlert("Delete?", "Are you sure?", "Yes", "No"))
                return;
            _collection.RemovePhone(ItemSelected);
            ItemSelected = null;
        }
        private async void ExecuteUpdateItem()
        {
            if (ItemSelected == null)
                return;
            PhonesBook item = ItemSelected;
            ItemSelected = null;
            await Application.Current.MainPage.Navigation.PushAsync(new NewItemPage(item));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}