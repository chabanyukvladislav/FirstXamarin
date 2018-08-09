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
        private PhonesCollection collection;
        private ObservableCollection<PhonesBook> items;
        private bool isRefreshing = false;

        public bool IsRefreshing
        {
            get
            {
                return isRefreshing;
            }
            private set
            {
                isRefreshing = value;
                OnPropertyChanged(nameof(IsRefreshing));
            }
        }
        public ObservableCollection<PhonesBook> Items
        {
            get
            {
                return items;
            }
            private set
            {
                items = value;
                OnPropertyChanged(nameof(Items));
            }
        }

        public PhonesBook ItemSelected { get; set; }
        public ICommand AddItem { get; private set; }
        public ICommand RemoveItem { get; private set; }
        public ICommand UpdateItem { get; private set; }

        public ItemsViewModel()
        {
            collection = PhonesCollection.GetPhonesCollection;
            AddItem = new Command(ExecuteAddItem);
            RemoveItem = new Command(ExecuteRemoveItem);
            UpdateItem = new Command(ExecuteUpdateItem);
            collection.CollectionChanged += ExecuteRefresh;
        }

        private void ExecuteRefresh(object sender, NotifyCollectionChangedEventArgs e)
        {
            IsRefreshing = true;
            Items = new ObservableCollection<PhonesBook>(collection.GetCollection);
            IsRefreshing = false;
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
            collection.RemovePhone(ItemSelected);
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