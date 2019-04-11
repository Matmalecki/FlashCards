using FlashCards.Models;
using FlashCards.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FlashCards.ViewModels.Cards
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PhotoCardDetailPage : ContentPage
	{
        Card card;
        Bundle bundle;
        public PhotoCardDetailPage(Bundle bundle, Card card)
        {
            InitializeComponent();

            this.card = card;
            this.bundle = bundle;
            BindingContext = card;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            EntryInformation.Behaviors.Add(new CardInfoValidator(bundle.Id, card.Id));
        }

        async void SaveButtonHandler(object sender, EventArgs e)
        {
            var card = (Card)BindingContext;
            card.BundleId = bundle.Id;
            CardInfoValidator entryValidator = EntryInformation.Behaviors.Where(b => b is CardInfoValidator).FirstOrDefault() as CardInfoValidator;
            if (entryValidator.IsValid)
            {
                int result = await App.Database.SaveCardAsync(card);
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