using FlashCards.Custom;
using FlashCards.Models;
using FlashCards.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FlashCards.ViewModels.Cards
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PhotoCardDetailPage : ContentPage, INotifyPropertyChanged
    {
        PhotoCard card;
        Bundle bundle;

        public PhotoCardDetailPage(Bundle bundle, PhotoCard card)
        {
            InitializeComponent();

            this.card = card;
            this.bundle = bundle;
            BindingContext = card;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ImageDisplay.SetBinding(Image.SourceProperty, new Binding("Information", BindingMode.Default, new StreamToStringConverter()));
        }

        async void PickPhotoHandler(object sender, EventArgs e)
        {
            var button = (Button)sender;
            button.IsEnabled = false;
            Stream stream = await DependencyService.Get<IPicturePicker>().GetImageStreamAsync();

            if (stream != null)
            {
                card.Information = ImagesConverter.ImageToStringBase64(stream);
                //ImageDisplay.Source = ImageSource.FromStream(() => ImagesConverter.StringBase64ToImage(card.Information));
                button.IsEnabled = true;
            }
            else
            {
                button.IsEnabled = true;
            }
        }

        async void SaveButtonHandler(object sender, EventArgs e)
        {
            var card = (PhotoCard)BindingContext;
            card.BundleId = bundle.Id;
          
            int result = await App.Database.SaveCardAsync(card);
            await Navigation.PopAsync();
            

        }
    }
}