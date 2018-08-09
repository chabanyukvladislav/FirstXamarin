using FirstXamarin.Collections;
using FirstXamarin.Models;
using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace FirstXamarin.ViewModels
{
    public class NewItemViewModel
    {
        private PhonesCollection collection;

        public PhonesBook Item { get; set; }
        public ICommand Save { get; private set; }

        public NewItemViewModel()
        {
            collection = PhonesCollection.GetPhonesCollection;
            Save = new Command(ExecuteSave);
            Item = new PhonesBook();
        }
        public NewItemViewModel(PhonesBook item)
        {
            collection = PhonesCollection.GetPhonesCollection;
            Save = new Command(ExecuteSave);
            Item = item;
        }

        private async void ExecuteSave()
        {
            if (String.IsNullOrWhiteSpace(Item.Name) || String.IsNullOrWhiteSpace(Item.Phone))
                return;
            if (Item.Id == 0)
                collection.AddPhone(Item);
            else
            {
                collection.EditPhone(Item);
            }
            await Application.Current.MainPage.Navigation.PopAsync();
        }
    }
}