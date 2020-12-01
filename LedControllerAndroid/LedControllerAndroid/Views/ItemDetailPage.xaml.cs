using System.ComponentModel;
using Xamarin.Forms;
using LedControllerAndroid.ViewModels;

namespace LedControllerAndroid.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}