using Xamarin.Forms;
using FirstXamarin.Models;
using FirstXamarin.ViewModels;

namespace FirstXamarin.Views
{
    public partial class NewItemPage : ContentPage
    {
        public NewItemPage()
        {
            InitializeComponent();
            NewItemViewModel vm = new NewItemViewModel();
            BindingContext = vm;
        }
        public NewItemPage(PhonesBook item)
        {
            InitializeComponent();
            NewItemViewModel vm = new NewItemViewModel(item);
            BindingContext = vm;
        }
    }
}