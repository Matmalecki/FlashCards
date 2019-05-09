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

            BindingContext = new BundlesViewModel();
            
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

        void OnContentViewSizeChanged(object sender, EventArgs args)
        {
            ContentPage view = (ContentPage)sender;


            if (view.Width <= 0 || view.Height <= 0)
                return;

            NamedSize namedSize = NamedSize.Default;
            if (view.Width < 300)
                namedSize = NamedSize.Micro;
            else if (view.Width < 600)
                namedSize = NamedSize.Small;
            else if (view.Width < 800)
                namedSize = NamedSize.Medium;
            else if (view.Width >= 800)
                namedSize = NamedSize.Large;
            // Set the final font size and the text with the embedded value.

            var labelStyle = new Style(typeof(Label))
            {
                Setters =
                {
                    new Setter { Property=Label.FontSizeProperty, Value= Device.GetNamedSize(namedSize, typeof(Label)) },
                }
            };

            view.Resources.Remove("labelStyle");
            view.Resources.Add("labelStyle", labelStyle);
        }


    }

   
}
