using FlashCards.Models;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FlashCards.ViewModels
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BundlesPage : ContentPage
    {

        public BundlesPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            BundlesView.ItemsSource = await App.Database.GetBundlesAsync();
        }

        async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;

            await Navigation.PushAsync(new CardsPage(e.Item as Bundle));
        }

        async void AddBundleHandler(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new BundleDetailPage()
            {
                BindingContext = new Bundle()
            });
        }



    }
}
