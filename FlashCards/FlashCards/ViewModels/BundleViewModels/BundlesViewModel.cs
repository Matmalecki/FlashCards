using FlashCards.Custom;
using FlashCards.Models;
using FlashCards.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;

namespace FlashCards.ViewModels.BundleViewModels
{
    class BundlesViewModel : BaseViewModel
    {


        private ObservableCollection<Bundle> _bundles;

        public ObservableCollection<Bundle> Bundles
        {
            get
            {
                return _bundles;
            }
            set
            {
                _bundles = value;
                OnPropertyChanged();
            }
        }
        public BundlesViewModel()
        {
            GetListOfBundles();
        }

        private async void GetListOfBundles()
        {
            Bundles = new ObservableCollection<Bundle>( await App.Database.GetBundlesAsync());
        }

        private Command _addBundleCommand;
        public Command AddBundleCommand
        {
            get
            {
                return _addBundleCommand ?? (_addBundleCommand = new Command(AddButtonHandlerAsync));
            }
        }

        private async void AddButtonHandlerAsync()
        {

            await Application.Current.MainPage.Navigation.PushAsync(new BundleDetailPage()
            {
                BindingContext = new BundleDetailViewModel()
            });
        }

        private Command _deleteBundleCommand;
        public Command DeleteBundleCommand
        {
            get
            {
                return _deleteBundleCommand ?? (_deleteBundleCommand = new Command<Bundle>(DeleteButtonHandlerAsync));
            }
        }


        private void DeleteButtonHandlerAsync(Bundle bundle)
        {
            var title = new Label { Text = $"Are you sure you want to delete {bundle.Name}?", HorizontalOptions = LayoutOptions.Center, FontAttributes = FontAttributes.Bold };
            var message = new Label { Text = "Enter this bundle's name to delete it:" };
            var input = new Entry { Text = "" };

            var btnOk = new Button
            {
                Text = "Remove",
                WidthRequest = 100,
                BackgroundColor = ColorConstants.RemoveButtonBgColor
            };
            btnOk.Clicked += async (s, e) =>
            {
                var result = input.Text;
                if (result == bundle.Name)
                {
                    await App.Database.DeleteBundleAsync(bundle);
                    await Application.Current.MainPage.Navigation.PopModalAsync();
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
                BackgroundColor = ColorConstants.AcceptButtonBgColor
            };
            btnCancel.Clicked += async (s, e) =>
            {
                await Application.Current.MainPage.Navigation.PopModalAsync();

            };

            var slButtons = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
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
            Application.Current.MainPage.Navigation.PushModalAsync(page);
            input.Focus();

        }

    }
}
