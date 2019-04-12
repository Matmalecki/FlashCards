using FlashCards.Models;
using FlashCards.ViewModels.BundleViewModels;
using FlashCards.Views.Cards;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FlashCards.Views
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

        private void DeleteBundle(object sender, EventArgs args)
        {
            var button = sender as Button;

            Bundle bundle = button?.BindingContext as Bundle;

            (BindingContext as BundlesViewModel).DeleteBundleCommand.Execute(bundle);

        }


    }
}
