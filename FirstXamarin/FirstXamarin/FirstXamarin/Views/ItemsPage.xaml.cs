using Xamarin.Forms;
using FirstXamarin.ViewModels;

namespace FirstXamarin.Views
{
    public partial class ItemsPage : ContentPage
    {
        public ItemsPage()
        {
            InitializeComponent();
            BindingContext = new ItemsViewModel();
        }
    }
}