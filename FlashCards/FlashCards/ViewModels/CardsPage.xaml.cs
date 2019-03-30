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
    public partial class CardsPage : ContentPage
    {
        Bundle bundle;

        public CardsPage(Bundle bundle)
        {
            InitializeComponent();
            this.bundle = bundle;
			
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            CardsView.ItemsSource = await App.Database.GetCardsFromBundle(bundle.Id);
        }

        async void SelectedCardHandler(object sender, ItemTappedEventArgs e)
        {
            
        }

        async void EditBundleHandler(object sender, ItemTappedEventArgs e)
        {
            await Navigation.PushAsync(new BundleDetailPage()
            {
                BindingContext = bundle
            });
        }

        async void DeleteBundleHandler(object sender, ItemTappedEventArgs e)
        {
            InputBox(this.Navigation);
        }

        public async void InputBox(INavigation navigation)
        {
            // wait in this proc, until user did his input 

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
                    await navigation.PopModalAsync();
                    await navigation.PopToRootAsync();
                    
                } else
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
                await navigation.PopModalAsync();

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
                Children = { title,message,input, slButtons },
            };

            // create and show page
            var page = new ContentPage();
            page.Content = layout;
            navigation.PushModalAsync(page);
            // open keyboard
            input.Focus();

            // code is waiting her, until result is passed with tcs.SetResult() in btn-Clicked
            // then proc returns the result
        }

    }
}
