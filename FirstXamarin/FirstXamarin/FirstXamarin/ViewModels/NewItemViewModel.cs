using FirstXamarin.Collections;
using FirstXamarin.Models;
using System.Windows.Input;
using Xamarin.Forms;
using static System.String;

namespace FirstXamarin.ViewModels
{
    public class NewItemViewModel
    {
        private readonly PhonesCollection _collection;

        public PhonesBook Item { get; set; }
        public ICommand Save { get; }

        public NewItemViewModel()
        {
            _collection = PhonesCollection.GetPhonesCollection;
            Save = new Command(ExecuteSave);
            Item = new PhonesBook();
        }
        public NewItemViewModel(PhonesBook item)
        {
            _collection = PhonesCollection.GetPhonesCollection;
            Save = new Command(ExecuteSave);
            PhonesBook phone = new PhonesBook
            {
                Id = item.Id,
                Name = item.Name,
                Surname = item.Surname,
                Phone = item.Phone
            };
            Item = phone;
        }

        private async void ExecuteSave()
        {
            if (IsNullOrWhiteSpace(Item.Name) || IsNullOrWhiteSpace(Item.Phone))
                return;
            if (Item.Id == 0)
                _collection.AddPhone(Item);
            else
            {
                _collection.EditPhone(Item);
            }
            await Application.Current.MainPage.Navigation.PopAsync();
        }
    }
}