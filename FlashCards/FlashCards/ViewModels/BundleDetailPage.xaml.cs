using FlashCards.Models;
using FlashCards.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FlashCards.ViewModels
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BundleDetailPage : ContentPage
    {
        public BundleDetailPage()
        {
            InitializeComponent();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();

            var bundle = (Bundle)BindingContext;
            EntryName.Behaviors.Add(new BundleNameValidator() { IdOfBundle = bundle.Id });
        }

        async void SaveButtonHandler(object sender, EventArgs e)
        {
            var bundle = (Bundle)BindingContext;
            BundleNameValidator entryValidator = EntryName.Behaviors.Where(b => b is BundleNameValidator).FirstOrDefault() as BundleNameValidator;
            if (entryValidator.IsValid)
            {
                int result = await App.Database.SaveBundleAsync(bundle);
                await Navigation.PopAsync();
            }
            else
            {
                ResultField.Text = "Invalid Name";
                ResultField.TextColor = Color.Bisque;
            }
        }
    }
}