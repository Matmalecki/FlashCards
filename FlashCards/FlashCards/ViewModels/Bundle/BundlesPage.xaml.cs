using FlashCards.Models;
using FlashCards.ViewModels.Cards;
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

        async void DeleteBundleHandler(object sender, ItemTappedEventArgs e)
        {
            Button btn = sender as Button;
            Bundle bundle = await App.Database.GetBundleAsync(int.Parse(btn.CommandParameter.ToString()));
            
            InputBox(bundle);
        }




        public void InputBox(Bundle bundle)
        {
            var title = new Label { Text = "Are you sure?", HorizontalOptions = LayoutOptions.Center, FontAttributes = FontAttributes.Bold };
            var message = new Label { Text = "Enter this bundle's name:" };
            var input = new Entry { Text = "" };

            var btnOk = new Button
            {
                Text = "Ok",
                WidthRequest = 100,
                BackgroundColor = Color.BlueViolet
            };
            btnOk.Clicked += async (s, e) =>
            {
                var result = input.Text;
                if (result == bundle.Name)
                {
                    await App.Database.DeleteBundleAsync(bundle);
                    await Navigation.PopModalAsync();
                }
                else
                {
                    message.TextColor = Color.Plum;
                }
            };

            var btnCancel = new Button
            {
                Text = "Cancel",
                WidthRequest = 100,
                BackgroundColor = Color.BlueViolet
            };
            btnCancel.Clicked += async (s, e) =>
            {
                await Navigation.PopModalAsync();

            };

            var slButtons = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Children = { btnOk, btnCancel },
            };

            var layout = new StackLayout
            {
                Padding = new Thickness(0, 40, 0, 0),
                VerticalOptions = LayoutOptions.StartAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                Orientation = StackOrientation.Vertical,
                Children = { title, message, input, slButtons },
            };


            var page = new ContentPage();
            page.Content = layout;
            Navigation.PushModalAsync(page);
            input.Focus();

        }



    }
}
